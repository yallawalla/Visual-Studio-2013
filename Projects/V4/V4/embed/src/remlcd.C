#ifndef WIN32
#include	"remmenu.h"
#include	"io332.h"
#include	"lcdfont.inc"
/*--------------------------------------------------------------------------*/
#include	<rcopy.h>			/* PROM compile option						*/
extern void	init_ram(void);		/* define init. segment 					*/
/*--------------------------------------------------------------------------*/
/* KEYPAD DRIVER VARIABLES													*/
/*																			*/
/*--------------------------------------------------------------------------*/
int		key_count,				/* debounce counter							*/
		key_slow,				/* repeat stroke slow counter				*/
		key_old,				/* active key								*/
		key;					/* debounced key							*/
#define	KEY_BOUNCE	2
/*--------------------------------------------------------------------------*/
uchar	shaft_buff = 0x40;		/* enc. register mirror, write only			*/
/*--------------------------------------------------------------------------*/
void	ResetInit()
{
		int		i;
		PORTC=0x76;			/* PORTC=0x7E;									*/
		SYPCR	=0x00D0;	/* fffa21	*/
		CSBARBT	=0x0006;	/* fffa48 FLASH512k, RW, 16-bit port, 0x000000	*/
		CSORBT	=0x7B70;	/* fffa4a	*/
		CSBAR0	=0x0C05;	/* fffa4c RAM 256k, RW, 16-bit port, 0x080000	*/
		CSOR0	=0x7B70;	/* fffa4e	*/
		CSBAR1	=0x2000;	/* fffa50 LCD   2k, RW, 8-bit 68xx, 0x200000	*/
		CSOR1	=0x7B70;	/* fffa52	*/											/*V3.0*/
		CSBAR2	=0x2010;	/* fffa54 DSP   2k, RW, 16-bit port, 0x201000	*/
		CSOR2	=0x7B70;	/* fffa56	*/
		CSBAR3  =0xfff8;	/* fffa58 DSPavec 2k,RW,16-bit port, 0xffff80	*/
		CSOR3   =0x7801;	/* fffa5a all external IRQs use AUTOVECTORS		*/
		CSBAR4	=0x2020;	/* fffa5c input 2k,  R, 16-bit port, 0x202000	*/
		CSOR4	=0x6B70;	/* fffa5e	*/
		CSBAR5	=0x2030;	/* fffa60 output2k,  W, 16-bit port, 0x203000	*/
		CSOR5	=0x7370;	/* fffa62	*/
		CSBAR9	=0x0007;	/* fffa70 WE lo 1M, Wl, 16-bit port, 0x000000	*/
		CSOR9	=0x3370;	/* fffa72	*/
		CSBAR10	=0x0007;	/* fffa74 WE hi 1M, Wh, 16-bit port, 0x000000	*/
		CSOR10	=0x5370;	/* fffa76	*/

		CSPAR0	=0x3cef;	/* fffac4	*/											/*V3.0*/
		CSPAR1	=0x03c0;	/* fffac6	A19 = output !!!					*/ 

		DDRE	=0xFF;		/* fffa15 port E = outputs						*/
		PORTE	=0xBF;		/* fffa11 C100 = GPS = off 						*/
		ENCw	=0x40;		/* encoders off									*/

		watchdog();
		rcopy(init_ram);	/* inicializacija NV spremenljivk				*/
		watchdog();
  		for(i=2; i<25; ++i)	/* vector lookup table setup					*/
			SetVect(reset_exc,i);
		watchdog();
		initSPI();
		Initialize();
		InitTPUchannels ();	/* pojavi crn trak!								*/
		_SPL(0);			/* RS232, PWM (-Vcc) in temp. senzor			*/
		SYNCR	=0x7f80;	/* speedup to 16.777 MHz, Ediv = 1:16			*/		/*V3.0*/
		CSORBT	=0x7830;	
		CSOR0	=0x7830;
		/*------------------------------------------------------------------*/
		if(readRTC(0x30) & 0x10)	{
			shutdown_save(NULL);
			writeRTC(0x20,0x00);	/* seconds								*/
			writeRTC(0x21,0x00);	/* minutes								*/
			writeRTC(0x22,0x00);	/* hours								*/
			writeRTC(0x23,0x06);	/* day									*/
			writeRTC(0x24,0x11);	/* date									*/
			writeRTC(0x25,0x03);	/* month								*/
			writeRTC(0x26,0x94);	/* year									*/
			for(i=0; i<8; ++i)
				writeRTC(i,0x00);	/* submit registers						*/
			}
		PIT_task = SetTask(NULL,null,PIT_task);
		RTC_task = SetTask(NULL,null,RTC_task);
        ReadTime(&Time);
/*		eerror=RSR & 0x7F;
*/		main();
}
/*----------------------------------------------------------------------*/
_IH		void	PITservice(void)
{
void	IRQservice(void);
		IRQservice();
}
/*----------------------------------------------------------------------*/
void	PIT_enable(void)
{
		SetVect(PITservice,0x24);	
		PITR=0x00A0;				/* 19.5 msec, ca. 50Hz				*/
		PICR=0x0124;				/* level 1, int 24H					*/
}
/*----------------------------------------------------------------------*/
void	PIT_disable(void)
{
		PITR=0;						/* timer stop						*/
}
/*----------------------------------------------------------------------*/
void	SetVect(void (*f)(void),int n)
{
		void	*p;
		get_byte(&p,(void *)(n<<2),sizeof(void *));
		if(p!=f)
			prog_vect(&f,(uchar *)(n<<2),sizeof(void *));
}
/*----------------------------------------------------------------------*/
/*	SPI driver															*/
/*																		*/
/*----------------------------------------------------------------------*/
void	initSPI(void)
{
/*----------------------------------------------------------------------*/
		QPDR=0xe3;
		QPAR=0x7b;
		QDDR=0xfe;
/*----------------------------------------------------------------------*/
/* MAX132 SPI na fix. lokacijah 0-5, RTC na lokaciji 6 !!!				*/

		tx_ram[0]=tx_ram[3]=0x40;
		tx_ram[1]=tx_ram[4]=0x44;
		tx_ram[2]=tx_ram[5]=0xD2;

		com_ram[0]=com_ram[1]=com_ram[2]=0x78; /* PCS2=1, PCR3=0		*/
		com_ram[3]=com_ram[4]=com_ram[5]=0x74; /* PCS2=0, PCR3=1		*/
		com_ram[6]=0x7E;					   /* DS1620 used			*/

		SPCR0	=	0xA108;		/* master, 8 bits, CPOL=0, CPHA _-		*/
		SPCR2	=	0x0500;
		SPCR3	=	0x0000;
		SPCR1	|=	0x8404;
/*----------------------------------------------------------------------*/
		writeRTC(0x31,0xB8);		/* clock start, xtal 32kHz			*/
									/* 50Hz, xtal output				*/
		writeRTC(0x32,0x0C);		/* watchdog off, alarm off			*/
									/* 1 Hz output						*/
		if((readRTC(0x31) != 0xB8) || (readRTC(0x32) != 0x0C))
		{
			com_ram[6]=0x7F;		/* DS1620 not used					*/
			writeRTC(0x31,0xB8);
			writeRTC(0x32,0x0C);
		}
		else
			readTEMP(0xEE,eof);
}
/*----------------------------------------------------------------------*/
int		readRTC(int a)
{
		int	i;
		while(SPI_busy());

		i = _GPL();
		_SPL(7);
		tx_ram[6]=(a<<8) | 0xff;/* hibyte = a, lobyte = ff !!!			*/

		SPSR	&=	~0x80;
		SPCR0	=	0x8108;		/* master, 16 bits, CPOL=0, CPHA -_		*/
		SPCR2	=	0x0606;
		SPCR1	|=	0x8000;
		_SPL(i);

		while(SPI_busy());
		return(rx_ram[6] & 0xFF);
}
/*----------------------------------------------------------------------*/
int		writeRTC(int a, int x)
{
		int	i;
		while(SPI_busy());

		i = _GPL();
		_SPL(7);
		tx_ram[6]=((a | 0x80)<<8) + (x & 0xff);

		SPSR	&=	~0x80;
		SPCR0	=	0x8108;		/* master, 16 bits, CPOL=0, CPHA -_		*/
		SPCR2	=	0x0606;
		SPCR1 	|=	0x8000;
		_SPL(i);

		while(SPI_busy());
		return(rx_ram[6] & 0xFF);
}
/*----------------------------------------------------------------------*/
double	getT(void)
{
		double	Tsw,Toff;
		if(tcm2.status & 0x80)
			return(tcm2.t);
		if(com_ram[6] == 0x7F)
		{
			Tsw		= 	Tmax-Tmin;
			Tsw 	/=	tn_max-tn_min;
			Toff	= Tmax-Tsw*tn_max;
			return(Tsw * (temp >> 5) + Toff);
		}
		return(readTEMP(0xAA,eof)/256.0);
}
/*----------------------------------------------------------------------*/
/*** 	MAX132 conversion		***/

