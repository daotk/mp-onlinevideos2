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

#include "StdAfx.h"

#include "MmsCurlInstance.h"

CMmsCurlInstance::CMmsCurlInstance(CLogger *logger, HANDLE mutex, const wchar_t *url, const wchar_t *protocolName, const wchar_t *instanceName)
  : CHttpCurlInstance(logger, mutex, url, protocolName, instanceName)
{
}


CMmsCurlInstance::~CMmsCurlInstance(void)
{
}

bool CMmsCurlInstance::Initialize(void)
{
  return __super::Initialize();
}

void CMmsCurlInstance::CurlDebug(curl_infotype type, const wchar_t *data)
{
  if (type == CURLINFO_HEADER_OUT)
  {
    // we are just interested in headers comming in from peer
    this->logger->Log(LOGGER_VERBOSE, L"%s: %s: sent HTTP header: '%s'", this->protocolName, METHOD_CURL_DEBUG_CALLBACK, data);
  }
}