// Fat.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "ff.h"
#include "diskio.h"
#include "ftp.h"

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
	fres = f_open(&f, (const TCHAR *)"prvi.txt", FA_CREATE_ALWAYS | FA_WRITE);
	f_printf(&f, "%s", "prvi...");
	fres = f_close(&f);
	fres = f_open(&f, (const TCHAR *)"drugi.txt", FA_CREATE_ALWAYS | FA_WRITE);
	f_printf(&f, "%s", "drugi...");
	fres = f_close(&f);
	fres = f_mkdir((const TCHAR *)"tretji");
	fres = f_chdir((const TCHAR *)"tretji");
	fres = f_getcwd((TCHAR *)cwd, sizeof(cwd));
	fres = f_open(&f, (const TCHAR *)"prvi.txt", FA_CREATE_ALWAYS | FA_WRITE);
	f_printf(&f, "%s", "prvi...");
	fres = f_close(&f);
	fres = f_chdir((const TCHAR *)"..");
	fres = f_getcwd((TCHAR *)cwd, sizeof(cwd));

	_FSM *fsm = Listen("21", MsgHandle);
	if (fsm) {
		fsm->fatfs = &fs;
		fsm->dir = &dir;
		while (1);
	}
}