void	writeADC(void)
{
		int	i;
		while(SPI_busy());

		i = _GPL();
		_SPL(7);
		SPSR	&=	~0x80;
		SPCR0	=	0xA108;		/*** master, 8 bits, CPOL=0, CPHA _- ***/
		SPCR2	=	0x0500;
		SPCR1	|=	0x8000;
		_SPL(i);
}
/*----------------------------------------------------------------------*/
void	ADC_scan(void)
{
		if(PITcnt % 2) 
		{
			p_off[PITcnt/2] = (rx_ram[5]<<8) + rx_ram[4];
			r_off[PITcnt/2] = (rx_ram[2]<<8) + rx_ram[1];
			tx_ram[2]=tx_ram[5]=0xD2;
		}
		else
		{
			p_filt[PITcnt/2] = (rx_ram[5]<<8) + rx_ram[4];
			r_filt[PITcnt/2] = (rx_ram[2]<<8) + rx_ram[1];
			tx_ram[2]=tx_ram[5]=0xC2;
		}
		writeADC();
		PITcnt = ++PITcnt % (2 * 8);
}
/*----------------------------------------------------------------------*/
/*		keyboard driver													*/
/*																		*/
/*----------------------------------------------------------------------*/
/* old  PC, PT 1,2,3,4													*/
#ifdef	WIN32
int		KEY0_1st[]=
{								0x15,	0x14,	0x13,	0x12,
								0x25,	0x24,	0x23,	0x22,
								0x35,	0x34,	0x33,	0x32,
								0x45,	0x44,	0x43,	0x42,

											0x54,
	0x66,0x61,0x64,		0x63,0x62,		0x55,	0x52,
											0x53
};
/*----------------------------------------------------------------------*/
/* rev. PC, PT 5,6,7,8,9												*/
#else
int		KEY1_1st[]=
{								0x11,	0x14,	0x13,	0x12,
								0x21,	0x24,	0x23,	0x22,
								0x31,	0x34,	0x33,	0x32,
								0x41,	0x44,	0x43,	0x42,

											0x54,
	0x65,0x61,0x64,		0x63,0x62,		0x51,	0x52,
											0x53
};
#endif
/*----------------------------------------------------------------------*/
int		KEY_2nd[]={				F1,	'7',	'8',	'9',
								F2,	'4',	'5',	'6',
								F3,	'1',	'2',	'3',
								F4,	CLEAR,	'0',	ENTER,

											UP,
	ONOFF,ONOFF,BACKLIT,	ALPHA,	SIGN,	LEFT,	RIGHT,
											DOWN

																};
