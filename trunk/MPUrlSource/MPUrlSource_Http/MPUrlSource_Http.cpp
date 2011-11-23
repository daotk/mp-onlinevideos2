/*
    Copyright (C) 2007-2010 Team MediaPortal
    http://www.team-mediaportal.com

    This file is part of MediaPortal 2

    MediaPortal 2 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal 2 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal 2.  If not, see <http://www.gnu.org/licenses/>.
*/

#include "stdafx.h"

#include "MPUrlSource_HTTP.h"
#include "Network.h"
#include "Utilities.h"
#include "LockMutex.h"

#include <WinInet.h>
#include <stdio.h>

// protocol implementation name
#define PROTOCOL_IMPLEMENTATION_NAME                                    _T("CMPUrlSource_Http")

#define METHOD_COMPARE_RANGES_BUFFERS_NAME                              _T("CompareRangesBuffers()")

PIProtocol CreateProtocolInstance(CParameterCollection *configuration)
{
  return new CMPUrlSource_Http(configuration);
}

void DestroyProtocolInstance(PIProtocol pProtocol)
{
  if (pProtocol != NULL)
  {
    CMPUrlSource_Http *pClass = (CMPUrlSource_Http *)pProtocol;
    delete pClass;
  }
}

CMPUrlSource_Http::CMPUrlSource_Http(CParameterCollection *configuration)
{
  this->configurationParameters = new CParameterCollection();
  if (configuration != NULL)
  {
    this->configurationParameters->Append(configuration);
  }

  this->logger = new CLogger(this->configurationParameters);
  this->logger->Log(LOGGER_INFO, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CONSTRUCTOR_NAME);
  
  this->receiveDataTimeout = HTTP_RECEIVE_DATA_TIMEOUT_DEFAULT;
  this->openConnetionMaximumAttempts = HTTP_OPEN_CONNECTION_MAXIMUM_ATTEMPTS_DEFAULT;
  this->filter = NULL;
  this->streamLength = 0;
  this->setLenght = false;
  this->streamTime = 0;
  this->endStreamTime = 0;
  this->lockMutex = CreateMutex(NULL, FALSE, NULL);
  this->url = NULL;
  this->internalExitRequest = false;
  this->wholeStreamDownloaded = false;
  this->rangesSupported = RANGES_STATE_UNKNOWN;
  this->receivedDataFromStart = NULL;
  this->receivedDataFromRange = NULL;
  this->mainCurlInstance = NULL;
  this->rangesDetectionCurlInstance = NULL;
  this->filledReceivedDataFromStart = false;
  this->filledReceivedDataFromRange = false;

  this->logger->Log(LOGGER_INFO, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CONSTRUCTOR_NAME);
}

CMPUrlSource_Http::~CMPUrlSource_Http()
{
  this->logger->Log(LOGGER_INFO, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_DESTRUCTOR_NAME);

  if (this->IsConnected())
  {
    this->CloseConnection();
  }

  if (this->mainCurlInstance != NULL)
  {
    delete this->mainCurlInstance;
    this->mainCurlInstance = NULL;
  }

  if (this->rangesDetectionCurlInstance != NULL)
  {
    delete this->rangesDetectionCurlInstance;
    this->rangesDetectionCurlInstance = NULL;
  }
  
  delete this->configurationParameters;
  FREE_MEM(this->url);

  if (this->lockMutex != NULL)
  {
    CloseHandle(this->lockMutex);
    this->lockMutex = NULL;
  }

  this->logger->Log(LOGGER_INFO, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_DESTRUCTOR_NAME);

  delete this->logger;
  this->logger = NULL;
}

HRESULT CMPUrlSource_Http::ClearSession(void)
{
  this->logger->Log(LOGGER_INFO, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CLEAR_SESSION_NAME);

  if (this->IsConnected())
  {
    this->CloseConnection();
  }
 
  this->internalExitRequest = false;
  this->streamLength = 0;
  this->setLenght = false;
  this->streamTime = 0;
  this->endStreamTime = 0;
  FREE_MEM(this->url);
  this->wholeStreamDownloaded = false;
  this->rangesSupported = RANGES_STATE_UNKNOWN;
  this->filledReceivedDataFromStart = false;
  this->filledReceivedDataFromRange = false;
  this->receiveDataTimeout = HTTP_RECEIVE_DATA_TIMEOUT_DEFAULT;
  this->openConnetionMaximumAttempts = HTTP_OPEN_CONNECTION_MAXIMUM_ATTEMPTS_DEFAULT;

  if (this->receivedDataFromStart != NULL)
  {
    delete this->receivedDataFromStart;
    this->receivedDataFromStart = NULL;
  }

  if (this->receivedDataFromRange != NULL)
  {
    delete this->receivedDataFromRange;
    this->receivedDataFromRange = NULL;
  }

  this->logger->Log(LOGGER_INFO, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CLEAR_SESSION_NAME);
  return S_OK;
}

