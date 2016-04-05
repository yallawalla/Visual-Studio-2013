#include	"MemMapFile.h"
#include	"remmenu.h"
#define		SECTLEN	0x20000
/*--------------------------------------------------------*/
HANDLE 	modem, mp;
/*--------------------------------------------------------*/

void	ADCexe(void);
void	IRQservice(void);
void	SysCtl(void);
void	ScanConsole(void);
/*--------------------------------------------------------*/
char	PORTE;
char	dsp_gain[4];
int		nrf,arf,Elevation,Azimuth,Pitch,Roll;
uint	rx_ram[32],tx_ram[32];
/*--------------------------------------------------------*/
void	RTCtime(void);

FILE	*fflash,*frtc;

uchar	rtc[64];
char	auxExtBuffer[256],auxLcdBuffer[256], auxLcdFlag;
/*--------------------------------------------------------*/
void	stack_top(void)
{
}
/*--------------------------------------------------------*/
void	SystemInit(void)
{
		long		i;
		char	c[256];
/*--------------------------------------------------------*/
		FILE *f=fopen("sysctlw.dat","w");
		if(f)
			fclose(f);

		modem = CreateFile(TEXT("modem"),
			GENERIC_READ | GENERIC_WRITE,
			FILE_SHARE_READ | FILE_SHARE_WRITE,
			NULL,  
			CREATE_NEW | CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
		mp = CreateFile(TEXT("mp"),
			GENERIC_READ | GENERIC_WRITE,
			FILE_SHARE_READ | FILE_SHARE_WRITE,
			NULL,  
			CREATE_NEW | CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
/*--------------------------------------------------------*/
		library=malloc(SECTLEN*sizeof(char));
		f=fopen("libflash.art","rb");
		if(f)
		{
			fread(library,sizeof(char),SECTLEN,f);
			fclose(f);
		}
		else
		{
			for(i=0; i<SECTLEN; ++i)
				library[i]=0xff;
		}
/*--------------------------------------------------------*/
		frtc=fopen("rtcW32.dat","rb");
		if(frtc)
		{
			fread(rtc,sizeof(char),64,frtc);
			fclose(frtc);
		}
/*--------------------------------------------------------*/
		clrlcd();
		for(i=0; i<256; ++i)
		{
			auxLcdBuffer[i]=' ';
			auxExtBuffer[i]=' ';
		}
		PIT_task=	SetTask(NULL,null,PIT_task);
		RTC_task=	SetTask(NULL,null,RTC_task);
		PIT_task=	SetTask(ADCexe,5,PIT_task);

		SetTask(RTCtime,0,RTC_task);
		shaft_enable();

// mišmaš zaradi inicializacije const *[] 
		for(i=0; _MenuCode[i]; ++i)
		{
			strcpy(c,_MenuCode[i]);
			_MenuCode[i]=memalloc((strlen(c)+1)*sizeof(char));
			strcpy(_MenuCode[i],c);
		}
		for(i=0; _TextCode[i]; ++i)
		{
			strcpy(c,_TextCode[i]);
			_TextCode[i]=memalloc((strlen(c)+1)*sizeof(char));
			strcpy(_TextCode[i],c);
		}
		for(i=0; _MenuHead[i]; ++i)
		{
			strcpy(c,_MenuHead[i]);
			_MenuHead[i]=memalloc((strlen(c)+1)*sizeof(char));
			strcpy(_MenuHead[i],c);
		}
		Initialize();
//		SetTask(SysCtl,0,RTC_task);
//		SetTask(ScanConsole,10,RTC_task);
}
/*--------------------------------------------------------*/
int		BatteryOK(void)
		{
		return(-1);
		}
/*--------------------------------------------------------*/
void	watchdog(void)
		{
		}
/*--------------------------------------------------------*/
#ifdef	WIN32
extern void (__stdcall *fcallback)(int, void *) ;
#endif
void	Shutdown(void)
		{
		//DeleteTask(Shutdown,RTC_task);
		//shutdown_save(NULL);
		//SaveEE(NULL);

		////frtc=fopen("rtcW32.dat","wb");
		////fwrite(rtc,sizeof(char),64,frtc);
		//fcloseall();
		//wait(100);
#ifdef	WIN32
		fcallback(-1, NULL) ;
#endif
		ExitThread(0);
		}
/*--------------------------------------------------------*/
void	beep(int i)
		{
		MessageBeep(0xFFFFFFFF);
		}
/*--------------------------------------------------------*/
int		GPS_on(void)
		{
int		i=PORTE;
		PORTE=PORTE & 0x7F;
		return((~i) & 0x80);
		}
/*------------------------------------------------------------------*/
int		GPS_off(void)
		{
int		i=PORTE;
		PORTE=PORTE | 0x80;
		return(i & 0x80);
		}
/*--------------------------------------------------------*/
int   	LcdLevel(int i)
		{
		lcd=i;
		return(i);
		}
/*--------------------------------------------------------*/
void	C100_on(void)
		{}
/*--------------------------------------------------------*/
void	C100_off(void)
		{}
/*--------------------------------------------------------*/
int		DspGain(int i, int j)
		{
		dsp_gain[i] += j;
		return(dsp_gain[i]);
		}
/*--------------------------------------------------------*/
void	DspAddr(unsigned int i)
		{
		arf=i;
		}
/*--------------------------------------------------------*/
void	DspHold(int t)
		{
		}
/*--------------------------------------------------------*/
int		dspinit(unsigned int i)
		{
		arf=i;
		return(null);
		}
//*--------------------------------------------------------*/
void	shaft_driver(void)
		{
        if(shaft & 0x80) {
        	shaft_data[0] = Elevation / SHtoR;
			shaft_data[1] = Azimuth / SHtoR;
			shaft &= 0x7f;
            }
        }
void	shaft_ITEK16uS(void)
		{
        shaft_driver();
		}
void	shaft_ROC415(void)
		{
        shaft_driver();
		}
void	shaft_ROC417(void)
		{
        shaft_driver();
		}
void	shaft_ROC415G(void)
		{
		shaft_driver();
		}
/*--------------------------------------------------------*/
void	key_scan(void)
		{}
/*--------------------------------------------------------*/
void	writeADC(void)
		{}
/*--------------------------------------------------------*/
void	sector_erase(uchar *p, int r)
{
	long	i;
	FILE	*f=fopen("libflash.art","wb");
	for(i=0; i<SECTLEN; ++i)
		library[i]=0xff;
	fwrite(library,sizeof(char),SECTLEN,f);
	fclose(f);
	}
/*--------------------------------------------------------*/
void	FileFlash(void)
{
	FILE	*f;
	f=fopen("libflash.art","wb");
	fwrite(library,sizeof(char),SECTLEN,f);
	fclose(f);
	DeleteTask(FileFlash,RTC_task);
	}
/*--------------------------------------------------------*/
void	*prog_byte(void *src,uchar *dst,size_t n)
{
	uchar	*from=src;
	while(n--)
	{
		if(dst == &library[SECTLEN])
		{
			eerror |= 0x80;
			continue;
		}
		if(*dst==*from)
		{
			++dst;
			++from;
			continue;
		}
		eerror |= 0x01;
		if(*dst == 0xff)
		{
			if(eerror & 0x80)
			{
				++dst;
				++from;
				continue;
			}
			else
			{
				*dst++ = *from++;
				eerror &= ~0x01;
			}
		}
		else
		{
			++dst;
			++from;
			eerror |= 0x80;
		}
	}
	SetTask(FileFlash,0,RTC_task);
return(dst);
}
/*--------------------------------------------------------*/
void	*load_byte(void *dst, uchar *src, size_t n)
{
		uchar	*to=dst;
			while(n--)
			*to++ = *src++;
		return(src);
}
/*--------------------------------------------------------*/
int		SendStat(int i)
{
		return(null);
}
/*--------------------------------------------------------*/
int		sendmp(char c)
{
DWORD	dwBytesWritten;
		if (mp != INVALID_HANDLE_VALUE)
			WriteFile (mp, (char *)&c, 1,&dwBytesWritten, NULL);
		return(null);
}
/*--------------------------------------------------------*/
int		recvmp(char *c)
{
		DWORD dwBytesRead;
		if (mp != INVALID_HANDLE_VALUE && ReadFile (mp, c, 1, &dwBytesRead, NULL) && *c)
			return(eof);
		else
			return(null);
}
/*--------------------------------------------------------*/
int		sendblk(unsigned int addr, char *c, int n, unsigned int type)
{
		DWORD dwBytesWritten;
		int	i=LinkAddr(OBP);
		if (modem != INVALID_HANDLE_VALUE) {
			WriteFile (modem, (char *)&addr, sizeof(int),&dwBytesWritten, NULL);
			WriteFile (modem, (char *)&n, sizeof(int),&dwBytesWritten, NULL);
			WriteFile (modem, (char *)&i, sizeof(int),&dwBytesWritten, NULL);
			WriteFile (modem, (char *)c, n,&dwBytesWritten, NULL);
		}
		return(null);
}
/*--------------------------------------------------------*/
int		recblk(unsigned int *a, char *p)
{
DWORD	dwBytesRead;
static	int		i=0,aa,nn,ss;
static	BOOLEAN	f=FALSE;
		int		*j=(int *)p;
		if (mp != INVALID_HANDLE_VALUE)
			while(ReadFile (mp, &p[i], 1, &dwBytesRead, NULL))
			{
				++i;
				if(!f)
				{
					if(i==3*sizeof(int))
					{
						aa=j[0];
						nn=j[1];
						ss=j[2];
						i=0;
						f=TRUE;
					}
				}
				else
				{
					if(i>=nn)
					{
						f=FALSE;
						i=0;
						if(aa ==  LinkAddr(OBP) || !aa)
						{
							*a=ss;
							return(nn);
						}
					}
				}
			}
		return(null);
}
/*--------------------------------------------------------
void   	ExtCtl(void)
{
static	int		i;
		char	c[128],cc[32];
		FILE	*f=_fsopen("\\fkey.dat","r",SH_DENYNO);
		if(f)
		{
			fseek(f,i,0);
			if(fgets(c,127,f))
			{
			    if(sscanf(c,"k=%s\n",cc))
					Key=chrtx(cc);
				if(sscanf(c,"p=%6d\n",&i))
		        	Pitch=(double)i*10.0;
			    if(sscanf(c,"r=%6d\n",&i))
					Roll=(double)i*10.0;
				if(sscanf(c,"e=%6d\n",&i))
		        	Elevation= fmod((double)i/10000.0+2.0*M_PI,2.0*M_PI)/M_PI*0x4000;
			    if(sscanf(c,"a=%6d\n",&i))
					Azimuth= fmod((double)i/10000.0+2.0*M_PI,2.0*M_PI)/M_PI*0x4000;
				if(sscanf(c,"c=%6d\n",&i))
		        	c100.az = fmod((double)i/10000.0+2.0*M_PI,2.0*M_PI)/M_PI*180.0;
				i=ftell(f);
			}
			fclose(f);
		}
}
--------------------------------------------------------*/
void	RTCtime(void)
		{
char	i=rtc[0x20];

		SYSTEMTIME tt;
		GetLocalTime(&tt);

		rtc[0x22]=toBCD(tt.wHour);
		rtc[0x21]=toBCD(tt.wMinute);
		rtc[0x20]=toBCD(tt.wSecond);
		rtc[0x23]=toBCD(tt.wDayOfWeek+1);
		rtc[0x24]=toBCD(tt.wDay);
		rtc[0x25]=toBCD(tt.wMonth);
		rtc[0x26]=toBCD(tt.wYear % 100);
		
		if(i != rtc[0x20])
			rtc[0x30] |= 0x01;
		else
			rtc[0x30] &= ~0x01;
		}
/*--------------------------------------------------------*/
void	ADCexe(void)
		{
extern	int	PITcnt;
void	ADC_scan(void);

		if(!(PITcnt % 2))	{
			rx_ram[1] = Roll  % 256;
			rx_ram[2] = Roll  / 256;
			rx_ram[4] = Pitch % 256;
			rx_ram[5] = Pitch / 256;
			}
		else {
			rx_ram[1] = 0;
			rx_ram[2] = 0;
			rx_ram[4] = 0;
			rx_ram[5] = 0;
			}
		ADC_scan();
}
/*--------------------------------------------------------*/
void	PIT_enable(void)
		{}
void	PIT_disable(void)
		{}
int		heat_on(void *vp)
		{
		return(null);
		}
int		heat_off(void *vp)
		{
		return(null);
		}
void	InProg(void)
		{}
/*--------------------------------------------------------*/
void	ADC_scan(void)
		{
#define	AVRG		8
extern	int			r_filt[AVRG];			/*					*/
extern	int			p_filt[AVRG];			/* filter podatkov	*/
extern	int			r_off[AVRG];			/*					*/
extern	int			p_off[AVRG];			/* offset A/D		*/
extern	int			PITcnt;

		if(PITcnt % 2) {
				p_off[PITcnt/2] = (rx_ram[5]<<8) + rx_ram[4];
				r_off[PITcnt/2] = (rx_ram[2]<<8) + rx_ram[1];
				tx_ram[2]=tx_ram[5]=0xD2;
				}
		else	{
				p_filt[PITcnt/2] = (rx_ram[5]<<8) + rx_ram[4];
				r_filt[PITcnt/2] = (rx_ram[2]<<8) + rx_ram[1];
				tx_ram[2]=tx_ram[5]=0xC2;
				}

		writeADC();
		PITcnt = ++PITcnt % (2 * AVRG);
		}
/*--------------------------------------------------------*/
int		writeRTC(int a,int dd)
		{
		rtc[a]=dd;
		return(dd);
		}
/*--------------------------------------------------------*/
int		readRTC(int a)
		{
		return(rtc[a]);
		}
/*--------------------------------------------------------*/
void	HeatOn(void)
		{}
/*--------------------------------------------------------*/
void	HeatOff(void)
		{}
/*--------------------------------------------------------*/
void	BacklitOn(void)
		{
		}
/*--------------------------------------------------------*/
void	BacklitOff(void)
		{
		}
/*--------------------------------------------------------*/
int		readTEMP(int a, int b)
		{
		return(a);
		}
/*--------------------------------------------------------*/
void	disable_interrupts(void)
{
}
void	enable_interrupts(void)
{
}
/*--------------------------------------------------------*/
double	getT(void)
		{
		return(35.0);
		}
/*--------------------------------------------------------*/
void	*ralloc(size_t n)
		{
        return(malloc(n));
        }
/*--------------------------------------------------------*/
void	rfree(void *p)
		{
        free(p);
		}
/*--------------------------------------------------------*/
void	initLCD(void)
{
}

/*--------------------------------------------------------*/
void (__stdcall *fcallback)(int, void *)=NULL;

void	RefreshScreen(void)
{
int	Adec(uchar *, uchar *, int, int);
	uchar	i;
	char	c[32];
	
	for(i=0; i<4; ++i)
	{
		Adec(&LcdBuffer[20*i+19],&auxLcdBuffer[20*i+19],0,20);
		Adec(&LcdExt[8*i+7],&auxExtBuffer[8*i+7],0,8);
		strncpy(c,&LcdBuffer[20*i],20);
		c[20]='\0';
	}

	auxLcdBuffer[80]=LcdBuffer[80];
	auxLcdBuffer[81]=LcdBuffer[81];
	SetTask(RefreshScreen,500,PIT_task);
	auxLcdFlag=1;
	if(fcallback != NULL) {
		fcallback(0,auxLcdBuffer);
		fcallback(1,LcdExt);
	}
}
/*--------------------------------------------------------*/
/* called when key is pressed */
void ButtonPress(int nNum)
{
	 Key=nNum;
}
/* get display data */
void GetDisplay(char p[])
{
	signed char i,j;
	i=(signed char)LcdBuffer[80]/20;
	j=(signed char)LcdBuffer[80]%20;


	p[125]=28*i+j+8;
	p[126]=auxLcdBuffer[81];

	for(i=0; i<4;++i)
	{
		Adec(&LcdExt[8*i+7],&p[28*i+7],0,8);
		Adec(&LcdBuffer[20*i+19],&p[28*i+27],0,20);
	}
}