/*----------------------------------------------------------------------*/
void	DecodeKey(void)
{
		int		i,*p;

#ifdef	WIN32
		p=KEY0_1st;
#else
		p=KEY1_1st;
#endif

		Key=0;
		for(i=0;i<25;++i)
			if(key == p[i]) {
				Key=KEY_2nd[i];
				break;
				}
}
/*----------------------------------------------------------------------*/
void	key_scan(void)
{
		int		i,j,k;
		int		log2i(int);

		for(i=1,j=0;i<64;i=i*2)
		{
			PORTE=((~i) & 0x3F)|(PORTE & 0xC0);
			for(k=0;k<10;++k);
			k=PORTF;
			if((k & 0x1F) != 0x1F)
				j=16*log2i(i)+log2i(~k & 0x1F);
		}

		if(j != key_old)
		{
			key_old=j;
			key_count=KEY_BOUNCE;
			key_slow=KEY_BOUNCE*10;
		}
		else
		{
			if(key_count)
				--key_count;
			else
				{
				key_count=key_slow;
				key=key_old;
				key_slow=KEY_BOUNCE*3;
				DecodeKey();
				}
		}
		if(PushCount)
			--PushCount;
		else
			if(PushKey)
			{
				Key=PushKey;
				PushKey=null;
			}
}
/*----------------------------------------------------------------------*/
int		log2i(int n)
{
		int	i=1;
		if(n==0)	return(0);
		while(n%2 == 0)
		{
			++i;
			n=n/2;
		}
		return(i);
	}
