/*
 *      Copyright (C) 2011 Hendrik Leppkes
 *      http://www.1f0.de
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 *  Initial design and concept by Gabest and the MPC-HC Team, copyright under GPLv2
 *  Contributions by Ti-BEN from the XBMC DSPlayer Project, also under GPLv2
 */

#pragma once

#include <string>
#include <list>
#include <set>
#include <vector>
#include <map>
#include "PacketQueue.h"

#include "BaseDemuxer.h"

#include "LAVSplitterSettingsInternal.h"
#include "SettingsProp.h"

#include "Download.h"
#include "IFilter.h"

//#define LAVF_REGISTRY_KEY L"Software\\LAV\\Splitter"
//#define LAVF_REGISTRY_KEY_FORMATS LAVF_REGISTRY_KEY L"\\Formats"
//#define LAVF_LOG_FILE     L"LAVSplitter.txt"

class CLAVOutputPin;
class CLAVInputPin;

#ifdef	_MSC_VER
#pragma warning(disable: 4355)
#endif

[uuid("59ED045A-A938-4A09-A8A6-8231F5834259")]
class CLAVSplitter 
  : public CBaseFilter
  , public CCritSec
  , protected CAMThread
  , public IFileSourceFilter
  , public IMediaSeeking
  , public IAMStreamSelect
  , public ILAVFSettingsInternal
  , public IObjectWithSite
  , public IDownload
  , public IFilter
{
public:
  CLAVSplitter(LPUNKNOWN pUnk, HRESULT* phr);
  virtual ~CLAVSplitter();

  // IUnknown
  DECLARE_IUNKNOWN;
  STDMETHODIMP NonDelegatingQueryInterface(REFIID riid, void** ppv);

  // CBaseFilter methods
  int GetPinCount();
  CBasePin *GetPin(int n);
  STDMETHODIMP GetClassID(CLSID* pClsID);

  STDMETHODIMP Stop();
  STDMETHODIMP Pause();
  STDMETHODIMP Run(REFERENCE_TIME tStart);

  // IFileSourceFilter
  STDMETHODIMP Load(LPCOLESTR pszFileName, const AM_MEDIA_TYPE * pmt);
  STDMETHODIMP GetCurFile(LPOLESTR *ppszFileName, AM_MEDIA_TYPE *pmt);

  // IMediaSeeking
  STDMETHODIMP GetCapabilities(DWORD* pCapabilities);
  STDMETHODIMP CheckCapabilities(DWORD* pCapabilities);
  STDMETHODIMP IsFormatSupported(const GUID* pFormat);
  STDMETHODIMP QueryPreferredFormat(GUID* pFormat);
  STDMETHODIMP GetTimeFormat(GUID* pFormat);
  STDMETHODIMP IsUsingTimeFormat(const GUID* pFormat);
  STDMETHODIMP SetTimeFormat(const GUID* pFormat);
  STDMETHODIMP GetDuration(LONGLONG* pDuration);
  STDMETHODIMP GetStopPosition(LONGLONG* pStop);
  STDMETHODIMP GetCurrentPosition(LONGLONG* pCurrent);
  STDMETHODIMP ConvertTimeFormat(LONGLONG* pTarget, const GUID* pTargetFormat, LONGLONG Source, const GUID* pSourceFormat);
  STDMETHODIMP SetPositions(LONGLONG* pCurrent, DWORD dwCurrentFlags, LONGLONG* pStop, DWORD dwStopFlags);
  STDMETHODIMP GetPositions(LONGLONG* pCurrent, LONGLONG* pStop);
  STDMETHODIMP GetAvailable(LONGLONG* pEarliest, LONGLONG* pLatest);
  STDMETHODIMP SetRate(double dRate);
  STDMETHODIMP GetRate(double* pdRate);
  STDMETHODIMP GetPreroll(LONGLONG* pllPreroll);

  // IAMStreamSelect
  STDMETHODIMP Count(DWORD *pcStreams);
  STDMETHODIMP Enable(long lIndex, DWORD dwFlags);
  STDMETHODIMP Info(long lIndex, AM_MEDIA_TYPE **ppmt, DWORD *pdwFlags, LCID *plcid, DWORD *pdwGroup, WCHAR **ppszName, IUnknown **ppObject, IUnknown **ppUnk);

  // IObjectWithSite
  STDMETHODIMP SetSite(IUnknown *pUnkSite);
  STDMETHODIMP GetSite(REFIID riid, void **ppvSite);

  // ILAVFSettings
  STDMETHODIMP SetRuntimeConfig(BOOL bRuntimeConfig);
  STDMETHODIMP GetPreferredLanguages(WCHAR **ppLanguages);
  STDMETHODIMP SetPreferredLanguages(WCHAR *pLanguages);
  STDMETHODIMP GetPreferredSubtitleLanguages(WCHAR **ppLanguages);
  STDMETHODIMP SetPreferredSubtitleLanguages(WCHAR *pLanguages);
  STDMETHODIMP_(LAVSubtitleMode) GetSubtitleMode();
  STDMETHODIMP SetSubtitleMode(LAVSubtitleMode mode);
  STDMETHODIMP_(BOOL) GetSubtitleMatchingLanguage();
  STDMETHODIMP SetSubtitleMatchingLanguage(BOOL dwMode);
  STDMETHODIMP_(BOOL) GetPGSForcedStream();
  STDMETHODIMP SetPGSForcedStream(BOOL bFlag);
  STDMETHODIMP_(BOOL) GetPGSOnlyForced();
  STDMETHODIMP SetPGSOnlyForced(BOOL bForced);
  STDMETHODIMP_(int) GetVC1TimestampMode();
  STDMETHODIMP SetVC1TimestampMode(int iMode);
  STDMETHODIMP SetSubstreamsEnabled(BOOL bSubStreams);
  STDMETHODIMP_(BOOL) GetSubstreamsEnabled();
  STDMETHODIMP SetVideoParsingEnabled(BOOL bEnabled);
  STDMETHODIMP_(BOOL) GetVideoParsingEnabled();
  STDMETHODIMP SetFixBrokenHDPVR(BOOL bEnabled);
  STDMETHODIMP_(BOOL) GetFixBrokenHDPVR();
  STDMETHODIMP_(HRESULT) SetFormatEnabled(const char *strFormat, BOOL bEnabled);
  STDMETHODIMP_(BOOL) IsFormatEnabled(const char *strFormat);
  STDMETHODIMP SetStreamSwitchRemoveAudio(BOOL bEnabled);
  STDMETHODIMP_(BOOL) GetStreamSwitchRemoveAudio();
  STDMETHODIMP GetAdvancedSubtitleConfig(WCHAR **ppAdvancedConfig);
  STDMETHODIMP SetAdvancedSubtitleConfig(WCHAR *pAdvancedConfig);

  // ILAVSplitterSettingsInternal
  STDMETHODIMP_(const char*) GetInputFormat() { if (m_pDemuxer) return m_pDemuxer->GetContainerFormat(); return NULL; }
  STDMETHODIMP_(std::set<FormatInfo>&) GetInputFormats();
  STDMETHODIMP_(BOOL) IsVC1CorrectionRequired();

  // IAMOpenProgress interface
  STDMETHODIMP QueryProgress(LONGLONG *pllTotal, LONGLONG *pllCurrent);
  STDMETHODIMP AbortOperation(void);

  // IDownload interface
  STDMETHODIMP Download(LPCOLESTR uri, LPCOLESTR fileName);
  STDMETHODIMP DownloadAsync(LPCOLESTR uri, LPCOLESTR fileName, IDownloadCallback *downloadCallback);

  // Settings helper
  std::list<std::string> GetPreferredAudioLanguageList();
  std::list<CSubtitleSelector> GetSubtitleSelectors();

  bool IsAnyPinDrying();
  void SetFakeASFReader(BOOL bFlag) { m_bFakeASFReader = bFlag; }

protected:
  // CAMThread
  enum {CMD_EXIT, CMD_SEEK};
  DWORD ThreadProc();

  HRESULT DemuxSeek(REFERENCE_TIME rtStart);
  HRESULT DemuxNextPacket();
  HRESULT DeliverPacket(Packet *pPacket);

  void DeliverBeginFlush();
  void DeliverEndFlush();

  STDMETHODIMP Close();
  STDMETHODIMP DeleteOutputs();

  STDMETHODIMP InitDemuxer();

  friend class CLAVInputPin;

  friend class CLAVOutputPin;
  STDMETHODIMP SetPositionsInternal(void *caller, LONGLONG* pCurrent, DWORD dwCurrentFlags, LONGLONG* pStop, DWORD dwStopFlags);

public:
  CLAVOutputPin *GetOutputPin(DWORD streamId, BOOL bActiveOnly = FALSE);
  STDMETHODIMP RenameOutputPin(DWORD TrackNumSrc, DWORD TrackNumDst, std::vector<CMediaType> pmts);
  STDMETHODIMP UpdateForcedSubtitleMediaType();

  // creates demuxer after loading some data from stream
  // this method should be called by input pin while not returned S_OK
  STDMETHODIMP CreateDemuxer(wchar_t *pszFileName);
  CBaseDemuxer *GetDemuxer(void);

  // IFilter interface

  // gets seeking capabilities of protocol
  // @return : bitwise combination of SEEKING_METHOD flags
  unsigned int GetSeekingCapabilities(void);

  // gets logger instance
  // @return : logger instance or NULL if error
  CLogger *GetLogger(void);

  // seeks to time (in ms)
  // @return : time in ms where seek finished or lower than zero if error
  int64_t SeekToTime(int64_t time);

  // request protocol implementation to receive data from specified position to specified position
  // @param start : the requested start position (zero is start of stream)
  // @param end : the requested end position, if end position is lower or equal to start position than end position is not specified
  // @return : position where seek finished or lower than zero if error
  int64_t SeekToPosition(int64_t start, int64_t end);

protected:
  STDMETHODIMP LoadDefaults();
  STDMETHODIMP LoadSettings();
  STDMETHODIMP SaveSettings();

protected:
  CLAVInputPin *m_pInput;
  CLogger *logger;

private:
  CCritSec m_csPins;
  std::vector<CLAVOutputPin *> m_pPins;
  std::vector<CLAVOutputPin *> m_pActivePins;
  std::vector<CLAVOutputPin *> m_pRetiredPins;
  std::set<DWORD> m_bDiscontinuitySent;

  std::wstring m_processName;

  CBaseDemuxer *m_pDemuxer;

  BOOL m_bPlaybackStarted;
  BOOL m_bFakeASFReader;

  // flushing
  bool m_fFlushing;
  CAMEvent m_eEndFlush;

  std::set<FormatInfo> m_InputFormats;

  // Settings
  struct Settings {
    std::wstring prefAudioLangs;
    std::wstring prefSubLangs;
    std::wstring subtitleAdvanced;
    LAVSubtitleMode subtitleMode;
    BOOL PGSForcedStream;
    BOOL PGSOnlyForced;
    int vc1Mode;
    BOOL substreams;
    BOOL videoParsing;

    BOOL StreamSwitchRemoveAudio;

    std::map<std::string, BOOL> formats;
  } m_settings;

  BOOL m_bRuntimeConfig;

  IUnknown *m_pSite;

  // ffmpeg log callback
  static void ffmpeg_log_callback(void *ptr, int log_level, const char *format, va_list vl);
};