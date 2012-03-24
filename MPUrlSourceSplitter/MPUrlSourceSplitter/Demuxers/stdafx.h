// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once


#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
#define VC_EXTRALEAN

#include "streams.h"

#pragma warning(push)
#pragma warning(disable:4244)

extern "C" {
#define __STDC_CONSTANT_MACROS
#include "libavformat/avformat.h"
#include "libavutil/intreadwrite.h"
#include "libavutil/pixdesc.h"

//#include "libbluray/bluray.h"
}
#pragma warning(pop)

#define AVFORMAT_INTERNAL_H
typedef struct AVCodecTag {
    enum CodecID id;
    unsigned int tag;
} AVCodecTag;

//#include "libbluray/bdnav/clpi_parse.h"

//#include "util/log_control.h"

#include "DShowUtil.h"
#include <MMReg.h>

#include <Shlwapi.h>


// TODO: reference additional headers your program requires here
#include <Memory.h>