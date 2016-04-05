// lwip.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <commctrl.h>
#include <iostream>
#include <time.h>
#include <stdio.h>
#include <strsafe.h>
#include "serial.h"

using namespace std;

extern "C"	int	putVCP (int);
extern "C"	int	getVCP (void);

extern "C"	void LwIP_Init(void);
extern "C"	void LwIP_Pkt_Handle(void);
extern "C"	void LwIP_Periodic_Handle(int);
extern "C"  void ftpd_init(void);

extern "C"  int	 pcap_init(void);
extern "C"  int  pcap_loop(void);

HANDLE		hComm;
CSerial		*Com;
OVERLAPPED	ovlr,ovlw;


int		_tmain(int argc, _TCHAR* argv[])
{
		int to;	
		
		
		Com=new CSerial();
		Com->Open(_T("COM8:"),125000);

		LwIP_Init();
		pcap_init();
		ftpd_init();

		while(1) {
			to=clock();
			pcap_loop();
			LwIP_Periodic_Handle(to);
		}
		return 0;
}

int		putVCP (int c) {
		Com->SendData((const char *)&c,1);
		return(c);
}

int		getVCP () {
BYTE	c;

		if(Com->ReadData(&c,1))
			return c;
		else
			return -1;
}
