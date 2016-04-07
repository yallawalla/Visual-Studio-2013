// crc.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdio.h>
#include <string.h>

typedef enum { _DATA=0, _EOF, _ESA, _SSA, _ELA, _SLA } record;
#define POLYNOMIAL 0x04c11db7      									// Eth CRC-32 polynomial
int crc(int crc, int data) 
{
	int i = 32;
	crc ^= data;
	while (i--)
		if (crc < 0)
			crc = (crc << 1) ^ POLYNOMIAL;
		else
			crc = (crc << 1);
	return crc;
}

unsigned char* Idump(unsigned char *p, int size, int addr,int type,FILE *f)
{
	unsigned char chk = size + (addr / 256) + (addr % 256) + type;
	fprintf(f, ":%02X%04X%02X", size, addr, type);
	for (int i = 0; i < size; ++i)
	{
		fprintf(f, "%02X", *p);
		chk += *p++;
	}
	chk = ~chk + 1;
	fprintf(f, "%02X\n",chk);
	return p;
}

int	Scanning(FILE *f, unsigned int *min, unsigned int *max, unsigned  char block[])
{
	unsigned int segaddr = 0;
	do
	{
		unsigned int	j, size, address;
		record			type;
		if (fscanf(f, ":%02X%04X%02X", &size, &address, &type) == 3)
		{
			switch (type)
			{
			case _DATA:
				while (size--)
				{
					fscanf(f, "%02X", &j);
					if (block)
						block[address + (segaddr << 16) - *min] = j;
					if (address + (segaddr << 16) < *min)
						*min = address + (segaddr << 16);
					address++;
				}
				fscanf(f, "%02X\n", &j);
				if (address + (segaddr << 16) > *max)
					*max = address + (segaddr << 16);
				break;

			case _ELA:
				fscanf(f, "%04X", &segaddr);
				fscanf(f, "%02X\n", &j);
				break;

			case _EOF:
				fscanf(f, "%02X\n", &j);
				break;

			default:
				while (size--)
					fscanf(f, "%02X", &j);
				fscanf(f, "%02X\n", &j);
				break;
			}
		}
	} while (!feof(f));
	return -1;
}

int main(int argc, char* argv[])
{
	unsigned int i,min = 0xffffffff, max = 0;
	char c[256];

	if (argc != 2)
	{
		printf("...error, mising filenames\r\n");
		return 0;
	}


	strcpy(c, argv[1]);
	if (strchr(c, '.'))
		*strchr(c, '.') = '\0';
	strcat(c, "Signed.hex");

	FILE *f = fopen(argv[1],"r");
	if (!f)
	{
		printf("..input file error\r\n");
		return 0;
	}

	FILE *ff = fopen(c, "w");
	if (!ff)
	{
		printf("..output file error\r\n");
		return 0;
	}

	Scanning(f,&min,&max,NULL);
	unsigned char *p = new unsigned char[max-min]();
	memset(p, 0xff, max - min);
	fclose(f);
	f = fopen(argv[1], "r");
	Scanning(f, &min, &max, p);

	int _crc = -1;
	int *q = (int *)p;
	for (i = 0; i < (max - min) / 4; ++i)
		_crc = crc(_crc,*q++);

	fclose(f);
	f = fopen(argv[1], "r");
	while (fgets(c, 256, f))
		if (!strncmp(c, ":04000005", 9) || !strncmp(c, ":00000001", 9))
			break;
		else
			fputs(c, ff);

	p[0] = min >> 24;
	p[1] = (min >> 16) & 0xff;
	Idump(p, sizeof(short), 0, _ELA, ff);

	i = crc(-1, (max - min) / 4);
	i = crc(i, _crc);
	i = crc(i, min);

	memset(p, 0xff, max - min);
	q = (int *)p;
	*++q = i;
	*++q = (max - min) / 4;
	*++q = _crc;
	*++q = min;

	p = Idump(p, 16, (min & 0xffff) - 32, _DATA, ff);
	p = Idump(p, 16, (min & 0xffff) - 16, _DATA, ff);

	if (!feof(f))
	{
		fputs(c, ff);
		while (fgets(c, 256, f))
			fputs(c, ff);
	}
	fclose(f);
	fclose(ff);
	return 0;
}

//:207FE000FFFFFFFF49E42C9C384A0000DA7DF42300800008FFFFFFFFFFFFFFFFFFFFFFFF24
