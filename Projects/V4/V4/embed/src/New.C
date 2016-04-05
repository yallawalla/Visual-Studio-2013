#include	"remmenu.h"
/*--------------------------------------------------------*/
int	ShowTg(void *vp)
{
	char	c[16];
	command *cp = Mscan->c;

	if(!LinkAddrText(cp->tg,c))
		return(1);
	if(!FindLib(cp->tg))
		return(1);
	SetMpos(0);
	msprint(c);
	SetMpos(7);
	msprint(":");
	return(null);
}
/*--------------------------------------------------------*/
int	DefAzimuth(void *vp)
{
	return(SetRefDir(vp));
}
/*--------------------------------------------------------*/
int	GunID(void *vp)
{
	return(SetGunref(vp));
}
/*--------------------------------------------------------*/
int	EnterEffect(void *vp)
{
	command	*c=Mscan->c;
	lib		*l=FindLib(c->tg);

	if(l)
	{
		if(Kbhit(CHLCD))
			switch(Getch(CHLCD))
			{
			case LEFT:
				if(c->effect)
					--c->effect;
				break;
			case RIGHT:
				++c->effect;
					break;
			}
			switch(c->effect)
			{
				case 0:
					msprint(EFF_DESTROY);
					break;	
				case 1:
					msprint(EFF_NEUTRALIZE);
					break;	
				case 2:
					msprint(EFF_SMOKE);
					break;
				default:
					c->effect=3;
				case 3: 
					msprint(EFF_ILLUMINATE);
						break;
			}
		}
	return(null);
}
/*--------------------------------------------------------*/
int	EnterTraj(void *vp)
{
	command	*c=Mscan->c;
	lib		*l=FindLib(c->to);

	if(Guns && l)
	{
		switch(Guns[l->gun].gmode & (TRAJ_UPPER | TRAJ_LOWER))
		{
		case TRAJ_LOWER:
			c->traj=0;
			break;
		case TRAJ_UPPER:
			c->traj=1;
		}
		if(Kbhit(CHLCD))
			switch(Getch(CHLCD))
			{
			case LEFT:
				if(c->traj && (Guns[l->gun].gmode & TRAJ_LOWER))
					--c->traj;
				break;
			case RIGHT:
				if(!c->traj && (Guns[l->gun].gmode & TRAJ_UPPER))
					++c->traj;
				break;
			}
		if(c->traj)
			msprint(TRAJ_HIGH);
		else
			msprint(TRAJ_LOW);
		}
	return(null);	
}
/*--------------------------------------------------------*/
int	EnterSect(void *vp)
{
	command	*c=Mscan->c;
	lib		*l=FindLib(c->to);
	char	cc[32];
	int		i;

	if(l) {
		if(Kbhit(CHLCD))
			switch(i=Getch(CHLCD))
			{
				case LEFT:
					if(c->section)
						--c->section;
					break;
				case RIGHT:
					++c->section;
						break;
				case REFRESH:
					break;
				case CLEAR:
					c->section &= 0xf;
					break;
				default:
					if(isdigit(i))
					{
						i -= '0';
						if(i > targets_n(l->x))
							i = targets_n(l->x);
						c->section &= 0xf;
						c->section |= i<<4;
					}
					break;
			}
			switch((c->section) & 0x0f)
			{
				case 0:	
					msprint(GUN_BATTERY);
					break;					/* baterija				*/
				case 1:	
					msprint(GUN_PLT1);		/* 1. vod				*/						/*						*/
					break;							/*						*/
				case 2: 
					msprint(GUN_PLT2);		/* 2.vod				*/
					break;							/*						*/
				case 3:
					if((c->section) >> 4)
						
						sprintf(cc,"%s %d",GUN_MESS,(c->section) >> 4);
					else
						sprintf(cc,"%s ...",GUN_MESS);
					strswap(cc," ");
					msprint(cc);
					break;							/* hi nibble			*/
				default:
					c->section &= 0xf0;
					c->section |= 3;
					return(EnterSect(vp));
			}
	}
	return(null);
}
/*--------------------------------------------------------*/
int	EnterFuze(void *vp)
	{
	command	*c=Mscan->c;
	lib		*l=FindLib(c->to);
	int		i;

	if(Guns && l)
	{
		for(i=0; i <= c->fuze; ++i)
		{
			if(!Guns[l->gun].fuze[c->ammo][i])
				break;
		}
		c->fuze = --i;
			
		if(Kbhit(CHLCD))
			switch(Getch(CHLCD))
			{
			case LEFT:
				if(c->fuze)
					--c->fuze;
				break;
			case RIGHT:
				if(!Guns[l->gun].fuze[c->ammo][++c->fuze])
					--c->fuze;
				break;
			}
		msprint(Guns[l->gun].fuze[c->ammo][c->fuze]);
	}
	return(null);
}
/*--------------------------------------------------------*/
int	EnterCharge(void *vp)
{
	int		i;
	command	*c=Mscan->c;
	lib		*l=FindLib(c->to);
	char	cc[32];
	if(Guns && l)
	{
		for(i=0; i < c->charge; ++i)
		{
			if(!Guns[l->gun].mv[c->ammo][i+1])
				break;
		}
		c->charge = i;

		if(Kbhit(CHLCD))
			switch(Getch(CHLCD))
			{
			case LEFT:
				if(c->charge)
				{
					--c->charge;
					blll=freelib(blll);
				}
			break;
			case RIGHT:
				if(!Guns[l->gun].mv[c->ammo][++c->charge])
					--c->charge;
				else
					blll=freelib(blll);
				break;
			}
			sprintf(cc,"\x86");
			for(i=c->charge;i>0;--i)
				sprintf(strchr(cc,0),"%c",'+');
			if(Guns[l->gun].charge[c->ammo])
				if(Guns[l->gun].charge[c->ammo][c->charge])
				   sprintf(cc,"%s",Guns[l->gun].charge[c->ammo][c->charge]);
			strswap(cc,"\x86");
				msprint(cc);
	}
return(null);
}
/*--------------------------------------------------------*/
int	EnterRounds(void *vp)
{
	command	*c=Mscan->c;
	lib		*l=FindLib(c->to);
	char	cc[32];
	int		i;

	if(l)
	{
		if(Kbhit(CHLCD))
		switch(i=Getch(CHLCD))
		{
			case LEFT:
				if(c->fmode)
					--c->fmode;
				break;
			case RIGHT:	
				++c->fmode;
				break;
			case REFRESH:
				break;
			case CLEAR:
				c->rounds = 0;
				break;
			default:
				if(isdigit(i))
				{
					i += c->rounds * 10 - '0';
					if(i <= 250)
						c->rounds = i;
				}
				break;
		}
		switch(c->fmode)
		{
			case 0:	
			case 1:	
			case 2:
			default:
				c->fmode = 2;
		}
		i = c->rounds;
		if(i)
			sprintf(cc,"%d",i);
		else
			sprintf(cc,"...");
		msprint(cc);
	}
	return(null);
}
/*--------------------------------------------------------*/
int	EnterInterval(void *vp)
{
	command	*c=Mscan->c;
	lib		*l=FindLib(c->to);
	char	cc[32];
	int		i;

	if(l)
	{
		if(Kbhit(CHLCD))
			switch(i=Getch(CHLCD))
			{
			case REFRESH:
				break;
			case CLEAR:
				c->interval = 0;
				break;
				default:
					if(isdigit(i))
					{
					i += c->interval * 10 - '0';
					if(i <= 250)
						c->interval = i;
					}
					break;
			}
		i = c->interval;
		if(i)
		{
			sprintf(cc,"%d %s",i,M_SECOND);
			strswap(cc," ");
			msprint(cc);
		}
		else
			msprint("...");
	}
	return(null);
}
/*--------------------------------------------------------*/
int	EnterProj(void *vp)
{
	return(SetAmm(vp));
}
/*--------------------------------------------------------*/
int	EnterFiretime(void *vp)
{
	command	*c=Mscan->c;
	char	cc[32];
	int		i;

	if(Kbhit(CHLCD))
		switch(i=Getch(CHLCD))
		{
			case LEFT:
				if(c->tmode)
					--c->tmode;
				break;
			case RIGHT:
				++c->tmode;
				break;
			case REFRESH:
				break;
			default:
				Ungetch(i,0);
		}
		i=GetMpos();
		switch(c->tmode)
		{
			case 0:
				strcpy(cc,FIRE_HOLD);
				break;
			case 1:
				strcpy(cc,FIRE_WHEN_READY);
				break;
			case 2:
				i += ReadHMS(&c->ftime);
				sprintf(cc,"%02d:%02d:%02d",c->ftime.hour,c->ftime.min,c->ftime.sec);
				break;
			default:
				c->tmode = 3;
			case 3:	
				if(dtime(&c->ftime,&Time) > 0)
				{
					calc_rtime(0,dtime(&c->ftime,&Time),&c->ftime);
					i += ReadHMS(&c->ftime);
					if(!c->ftime.hour && !c->ftime.min && !c->ftime.sec)
						addsecs(&c->ftime,track_delay);
					sprintf(cc,"%02d:%02d:%02d",c->ftime.hour,c->ftime.min,c->ftime.sec);
					addtime(&Time,&c->ftime);
				}
				else
				{
					i += ReadHMS(&c->ftime);
					if(!c->ftime.hour && !c->ftime.min && !c->ftime.sec) {
						c->ftime = Time;
					addsecs(&c->ftime,track_delay);
					if(track_delay)
						return(EnterFiretime(vp));
					}
					sprintf(cc," ---");
				}
		}
/*		strswap(cc,"::");
*/		msprint(cc);
		SetMpos(i);
		return(null);
}
/*--------------------------------------------------------*/
void	Cursor(char i)
{
	LcdBuffer[81]=i;
	SetTask(RefreshScreen,3,PIT_task);
}
/*--------------------------------------------------------*/
void	xyLCD(int x, int y)
{	
	if(x > 19)
		x = 19;
	if(y > 3)
		y = 3;
	SetLCDaddr(y*20+x);
}
/*--------------------------------------------------------*/
void	SetLCDaddr(int i)
{
	LcdBuffer[80]=i;
	SetTask(RefreshScreen,3,PIT_task);
}
/*--------------------------------------------------------*/
int		GetLCDaddr(void)
{
	return(LcdBuffer[80]);
}
/*--------------------------------------------------------*/
void	screen_unswap(void)
{
	DeleteTask(screen_unswap,PIT_task);
	memcpy(LcdBuffer,LcdBackup,80);
/*	WriteSophie(NULL);
*/	SetTask(RefreshScreen,3,PIT_task);
}
/*--------------------------------------------------------*/
void	screen_swap(int	n)
{
	int		i;

	if(!CheckTask(screen_unswap,PIT_task))
	{
		memcpy(LcdBackup,LcdBuffer,80);
		for(i=0; i<4; ++i)
			memcpy(&LcdBuffer[20*i],error_screen[i],20);
	}
	SetTask(RefreshScreen,3,PIT_task);
	SetTask(screen_unswap,n,PIT_task);
}
/*--------------------------------------------------------*/
int		put_lcd(int i)
{
	char	*p;
	if(CheckTask(BacklitOff,RTC_task))
		SetTask(BacklitOff,Backlit/2,RTC_task);
	if(CheckTask(CheckScreen,RTC_task))
		SetTask(CheckScreen,0,RTC_task);
	if(!CheckTask(screen_unswap,PIT_task))
       	p = LcdBuffer;
    else
       	p = LcdBackup;
	if(LcdBuffer[80]<80)
		p[LcdBuffer[80]++]=i;
	SetTask(RefreshScreen,3,PIT_task);
	return(i);
}
/*--------------------------------------------------------*/
void	clrlcd(void)
{
	int	i;
	if(!CheckTask(screen_unswap,PIT_task))
	{
		for(i=0; i<80; ++i)
			LcdBuffer[i]=' ';
		for(i=0; i<32; ++i)
			LcdExt[i]=' ';
	}
	else
		for(i=0; i<80; ++i)
			LcdBackup[i]=' ';
	Cursor(CURS_OFF);
	SetTask(RefreshScreen,3,PIT_task);
}
/*--------------------------------------------------------*/
/*	Kodna tabela fontov!
	Samostojna, leva, srednja desna
	Prvih enajst znakov se ne veže z naslednjim (levim)
	Driver Adec
	Èe hoèeš zaporedne samostojne znake, vpišeš zaèetni (desni) 
	znak in Adec vrne samostojne!
*/
uchar	Atab[][4]=
{
	{0xC2,0x02,0x02,0xC2},
	{0xC3,0x01,0x01,0xC3},
	{0xC5,0x03,0x03,0xC5},
	{0xC7,0x00,0x00,0xC7},
	{0xD0,0xD0,0xD0,0xD0},
	{0xD1,0xD1,0xD1,0xD1},
	{0xD2,0xD2,0xD2,0xD2},
	{0xCF,0xCF,0xCF,0xCF},
	{0xE6,0xE6,0xE6,0xE6},
	{0xC4,0xC4,0xC4,0xC4},
	{0xC1,0xC1,0xC1,0xC1},
	{0xA5,0xAA,0xAA,0xA5},
	{0xB2,0xB3,0xB3,0xB2},
	{0xB4,0xB9,0xB9,0xB4},
	{0xFC,0xFF,0xFF,0xFC},

	{0xC8,0xC8,0x04,0x04},
	{0xCA,0xCA,0x05,0x05},
	{0xCB,0xCB,0x06,0x06},
	{0xCC,0x08,0x07,0x07},
	{0xCD,0x0A,0x09,0x09},
	{0xCE,0x0C,0x0B,0x0B},
	{0xD8,0xD8,0xD8,0xD8},
	{0xD9,0xD9,0xD9,0xD9},
	{0xDE,0xDE,0x8A,0x83},
	{0xDD,0xDD,0x81,0x80},
	{0xE1,0xE1,0x8D,0x8D},
	{0xDF,0xDF,0x8C,0x8C},
	{0xE4,0xE4,0x8F,0x8F},
	{0xE3,0xE3,0x8E,0x8E},
	{0xEC,0xA0,0x9A,0x9A},
	{0xED,0x9C,0x9A,0x9A},
	{0xC6,0xB5,0xA2,0xA2},
	{0xE5,0x98,0x95,0x90},
	{0xC9,0x9F,0x95,0x90},
	{0xD3,0xD3,0x0D,0x0D},
	{0xD4,0xD4,0x0E,0x0E},
	{0xD5,0xD5,0x0F,0x0F},
	{0xD6,0xD6,0x1A,0x1A},
	{0xDA,0x1D,0x1C,0x1B},
	{0xDB,0x7F,0x1F,0x1E},

	{0,0,0,0}
};
/*--------------------------------------------------------*/
int	Adec(uchar *from, uchar *to, int flag, int n)
{
	int	i;
	char *p;
	if(n)
	{
		for(i=0; Atab[i][0]; ++i)
		{
			if(*from == Atab[i][0])
			{
/* !!! */		if(i<15)
				{
					if(flag)
						*to=Atab[i][1];
					else
						*to=Atab[i][0];
					Adec(--from, --to, 0, --n);
				}
				else
				{
					if(Adec(--from, --to, Atab[i][0], --n))
						if(flag)
							*++to = Atab[i][2];
						else
							*++to = Atab[i][3];
					else
						if(flag)
							*++to=Atab[i][1];
						else
							*++to=Atab[i][0];
				}
				return(eof);
			}
			if(*from == Atab[i][3])
			{
				*to=Atab[i][0];
				Adec(--from, --to, 0, --n);
				return(null);
			}
		}

		if(*from >= '0' && *from <= '9')
		{
			p=KEY_NUMERIC;
			i=*from-'0';
			while(i--)
				while(*p++ != '/');
			*to=*p;
		}
		else
			*to=*from;
		Adec(--from,--to,0,--n);
	}
	return(null);
}
/*--------------------------------------------------------*/
menu	Testmenu;
command	*Testcomm;
/*--------------------------------------------------------*/
void	EditLmenu(int n)
{
	int		i,j,z,zz,x=0;
	char	p[128],q[128],m[16],h[16],*mm[32],*mmm[32];


	strcpy(q,Mscan->text[0]);
	Cursor(CURS_UND);
	if(RightToLeft)
		xyLCD(20,0);
	else
		xyLCD(12,0);
	do
	{
		j=Getch(CHLCD);
		x=EditStr(j,q,x);
		xyLCD(0,0);
		puts_lcd(q);
		if(RightToLeft)
			xyLCD(strlen(q)-x-1,0);
		else
			xyLCD(x,0);
	} while(j!=ENTER);
	strscan(q,mm,':',2);
	if(RightToLeft)
	{
		strcpy(m,mm[0]);
		strcpy(h,mm[1]);
	}
	else
	{
		strcpy(m,mm[1]);
		strcpy(h,mm[0]);
	}
	z=strscan(_MenuHead[n],mmm,'|',32);
	if(language > z-1)
		zz=z-1;
	else
		zz=language;
	mmm[zz]=h;
	for(i=0,p[0]=0; i<z; ++i)
	{
		if(i)
			strcat(p,"|");
		strcat(p,mmm[i]);
	}
	_MenuHead[n]=memalloc((strlen(p)+1)*sizeof(char));
	strcpy(_MenuHead[n],p);

	z=strscan(_MenuCode[n],mmm,'|',32);
	i=strscan(mmm[zz],mm,' ',32);
	if(strcmp(mm[Mscan->c->opt[0]],"/"))
	{
		mm[Mscan->c->opt[0]]=m;
		for(j=0,p[0]=0; j<i; ++j)
		{
			if(j)
				strcat(p," ");
			strcat(p,mm[j]);
		}
		mmm[zz]=p;
		for(i=0,q[0]=0; i<z; ++i)
		{
			if(i)
				strcat(q,"|");
			strcat(q,mmm[i]);
		}
		_MenuCode[n]=memalloc((strlen(q)+1)*sizeof(char));
		strcpy(_MenuCode[n],q);
	}
}
/*--------------------------------------------------------*/
void	EditText(int n)
{
	int		i,j,z,zz,x=0;
	char	p[128],q[128],*mm[32],*mmm[32];

	sprintf(q,"%20s",GetMessCode(n));
	Cursor(CURS_UND);
	if(RightToLeft)
		xyLCD(20,0);
	else
		xyLCD(0,0);
	do
	{
		j=Getch(CHLCD);
		x=EditStr(j,q,x);
		xyLCD(0,0);
		puts_lcd(q);
		if(RightToLeft)
			xyLCD(strlen(q)-x-1,0);
		else
			xyLCD(x,0);
	} while(j!=ENTER);
	strscan(q,mm,'|',32);				/* samo precisti blanke*/
	z=strscan(_TextCode[n],mmm,'|',32);
	if(language > z-1)
		zz=z-1;
	else
		zz=language;
	mmm[zz]=mm[0];
	for(i=0,p[0]=0; i<z; ++i)
	{
		if(i)
			strcat(p,"|");
		strcat(p,mmm[i]);
	}
	_TextCode[n]=memalloc((strlen(p)+1)*sizeof(char));
	strcpy(_TextCode[n],p);
}
/*--------------------------------------------------------*/
void	EditSmenu(int n)
{
	int		i,j,k,l,z,zz,x=0,y=0;
	char	p[128],q[128],m[4][16],*mm[32],*mmm[32],b1[4][16],b2[4][16];

	z=strscan(_MenuCode[n],mmm,'|',32);
	if(language > z-1)
		zz=z-1;
	else
		zz=language;
	strcpy(q,mmm[zz]);
	i=strscan(q,mm,' ',32);
	for(j=0;j<4;++j)
		sprintf(m[j],"%8s",mm[j]);
	Cursor(CURS_UND);
	if(RightToLeft)
		xyLCD(20,0);
	else
		xyLCD(12,0);
	do
	{
		j=Getch(CHLCD);
		switch(j)
		{
		case UP:
			if(y)
				--y;
			break;
		case DOWN:
			if(y<3)
				++y;
			break;
		default:
			if(strcmp(m[y],"/"))
			{
				x=EditStr(j,m[y],x);
				for(k=0; k<4; ++k)
				{
					xyLCD(12,k);
					puts_lcd(m[k]);
				}
			}
		}
		if(RightToLeft)
			xyLCD(12+strlen(m[y])-x-1,y);
		else
			xyLCD(12+x,y);
	} while(j!=ENTER);
	
	clrlcd();
	for(j=0;j<4;++j)				/* backup v b1	*/
		strcpy(b1[j],mm[j]);
	for(j=0;j<4;++j)
		strscan(m[j],&mm[j],' ',1);	/* precisti b2	*/
	for(j=0;j<4;++j)
		strcpy(b2[j],mm[j]);

	sprintf(p,"%s",mm[0]);
	for(j=1;j<i;++j)
		sprintf(strchr(p,0)," %s",mm[j]);

	for(j=k=0; k<z;++k)
		if(k==zz)
			k+=strlen(p);
		else
			k+=strlen(mmm[j]);
	_MenuCode[n]=memalloc(k+1);
	*_MenuCode[n]=0;
	for(j=0; j<z;++j)
	{
		if(j)
			strcat(_MenuCode[n],"|");
		if(j==zz)
			strcat(_MenuCode[n],p);
		else
			strcat(_MenuCode[n],mmm[j]);
	}

	for(i=0; _MenuCode[i]; ++i)
	{
		z=strscan(_MenuCode[i],mmm,'|',32);
		strcpy(q,mmm[zz]);
		j=strscan(q,mm,' ',32);
		for(k=0; k<j; ++k)
			for(l=0; l<4; ++l)
				if(!strcmp(mm[k],b1[l]))
					mm[k]=b2[l];

		sprintf(p,"%s",mm[0]);
		for(k=1;k<j;++k)
			sprintf(strchr(p,0)," %s",mm[k]);
		for(j=k=0; j<z;++j)
			if(i==zz)
				k+=strlen(p);
			else
				k+=strlen(mmm[j]);
		_MenuCode[i]=memalloc(k+1);
		*_MenuCode[i]=0;
		for(j=0; j<z;++j)
		{
			if(j)
				strcat(_MenuCode[i],"|");
			if(j==zz)
				strcat(_MenuCode[i],p);
			else
				strcat(_MenuCode[i],mmm[j]);
		}
	}
}
/*--------------------------------------------------------*/
int	TextView(void)
{
	int		i,n=0;
	char	p[128];

	while(1)
	{
		clrlcd();		
		for(i=0; i<4; ++i)
			if(_TextCode[n+i])
			{
				sprintf(p,"%20s",GetMessCode(n+i));
				p[20]=0;
				xyLCD(0,i);
				puts_lcd(p);
			}
		switch(i=Getch(CHLCD))
		{
		case ALPHA:
			clrlcd();
			return(eof);
		case UP:
			if(n)
				--n;
			else
				return(null);
			break;
		case DOWN:
			if(_TextCode[n+1])
				++n;
			break;
		case ENTER:
			EditText(n);
			break;
		default:
			if(isdigit(i))
				language=i-'0';
		break;
		}
	}
	return(eof);
}
/*--------------------------------------------------------*/
int LmenuView(void)
{
	int	i,j;
	comm_enable(&Testcomm);	
	clrlcd();
	j=Testmenu.n;
	for(i=0; i<4; ++i)
	{
		if(!_MenuHead[Testmenu.n])
			break;
		if(*_MenuHead[Testmenu.n] && *_MenuHead[Testmenu.n] != '|' )
		{
			lmenu(&Testmenu);
			++Testmenu.n;
		}
		else
			break;
	}			
	for(i=0;i<4 && i<= Mscan->level; ++i)
	{
		xyLCD(0,i);
		puts_lcd(Mscan->text[i]);
	}
	Testmenu.n=j;
	return(null);
}
/*--------------------------------------------------------*/
int	MenuView(void *vp)
{
	int		i;
	comm_enable(&Testcomm);	
	while(1)
	{
		clrlcd();		
		if(_MenuHead[Testmenu.n])
			if(*_MenuHead[Testmenu.n] && *_MenuHead[Testmenu.n]!= '|' )
				LmenuView();
			else
				smenu(&Testmenu);
		switch(i=Getch(CHLCD))
		{
		case ENTER:
			if(*_MenuHead[Testmenu.n] && *_MenuHead[Testmenu.n]!= '|' )
				EditLmenu(Testmenu.n);
			else
				EditSmenu(Testmenu.n);
			break;
		case ALPHA:
			clrlcd();
			Testmenu.n=0;
			return(eof);
		case UP:
			if(Testmenu.n)
				--Testmenu.n;
			break;
		case DOWN:
			if(_MenuHead[Testmenu.n+1])
				++Testmenu.n;
			else
				Ungetch(TextView(),0);
			break;
		case LEFT:
			for(i=0; i<MAXOPT; ++i)
				if(Testcomm->opt[i])
					--Testcomm->opt[i];
			break;
		case RIGHT:
			for(i=0; i<MAXOPT; ++i)
				++Testcomm->opt[i];
			break;
		default:
			if(isdigit(i))
				language=i-'0';
		break;
		}
	}
	return(eof);
}
/*----------------------------------------------------------*/
void	SophieRepeat(void)
{
	WriteSophie(SophieBuff);
}
/*----------------------------------------------------------*/
void WriteSophie (char *c)
{
	char	p[128],b=0x3F;
	uint i=0, j=0, chk=0xCA ^ 0x11 ^ 0x20;

	if(rld_mode & 0x200)
	{
		if(c)
		{
			if(c != SophieBuff)
			{
				strcpy(SophieBuff,c);
				DeleteTask(SophieRepeat,RTC_task);
			}
		}
		else
		{
			getLCD(p);
			strncpy(&SophieBuff[0],&p[0],7);
			strncpy(&SophieBuff[7],&p[20],7);
			strncpy(&SophieBuff[14],&p[40],7);
			SophieBuff[21]=0;
			SetTask(SophieRepeat,3,RTC_task);
		}
		p[j++]=0xCA;
		p[j++]=0x11;
		p[j++]=0x20;
			while(j<45)
		{
			if(SophieBuff[i])
			{
				if(SophieBuff[i]==' ')
					SophieBuff[i]=b;
				else
					b=' ';
				p[j++]=toupper(SophieBuff[i]);
				chk ^= toupper(SophieBuff[i++]);
			}
			else
			{
				p[j++]=0x3F;
				chk ^= 0x3F;
			}
		}
		p[j++]=chk;
		p[j++]=0xF1;

		for(i=0;i<j;++i)
			Putch(p[i],CHIR);
	}
}
/*----------------------------------------------------------*/
void	SophieReticle(void)
{
	int		i;
	char	c[]={0xCA,0x03,0xA9,0x60,0xF1};
	c[3]	=0xCA^0x03^0xA9;

	if(rld_mode & 0x200)			
		for(i=0;i<5;++i)
			Putch(c[i],CHIR);
}
/************************************************/
/* prepise string v flash ali na ser. port.		*/
/* Biti mora v RAMU !!!							*/
void	_writegun(char *c, uchar **f)
{
	if(f)
	{
		strcat(c,"\r");
		*f=prog_byte(c,*f,strlen(c)*sizeof(char));
	}
	else
	{
		strcat(c,"\r\n");
		puts_pc(c);
	}
}
/************************************************/
void	WriteGunData(gun *g, uchar **f)
{
	int		i,j,k,m;
	char	c[256],*p;

	if(g)
	{
		if(!f)
		{
			sprintf(c,"=G ");
			for(i=0; g[i].name; ++i)
			{
				for(j=0; g[i].ammo[j]; ++j);
				sprintf(strchr(c,0),"%d,",j);
			}
			p=strchr(c,0);
			*--p=0;
			_writegun(c,f);
		}
		for(i=0; g[i].name; ++i)
		{
			sprintf(c,"G %s,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%.3lf,%.lf",
				g[i].name,g[i].left,g[i].right,g[i].hi,g[i].lo,g[i].mils,
					g[i].zeroA,g[i].scaleA,
						g[i].zeroE,g[i].scaleE,
							g[i].gmode,g[i].mcal,g[i].rifle);
			_writegun(c,f);
			for(j=0; g[i].ammo[j]; ++j)
			{
				sprintf(c,"A %s,%.1lf,%.3lf",g[i].ammo[j],g[i].mass[j],g[i].dmass[j]);
				_writegun(c,f);
				sprintf(c,"F ");
				k=0; 
				do
					sprintf(strchr(c,0),"%s,", g[i].fuze[j][k++]);
				while(g[i].fuze[j][k]);
				k=0; 
				do
				{
					sprintf(strchr(c,0),"%d", g[i].alt[j][k]);
					++k;
					if(g[i].fuze[j][k])
						sprintf(strchr(c,0),",");
				} while(g[i].fuze[j][k]);
				_writegun(c,f);
				for(k=0; g[i].mv[j][k]; ++k)
				{
					sprintf(c,"M %.1lf",g[i].mv[j][k]);
					if(g[i].charge[j])
						if(g[i].charge[j][k])
							sprintf(strchr(c,0),",%s",g[i].charge[j][k]);
					_writegun(c,f);
					sprintf(c,"C %lf,%lf",g[i].coeff[j][k][1],g[i].coeff[j][k][2]);
					for(m=0; m < g[i].coeff[j][k][0]; ++m)
						sprintf(strchr(c,0),",%lf",g[i].coeff[j][k][m+3]);
					_writegun(c,f);
					if(g[i].drift)
					{
						sprintf(c,"D %.6lf,%.6lf,%.6lf",
											g[i].drift[j][k][0],
											g[i].drift[j][k][1],
											g[i].drift[j][k][2]);

						if(g[i].dvtemp[j][k])
						{
							sprintf(strchr(c,0),",%.6lf,%.6lf,%.6lf,%.6lf,%.6lf",
											g[i].dvmass[j][k],
											g[i].dvtemp[j][k][0],
											g[i].dvtemp[j][k][1],
											g[i].dvtemp[j][k][2],
											g[i].dvtemp[j][k][3]);
						}

						_writegun(c,f);
					}
					if(g[i].ABC)
					{
						sprintf(c,"Y %.6lf,%.6lf,%.6lf,%.6lf,%.6lf,%.6lf",g[i].ABC[j][k][0],
											g[i].ABC[j][k][1],
											g[i].ABC[j][k][2],
											g[i].ABC[j][k][3],
											g[i].ABC[j][k][4],
											g[i].ABC[j][k][5]);
						_writegun(c,f);
					}
				}
			}
		}
	}
	sprintf(c,"=G");
	_writegun(c,f);
}
/*------------------------------------------------------*/
gun	NullGun;