/*----------------------------------------------------------------------*/
/* shaft_off, izklop enkoderjev po 3 sek. neaktivnosti					*/
/*----------------------------------------------------------------------*/
void	shaft_off(void)
{
		shaft_buff |= 0x40;
		ENCw=shaft_buff;
		DeleteTask(shaft_off,RTC_task);
}
/*----------------------------------------------------------------------*/
/* Heidenhain ROC415, GRAY driver										*/
/*----------------------------------------------------------------------*/
void	shaft_ROC415G(void)
{
	int		i,j,k;
	long	a,e;
	char	c[64];


	if(shaft & 0x80)
	{
		if(shaft_buff & 0x40)
		{
			shaft_buff &= 0x80;					/* brisi vse + vklop	*/
			shaft_buff |= 0x09;					/* clock 1,2 = 1		*/
			ENCw=shaft_buff;					/* mirror				*/
			SetTask(shaft_ROC415G,10,PIT_task);
		}
		else
		{
			for(i=j=k=0; i< 16; ++i)
			{
				shaft_buff &= 0x80;
				shaft_buff |= 0x12;				/* clock 1,2= 0			*/
				ENCw=shaft_buff;
				shaft_buff &= 0x80;
				shaft_buff |= 0x09;				/* clock 1,2= 1			*/
				ENCw=shaft_buff;
				j = (j<<1) + ((~ENCr & 0x02)>>1);
				k = (k<<1) + ((~ENCr & 0x08)>>3);
			}
			for(a=e=0L,i=0; i<16; ++i)
			{
				a = (a<<1) | ((a ^ j) & 0x8000);
				j=(j<<1);
				e = (e<<1) | ((e ^ k) & 0x8000);
				k=(k<<1);
			}
			shaft_data[0]=e>>16;
			shaft_data[1]=a>>16;
			if(SHAFT_ECHO)
			{
				sprintf(c,"=e %d,%d\r\n",shaft_data[1],shaft_data[0]);
				puts_pc(c);
			}
			shaft &= 0x7f;
			SetTask(shaft_ROC415G,5,PIT_task);
		}
		SetTask(shaft_off,3,RTC_task);
	}
}
/*----------------------------------------------------------------------*/
/* Heidenhain ROC415, PURE BINARY driver								*/
/*----------------------------------------------------------------------*/
void	shaft_ROC415(void)
{
	uint	i,j,k,jj,kk,jjj,kkk,n[16];
	char	c[64];

	if(shaft & 0x80)
	{
		if(shaft_buff & 0x40)
		{
			shaft_buff &= 0x80;					/* brisi vse + vklop	*/
			shaft_buff |= 0x09;					/* clock 1,2 = 1		*/
			ENCw=shaft_buff;
			SetTask(shaft_ROC415,30,PIT_task);
		}
		else
		{
			for(i=0; i< 16; ++i)
			{
				ENCw=(shaft_buff & 0x80) | 0x12;/* clock 1,2= 0			*/
				ENCw=(shaft_buff & 0x80) | 0x09;/* clock 1,2= 1			*/
				n[i]=ENCr;
			}	
			j=jj=jjj=k=kk=kkk=0;
			for(i=0; i< 16; ++i)
			{
				j = (j<<1) | ((~n[i] & 0x02)>>1);
				k = (k<<1) | ((~n[i] & 0x08)>>3);
				jj = (jj<<1) | (n[i] & 0x01);
				kk = (kk<<1) | ((n[i] & 0x04)>>2);
				jjj ^= n[i] & 0x01;
				kkk ^= n[i] & 0x04;
			}
			if(j==jj && k==kk && !jjj && !kkk)
			{
				shaft_data[0]=k>>1;
				shaft_data[1]=j>>1;
				if(SHAFT_ECHO)
				{
					sprintf(c,"=e %d,%d\r\n",shaft_data[1],shaft_data[0]);
					puts_pc(c);
				}
				shaft &= 0x7f;
			}

			SetTask(shaft_ROC415,5,PIT_task);
		}
		SetTask(shaft_off,3,RTC_task);
	}	
}
/*----------------------------------------------------------------------*/
_ASM void disable_interrupts ()
{
	or	#$0700,sr
}

