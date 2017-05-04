// Fat.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "ff.h"
#include "diskio.h"
#include "ftp.h"

DWORD WINAPI MsgHandle(LPVOID);
struct ftpd_msgstate * Listen(PCSTR, LPVOID);

char test[] = "to je test";
int __cdecl mainn(void);

// all files without precompiled headers
// project character set not set
//
int _tmain(int argc, _TCHAR* argv[])
{
	int	wbuf[SECTOR_SIZE],n;
	TCHAR cwd[100];
	FRESULT fres;
	FIL		f;
	FATFS	fs;
	DIR		dir;
	fres = f_mkfs((const TCHAR*)FS_CPU, FM_ANY, 0, wbuf, SECTOR_SIZE*sizeof(int));
	fres = f_mount(&fs, (const TCHAR*)FS_CPU, 1);
	fres = f_open(&f, (const TCHAR *)"test.c", FA_CREATE_ALWAYS | FA_WRITE);
	fres = f_write(&f, test, sizeof(test), (UINT *)&n);
	fres = f_close(&f);
	fres = f_mkdir((const TCHAR *)"first");
//	fres = f_chdir((const TCHAR *)"first");
//	fres = f_getcwd((TCHAR *)cwd, sizeof(cwd));

	struct	ftpd_msgstate *msg = Listen("21", MsgHandle);
	if (msg) {
		msg->fatfs = &fs;
		msg->dir = &dir;
		while (1);
	}
}

