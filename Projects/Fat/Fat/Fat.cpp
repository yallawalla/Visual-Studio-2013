// Fat.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "ff.h"
#include "diskio.h"
char test[] = "to je test";
int __cdecl mainn(void);

int _tmain(int argc, _TCHAR* argv[])
{
	int	wbuf[SECTOR_SIZE],n;
	FRESULT fres;
	FIL		f;
	FATFS	fs;
	fres = f_mkfs((const TCHAR*)FS_CPU, FM_ANY, 0, wbuf, SECTOR_SIZE*sizeof(int));
	fres = f_mount(&fs, (const TCHAR*)FS_CPU, 1);
	fres = f_open(&f, (const TCHAR *)"test.c", FA_CREATE_ALWAYS | FA_WRITE);
	fres = f_write(&f, test, sizeof(test), (UINT *)&n);
	fres = f_close(&f);

	return mainn();
}