_ASM void enable_interrupts ()
{
	and	#$F8FF,sr
}
/*----------------------------------------------------------------------*/
/* V4.0 Heidenhain ROC417, driver										*/
/*----------------------------------------------------------------------*/
void	shaft_ROC417(void)
{
	uint	i,jjj,kkk,n[18],*nn,clkh,clkl;
	ulong	j,k,jj,kk;
	char	c[64];

	if(shaft & 0x80)
	{
		if(shaft_buff & 0x40)
		{
			shaft_buff &= 0x80;					/* brisi vse + vklop	*/
			shaft_buff |= 0x09;					/* clock 1,2 = 1		*/
			ENCw=shaft_buff;
			SetTask(shaft_ROC417,30,PIT_task);
		}
		else
		{
			nn=n;
			clkh=(shaft_buff & 0x80) | 0x12;
			clkl=(shaft_buff & 0x80) | 0x09;
			disable_interrupts ();
			for(i=0; i< 18; ++i)
			{ 
				ENCw=clkh;						/* clock 1,2= 0			*/
				ENCw=clkl;						/* clock 1,2= 1			*/
				*nn++=ENCr;
			}
			enable_interrupts ();
			j=jj=jjj=k=kk=kkk=0;
			for(i=0; i< 18; ++i)
			{
				j = (j<<1) | ((~n[i] & 0x02)>>1);
				k = (k<<1) | ((~n[i] & 0x08)>>3);
				jj = (jj<<1) | (n[i] & 0x01);
				kk = (kk<<1) | ((n[i] & 0x04)>>2);
				jjj ^= n[i] & 0x01;
				kkk ^= n[i] & 0x04;
			}
/*			if(j==jj && k==kk && !jjj && !kkk)
*/			if(!jjj && !kkk)
			{
				shaft_data[0]=kk>>3;
				shaft_data[1]=jj>>3;
				if(SHAFT_ECHO)
				{
					sprintf(c,"=e %d,%d,%d,%d\r\n",shaft_data[1],shaft_data[0],jjj,kkk);
					puts_pc(c);
				}
				shaft &= 0x7f;
			}

			SetTask(shaft_ROC417,5,PIT_task);
		}
		SetTask(shaft_off,3,RTC_task);
	}	
}
/*----------------------------------------------------------------------*/
int		BatteryOK(void)
		{
		return(PORTF & 0x80);
		}