HRESULT CMPUrlSource_Http::Initialize(IOutputStream *filter, CParameterCollection *configuration)
{
  this->filter = filter;
  if (this->filter == NULL)
  {
    return E_POINTER;
  }

  if (this->lockMutex == NULL)
  {
    return E_FAIL;
  }

  if (configuration != NULL)
  {
    this->configurationParameters->Clear();
    this->configurationParameters->Append(configuration);
  }
  this->configurationParameters->LogCollection(this->logger, LOGGER_VERBOSE, PROTOCOL_IMPLEMENTATION_NAME, METHOD_INITIALIZE_NAME);

  this->receiveDataTimeout = this->configurationParameters->GetValueLong(PARAMETER_NAME_HTTP_RECEIVE_DATA_TIMEOUT, true, HTTP_RECEIVE_DATA_TIMEOUT_DEFAULT);
  this->openConnetionMaximumAttempts = this->configurationParameters->GetValueLong(PARAMETER_NAME_HTTP_OPEN_CONNECTION_MAXIMUM_ATTEMPTS, true, HTTP_OPEN_CONNECTION_MAXIMUM_ATTEMPTS_DEFAULT);

  this->receiveDataTimeout = (this->receiveDataTimeout < 0) ? HTTP_RECEIVE_DATA_TIMEOUT_DEFAULT : this->receiveDataTimeout;
  this->openConnetionMaximumAttempts = (this->openConnetionMaximumAttempts < 0) ? HTTP_OPEN_CONNECTION_MAXIMUM_ATTEMPTS_DEFAULT : this->openConnetionMaximumAttempts;

  return S_OK;
}

TCHAR *CMPUrlSource_Http::GetProtocolName(void)
{
  return Duplicate(PROTOCOL_NAME);
}

