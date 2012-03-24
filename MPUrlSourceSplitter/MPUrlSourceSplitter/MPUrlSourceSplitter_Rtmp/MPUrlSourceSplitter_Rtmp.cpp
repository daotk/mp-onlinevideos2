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

#include "MPUrlSourceSplitter_RTMP.h"
#include "Utilities.h"
#include "LockMutex.h"
#include "FlvPacket.h"

#include <WinInet.h>
#include <stdio.h>

// protocol implementation name
#ifdef _DEBUG
#define PROTOCOL_IMPLEMENTATION_NAME                                    L"MPUrlSourceSplitter_Rtmpd"
#else
#define PROTOCOL_IMPLEMENTATION_NAME                                    L"MPUrlSourceSplitter_Rtmp"
#endif

PIProtocol CreateProtocolInstance(CParameterCollection *configuration)
{
  return new CMPUrlSourceSplitter_Rtmp(configuration);
}

void DestroyProtocolInstance(PIProtocol pProtocol)
{
  if (pProtocol != NULL)
  {
    CMPUrlSourceSplitter_Rtmp *pClass = (CMPUrlSourceSplitter_Rtmp *)pProtocol;
    delete pClass;
  }
}

CMPUrlSourceSplitter_Rtmp::CMPUrlSourceSplitter_Rtmp(CParameterCollection *configuration)
{
  this->configurationParameters = new CParameterCollection();
  if (configuration != NULL)
  {
    this->configurationParameters->Append(configuration);
  }

  this->logger = new CLogger(this->configurationParameters);
  this->logger->Log(LOGGER_INFO, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CONSTRUCTOR_NAME);
  
  this->receiveDataTimeout = RTMP_RECEIVE_DATA_TIMEOUT_DEFAULT;
  this->openConnetionMaximumAttempts = RTMP_OPEN_CONNECTION_MAXIMUM_ATTEMPTS_DEFAULT;
  this->filter = NULL;
  this->streamLength = 0;
  this->setLenght = false;
  this->streamTime = 0;
  this->lockMutex = CreateMutex(NULL, FALSE, NULL);
  this->url = NULL;
  this->internalExitRequest = false;
  this->wholeStreamDownloaded = false;
  this->mainCurlInstance = NULL;
  this->streamDuration = 0;
  this->bytePosition = 0;
  this->seekingActive = false;
  this->supressData = false;
  this->bufferForProcessing = NULL;

  this->logger->Log(LOGGER_INFO, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CONSTRUCTOR_NAME);
}

CMPUrlSourceSplitter_Rtmp::~CMPUrlSourceSplitter_Rtmp()
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

  if (this->bufferForProcessing != NULL)
  {
    delete this->bufferForProcessing;
    this->bufferForProcessing = NULL;
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

HRESULT CMPUrlSourceSplitter_Rtmp::ClearSession(void)
{
  this->logger->Log(LOGGER_INFO, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CLEAR_SESSION_NAME);

  if (this->IsConnected())
  {
    this->CloseConnection();
  }

  if (this->bufferForProcessing != NULL)
  {
    delete this->bufferForProcessing;
    this->bufferForProcessing = NULL;
  }
 
  this->internalExitRequest = false;
  this->streamLength = 0;
  this->setLenght = false;
  this->streamTime = 0;
  FREE_MEM(this->url);
  this->wholeStreamDownloaded = false;
  this->receiveDataTimeout = RTMP_RECEIVE_DATA_TIMEOUT_DEFAULT;
  this->openConnetionMaximumAttempts = RTMP_OPEN_CONNECTION_MAXIMUM_ATTEMPTS_DEFAULT;
  this->streamDuration = 0;

  this->logger->Log(LOGGER_INFO, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CLEAR_SESSION_NAME);
  return S_OK;
}

HRESULT CMPUrlSourceSplitter_Rtmp::Initialize(IOutputStream *filter, CParameterCollection *configuration)
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

  this->receiveDataTimeout = this->configurationParameters->GetValueLong(PARAMETER_NAME_RTMP_RECEIVE_DATA_TIMEOUT, true, RTMP_RECEIVE_DATA_TIMEOUT_DEFAULT);
  this->openConnetionMaximumAttempts = this->configurationParameters->GetValueLong(PARAMETER_NAME_RTMP_OPEN_CONNECTION_MAXIMUM_ATTEMPTS, true, RTMP_OPEN_CONNECTION_MAXIMUM_ATTEMPTS_DEFAULT);

  this->receiveDataTimeout = (this->receiveDataTimeout < 0) ? RTMP_RECEIVE_DATA_TIMEOUT_DEFAULT : this->receiveDataTimeout;
  this->openConnetionMaximumAttempts = (this->openConnetionMaximumAttempts < 0) ? RTMP_OPEN_CONNECTION_MAXIMUM_ATTEMPTS_DEFAULT : this->openConnetionMaximumAttempts;

  return S_OK;
}