/*----------------------------------------------------------------------*/
void	C100_on(void)
		{
		PORTE=PORTE | 0x40;
		c100.status &= ~0x80;
		DeleteTask(CheckRLD_off,RTC_task);
		DeleteTask(CheckRLD_on,RTC_task);
		DeleteTask(ScanRLD,RTC_task);
		SetTask(RLD_Remote,1,RTC_task);
		SetTask(RLD_PowerOff,1,RTC_task);
		RLD_PowerOff();		
		}
/*----------------------------------------------------------------------*/
void	C100_off(void)
		{
		PORTE=PORTE & 0xBF;
		rld.status = null;
		DeleteTask(RLD_Remote,RTC_task);
		DeleteTask(RLD_PowerOff,RTC_task);
		SetTask(CheckRLD_off,1,RTC_task);
		SetTask(ScanRLD,0,RTC_task);
		}
/*----------------------------------------------------------------------*/
void	watchdog(void)
		{
		SWSR=0x55;
		SWSR=0xAA;
		}
/*----------------------------------------------------------------------*/
int		GPS_on(void)
		{
int		i=PORTE;
		PORTE=PORTE & 0x7F;
		return((~i) & 0x80);
		}
/*----------------------------------------------------------------------*/
int		GPS_off(void)
		{
int		i=PORTE;
		PORTE=PORTE | 0x80;
		return(i & 0x80);
		}
/*----------------------------------------------------------------------*/
void	BacklitOff(void)
		{
		shaft_buff &= ~0x80;
		ENCw = shaft_buff;
        Backlit &= ~1;
		DeleteTask(BacklitOff,RTC_task);
		}
/*----------------------------------------------------------------------*/
void	BacklitOn(void)
		{
        if(Backlit/2)
			SetTask(BacklitOff,Backlit/2,RTC_task);
        Backlit |= 1;
		shaft_buff |= 0x80;
		ENCw = shaft_buff;
		}
/*----------------------------------------------------------------------*/
/*		Korekcije regulacije lcd na V3.0
		--------------------------------
		nivo kontrasta je v spodnjih 10 bitih
		zgornji 4 biti oznacujejo tip displaya
		0 - alpha
		1 - SED1553
		2 - S1D13700
		Samo pri tipu 1 se osvetljenost zmanjsuje s temperaturo
*/
int		LcdLevel(int i)
		{
		int j=i & 0x0fff;
		if(!j)
			return((i & 0xf000)+1);
		if(j >= CHaPAR3)
			return((i & 0xf000) + CHaPAR3-1);
		if(lcd & 0x1000)
			j -= (Tamb-25.0)/4.0;
		else
			j += (Tamb-25.0)/4.0;
		if(j>0 && j<CHaPAR3)	
			CHaPAR2=j;
		return(i);
		}
/*----------------------------------------------------------------------*/
void	device_off(void)
		{
/* alarm mask + powerdown, periodic = off !!!							*/
		writeRTC(0x32,0x40 | (readRTC(0x32) & 0x10));
		}
/*----------------------------------------------------------------------*/
void	HeatOn(void)
		{
		PORTC |= 0x01;
		}
/*----------------------------------------------------------------------*/
void	HeatOff(void)
		{
		PORTC &= ~0x01;
		}
/*----------------------------------------------------------------------*/
void	SystemInit(void)
		{
		void	code_top();
		void	(*p)()=code_top;
		long	*f=(long *)FLASHTOP;

		PIT_enable();
/*----------------------------------------------------------------------*/
		library=LIBRARY;
		fonts=FONTS;
		syspar=SYSPAR;
/* check for Am29F010	------------------------------------------------*/
		if(f[0] == f[0x10000])
			library=(char *)p;		/* konec kode, glej remflash		*/
/*----------------------------------------------------------------------*/
/*		PIT irq tasks													*/

		SetTask(ADC_scan,5,PIT_task);
		SetTask(key_scan,1,PIT_task);
		if(!shutdown_load(NULL))
		{
			LcdLevel(lcd);
			if(Backlit % 2)
	       		BacklitOn();
			else
    	   		BacklitOff();
		}
		initLCD();					/* V3.0	LCD inicializacija, mora	*/
		watchdog();					/* biti za shutdown_load !!!		*/
		}
