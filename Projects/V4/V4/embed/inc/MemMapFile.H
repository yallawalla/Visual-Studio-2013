#ifndef __MemMapFile_h__
#define __MemMapFile_h__

#include "windows.h"

#ifdef __cplusplus
extern "C" {
#endif


typedef struct SFileHandleStruct *SFileHandle;

SFileHandle WINAPI CreateFile1(LPCSTR pszName, DWORD dwBuffSize, DWORD dwData);
BOOL WINAPI  CloseHandle1(SFileHandle hHandle);
DWORD WINAPI WriteFile1(SFileHandle hHandle, const BYTE *pbyBuff, DWORD dwSize);
DWORD WINAPI ReadFile1(SFileHandle hHandle, BYTE *pbyBuff, DWORD dwSize);

#ifdef __cplusplus
}
#endif

#endif