wchar_t *CMPUrlSourceSplitter_Rtmp::GetProtocolName(void)
{
  return Duplicate(PROTOCOL_NAME);
}

HRESULT CMPUrlSourceSplitter_Rtmp::ParseUrl(const wchar_t *url, const CParameterCollection *parameters)
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
    this->logger->Log(LOGGER_ERROR, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, L"cannot allocate memory for 'url components'");
    result = E_OUTOFMEMORY;
  }

  if (result == S_OK)
  {
    ZeroURL(urlComponents);
    urlComponents->dwStructSize = sizeof(URL_COMPONENTS);

    this->logger->Log(LOGGER_INFO, L"%s: %s: url: %s", PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, url);

    if (!InternetCrackUrl(url, 0, 0, urlComponents))
    {
      this->logger->Log(LOGGER_ERROR, L"%s: %s: InternetCrackUrl() error: %u", PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, GetLastError());
      result = E_FAIL;
    }
  }

  if (result == S_OK)
  {
    int length = urlComponents->dwSchemeLength + 1;
    ALLOC_MEM_DEFINE_SET(protocol, wchar_t, length, 0);
    if (protocol == NULL) 
    {
      this->logger->Log(LOGGER_ERROR, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, L"cannot allocate memory for 'protocol'");
      result = E_OUTOFMEMORY;
    }

    if (result == S_OK)
    {
      wcsncat_s(protocol, length, urlComponents->lpszScheme, urlComponents->dwSchemeLength);

      bool supportedProtocol = false;
      for (int i = 0; i < TOTAL_SUPPORTED_PROTOCOLS; i++)
      {
        if (_wcsnicmp(urlComponents->lpszScheme, SUPPORTED_PROTOCOLS[i], urlComponents->dwSchemeLength) == 0)
        {
          supportedProtocol = true;
          break;
        }
      }

      if (!supportedProtocol)
      {
        // not supported protocol
        this->logger->Log(LOGGER_INFO, L"%s: %s: unsupported protocol '%s'", PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, protocol);
        result = E_FAIL;
      }
    }
    FREE_MEM(protocol);

    if (result == S_OK)
    {
      this->url = Duplicate(url);
      if (this->url == NULL)
      {
        this->logger->Log(LOGGER_ERROR, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME, L"cannot allocate memory for 'url'");
        result = E_OUTOFMEMORY;
      }
    }
  }

  FREE_MEM(urlComponents);

  this->logger->Log(LOGGER_INFO, SUCCEEDED(result) ? METHOD_END_FORMAT : METHOD_END_FAIL_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_PARSE_URL_NAME);
  return result;
}