HRESULT CMPUrlSource_Http::ParseUrl(const TCHAR *url, const CParameterCollection *parameters)
{
  HRESULT result = S_OK;
  this->logger->Log(LOGGER_INFO, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME);

  this->ClearSession();
  if (parameters != NULL)
  {
    this->configurationParameters->Clear();
    this->Initialize(this->filter, (CParameterCollection *)parameters);
  }

  ALLOC_MEM_DEFINE_SET(urlComponents, URL_COMPONENTS, 1, 0);
  if (urlComponents == NULL)
  {
    this->logger->Log(LOGGER_ERROR, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, _T("cannot allocate memory for 'url components'"));
    result = E_OUTOFMEMORY;
  }

  if (result == S_OK)
  {
    ZeroURL(urlComponents);
    urlComponents->dwStructSize = sizeof(URL_COMPONENTS);

    this->logger->Log(LOGGER_INFO, _T("%s: %s: url: %s"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, url);

    if (!InternetCrackUrl(url, 0, 0, urlComponents))
    {
      this->logger->Log(LOGGER_ERROR, _T("%s: %s: InternetCrackUrl() error: %u"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, GetLastError());
      result = E_FAIL;
    }
  }

  if (result == S_OK)
  {
    int length = urlComponents->dwSchemeLength + 1;
    ALLOC_MEM_DEFINE_SET(protocol, TCHAR, length, 0);
    if (protocol == NULL) 
    {
      this->logger->Log(LOGGER_ERROR, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, _T("cannot allocate memory for 'protocol'"));
      result = E_OUTOFMEMORY;
    }

    if (result == S_OK)
    {
      _tcsncat_s(protocol, length, urlComponents->lpszScheme, urlComponents->dwSchemeLength);

      bool supportedProtocol = false;
      for (int i = 0; i < TOTAL_SUPPORTED_PROTOCOLS; i++)
      {
        if (_tcsncicmp(urlComponents->lpszScheme, SUPPORTED_PROTOCOLS[i], urlComponents->dwSchemeLength) == 0)
        {
          supportedProtocol = true;
          break;
        }
      }

      if (!supportedProtocol)
      {
        // not supported protocol
        this->logger->Log(LOGGER_INFO, _T("%s: %s: unsupported protocol '%s'"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, protocol);
        result = E_FAIL;
      }
    }
    FREE_MEM(protocol);

    if (result == S_OK)
    {
      this->url = Duplicate(url);
      if (this->url == NULL)
      {
        this->logger->Log(LOGGER_ERROR, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, _T("cannot allocate memory for 'url'"));
        result = E_OUTOFMEMORY;
      }
    }
  }

  FREE_MEM(urlComponents);

  this->logger->Log(LOGGER_INFO, SUCCEEDED(result) ? METHOD_END_FORMAT : METHOD_END_FAIL_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME);
  return result;
}

HRESULT CMPUrlSource_Http::OpenConnection(void)
{
  HRESULT result = S_OK;
  CHECK_POINTER_DEFAULT_HRESULT(result, this->url);
  this->logger->Log(LOGGER_INFO, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_OPEN_CONNECTION_NAME);

  // lock access to stream
  CLockMutex lock(this->lockMutex, INFINITE);

  this->wholeStreamDownloaded = false;

  if (result == S_OK)
  {
    this->mainCurlInstance = new CCurlInstance(this->logger, this->url, PROTOCOL_IMPLEMENTATION_NAME);
    result = (this->mainCurlInstance != NULL) ? S_OK : E_POINTER;
  }

  if (result == S_OK)
  {
    if (this->rangesSupported == RANGES_STATE_UNKNOWN)
    {
      // we don't know if ranges are supported
      this->receivedDataFromStart = new LinearBuffer();
      this->receivedDataFromRange = new LinearBuffer();

      result = (this->receivedDataFromStart == NULL) ? E_POINTER : result;
      result = (this->receivedDataFromRange == NULL) ? E_POINTER : result;

      if (result == S_OK)
      {
        result = (this->receivedDataFromStart->InitializeBuffer(RANGES_SUPPORTED_BUFFER_SIZE)) ? result : E_FAIL;
        result = (this->receivedDataFromRange->InitializeBuffer(RANGES_SUPPORTED_BUFFER_SIZE)) ? result : E_FAIL;
      }

      if (result == S_OK)
      {
        this->rangesDetectionCurlInstance = new CCurlInstance(this->logger, this->url, PROTOCOL_IMPLEMENTATION_NAME);
        result = (this->rangesDetectionCurlInstance != NULL) ? S_OK : E_POINTER;
      }

      if (result == S_OK)
      {
        // set ranges supported state to pending request (this will store data)
        this->rangesSupported = RANGES_STATE_PENDING_REQUEST;
      }
    }
  }

  if (result == S_OK)
  {
    this->mainCurlInstance->SetReceivedDataTimeout(this->receiveDataTimeout);
    this->mainCurlInstance->SetWriteCallback(CMPUrlSource_Http::CurlReceiveData, this);
    this->mainCurlInstance->SetStartStreamTime(this->streamTime);
    this->mainCurlInstance->SetEndStreamTime(this->endStreamTime);
    this->mainCurlInstance->SetReferer(this->configurationParameters->GetValue(PARAMETER_NAME_HTTP_REFERER, true, NULL));
    this->mainCurlInstance->SetUserAgent(this->configurationParameters->GetValue(PARAMETER_NAME_HTTP_USER_AGENT, true, NULL));
    this->mainCurlInstance->SetCookie(this->configurationParameters->GetValue(PARAMETER_NAME_HTTP_COOKIE, true, NULL));
    this->mainCurlInstance->SetHttpVersion(this->configurationParameters->GetValueLong(PARAMETER_NAME_HTTP_VERSION, true, HTTP_VERSION_DEFAULT));
    this->mainCurlInstance->SetIgnoreContentLength((this->configurationParameters->GetValueLong(PARAMETER_NAME_HTTP_IGNORE_CONTENT_LENGTH, true, HTTP_IGNORE_CONTENT_LENGTH_DEFAULT) == 1L));

    result = (this->mainCurlInstance->Initialize()) ? S_OK : E_FAIL;

    if (result == S_OK)
    {
      // all parameters set
      // start receiving data

      result = (this->mainCurlInstance->StartReceivingData()) ? S_OK : E_FAIL;
    }

    if (result == S_OK)
    {
      // wait for HTTP status code

      long responseCode = 0;
      while (responseCode == 0)
      {
        CURLcode errorCode = this->mainCurlInstance->GetResponseCode(&responseCode);
        if (errorCode == CURLE_OK)
        {
          if ((responseCode != 0) && ((responseCode < 200) || (responseCode >= 400)))
          {
            // response code 200 - 299 = OK
            // response code 300 - 399 = redirect (OK)
            this->logger->Log(LOGGER_VERBOSE, _T("%s: %s: HTTP status code: %u"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, responseCode);
            result = E_FAIL;
          }
        }
        else
        {
          this->mainCurlInstance->ReportCurlErrorMessage(LOGGER_WARNING, PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, _T("error while requesting HTTP status code"), errorCode);
          result = E_FAIL;
          break;
        }

        if ((responseCode == 0) && (this->mainCurlInstance->GetCurlState() == CURL_STATE_RECEIVED_ALL_DATA))
        {
          // we received data too fast
          result = E_FAIL;
          break;
        }

        // wait some time
        Sleep(1);
      }
    }
  }

  if (FAILED(result))
  {
    this->CloseConnection();
  }

  this->logger->Log(LOGGER_INFO, SUCCEEDED(result) ? METHOD_END_FORMAT : METHOD_END_FAIL_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_OPEN_CONNECTION_NAME);
  return result;
}

bool CMPUrlSource_Http::IsConnected(void)
{
  return ((this->mainCurlInstance != NULL) || (this->wholeStreamDownloaded));
}

void CMPUrlSource_Http::CloseConnection(void)
{
  this->logger->Log(LOGGER_INFO, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CLOSE_CONNECTION_NAME);

  // lock access to stream
  CLockMutex lock(this->lockMutex, INFINITE);

  if (this->mainCurlInstance != NULL)
  {
    delete this->mainCurlInstance;
    this->mainCurlInstance = NULL;
  }

  if (this->rangesDetectionCurlInstance != NULL)
  {
    delete this->rangesDetectionCurlInstance;
    this->rangesDetectionCurlInstance = NULL;
  }

  if (this->receivedDataFromStart != NULL)
  {
    delete this->receivedDataFromStart;
    this->receivedDataFromStart = NULL;
  }

  if (this->receivedDataFromRange != NULL)
  {
    delete this->receivedDataFromRange;
    this->receivedDataFromRange = NULL;
  }

  this->filledReceivedDataFromStart = false;
  this->filledReceivedDataFromRange = false;

  // reset ranges supported state only if ranges are not sure
  switch (this->rangesSupported)
  {
  case RANGES_STATE_UNKNOWN:
    break;
  case RANGES_STATE_NOT_SUPPORTED:
    // do not reset ranges state
    // CloseConnection() is called when processing ranges request
    break;
  case RANGES_STATE_PENDING_REQUEST:
    this->rangesSupported = RANGES_STATE_UNKNOWN;
    break;
  case RANGES_STATE_SUPPORTED:
    // do not reset ranges state
    // CloseConnection() is called when processing ranges request
    break;
  default:
    this->rangesSupported = RANGES_STATE_UNKNOWN;
    break;
  }

  this->logger->Log(LOGGER_INFO, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CLOSE_CONNECTION_NAME);
}

void CMPUrlSource_Http::ReceiveData(bool *shouldExit)
{
  this->logger->Log(LOGGER_DATA, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME);
  this->shouldExit = *shouldExit;

  CLockMutex lock(this->lockMutex, INFINITE);

  if (this->internalExitRequest)
  {
    // there is internal exit request pending == changed timestamp
    // close connection
    this->CloseConnection();

    // reopen connection
    // OpenConnection() reset wholeStreamDownloaded
    this->OpenConnection();

    this->internalExitRequest = false;
  }

  if (this->IsConnected())
  {
    if (!this->wholeStreamDownloaded)
    {
      if (this->mainCurlInstance->GetCurlState() == CURL_STATE_RECEIVED_ALL_DATA)
      {
        // all data received, we're not receiving data

        if (this->mainCurlInstance->GetErrorCode() == CURLE_OK)
        {
          // whole stream downloaded
          this->wholeStreamDownloaded = true;

          if (!this->setLenght)
          {
            this->streamLength = this->streamTime;
            this->logger->Log(LOGGER_VERBOSE, _T("%s: %s: setting total length: %u"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, this->streamLength);
            this->filter->SetTotalLength(OUTPUT_PIN_NAME, this->streamLength, false);
            this->setLenght = true;
          }

          // notify filter the we reached end of stream
          // EndOfStreamReached() can call ReceiveDataFromTimestamp() which can set this->streamTime
          REFERENCE_TIME streamTime = this->streamTime;
          this->streamTime = this->streamLength;
          this->filter->EndOfStreamReached(OUTPUT_PIN_NAME, max(0, streamTime - 1));
        }

        // connection is no longer needed
        this->CloseConnection();
      }
    }
  }
  else
  {
    this->logger->Log(LOGGER_WARNING, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, _T("connection closed, opening new one"));
    // re-open connection if previous is lost
    if (this->OpenConnection() != S_OK)
    {
      this->CloseConnection();
    }
  }

  this->logger->Log(LOGGER_DATA, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME);
}

unsigned int CMPUrlSource_Http::GetReceiveDataTimeout(void)
{
  return this->receiveDataTimeout;
}

GUID CMPUrlSource_Http::GetInstanceId(void)
{
  return this->logger->loggerInstance;
}

unsigned int CMPUrlSource_Http::GetOpenConnectionMaximumAttempts(void)
{
  return this->openConnetionMaximumAttempts;
}

CStringCollection *CMPUrlSource_Http::GetStreamNames(void)
{
  CStringCollection *streamNames = new CStringCollection();

  streamNames->Add(Duplicate(OUTPUT_PIN_NAME));

  return streamNames;
}

HRESULT CMPUrlSource_Http::ReceiveDataFromTimestamp(REFERENCE_TIME startTime, REFERENCE_TIME endTime)
{
  this->logger->Log(LOGGER_VERBOSE, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_FROM_TIMESTAMP_NAME);
  this->logger->Log(LOGGER_VERBOSE, _T("%s: %s: from time: %llu, to time: %llu"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_FROM_TIMESTAMP_NAME, startTime, endTime);

  HRESULT result = E_FAIL;

  // lock access to stream
  CLockMutex lock(this->lockMutex, INFINITE);

  if (startTime >= this->streamLength)
  {
    result = E_INVALIDARG;
  }
  else if (this->internalExitRequest)
  {
    // there is pending request exit request
    // set stream time to new value
    this->streamTime = startTime;
    this->endStreamTime = endTime;

    // connection should be reopened automatically
    result = S_OK;
  }
  else
  {
    // only way how to "request" curl to interrupt transfer is set internalExitRequest to true
    this->internalExitRequest = true;

    // set stream time to new value
    this->streamTime = startTime;
    this->endStreamTime = endTime;

    // connection should be reopened automatically
    result = S_OK;
  }

  this->logger->Log(LOGGER_VERBOSE, METHOD_END_HRESULT_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_FROM_TIMESTAMP_NAME, result);
  return result;
}

HRESULT CMPUrlSource_Http::AbortStreamReceive()
{
  this->logger->Log(LOGGER_VERBOSE, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_ABORT_STREAM_RECEIVE_NAME);
  CLockMutex lock(this->lockMutex, INFINITE);

  // close connection and set that whole stream downloaded
  this->CloseConnection();
  this->wholeStreamDownloaded = true;

  this->logger->Log(LOGGER_VERBOSE, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_ABORT_STREAM_RECEIVE_NAME);
  return S_OK;
}

HRESULT CMPUrlSource_Http::QueryStreamProgress(LONGLONG *total, LONGLONG *current)
{
  this->logger->Log(LOGGER_DATA, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_QUERY_STREAM_PROGRESS_NAME);

  HRESULT result = S_OK;
  CHECK_POINTER_DEFAULT_HRESULT(result, total);
  CHECK_POINTER_DEFAULT_HRESULT(result, current);

  if (result == S_OK)
  {
    *total = this->streamLength;
    *current = this->streamTime;
  }

  this->logger->Log(LOGGER_DATA, (SUCCEEDED(result)) ? METHOD_END_HRESULT_FORMAT : METHOD_END_FAIL_HRESULT_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_QUERY_STREAM_PROGRESS_NAME, result);
  return result;
}

HRESULT CMPUrlSource_Http::QueryStreamAvailableLength(CStreamAvailableLength *availableLength)
{
  HRESULT result = S_OK;
  CHECK_POINTER_DEFAULT_HRESULT(result, availableLength);

  if (result == S_OK)
  {
    availableLength->SetQueryResult(S_OK);
    availableLength->SetAvailableLength((availableLength->IsFilterConnectedToAnotherPin() && (this->rangesSupported == RANGES_STATE_SUPPORTED)) ? this->streamLength : this->streamTime);
  }

  return result;
}

HRESULT CMPUrlSource_Http::QueryRangesSupported(CRangesSupported *rangesSupported)
{
  HRESULT result = S_OK;
  CHECK_POINTER_DEFAULT_HRESULT(result, rangesSupported);

  if (result == S_OK)
  {
    if (rangesSupported->IsFilterConnectedToAnotherPin())
    {
      rangesSupported->SetQueryResult(S_OK);
      switch (this->rangesSupported)
      {
      case RANGES_STATE_UNKNOWN:
        rangesSupported->SetRangesSupported(false);
        break;
      case RANGES_STATE_NOT_SUPPORTED:
        rangesSupported->SetRangesSupported(false);
        break;
      case RANGES_STATE_PENDING_REQUEST:
        rangesSupported->SetQueryResult(E_PENDING);
        rangesSupported->SetRangesSupported(false);
        break;
      case RANGES_STATE_SUPPORTED:
        rangesSupported->SetRangesSupported(true);
        break;
      default:
        rangesSupported->SetRangesSupported(false);
        break;
      }
    }
    else
    {
      // we are not connected to another pin, assume that ranges are not supported (it makes connection to another filter more faster)
      // in the case on web streams we can assume that we don't need data from end of stream
      rangesSupported->SetQueryResult(S_OK);
      rangesSupported->SetRangesSupported(false);
    }
  }

  return result;
}

size_t CMPUrlSource_Http::CurlReceiveData(char *buffer, size_t size, size_t nmemb, void *userdata)
{
  CMPUrlSource_Http *caller = (CMPUrlSource_Http *)userdata;
  CLockMutex lock(caller->lockMutex, INFINITE);
  unsigned int bytesRead = size * nmemb;

  if (!((caller->shouldExit) || (caller->internalExitRequest)))
  {
    long responseCode = 0;
    CURLcode errorCode = caller->mainCurlInstance->GetResponseCode(&responseCode);
    if (errorCode == CURLE_OK)
    {
      if ((responseCode < 200) && (responseCode >= 400))
      {
        // response code 200 - 299 = OK
        // response code 300 - 399 = redirect (OK)
        caller->logger->Log(LOGGER_VERBOSE, _T("%s: %s: error response code: %u"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, responseCode);
        // return error
        bytesRead = 0;
      }
    }

    if ((responseCode >= 200) && (responseCode < 400))
    {
      if (!caller->setLenght)
      {
        double streamSize = 0;
        CURLcode errorCode = curl_easy_getinfo(caller->mainCurlInstance->GetCurlHandle(), CURLINFO_CONTENT_LENGTH_DOWNLOAD, &streamSize);
        if ((errorCode == CURLE_OK) && (streamSize > 0) && (caller->streamTime < streamSize))
        {
          LONGLONG total = LONGLONG(streamSize);
          caller->streamLength = total;
          caller->logger->Log(LOGGER_VERBOSE, _T("%s: %s: setting total length: %u"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, total);
          caller->filter->SetTotalLength(OUTPUT_PIN_NAME, total, false);
          caller->setLenght = true;
        }
        else
        {
          if (caller->streamLength == 0)
          {
            // stream length not set
            // just make guess
            unsigned int bufferingPercentage = caller->configurationParameters->GetValueLong(PARAMETER_NAME_BUFFERING_PERCENTAGE, true, BUFFERING_PERCENTAGE_DEFAULT);
            caller->streamLength = LONGLONG(MINIMUM_RECEIVED_DATA_FOR_SPLITTER * 100 / bufferingPercentage);
            caller->logger->Log(LOGGER_VERBOSE, _T("%s: %s: setting total length: %u"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, caller->streamLength);
            caller->filter->SetTotalLength(OUTPUT_PIN_NAME, caller->streamLength, false);
          }
          else if ((caller->streamTime > (caller->streamLength * 3 / 4)))
          {
            // it is time to adjust stream length, we are approaching to end but still we don't know total length
            caller->streamLength = caller->streamTime * 2;
            caller->logger->Log(LOGGER_VERBOSE, _T("%s: %s: setting total length: %u"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, caller->streamLength);
            caller->filter->SetTotalLength(OUTPUT_PIN_NAME, caller->streamLength, false);
          }
        }
      }

      if (bytesRead != 0)
      {
        if (caller->rangesDetectionCurlInstance != NULL)
        {
          if (caller->rangesDetectionCurlInstance->GetCurlState() == CURL_STATE_CREATED)
          {
            // ranges detection wasn't initialized and started
            caller->rangesDetectionCurlInstance->SetReceivedDataTimeout(caller->receiveDataTimeout);
            caller->rangesDetectionCurlInstance->SetWriteCallback(CMPUrlSource_Http::CurlRangesDetectionReceiveData, caller);
            caller->rangesDetectionCurlInstance->SetStartStreamTime(caller->streamLength / 2);
            caller->rangesDetectionCurlInstance->SetEndStreamTime(caller->streamLength / 2);
            caller->rangesDetectionCurlInstance->SetReferer(caller->configurationParameters->GetValue(PARAMETER_NAME_HTTP_REFERER, true, NULL));
            caller->rangesDetectionCurlInstance->SetUserAgent(caller->configurationParameters->GetValue(PARAMETER_NAME_HTTP_USER_AGENT, true, NULL));
            caller->rangesDetectionCurlInstance->SetCookie(caller->configurationParameters->GetValue(PARAMETER_NAME_HTTP_COOKIE, true, NULL));
            caller->rangesDetectionCurlInstance->SetHttpVersion(caller->configurationParameters->GetValueLong(PARAMETER_NAME_HTTP_VERSION, true, HTTP_VERSION_DEFAULT));
            caller->rangesDetectionCurlInstance->SetIgnoreContentLength((caller->configurationParameters->GetValueLong(PARAMETER_NAME_HTTP_IGNORE_CONTENT_LENGTH, true, HTTP_IGNORE_CONTENT_LENGTH_DEFAULT) == 1L));

            if (caller->rangesDetectionCurlInstance->Initialize())
            {
              caller->rangesDetectionCurlInstance->StartReceivingData();
            }
          }
        }

        if (caller->rangesSupported == RANGES_STATE_PENDING_REQUEST)
        {
          // there is pending request if ranges are supported or not          

          if ((!caller->filledReceivedDataFromStart) && (caller->receivedDataFromStart->AddToBuffer(buffer, min(bytesRead, caller->receivedDataFromStart->GetBufferFreeSpace())) != bytesRead))
          {
            caller->filledReceivedDataFromStart = true;
            // data wasn't added to buffer
            // compare buffers and set ranges supported state

            caller->CompareRangesBuffers();
          }
        }

        // create media packet
        // set values of media packet
        CMediaPacket *mediaPacket = new CMediaPacket();
        mediaPacket->GetBuffer()->InitializeBuffer(bytesRead);
        mediaPacket->GetBuffer()->AddToBuffer(buffer, bytesRead);

        REFERENCE_TIME timeEnd = caller->streamTime + bytesRead - 1;
        HRESULT result = mediaPacket->SetTime(&caller->streamTime, &timeEnd);
        if (result != S_OK)
        {
          caller->logger->Log(LOGGER_WARNING, _T("%s: %s: stream time not set, error: 0x%08X"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, result);
        }

        if (FAILED(caller->filter->PushMediaPacket(OUTPUT_PIN_NAME, mediaPacket)))
        {
          caller->logger->Log(LOGGER_WARNING, _T("%s: %s: error occured while adding media packet"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME);
        }
        caller->streamTime += bytesRead;
      }
    }
  }

  // if returned 0 (or lower value than bytesRead) it cause transfer interruption
  return ((caller->shouldExit) || (caller->internalExitRequest)) ? 0 : (bytesRead);
}

size_t CMPUrlSource_Http::CurlRangesDetectionReceiveData(char *buffer, size_t size, size_t nmemb, void *userdata)
{
  CMPUrlSource_Http *caller = (CMPUrlSource_Http *)userdata;
  CLockMutex lock(caller->lockMutex, INFINITE);
  unsigned int bytesRead = size * nmemb;

  if (!((caller->shouldExit) || (caller->internalExitRequest)))
  {
    long responseCode = 0;
    CURLcode errorCode = caller->rangesDetectionCurlInstance->GetResponseCode(&responseCode);
    if (errorCode == CURLE_OK)
    {
      if ((responseCode < 200) && (responseCode >= 400))
      {
        // response code 200 - 299 = OK
        // response code 300 - 399 = redirect (OK)
        caller->logger->Log(LOGGER_VERBOSE, _T("%s: %s: ranges detection error response code: %u"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, responseCode);
        // return error
        bytesRead = 0;
      }
    }

    if ((responseCode >= 200) && (responseCode < 400))
    {
      if (bytesRead != 0)
      {
        if (caller->rangesSupported == RANGES_STATE_PENDING_REQUEST)
        {
          // there is pending request if ranges are supported or not

          if ((!caller->filledReceivedDataFromRange) && (caller->receivedDataFromRange->AddToBuffer(buffer, min(bytesRead, caller->receivedDataFromRange->GetBufferFreeSpace())) != bytesRead))
          {
            caller->filledReceivedDataFromRange = true;
            // data wasn't added to buffer
            // compare buffers and set ranges supported state

            caller->CompareRangesBuffers();
          }
        }

        if ((caller->rangesSupported == RANGES_STATE_NOT_SUPPORTED) || (caller->rangesSupported == RANGES_STATE_SUPPORTED))
        {
          // stop receiving data from ranges detection
          bytesRead = 0;
        }
      }
    }
  }

  // if returned 0 (or lower value than bytesRead) it cause transfer interruption
  return ((caller->shouldExit) || (caller->internalExitRequest)) ? 0 : (bytesRead);
}

void CMPUrlSource_Http::CompareRangesBuffers()
{
  this->logger->Log(LOGGER_INFO, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_COMPARE_RANGES_BUFFERS_NAME);

  if (this->rangesSupported == RANGES_STATE_PENDING_REQUEST)
  {
    // there is pending request if ranges are supported or not

    this->logger->Log(LOGGER_VERBOSE, _T("%s: %s: received data from start: %u, received data from ranges: %u"), PROTOCOL_IMPLEMENTATION_NAME, METHOD_COMPARE_RANGES_BUFFERS_NAME, this->receivedDataFromStart->GetBufferOccupiedSpace(), this->receivedDataFromRange->GetBufferOccupiedSpace());

    unsigned int size = min(this->receivedDataFromStart->GetBufferOccupiedSpace(), this->receivedDataFromRange->GetBufferOccupiedSpace());

    if (size == this->receivedDataFromStart->GetBufferSize())
    {
      ALLOC_MEM_DEFINE_SET(bufferFromStart, char, size, 0);
      ALLOC_MEM_DEFINE_SET(bufferFromRange, char, size, 0);

      if ((bufferFromStart != NULL) && (bufferFromRange != NULL))
      {
        if (this->receivedDataFromStart->CopyFromBuffer(bufferFromStart, size, 0, 0) == size)
        {
          if (this->receivedDataFromRange->CopyFromBuffer(bufferFromRange, size, 0, 0) == size)
          {
            if (memcmp(bufferFromStart, bufferFromRange, size) != 0)
            {
              // buffers are not same => ranges are supported
              this->logger->Log(LOGGER_INFO, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_COMPARE_RANGES_BUFFERS_NAME, _T("ranges are supported"));
              this->rangesSupported = RANGES_STATE_SUPPORTED;
            }
            else
            {
              this->logger->Log(LOGGER_WARNING, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_COMPARE_RANGES_BUFFERS_NAME, _T("range buffers are same, ranges are not supported"));
              this->rangesSupported = RANGES_STATE_NOT_SUPPORTED;
            }
          }
          else
          {
            this->logger->Log(LOGGER_WARNING, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_COMPARE_RANGES_BUFFERS_NAME, _T("cannot copy data for comparing buffers, ranges are not supported"));
            this->rangesSupported = RANGES_STATE_NOT_SUPPORTED;
          }
        }
        else
        {
          this->logger->Log(LOGGER_WARNING, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_COMPARE_RANGES_BUFFERS_NAME, _T("cannot copy data for comparing buffers, ranges are not supported"));
          this->rangesSupported = RANGES_STATE_NOT_SUPPORTED;
        }
      }
      else
      {
        this->logger->Log(LOGGER_WARNING, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_COMPARE_RANGES_BUFFERS_NAME, _T("cannot allocate enough memory for comparing buffers, ranges are not supported"));
        this->rangesSupported = RANGES_STATE_NOT_SUPPORTED;
      }

      FREE_MEM(bufferFromStart);
      FREE_MEM(bufferFromRange);
    }
  }

  this->logger->Log(LOGGER_INFO, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_COMPARE_RANGES_BUFFERS_NAME);
}