/*----------------------------------------------------------------------*/
/* LCD driver															*/
/*----------------------------------------------------------------------*/
void		LCD_WRITE(int	port, unsigned char value)
{
	int	i;
			LCD[port]=value;
			if(port==1 && value==0x40)	/* V4.0							*/
				for(i=0; i<40;++i);		/* navadna zakasnitev, brez		*/
}										/* TPU interrupta				*/
/*----------------------------------------------------------------------*/
/* LCD driver, izpis slike v LcdBuffer na zaslon						*/
/* nivo PIT, inic. ob vsaki spremembi vsebine LcdBuffer					*/
/* SetLCDaddr, Cursor, putLCD in screen_swap skrajsajo					*/
/* klicni cas 5 tickov													*/
/*																		*/
/* V3.0																	*/
/*----------------------------------------------------------------------*/
void	initLCD(void)
{
		int				i;
		unsigned char	*p;
static	int	n=0;
		if(S1D13700 || SED1335)
		{
			if(!CheckTask(initLCD,PIT_task))	
				SetTask(initLCD,25,PIT_task);	/* initial delay 500mS	*/
			else
			{
				if(S1D13700)
					p=set_S1D13700;
				else
					p=set_SED1335;
				while(*p != 4)
				{
					if(*p == 2)
					{
						for(i=0;i<4096;i++)
							LCD_WRITE(0,aLcdFont[i]);
					}
					else
					{
						if(*p == 3)
							for(i=0;i<30*4;i++)
								LCD_WRITE(0,' ');
						else
							LCD_WRITE(p[0],p[1]);
					}
					++p;++p;
				}
				if(!(n = ++n % 3))
					DeleteTask(initLCD,PIT_task);
			}
		}
		else								/* initialize BT42008 driver */
		{
			LCD_WRITE(0,0x38);
			LCD_WRITE(0,0x01);
			LCD_WRITE(0,0x02);
			LCD_WRITE(0,0x06);
			LCD_WRITE(0,0x0C);

			symLCD(1,sym_DELTA);
			symLCD(2,sym_FOTONA);
			symLCD(3,sym_DEGREE);
			symLCD(4,sym_ARROWd);
			symLCD(5,sym_GPSf);
			symLCD(6,sym_dGPS);
			symLCD(7,sym_dGPSf);

			LCD_WRITE(0,0x80);
			LCD_WRITE(0,0x01);
			LCD_WRITE(0,0x02);
		}
}
/*----------------------------------------------------------------------*/
void	RefreshScreen(void)																	
{
static	int		n=0,inproc=0;
		int		Adec(uchar *, uchar *, int, int);
		int		i,j,k;
		uchar	p[120];

		if(CheckTask(initLCD,PIT_task))			/* skip until end of initLCD	*/
			return;
		if(!inproc)
		{
			++inproc;
			if(S1D13700 || SED1335)
			{
				for(i=0; i<4; ++i)
				{
					Adec(&LcdExt[8*i+7],&p[28*i+7],0,8);
					Adec(&LcdBuffer[20*i+19],&p[28*i+27],0,20);
				}

				LCD_WRITE(1,0x59);				/* cursor off					*/
				LCD_WRITE(0,0x04);

				LCD_WRITE(1,0x46);				/* zacetek vrstice				*/
				LCD_WRITE(0,0x00);
				LCD_WRITE(0,0x00);

				LCD_WRITE(1,0x42);				/* IZPIS NA LCD 				*/
				for(i=0; i<112; ++i)			/* od 0 do 111(4*28)			*/
					LCD_WRITE(0,p[i]);

				j=(signed char)LcdBuffer[80]/20;
				k=(signed char)LcdBuffer[80]%20;
				j=28*j+k+8;
				
				LCD_WRITE(1,0x46);				/* KURZOR 						*/
				LCD_WRITE(0,j);	
				LCD_WRITE(0,0x00);

				switch(LcdBuffer[81])			/* VRSTA KURZORJA				*/
				{
				case CURS_UND:
					 LCD_WRITE(1,0x5D);
					 LCD_WRITE(0,0x07);
					 LCD_WRITE(0,0x0f);
					 LCD_WRITE(1,0x59);
					 LCD_WRITE(0,0x07);
					break;
				case CURS_FULL:
					 LCD_WRITE(1,0x5D);
					 LCD_WRITE(0,0x07);
					 LCD_WRITE(0,0x8f);
					 LCD_WRITE(1,0x59);
					 LCD_WRITE(0,0x07);
					break;
				case CURS_OFF:
					 LCD_WRITE(1,0x59);
					 LCD_WRITE(0,0x04);
					break;
				}
			}
			else								/* BT42008 driver				*/
			{
				for(i=0; i<80; ++i)
				{
					if(!(i%20))
					{
						switch(i/20)
						{
						case 0:
							LCD_WRITE(0,0x80);
							break;
						case 1:
							LCD_WRITE(0,0x80+64);
							break;
						case 2:
							LCD_WRITE(0,0x80+20);
							break;
						case 3:
							LCD_WRITE(0,0x80+84);
							break;
						}
					}
					switch(LcdBuffer[i])
					{
					case symDELTA:
						LcdBuffer[i]=0x01;
						break;
					case symFOTONA:
						LcdBuffer[i]=0x02;
						break;
					case symDEGREE:
						LcdBuffer[i]=0x03;
						break;
					case symARROWd:
						LcdBuffer[i]=0x04;
						break;
					case symGPSf:
						LcdBuffer[i]=0x05;
						break;
					case symdGPS:
						LcdBuffer[i]=0x06;
						break;
					case symdGPSf:
						LcdBuffer[i]=0x07;
						break;
					}
					LCD_WRITE(1,LcdBuffer[i]);
				}
				i=GetLCDaddr();
				switch(i/20)
				{
				case 0:	
					break;
				case 1:	
					i=i+44;
					break;
				case 2:
					i=i-20;
					break;
				case 3:
					i=i+24;
				}
				LCD_WRITE(0,((i & 0x7F) | 0x80));
				switch(LcdBuffer[81])
				{
				case CURS_UND:
					LCD_WRITE(0,0x0E);
					break;
				case CURS_FULL:
					LCD_WRITE(0,0x0F);
					break;
				case CURS_OFF:
					LCD_WRITE(0,0x0C);
					break;
				}
			}
			--inproc;
			if(!(n = ++n % 3))
				DeleteTask(RefreshScreen,PIT_task);
		}
}
/*------------------------------------------------------------------*/
void	symLCD(int n,char *p)
{
	int	i, j;
	j=GetLCDaddr();
	LCD_WRITE(0,0x40 + ( n<<3 ));
	for(i=0; i<8; ++i)
		LCD_WRITE(1,p[i]);
	SetLCDaddr(j);
} 
/*------------------------------------------------------------------*/
_IH	void	reset_exc(void)
{
	reset_f();
}
/*------------------------------------------------------------------*/
void	Shutdown(void)
		{
		rtime	t1,t2;

		clrlcd();
		t2=t1=Time;
		addsecs(&t1, 3600.0 * readRTC(0) + 60.0 * readRTC(1) + readRTC(2));
		if(dtime(&t1,&t2))
			AlarmEnable(&t1);
		shutdown_save(NULL);
		SaveEE(NULL);
		DeleteTask(ScanRLD,RTC_task);
		DeleteTask(CheckRLD_on,RTC_task);
		DeleteTask(CheckRLD_off,RTC_task);
		DeleteTask(Shutdown,RTC_task);
		RLD_PowerOff();
		SysMessage(CHPC);
		wait(10);
		device_off();
		stopCPU();
		}
#endif