gun	*ReadGunData (int ng, int na, int nc, int nr, uchar **f)
{
	char	cc[256],*c[16],*p;
	signed	char	i,j,k;
	gun		*g=NULL;
	p=cc;
	do
	{
		if(f)
			*f=load_byte(&i,*f,sizeof(char));
		else
		{
			while(!Kbhit(CHPC))
				RTCservice();
			i=Getch(CHPC);
		}
		switch(i)
		{
		case 0x0D:
		case 0x0A:
			if(p==cc)
				break;
			else
				*p=0;
			i=strscan(cc,c,',',16);
			c[0]=strchr(c[0],' ');
			++c[0];
			switch(cc[0])
			{
			case 'G':
				g=ReadGunData(ng+1,0,0,0,f);
				g[ng].name=memalloc(strlen(c[0])+1);
				strcpy(g[ng].name,c[0]);
				g[ng].left=atoi(c[1]);
				g[ng].right=atoi(c[2]);
				g[ng].hi=atoi(c[3]);
				g[ng].lo=atoi(c[4]);
				g[ng].mils=atoi(c[5]);
				g[ng].zeroA=atoi(c[6]);
				g[ng].scaleA=atoi(c[7]);
				g[ng].zeroE=atoi(c[8]);
				g[ng].scaleE=atoi(c[9]);
				g[ng].gmode=atoi(c[10]);
				g[ng].mcal=atof(c[11]);
				g[ng].rifle=atof(c[12]);
				break;
			case 'A':
				g=ReadGunData(ng,na+1,0,0,f);
				g[ng-1].ammo[na]=memalloc(strlen(c[0])+1);
				strcpy(g[ng-1].ammo[na],c[0]);
				g[ng-1].mass[na]=atof(c[1]);
				g[ng-1].dmass[na]=atof(c[2]);
				break;
			case 'M':
				g=ReadGunData(ng,na,nc+1,0,f);
				g[ng-1].mv[na-1][nc]=atof(c[0]);
				if(i>1 && *c[1])
				{
					g[ng-1].charge[na-1][nc]=memalloc(strlen(c[1])+1);
					strcpy(g[ng-1].charge[na-1][nc],c[1]);
				}
				break;
			case 'C':
				g=ReadGunData(ng,na,nc,nr+1,f);
				g[ng-1].coeff[na-1][nc-1]=memalloc((i+1)*sizeof(double));
				g[ng-1].coeff[na-1][nc-1][0]=i-2;
				while(i--)
					g[ng-1].coeff[na-1][nc-1][i+1]=atof(c[i]);
				break;
			case 'D':
				g=ReadGunData(ng,na,nc,nr,f);
				if(i==8)
				{
					g[ng-1].dvmass[na-1][nc-1]=atof(c[i-5]);
					g[ng-1].dvtemp[na-1][nc-1]=memalloc(4*sizeof(double));
					g[ng-1].dvtemp[na-1][nc-1][0]=atof(c[i-4]);
					g[ng-1].dvtemp[na-1][nc-1][1]=atof(c[i-3]);
					g[ng-1].dvtemp[na-1][nc-1][2]=atof(c[i-2]);
					g[ng-1].dvtemp[na-1][nc-1][3]=atof(c[i-1]);
				}
				if(!g[ng-1].drift)
				{
					g[ng-1].drift=memalloc(na*sizeof(double **));
					for(i=0; i<na; ++i)
						g[ng-1].drift[i]=NULL;
				}
				if(!g[ng-1].drift[na-1])
				{
					g[ng-1].drift[na-1]=memalloc((nc)*sizeof(double *));
					for(i=0; i<nc; ++i)
						g[ng-1].drift[na-1][i]=memalloc(3*sizeof(double));
				}
				g[ng-1].drift[na-1][nc-1][0]=atof(c[0]);
				g[ng-1].drift[na-1][nc-1][1]=atof(c[1]);
				g[ng-1].drift[na-1][nc-1][2]=atof(c[2]);
				break;
			case 'Y':
				g=ReadGunData(ng,na,nc,nr,f);
				if(!g[ng-1].ABC)
				{
					g[ng-1].ABC=memalloc(na*sizeof(double **));
					for(j=0; j<na; ++j)
						g[ng-1].ABC[j]=NULL;
				}
				if(!g[ng-1].ABC[na-1])
				{
					g[ng-1].ABC[na-1]=memalloc((nc)*sizeof(double *));
					for(j=0; j<nc; ++j)
					{
						g[ng-1].ABC[na-1][j]=memalloc(6*sizeof(double));
						for(k=0; k<6; ++k)
							g[ng-1].ABC[na-1][j][k]=0;
					}
				}
				while(--i >= 0)
					g[ng-1].ABC[na-1][nc-1][i]=atof(c[i]);
				break;
			case 'F':
				g=ReadGunData(ng,na,nc,nr,f);
				g[ng-1].alt[na-1]=memalloc((i/2)*sizeof(int));
				g[ng-1].fuze[na-1]=memalloc((i/2+1)*sizeof(char *));
				g[ng-1].fuze[na-1][i/2]=NULL;
				i=j=i/2;
				while(i--)
				{
					g[ng-1].alt[na-1][i]=atoi(c[i+j]);
					g[ng-1].fuze[na-1][i]=memalloc(strlen(c[i])+1);
					strcpy(g[ng-1].fuze[na-1][i],c[i]);
				}
			}
			if(ng && cc[0] == '=')
			{
				g=memalloc((ng+1)*sizeof(gun));
				for(i=0; i<=ng; ++i)
					g[i]=NullGun;
			}
			if(na && (cc[0] == '=' || cc[0] == 'G'))
			{
				g[ng-1].ammo=memalloc((na+1)*sizeof(char *));
				g[ng-1].mass=memalloc(na*sizeof(double));
				g[ng-1].dmass=memalloc(na*sizeof(double));
				g[ng-1].mv=memalloc(na*sizeof(double *));
				g[ng-1].dvmass=memalloc(na*sizeof(double *));
				g[ng-1].coeff=memalloc(na*sizeof(double *));
				g[ng-1].dvtemp=memalloc(na*sizeof(double *));
				g[ng-1].ammo[na]=NULL;
				g[ng-1].fuze=memalloc(na*sizeof(char **));
				g[ng-1].alt=memalloc(na*sizeof(char **));
				g[ng-1].charge=memalloc(na*sizeof(char **));
				for(i=0; i<na; ++i)
				{
					g[ng-1].charge[i]=NULL;
					g[ng-1].mass[i]=10.0;	/* default teza in sprememba, kar tako! */
					g[ng-1].dmass[i]=0.1;
				}
			}
			if(nc && (cc[0] == '=' || cc[0] == 'G'|| cc[0] == 'A'))
			{
				g[ng-1].dvmass[na-1]=memalloc((nc)*sizeof(double));
				g[ng-1].mv[na-1]=memalloc((nc+1)*sizeof(double));
				g[ng-1].mv[na-1][nc]=0;
				g[ng-1].charge[na-1]=memalloc((nc)*sizeof(char *));
				g[ng-1].dvtemp[na-1]=memalloc((nc)*sizeof(double *));
				g[ng-1].coeff[na-1]=memalloc((nc)*sizeof(double *));
				for(i=0; i<nc; ++i)
				{
					g[ng-1].dvmass[na-1][i]=0;
					g[ng-1].dvtemp[na-1][i]=NULL;
				}
			}
			p=cc;
			return(g);
		case eof:
			return(NULL);
		default:
			*p++=i;
			break;
		}
	} while (1);
return(g);
}
/*______________________________________________________________________________________________________________*/
double		BalKoef3(double *cd, double f)
{
	int		n;
	float	min, step;

 	n	=(int)*cd++;
	min	=*cd++;
	step=*cd++;
	min += (n-1)*step;
	while(n)
	{
		if(min < f)
			break;
		min -= step;
		--n;
	}
	if(n==0)
		return(cd[0]);
	else if(n==13)
		return(cd[n-1]);
	else
		return(cd[n-1]+(f-min)*(cd[n]-cd[n-1])/step);
}