HRESULT CMPUrlSourceSplitter_Rtmp::OpenConnection(void)
{
  HRESULT result = S_OK;
  CHECK_POINTER_DEFAULT_HRESULT(result, this->url);
  this->logger->Log(LOGGER_INFO, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_OPEN_CONNECTION_NAME);

  // lock access to stream
  CLockMutex lock(this->lockMutex, INFINITE);

  this->wholeStreamDownloaded = false;
  this->firstTimestamp = -1;
  this->firstVideoTimestamp = -1;

  if (result == S_OK)
  {
    this->mainCurlInstance = new CCurlInstance(this->logger, this->url, PROTOCOL_IMPLEMENTATION_NAME);
    result = (this->mainCurlInstance != NULL) ? S_OK : E_POINTER;
  }

  if (SUCCEEDED(result) && (this->bufferForProcessing == NULL))
  {
    this->bufferForProcessing = new LinearBuffer();
    result = (this->bufferForProcessing != NULL) ? S_OK : E_OUTOFMEMORY;

    if (SUCCEEDED(result))
    {
      result = this->bufferForProcessing->InitializeBuffer(BUFFER_FOR_PROCESSING_SIZE_DEFAULT, 0) ? S_OK : E_OUTOFMEMORY;
    }
  }

  if (result == S_OK)
  {
    this->mainCurlInstance->SetReceivedDataTimeout(this->receiveDataTimeout);
    this->mainCurlInstance->SetWriteCallback(CMPUrlSourceSplitter_Rtmp::CurlReceiveData, this);

    this->mainCurlInstance->SetRtmpApp(this->configurationParameters->GetValue(PARAMETER_NAME_RTMP_APP, true, RTMP_APP_DEFAULT));
    this->mainCurlInstance->SetRtmpArbitraryData(this->configurationParameters->GetValue(PARAMETER_NAME_RTMP_ARBITRARY_DATA, true, NULL));
    this->mainCurlInstance->SetRtmpBuffer(this->configurationParameters->GetValueUnsignedInt(PARAMETER_NAME_RTMP_BUFFER, true, RTMP_BUFFER_DEFAULT));
    this->mainCurlInstance->SetRtmpFlashVersion(this->configurationParameters->GetValue(PARAMETER_NAME_RTMP_FLASHVER, true, RTMP_FLASH_VER_DEFAULT));
    this->mainCurlInstance->SetRtmpAuth(this->configurationParameters->GetValue(PARAMETER_NAME_RTMP_AUTH, true, RTMP_AUTH_DEFAULT));
    this->mainCurlInstance->SetRtmpJtv(this->configurationParameters->GetValue(PARAMETER_NAME_RTMP_JTV, true, RTMP_JTV_DEFAULT));
    this->mainCurlInstance->SetRtmpLive(this->configurationParameters->GetValueBool(PARAMETER_NAME_RTMP_LIVE, true, RTMP_LIVE_DEFAULT));
    this->mainCurlInstance->SetRtmpPageUrl(this->configurationParameters->GetValue(PARAMETER_NAME_RTMP_PAGE_URL, true, RTMP_PAGE_URL_DEFAULT));
    this->mainCurlInstance->SetRtmpPlaylist(this->configurationParameters->GetValueBool(PARAMETER_NAME_RTMP_PLAYLIST, true, RTMP_PLAYLIST_DEFAULT));
    this->mainCurlInstance->SetRtmpPlayPath(this->configurationParameters->GetValue(PARAMETER_NAME_RTMP_PLAY_PATH, true, RTMP_PLAY_PATH_DEFAULT));
    this->mainCurlInstance->SetRtmpStart((this->streamTime >= 0) ? this->streamTime : this->configurationParameters->GetValueInt64(PARAMETER_NAME_RTMP_START, true, RTMP_START_DEFAULT));
    this->mainCurlInstance->SetRtmpStop(this->configurationParameters->GetValueInt64(PARAMETER_NAME_RTMP_STOP, true, RTMP_STOP_DEFAULT));
    this->mainCurlInstance->SetRtmpSubscribe(this->configurationParameters->GetValue(PARAMETER_NAME_RTMP_SUBSCRIBE, true, RTMP_SUBSCRIBE_DEFAULT));
    this->mainCurlInstance->SetRtmpSwfAge(this->configurationParameters->GetValueUnsignedInt(PARAMETER_NAME_RTMP_SWF_AGE, true, RTMP_SWF_AGE_DEFAULT));
    this->mainCurlInstance->SetRtmpSwfUrl(this->configurationParameters->GetValue(PARAMETER_NAME_RTMP_SWF_URL, true, RTMP_SWF_URL_DEFAULT));
    this->mainCurlInstance->SetRtmpSwfVerify(this->configurationParameters->GetValueBool(PARAMETER_NAME_RTMP_SWF_VERIFY, true, RTMP_SWF_VERIFY_DEFAULT));
    this->mainCurlInstance->SetRtmpTcUrl(this->configurationParameters->GetValue(PARAMETER_NAME_RTMP_TC_URL, true, RTMP_TC_URL_DEFAULT));
    this->mainCurlInstance->SetRtmpToken(this->configurationParameters->GetValue(PARAMETER_NAME_RTMP_TOKEN, true, RTMP_TOKEN_DEFAULT));

    result = (this->mainCurlInstance->Initialize()) ? S_OK : E_FAIL;

    if (result == S_OK)
    {
      // all parameters set
      // start receiving data

      result = (this->mainCurlInstance->StartReceivingData()) ? S_OK : E_FAIL;
    }

    if (result == S_OK)
    {
      // wait until we receive some data or transfer end (whatever comes first)
      unsigned int state = CURL_STATE_NONE;
      while ((state != CURL_STATE_RECEIVING_DATA) && (state != CURL_STATE_RECEIVED_ALL_DATA))
      {
        state = this->mainCurlInstance->GetCurlState();

        // wait some time
        Sleep(1);
      }

      if (state == CURL_STATE_RECEIVED_ALL_DATA)
      {
        // we received data too fast
        result = E_FAIL;
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

bool CMPUrlSourceSplitter_Rtmp::IsConnected(void)
{
  return ((this->mainCurlInstance != NULL) || (this->wholeStreamDownloaded));
}

void CMPUrlSourceSplitter_Rtmp::CloseConnection(void)
{
  this->logger->Log(LOGGER_INFO, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CLOSE_CONNECTION_NAME);

  // lock access to stream
  CLockMutex lock(this->lockMutex, INFINITE);

  if (this->mainCurlInstance != NULL)
  {
    this->mainCurlInstance->SetCloseWithoutWaiting(this->seekingActive);
    delete this->mainCurlInstance;
    this->mainCurlInstance = NULL;
  }

  if (this->bufferForProcessing != NULL)
  {
    delete this->bufferForProcessing;
    this->bufferForProcessing = NULL;
  }

  this->logger->Log(LOGGER_INFO, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_CLOSE_CONNECTION_NAME);
}

void CMPUrlSourceSplitter_Rtmp::ReceiveData(bool *shouldExit)
{
  this->logger->Log(LOGGER_DATA, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME);
  this->shouldExit = *shouldExit;

  CLockMutex lock(this->lockMutex, INFINITE);

  if (this->internalExitRequest)
  {
    // there is internal exit request pending == changed timestamp

    if (this->mainCurlInstance != NULL)
    {
      this->mainCurlInstance->SetCloseWithoutWaiting(true);
    }

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
      if ((!this->supressData) && (this->bufferForProcessing != NULL))
      {
        FlvPacket *flvPacket = new FlvPacket();
        if (flvPacket != NULL)
        {
          while (flvPacket->ParsePacket(this->bufferForProcessing))
          {
            // FLV packet parsed correctly
            // push FLV packet to filter

            if ((flvPacket->GetType() != FLV_PACKET_HEADER) && (this->firstTimestamp == (-1)))
            {
              this->firstTimestamp = flvPacket->GetTimestamp();
              this->logger->Log(LOGGER_VERBOSE, L"%s: %s: set first timestamp: %d", PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, this->firstTimestamp);
            }

            if ((flvPacket->GetType() == FLV_PACKET_VIDEO) && (this->firstVideoTimestamp == (-1)))
            {
              this->firstVideoTimestamp = flvPacket->GetTimestamp();
              this->logger->Log(LOGGER_VERBOSE, L"%s: %s: set first video timestamp: %d", PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, this->firstVideoTimestamp);
            }

            if ((flvPacket->GetType() == FLV_PACKET_VIDEO) && (this->firstVideoTimestamp != (-1)) && (this->firstTimestamp != (-1)))
            {
              // correction of video timestamps
              flvPacket->SetTimestamp(flvPacket->GetTimestamp() + this->firstTimestamp - this->firstVideoTimestamp);
            }

            if ((flvPacket->GetType() == FLV_PACKET_AUDIO) ||
              (flvPacket->GetType() == FLV_PACKET_HEADER) ||
              (flvPacket->GetType() == FLV_PACKET_META) ||
              (flvPacket->GetType() == FLV_PACKET_VIDEO))
            {
            }
            else
            {
              this->logger->Log(LOGGER_WARNING, L"%s: %s: unknown FLV packet: %d, size: %d", PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, flvPacket->GetType(), flvPacket->GetSize());
            }

            if ((flvPacket->GetType() != FLV_PACKET_HEADER) || (!this->seekingActive))
            {
              // create media packet
              // set values of media packet
              CMediaPacket *mediaPacket = new CMediaPacket();
              mediaPacket->GetBuffer()->InitializeBuffer(flvPacket->GetSize());
              mediaPacket->GetBuffer()->AddToBuffer(flvPacket->GetData(), flvPacket->GetSize());
              mediaPacket->SetStart(this->bytePosition);
              mediaPacket->SetEnd(this->bytePosition + flvPacket->GetSize() - 1);

              this->filter->PushMediaPacket(mediaPacket);
              this->bytePosition += flvPacket->GetSize();
            }
            // we are definitely not seeking
            this->seekingActive = false;
            this->bufferForProcessing->RemoveFromBufferAndMove(flvPacket->GetSize());

            flvPacket->Clear();
          }

          delete flvPacket;
          flvPacket = NULL;
        }
      }

      if (this->mainCurlInstance->GetCurlState() == CURL_STATE_RECEIVED_ALL_DATA)
      {
        // all data received, we're not receiving data

        if (this->mainCurlInstance->GetErrorCode() == CURLE_OK)
        {
          // whole stream downloaded
          this->wholeStreamDownloaded = true;

          if (this->streamTime == 0)
          {
            // if stream time is zero than we receive data from beginning
            // in that case we can set total length and call EndOfStreamReached() method (required for ending download)
            if (!this->setLenght)
            {
              this->streamLength = this->bytePosition;
              this->logger->Log(LOGGER_VERBOSE, L"%s: %s: setting total length: %u", PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, this->streamLength);
              this->filter->SetTotalLength(this->streamLength, false);
              this->setLenght = true;
            }

            // notify filter the we reached end of stream
            // EndOfStreamReached() can call ReceiveDataFromTimestamp() which can set this->streamTime
            this->filter->EndOfStreamReached(max(0, this->bytePosition - 1));
          }
          else
          {
            // set byte position to start of buffer, because we are re-opening connection
            // because we don't know exact time position to seek, we seek to last known (this->streamTime)
            this->bytePosition = 0;
          }
        }

        // connection is no longer needed
        this->CloseConnection();
      }
    }
  }
  else
  {
    this->logger->Log(LOGGER_WARNING, METHOD_MESSAGE_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, L"connection closed, opening new one");
    // re-open connection if previous is lost
    if (this->OpenConnection() != S_OK)
    {
      this->CloseConnection();
    }
  }

  this->logger->Log(LOGGER_DATA, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME);
}

unsigned int CMPUrlSourceSplitter_Rtmp::GetReceiveDataTimeout(void)
{
  return this->receiveDataTimeout;
}

GUID CMPUrlSourceSplitter_Rtmp::GetInstanceId(void)
{
  return this->logger->loggerInstance;
}

unsigned int CMPUrlSourceSplitter_Rtmp::GetOpenConnectionMaximumAttempts(void)
{
  return this->openConnetionMaximumAttempts;
}

int64_t CMPUrlSourceSplitter_Rtmp::SeekToPosition(int64_t start, int64_t end)
{
  this->logger->Log(LOGGER_VERBOSE, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_SEEK_TO_POSITION_NAME);
  this->logger->Log(LOGGER_VERBOSE, L"%s: %s: from time: %llu, to time: %llu", PROTOCOL_IMPLEMENTATION_NAME, METHOD_SEEK_TO_POSITION_NAME, start, end);

  int64_t result = -1;

  this->logger->Log(LOGGER_VERBOSE, METHOD_END_INT64_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_SEEK_TO_POSITION_NAME, result);
  return result;
}

HRESULT CMPUrlSourceSplitter_Rtmp::AbortStreamReceive()
{
  this->logger->Log(LOGGER_VERBOSE, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_ABORT_STREAM_RECEIVE_NAME);
  CLockMutex lock(this->lockMutex, INFINITE);

  // close connection and set that whole stream downloaded
  this->CloseConnection();
  this->wholeStreamDownloaded = true;

  this->logger->Log(LOGGER_VERBOSE, METHOD_END_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_ABORT_STREAM_RECEIVE_NAME);
  return S_OK;
}

HRESULT CMPUrlSourceSplitter_Rtmp::QueryStreamProgress(LONGLONG *total, LONGLONG *current)
{
  this->logger->Log(LOGGER_DATA, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_QUERY_STREAM_PROGRESS_NAME);

  HRESULT result = S_OK;
  CHECK_POINTER_DEFAULT_HRESULT(result, total);
  CHECK_POINTER_DEFAULT_HRESULT(result, current);

  if (result == S_OK)
  {
    *total = (this->streamLength == 0) ? 1 : this->streamLength;
    *current = (this->streamLength == 0) ? 0 : this->bytePosition;

    if (!this->setLenght)
    {
      result = VFW_S_ESTIMATED;
    }
  }

  this->logger->Log(LOGGER_DATA, (SUCCEEDED(result)) ? METHOD_END_HRESULT_FORMAT : METHOD_END_FAIL_HRESULT_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_QUERY_STREAM_PROGRESS_NAME, result);
  return result;
}

HRESULT CMPUrlSourceSplitter_Rtmp::QueryStreamAvailableLength(CStreamAvailableLength *availableLength)
{
  HRESULT result = S_OK;
  CHECK_POINTER_DEFAULT_HRESULT(result, availableLength);

  if (result == S_OK)
  {
    availableLength->SetQueryResult(S_OK);
    if (!this->setLenght)
    {
      availableLength->SetAvailableLength(this->bytePosition);
    }
    else
    {
      availableLength->SetAvailableLength(this->streamLength);
    }
  }

  return result;
}

size_t CMPUrlSourceSplitter_Rtmp::CurlReceiveData(char *buffer, size_t size, size_t nmemb, void *userdata)
{
  CMPUrlSourceSplitter_Rtmp *caller = (CMPUrlSourceSplitter_Rtmp *)userdata;
  CLockMutex lock(caller->lockMutex, INFINITE);
  unsigned int bytesRead = size * nmemb;

  if (!((caller->shouldExit) || (caller->internalExitRequest)))
  {
    if (!caller->setLenght)
    {
      double streamSize = 0;
      CURLcode errorCode = curl_easy_getinfo(caller->mainCurlInstance->GetCurlHandle(), CURLINFO_CONTENT_LENGTH_DOWNLOAD, &streamSize);
      if ((errorCode == CURLE_OK) && (streamSize > 0))
      {
        LONGLONG total = LONGLONG(streamSize);
        caller->streamLength = total;
        caller->logger->Log(LOGGER_VERBOSE, L"%s: %s: setting total length: %u", PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, total);
        caller->filter->SetTotalLength(total, false);
        caller->setLenght = true;
      }
    }

    if (caller->streamDuration == 0)
    {
      double streamDuration = 0;
      CURLcode errorCode = curl_easy_getinfo(caller->mainCurlInstance->GetCurlHandle(), CURLINFO_RTMP_TOTAL_DURATION, &streamDuration);
      if ((errorCode == CURLE_OK) && (streamDuration > 0))
      {
        caller->streamDuration = streamDuration;
        caller->logger->Log(LOGGER_VERBOSE, L"%s: %s: setting total duration: %lf", PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, streamDuration);
      }
    }

    if (!caller->setLenght)
    {
      if ((caller->streamLength == 0) || (caller->bytePosition > (caller->streamLength * 3 / 4)))
      {
        double currentTime = 0;
        CURLcode errorCode = curl_easy_getinfo(caller->mainCurlInstance->GetCurlHandle(), CURLINFO_RTMP_CURRENT_TIME, &currentTime);
        if ((errorCode == CURLE_OK) && (currentTime > 0) && (caller->streamDuration != 0))
        {
          LONGLONG tempLength = static_cast<LONGLONG>(caller->bytePosition * caller->streamDuration / currentTime);
          if (tempLength > caller->streamLength)
          {
            caller->streamLength = tempLength;
            caller->logger->Log(LOGGER_VERBOSE, L"%s: %s: setting quess by stream duration total length: %llu", PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, caller->streamLength);
            caller->filter->SetTotalLength(caller->streamLength, false);
          }
        }
        else if (caller->bytePosition != 0)
        {
          if (caller->streamLength == 0)
          {
            // error occured or stream duration is not set
            // just make guess
            caller->streamLength = LONGLONG(MINIMUM_RECEIVED_DATA_FOR_SPLITTER);
            caller->logger->Log(LOGGER_VERBOSE, L"%s: %s: setting quess total length: %u", PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, caller->streamLength);
            caller->filter->SetTotalLength(caller->streamLength, false);
          }
          else if ((caller->bytePosition > (caller->streamLength * 3 / 4)))
          {
            // it is time to adjust stream length, we are approaching to end but still we don't know total length
            caller->streamLength = caller->bytePosition * 2;
            caller->logger->Log(LOGGER_VERBOSE, L"%s: %s: adjusting quess total length: %u", PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, caller->streamLength);
            caller->filter->SetTotalLength(caller->streamLength, false);
          }
        }
      }
    }

    if (bytesRead != 0)
    {
      if (caller->bufferForProcessing != NULL)
      {
        unsigned int bufferSize = caller->bufferForProcessing->GetBufferSize();
        unsigned int freeSpace = caller->bufferForProcessing->GetBufferFreeSpace();

        if (freeSpace < bytesRead)
        {
          unsigned int bufferNewSize = max(bufferSize * 2, bufferSize + bytesRead);
          caller->logger->Log(LOGGER_VERBOSE, L"%s: %s: buffer to small, buffer size: %d, new size: %d", PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME, bufferSize, bufferNewSize);
          if (!caller->bufferForProcessing->ResizeBuffer(bufferNewSize))
          {
            caller->logger->Log(LOGGER_WARNING, L"%s: %s: resizing buffer unsuccessful, dropping received data", PROTOCOL_IMPLEMENTATION_NAME, METHOD_RECEIVE_DATA_NAME);
          }

          freeSpace = caller->bufferForProcessing->GetBufferFreeSpace();
        }

        if (freeSpace >= bytesRead)
        {
          caller->bufferForProcessing->AddToBuffer(buffer, bytesRead);
        }
      }
    }
  }

  // if returned 0 (or lower value than bytesRead) it cause transfer interruption
  return ((caller->shouldExit) || (caller->internalExitRequest)) ? 0 : (bytesRead);
}

unsigned int CMPUrlSourceSplitter_Rtmp::GetSeekingCapabilities(void)
{
  return SEEKING_METHOD_TIME;
}

int64_t CMPUrlSourceSplitter_Rtmp::SeekToTime(int64_t time)
{
  CLockMutex lock(this->lockMutex, INFINITE);

  this->logger->Log(LOGGER_VERBOSE, METHOD_START_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_SEEK_TO_TIME_NAME);
  this->logger->Log(LOGGER_VERBOSE, L"%s: %s: from time: %llu", PROTOCOL_IMPLEMENTATION_NAME, METHOD_SEEK_TO_TIME_NAME, time);

  int64_t result = -1;

  this->seekingActive = true;

  // there is internal exit request pending == changed timestamp
  // close connection
  this->CloseConnection();

  // RTMP protocol can seek only to seconds
  // time is in ms

  // 1 second back
  this->streamTime = max(0, time - 1000);

  // reopen connection
  // OpenConnection() reset wholeStreamDownloaded
  this->OpenConnection();

  if (this->IsConnected())
  {
    this->bytePosition = 0;
    result = max(0, time - 1000);
  }

  this->logger->Log(LOGGER_VERBOSE, METHOD_END_INT64_FORMAT, PROTOCOL_IMPLEMENTATION_NAME, METHOD_SEEK_TO_TIME_NAME, result);
  return result;
}

void CMPUrlSourceSplitter_Rtmp::SetSupressData(bool supressData)
{
  this->supressData = supressData;
}