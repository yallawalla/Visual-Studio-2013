#include "MemMapFile.h"
#include "TMacros.h"
#define MAX_STR_LEN 128
#define INC_PTR(P, S) ((P)+1==(S) ? 0 : (P)+1)



// memory organization
//   SMemMapBuffer[0] SMemMapBuffer[1]  .... SMemMapBuffer[MAX_CONN-1]
typedef struct SMemMapBuffer
  {
  DWORD m_dwWPtr;
  DWORD m_dwRPtr;
  BYTE  m_pbyBuff[0];
  } SMemMapBuffer;

#define MAX_CONN 0x10 // max number of connections to MemMap

typedef struct SMemMapFileHandle
  {
  HANDLE m_hMutex;          // mutex for MemMap file
  HANDLE m_hMutexExistance; // existance mutex
  HANDLE m_hFileMapping;
  DWORD m_dwPosition;          // own buffer position
  DWORD m_dwBufferSize;
  SMemMapBuffer *m_pBuff[MAX_CONN];  // array of buffer pointers
  } SMemMapFileHandle;

typedef struct SFileHandleStruct
  {
  HANDLE m_hComm;
  SMemMapFileHandle *m_pMM;
  } SFileHandleStruct;

void MMF_InitBuffers(SMemMapBuffer *pBuff, DWORD dwSize)
  {
  pBuff->m_dwWPtr=0;
  pBuff->m_dwRPtr=0;
  ZeroMemory(pBuff->m_pbyBuff, dwSize);
  }

BOOL MMF_MutexExist(LPCSTR pszName)
  {  
  HANDLE hMutex=OpenMutex(MUTEX_ALL_ACCESS, FALSE, pszName);
  CloseHandle(hMutex);
  return hMutex!=NULL;
  }

HANDLE MMF_CreateExistanceMutex(LPCSTR pszName, DWORD *pdwPos)
  {
  // existance mutex is signal that connection is available
  char pszNameMutex[MAX_STR_LEN];
  HANDLE hMutex;

  strncpy(pszNameMutex, pszName, sizeof(pszNameMutex));
  for(pdwPos[0]=0; pdwPos[0]<MAX_CONN; pdwPos[0]++)
    {
    pszNameMutex[0]='A'+(char)pdwPos[0];
    if(MMF_MutexExist(pszNameMutex))
      continue;
    // this connection is available
    hMutex=CreateMutex(NULL, TRUE, pszNameMutex);
    if(NULL==hMutex || ERROR_ALREADY_EXISTS==GetLastError())
      continue;
    return hMutex;
    }
  pdwPos[0]=-1;
  return NULL;
  }


SMemMapFileHandle *WINAPI CreateMemMapFile(LPCSTR pszName, DWORD dwBuffSize, DWORD dwFlags)
  {
  char pszNameMutex[MAX_STR_LEN]="_98234_sdafa83_";
  HANDLE hFM,hMutex,hMutexExistance;
  const DWORD dwConnSize=dwBuffSize+sizeof(SMemMapBuffer);
  const DWORD dwSize=MAX_CONN*dwConnSize;
  BYTE *pMem;
  SMemMapFileHandle *pH;
  DWORD dwPosition=-1;
  int i;

  // access mutex
  strncat(pszNameMutex, pszName, sizeof(pszNameMutex));
  hMutex=CreateMutex(NULL, FALSE, pszNameMutex);
  if(NULL==hMutex || WAIT_OBJECT_0!=WaitForSingleObject(hMutex, 100))
    return NULL;
  // create existance mutex
  hMutexExistance=MMF_CreateExistanceMutex(pszNameMutex, &dwPosition);
  if(NULL==hMutexExistance)
    goto EXIT_WITH_ERROR;
  // open or create kernel object
  hFM=CreateFileMapping(INVALID_HANDLE_VALUE ,NULL, PAGE_READWRITE, 0, dwSize, pszName);
  if(NULL==hFM)
    goto EXIT_WITH_ERROR;
  //bOpen=ERROR_ALREADY_EXISTS==GetLastError();
  pMem=(BYTE *)MapViewOfFile(hFM,FILE_MAP_ALL_ACCESS,0,0,0);
  if(NULL==pMem)
    goto EXIT_WITH_ERROR;
  // create and initialize handle
  pH=malloc(sizeof(SMemMapFileHandle));
  for(i=0; i<ARRAY_SIZE(pH->m_pBuff); i++)
    pH->m_pBuff[i]=(SMemMapBuffer*)(pMem+i*dwConnSize);
  MMF_InitBuffers(pH->m_pBuff[dwPosition],  dwBuffSize);
  pH->m_hMutex=hMutex;
  pH->m_hMutexExistance=hMutexExistance;
  pH->m_hFileMapping=hFM;
  pH->m_dwPosition=dwPosition;
  pH->m_dwBufferSize=dwBuffSize;
  ReleaseMutex(hMutex);
  return pH;

EXIT_WITH_ERROR:
  CloseHandle(hMutex);
  CloseHandle(hMutexExistance);
  CloseHandle(hFM); 
  return INVALID_HANDLE_VALUE;
  }

BOOL CloseMemMapFile(SMemMapFileHandle *pH)
  {
  //SMemMapFileHandle *pH=pHandle->m_pMM;
  HANDLE hFM=pH->m_hFileMapping;
  WaitForSingleObject(pH->m_hMutex, 100);
  ReleaseMutex(pH->m_hMutex);  
  CloseHandle(pH->m_hMutex);
  UnmapViewOfFile(pH->m_pBuff[0]);
  CloseHandle(hFM);
  CloseHandle(pH->m_hMutexExistance);
  free(pH);
  return TRUE;
  }

DWORD ReadMemMapFile(SMemMapFileHandle *pH, BYTE *pbyBuff, DWORD dwSize)
  {
  DWORD i;
  SMemMapBuffer *pB=pH->m_pBuff[pH->m_dwPosition];
  DWORD dwBuffSize=pH->m_dwBufferSize;
  // read from own buffer
  if(NULL==pH)
    return 0;
  if(WAIT_OBJECT_0!=WaitForSingleObject(pH->m_hMutex, 100))
    return 0;
  for(i=0; i<dwSize; i++)
    {
    if(pB->m_dwWPtr==pB->m_dwRPtr)
      break;
    pbyBuff[i]=pB->m_pbyBuff[pB->m_dwRPtr];
    pB->m_dwRPtr=INC_PTR(pB->m_dwRPtr, dwBuffSize);
    }
  ReleaseMutex(pH->m_hMutex);
  return i;
  }

DWORD WriteMemMapFile(SMemMapFileHandle *pH, const BYTE *pbyBuff, DWORD dwSize)
  {
  DWORD i,dwNext,j;
  DWORD dwMaxSent=0;
  SMemMapBuffer *pB;
  DWORD dwBuffSize=pH->m_dwBufferSize;
  if(NULL==pH)
    return 0;
  if(WAIT_OBJECT_0!=WaitForSingleObject(pH->m_hMutex, 100))
    return 0;
  // write to all buffers except own
  for(j=0; j<ARRAY_SIZE(pH->m_pBuff); j++)
    {
    if(j==pH->m_dwPosition)
      continue;
    pB=pH->m_pBuff[j];
    for(i=0; i<dwSize; i++)
      {
      dwNext=INC_PTR(pB->m_dwWPtr, dwBuffSize);
      if(dwNext==pB->m_dwRPtr)
        break;
      pB->m_pbyBuff[pB->m_dwWPtr]=pbyBuff[i];
      pB->m_dwWPtr=dwNext;
      }
    dwMaxSent=max(i, dwMaxSent);
    }
  ReleaseMutex(pH->m_hMutex);
  return dwMaxSent;
  }

void MMF_InitHandle(SFileHandle pHandle)
  {
  pHandle->m_hComm=INVALID_HANDLE_VALUE;
  pHandle->m_pMM=NULL;
  }


BOOL MMF_IsHandleValid(SFileHandle pHandle)
  {
  if(NULL==pHandle)
    return FALSE;
  return INVALID_HANDLE_VALUE!=pHandle->m_hComm || NULL!=pHandle->m_pMM;
  }

BOOL MMF_IsHandleMMF(SFileHandle pHandle)
  {
  return NULL!=pHandle->m_pMM;
  }

BOOL WINAPI CloseHandle1(SFileHandle pHandle)
  {
  BOOL bRet=FALSE;
  SFileHandleStruct handle; 
  if(!MMF_IsHandleValid(pHandle))
    return TRUE;
  COPY(handle, *pHandle);
  MMF_InitHandle(pHandle);
  if(MMF_IsHandleMMF(pHandle))
    bRet=CloseMemMapFile(pHandle->m_pMM);
  else
    bRet=CloseHandle(pHandle->m_hComm);
  free(pHandle);
  return bRet;
  }

DWORD WINAPI WriteFile1(SFileHandle pHandle, const BYTE *pbyBuff, DWORD dwSize)
  {
  if(!MMF_IsHandleValid(pHandle))
    return FALSE;
  if(MMF_IsHandleMMF(pHandle))
    return WriteMemMapFile(pHandle->m_pMM, pbyBuff, dwSize);
  else
    {
    DWORD dwCnt;
    WriteFile(pHandle->m_hComm, pbyBuff, dwSize, &dwCnt, NULL);
    return dwCnt;
    }
  }

DWORD WINAPI ReadFile1(SFileHandle pHandle, BYTE *pbyBuff, DWORD dwSize)
  {
  if(!MMF_IsHandleValid(pHandle))
    return FALSE;
  if(MMF_IsHandleMMF(pHandle))
    return ReadMemMapFile(pHandle->m_pMM, pbyBuff, dwSize);
  else
    {
    DWORD dwCnt;
    ReadFile(pHandle->m_hComm, pbyBuff, dwSize, &dwCnt, NULL);
    return dwCnt;
    }
  }

SFileHandle WINAPI CreateFile1(LPCSTR pszName, DWORD dwBuffSize, DWORD dwData)
  {
  DCB *pDCB=NULL;
  COMMTIMEOUTS ct={0};
  SFileHandle pHandle=malloc(sizeof(SFileHandleStruct));
  MMF_InitHandle(pHandle);

  if('|'==pszName[0])
    {
    pHandle->m_pMM=CreateMemMapFile(pszName, dwBuffSize, 0);
    }
  else
    {
    pHandle->m_hComm=CreateFile(pszName,GENERIC_READ | GENERIC_WRITE,0,NULL,OPEN_EXISTING,0,NULL);
    if(0!=dwData)
      {
      pDCB=(DCB *)dwData;
      SetCommState(pHandle->m_hComm, pDCB);
      ct.ReadIntervalTimeout=MAXDWORD;  
      ct.ReadTotalTimeoutMultiplier=0;  
      ct.ReadTotalTimeoutConstant=0;    
      ct.WriteTotalTimeoutMultiplier=10;  
      ct.WriteTotalTimeoutConstant=1000;    
      SetCommTimeouts(pHandle->m_hComm, &ct);
      PurgeComm(pHandle->m_hComm, PURGE_RXCLEAR|PURGE_TXCLEAR);
      }  
    }
  if(!MMF_IsHandleValid(pHandle))
    {
    free(pHandle);
    pHandle=NULL;
    }
  return pHandle;
  }

