#include	"remmenu.h"
/*_____________________________________________________________________________________________________________*/
/*	Urejanje koordinatnega zaslona X,Y in Z				*/
/*	Spreminjanje vsebine s proceduro edit_xyz, pred		*/
/*	klicem pretvorba v skladu z rel. rezimom in obliko,	*/
/*	ob izhodu kontrola glede na max. vrednosti			*/
/*_____________________________________________________________________________________________________________*/
int		EditX(void *vp)
{
	int	i = null;
	xyLCD(0,0);
	Cursor(CURS_FULL);
	if(!LL->ltype)
	{
		LL->ltype=addtype(TYPE_TG);
		auto_ntyp(vp);
		LL->txt=addtxt(LL,TG_UNDEF);
	}
/*														*/
/* ob prvem vstopu pretvori koordinate v del. obliko in */
/* postavi flag											*/
/*                                                      */
	if(edit_flag == eof)
	{
		edit_flag=d_mode;
		d_mode=LlaToCorr(LL,LL,ORG,NORTH,d_mode);
	}
	if(Kbhit(CHLCD))
	{
		i=Getch(CHLCD);
		if(!LL->xc && isdigit(i))
			LL->x=LL->xc=add_coord(LL,0,0,0);
		if(LL->xc)
		{
			switch(d_mode)
			{
			case N_E_Z:
				i=edit_xyz(&LL->xc->x, i);
				break;
			case EL_AZ:
			case EL_AZ_R:
				i=edit_pol(&LL->xc->y,maxmils,i,null);
				break;
			case AZ_R_dZ:
				i=edit_pol(&LL->xc->x,maxmils,i,null);
				break;
			}
		}
	}
	Display(LL,d_mode,maxmils);
	xyLCD(0,0);
	if(Kbhit(CHLCD))
		Getch(CHLCD);
	return(null);
}
/*_____________________________________________________________________________________________________________*/
int		EditY(void *vp)
{
	int	i=null;
	xyLCD(0,1);
	Cursor(CURS_FULL);
	if(edit_flag == eof)
	{
		edit_flag=d_mode;
		d_mode=LlaToCorr(LL,LL,ORG,NORTH,d_mode);
	}
	if(Kbhit(CHLCD))
	{
		i=Getch(CHLCD);
		if(!LL->xc && isdigit(i))
			LL->x=LL->xc=add_coord(LL,0,0,0);
		if(LL->xc)
		{
			switch(d_mode)
			{
			case N_E_Z:
				i=edit_xyz(&LL->xc->y,i);
				break;
			case EL_AZ_R:
				i=edit_xyz(&LL->xc->z,i);
				if(!NORTH && (LL->xc->z < 0))
				{
					beep(100);
					LL->xc->z *= -1;
				}
				if(!LL->xc->z)
					LL->xc->z += 0.1;
				break;
			case AZ_R_dZ:
				i=edit_xyz(&LL->xc->y,i);
				if(!NORTH && (LL->xc->y < 0))
				{
					beep(100);
					LL->xc->y *= -1;
				}
				if(!LL->xc->y)
					LL->xc->y += 0.1;
				break;
			}
		}
	}
	Display(LL,d_mode,maxmils);
	xyLCD(0,1);
	if(Kbhit(CHLCD))
		Getch(CHLCD);
	return(null);
}
/*_____________________________________________________________________________________________________________*/
int		EditZ(void *vp)
{
	int	i=null;
	xyLCD(0,2);
	Cursor(CURS_FULL);
	if(edit_flag == eof)
	{
		edit_flag=d_mode;
		d_mode=LlaToCorr(LL,LL,ORG,NORTH,d_mode);
	}
	if(Kbhit(CHLCD))
	{
		i=Getch(CHLCD);
		if(!LL->xc && isdigit(i))
			LL->x=LL->xc=add_coord(LL,0,0,0);
		if(LL->xc)
		{
			switch(d_mode)
			{
				case N_E_Z:
				case AZ_R_dZ:
					i=edit_xyz(&LL->xc->z,i);
					break;
				case EL_AZ:
				case EL_AZ_R:
					i=edit_pol(&LL->xc->x,maxmils,i,eof);
					break;
			}
		}
	}
	Display(LL,d_mode,maxmils);
	xyLCD(0,2);
	if(Kbhit(CHLCD))
		Getch(CHLCD);
	return(null);
}
/*_____________________________________________________________________________________________________________*/
int			EditClose(void *vp)
			{
			if(edit_flag != eof) {
				d_mode=edit_flag;
				CorrToLla(LL,LL,ORG,NORTH,d_mode);
				edit_flag=eof;
				}
			Displc(LL);
			Cursor(CURS_OFF);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			EditRefresh(void *vp)
			{
			int	i,j=GetLCDaddr();
			i=j % 20;

			EditClose(NULL);
			switch(j/20) {
				case 0:	if(i<6){
							EditX(NULL);
							return(smenu(edit_x));
							}
						if(i>9) {
							EditTR(NULL);
							return(smenu(edit_time_r));
							}
						EditTL(NULL);
						return(smenu(edit_time_l));
				case 1:	if(i<6) {
							EditY(NULL);
							return(smenu(edit_y));
							}
						EditType(NULL);
						return(smenu(edit_type));
				case 2:	EditZ(NULL);
						return(smenu(edit_z));
				case 3:	EditTxt(NULL);
						return(smenu(edit_txt));
				}
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
/*	Urejanje koordinatnega zaslona, koordinate xyz		*/
/*	aktivne tipke 0-9, ENTER, CLEAR, SIGN				*/
/*	izhod z ENTER !										*/
/*_____________________________________________________________________________________________________________*/
int			edit_xyz(double *p,int i)
			{
				switch(i)
					{
					case CLEAR:	*p=0.0;
								return(null);
					case SHIFTR:if(*p >= 0)
									*p = floor(*p/10);
								else
									*p = ceil(*p/10);
								return(null);
					case SIGN:  *p=-*p;
								break;
					case ENTER:	return(eof);

					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':   if(fabs(*p) < maxgrid)
									if(*p >= 0)
										*p = 10*floor(*p) + i - '0';
									else
										*p = 10*ceil(*p) - i + '0';
								else
									err(E_MAX_VAL);
								break;
					default	:	break;
					}
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
/*	Urejanje koordinatnega zaslona, polarne koordinate	*/
/*	aktivne tipke 0-9, ENTER, CLEAR, SIGN				*/
/*	izhod z ENTER !                                     */
/*  ce je sign 0, se neg. vrednosti interpr. kot maxmils*/
/*_____________________________________________________________________________________________________________*/
int			edit_pol(double *p,int mils,int i,int sign)
			{
			if(*p<0 && !NORTH && !sign)
				*p += mils;
				switch(i)
				{
					case CLEAR:	*p=0.0;
								return(null);
					case SIGN:	*p=-*p;
								return(edit_pol(p,mils,null,sign));
					case ENTER:	return(eof);

					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':   *p=fmod(floor(*p + 0.5),mils);
								if(*p >= 0)
									*p = 10 * floor(*p+0.5) + i - '0';
								else
									*p = 10  * ceil(*p-0.5) - i + '0';
								break;
					default	:	break;
				}
			if(fabs(*p) >= mils) {
				beep(100);
				*p = fmod(round(*p/10,4),mils);
				}
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
/*	Aktiviranje naslednje/predhodne  koordinatne grupe,	*/
/*	ce ta obstoja !										*/
/*_____________________________________________________________________________________________________________*/
int			next_xyz(void *vp)
			{
			if(!LL || !LL->x)
				return(null);
			if(LL->xc->next) {
				LL->xc = LL->xc->next;
				Displc(LL);
				}
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			last_xyz(void *vp)
			{
			coord	*p;
			if(!LL || !LL->x)
				return(null);
			if(LL->x != LL->xc) {
				for(p=LL->x; p->next != LL->xc; p=p->next);
				LL->xc=p;
				Displc(LL);
				}
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			add_xyz(void *vp)
			{
			lib		**l=vp,*ll;
			int		i;

			shaft_enable();
			if(!l)
				ll=LL;
			else
				ll=*l;

/*** ce je rezim korekcije, je dodana tocka kar NORTH	*/
			if(NORTH)
			{
				if(!AddCoord(ll,NORTH->xc->x,NORTH->xc->y,NORTH->xc->z))
					return(null);
			}
			else if(ORG)
			{
					if(!AddCoord(ll,ORG->xc->x,ORG->xc->y,ORG->xc->z))
						return(null);
			}
			else if(!AddCoord(ll,OBP->xc->x,OBP->xc->y,OBP->xc->z))
				return(null);
			ll->xc->refp = ScaleAngle(read_AZ() + maxmils/2,maxmils,MR10,eof);
/*_____________________________________________________________________________________________________________*/
/* predikcija premicnega cilja							*/
/*              										*/
			i=targets_n(ll->x);
			if(checktype(ll,TYPE_MT) && (i>2))
			{
				if(LastCoord(ll) != ll->xc)
					ll->xc->t = LastCoord(ll)->t;
				if(track_delay)
					addsecs(&ll->xc->t,track_delay);
				else
					addsecs(&ll->xc->t,1);
				if(!interpolate(ll))
				{
					erase_coord(ll,ll->xc);
					err(E_ILL_DATA);
				}
				while(targets_n(ll->x) > 3)
					erase_coord(ll,FirstCoord(ll));
			}
			if(!l)
				Displc(ll);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			erase_xyz_s(void *vp)
			{
			erase_coord(LL,LL->xc);
			Display(LL,d_mode,maxmils);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			erase_xyz_all(void *vp)
			{
			LL->x=LL->xc=freecoord(LL->x);
			Display(LL,d_mode,maxmils);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			set_time(void *vp)
			{
			if(LL->xc)
				LL->xc->t=Time;
			else
				err(E_VOID_DATA);
			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			EditTxt(void *vp)
			{
static		int	n=0;	
			Cursor(CURS_UND);
			if(LL->txt && Kbhit(CHLCD))
				n=EditStr(Getch(CHLCD),LL->txt->t,n);
			if(RightToLeft)
				xyLCD(strlen(LL->txt->t)-n-1,3);
			else
				xyLCD(n,3);
			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			mode_time(void *vp)
			{
			time_mode=++time_mode % MAX_TMODE;
			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			EditTL(void *vp)
			{
			xyLCD(6,0);
			Cursor(CURS_FULL);
			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			EditTR(void *vp)
			{
			xyLCD(12,0);
			Cursor(CURS_FULL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			incr_time_l(void *vp)
			{
			if(!LL->xc)
				return(null);
			if(RightToLeft && !vp)
				return(incr_time_r((void *)-1));

			switch(time_mode)
				{
				case HOUR_MIN:
						LL->xc->t.hour=++LL->xc->t.hour % 24;
						break;
				case MIN_SEC:
						LL->xc->t.min=++LL->xc->t.min % 60;
						break;
				case DAY_DATE:
						LL->xc->t.day=++LL->xc->t.day % 7;
						break;
				case MONTH_YEAR:
						LL->xc->t.month=++LL->xc->t.month % 12;
						break;
				}
			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			incr_time_r(void *vp)
			{
			if(!LL->xc)
				return(null);
			if(RightToLeft && !vp)
				return(incr_time_l((void *)-1));

			switch(time_mode)
				{
				case HOUR_MIN:	
						LL->xc->t.min=++LL->xc->t.min % 60;
						break;	
				case MIN_SEC:
						LL->xc->t.sec=++LL->xc->t.sec % 60;
						break;	
				case DAY_DATE:
						LL->xc->t.date=++LL->xc->t.date % 31;
						break;	
				case MONTH_YEAR:
						LL->xc->t.year=++LL->xc->t.year % 100;
						break;
				}
			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			decr_time_l(void *vp)
			{
			if(!LL->xc)
				return(null);
			if(RightToLeft && !vp)
				return(decr_time_r((void *)-1));

			switch(time_mode)
				{
				case HOUR_MIN:	
						if(!LL->xc->t.hour--)
							LL->xc->t.hour=23;
						break;	
				case MIN_SEC:
						if(!LL->xc->t.min--)
							LL->xc->t.min=59;
						break;	
				case DAY_DATE:
						if(!LL->xc->t.day--)
							LL->xc->t.day=6;
						break;	
				case MONTH_YEAR:
						if(!LL->xc->t.month--)
							LL->xc->t.month=11;
						break;	
				}
			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			decr_time_r(void *vp)
			{
			if(!LL->xc)
				return(null);
			if(RightToLeft && !vp)
				return(decr_time_l((void *)-1));

			switch(time_mode)
				{
				case HOUR_MIN:
						if(!LL->xc->t.min--)
							LL->xc->t.min=59;
						break;	
				case MIN_SEC:
						if(!LL->xc->t.sec--)
							LL->xc->t.sec=59;
						break;	
				case DAY_DATE:
						if(!LL->xc->t.date--)
							LL->xc->t.date=30;
						break;	
				case MONTH_YEAR:
						if(!LL->xc->t.year--)
							LL->xc->t.year=99;
						break;	
				}
			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			sel_txt_r(void *vp)
			{
			if(LL->txt) {
				if(LL->txt->next)
					LL->txt=LL->txt->next;
                }
			else
				if(LL->ltype)
					LL->txt = LL->ltype->typtxt;
			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			sel_txt_l(void *vp)
			{
            libtext	*p;
			if(LL->txt) {
				p = LL->ltype->typtxt;
                while((p != LL->txt) && (p->next != LL->txt))
                	p=p->next;
            	LL->txt = p;
                }
			else
				if(LL->ltype)
					LL->txt = LL->ltype->typtxt;
			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			add_txt(void *vp)
			{
			libtext	*p=addtxt(LL,"            ");
			if(p)
				{
				LL->txt=p;
				Displc(LL);
				}
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
int			select_type(void *vp)
			{
			ltype	*p;
			if(!LL || !LL->ltype)
				return(null);
			p = LL->ltype;
			if(p) {									/* not a void type		*/
				if(*p->t == '.')					/* special types !!!	*/
					return(null);
				do	{
					p = p->next;					/* search for the next	*/
					if(!p)							/* start of stack		*/
						p = LTYPES;					/* if NULL				*/
					}	while (*p->t == '.');		/* skip, if special		*/
				}
			else									/* select first type	*/
				p=LTYPES;							/* available, if void	*/
			LL->ltype	= p;
			LL->txt		= p->typtxt;
            Displc(LL);
			return(null);							/* renumber item !!!	*/
			}
/*_____________________________________________________________________________________________________________*/
int			SelectRight(void *vp)
			{
			ltype	*p;
			if(!LL || !LL->ltype)
				return(null);
			p = LL->ltype;
			if(p) {									/* not a void type		*/
				if(*p->t == '.')					/* special types !!!	*/
					return(null);
				do	{
					p = p->next;					/* search for the next	*/
					if(!p)							/* start of stack		*/
						p = LTYPES;					/* if NULL				*/
					}	while (*p->t == '.');		/* skip, if special		*/
				}
			else									/* select first type	*/
				p=LTYPES;							/* available, if void	*/
			LL->ltype	= p;
			LL->txt		= p->typtxt;
			return(auto_ntyp(vp));					/* renumber item !!!	*/
			}
/*_____________________________________________________________________________________________________________*/
int			SelectLeft(void *vp)
			{
			ltype	*p;
			if(!LL || !LL->ltype)
				return(null);

			p = LL->ltype;
			if(p)									/* not a void type		*/
				if(*p->t == '.')					/* special types !!!	*/
					return(null);

			for(p=LTYPES; (p->next != LL->ltype) && p->next; p=p->next);
			if(*p->t == '.')						/* special types !!!	*/
				return(SelectLeft(vp));
			LL->ltype	= p;
			LL->txt		= p->typtxt;
            Displc(LL);
			return(auto_ntyp(vp));					/* renumber item !!!	*/
			}
/*_____________________________________________________________________________________________________________*/
int			EditType(void *vp)
			{
			if(!Kbhit(CHLCD)) {
				xyLCD(6,1);
				Cursor(CURS_FULL);
				}
			else
				LL->n = abs(ReadInt(LL->n)) % 100;
			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
/* alocira in nastavi lib strukturo na vrednosti v        */
/* naslovu ! v primeru napake vrne NULL !!!               */
/*_____________________________________________________________________________________________________________*/
lib			*make_set(double x,double y,double z,char *type,char *txt)
			{
			lib		*p=makelib();

			if(!p)
				return(NULL);
			if(type)
				p->ltype=addtype(type);
			if(txt)
				p->txt=addtxt(p,txt);
			add_coord(p,x,y,z);
			return(p);
			}
/*_____________________________________________________________________________________________________________*/
/* vrne stevilo istovrstnih tipov v knjiznici             */
/*_____________________________________________________________________________________________________________*/
int			counttype(lib *l,ltype *typ)
			{
			lib	*p=l;
			int	i=0;

				do
				{
				if(p->ltype==typ)
					++i;
				p=p->next;
				} while(p!=l);
			return(i);
			}
/*_____________________________________________________________________________________________________________*/
/* vrne stevilo istovrstnih tekstov v knjiznici           */
/*_____________________________________________________________________________________________________________*/
int			counttext(lib *l,libtext *text)
			{
			lib	*p=l->next;
			int	i=0;

			do
				{
				if(p->txt==text)
					++i;
				p=p->next;
				} while(p != l);

			return(i);
			}
/*_____________________________________________________________________________________________________________*/
/* vrne kazalec na istovrstni tip in oznako v knjiznici   */
/* Ce ga ni vrne NULL !!!                                 */
/*_____________________________________________________________________________________________________________*/
lib			*checklib(lib *l,ltype *typ, int n) {
			lib	*p;
			if(l && typ) {
				p=l;
				do	{
					if((p->ltype==typ)&&(p->n==n))
						return(p);
					p=p->next;
					} while(p!=l);
				}
			return(NULL);
			}
/*_____________________________________________________________________________________________________________*/
/* vrne kazalec na istovrstni tip in oznako v knjiznici   */
/* Ce ga ni vrne NULL !!!                                 */
/*_____________________________________________________________________________________________________________*/
lib			*findtype(lib *l,char *c) {
			lib	*p;
			if(l && c) {
				p=l;
				do	{
					if(!strncmp(c,p->ltype->t,LIBTYPE_LEN))
						return(p);
					p=p->next;
					} while(p!=l);
				}
			return(NULL);
			}
/*_____________________________________________________________________________________________________________*/
/* vrne kazalec na istovrstni tip  v knjiznici   		  */
/* Ce ga ni vrne NULL !!!                                 */
/*_____________________________________________________________________________________________________________*/
ltype		*checktype(lib *l,char *c)
			{
			if(!l)
				return(NULL);
			if(!l->ltype)
				return(NULL);
			if(!strncmp(c,l->ltype->t,LIBTYPE_LEN))
				return(l->ltype);
			return(NULL);
			}
/*_____________________________________________________________________________________________________________*/
/* poisce element v knjiznici enakega tipa in oznake!	  */
/*_____________________________________________________________________________________________________________*/
int			find_ntyp(void *vp)
			{
			lib	*l=checklib(Ln,LL->ltype,LL->n);

			if(!l)
				warn(W_ITEM_N_FOUND);
			else
				copylib(l,LL);

			Displc(LL);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
/* Returns link address of type *p and num. designation n	*/
/* returns null, if type not found or void					*/
/*_____________________________________________________________________________________________________________*/
int			LinkAddr(lib *p)
			{
			ltype *q;
			int		i;

			if(!p || !p->ltype)
				return(null);

			for(i=1, q=LTYPES;	q; ++i, q=q->next)
				if(p->ltype == q)
					return((i<<8) + p->n);
			return(null);
			}
/*_____________________________________________________________________________________________________________*/
/* Returns lib pointer from string							*/
/* returns null, if type not found or void					*/
/*_____________________________________________________________________________________________________________*/
int			LinkAddrStr(char *p)
			{
			int		i,j;
			char	s[8],*ss[16];
			ltype	*q;
			i=strscan(p,ss,'-',16);
			if(i==2)
			{
				if(RightToLeft)
				{
					sprintf(s,"%*s",LIBTYPE_LEN,ss[1]);
					j=atoi(ss[0]);
				}
				else
				{
					sprintf(s,"%*s",LIBTYPE_LEN,ss[0]);
					j=atoi(ss[1]);
				}
				for(i=1, q=LTYPES;	q; ++i, q=q->next)
					if(!strncmp(q->t,s,LIBTYPE_LEN))
						return((i<<8) + j);
			}
			return(null);
}
/*_____________________________________________________________________________________________________________*/
void		DspAddress(int	a)
			{
/*
			lib	*l=FindLib(a);
			if(l)
				DspAddr(a+(targets_xn(l)<<12));
			else
				DspAddr(a+(1<<12));
*/
				DspAddr(a);
				}
/*_____________________________________________________________________________________________________________*/
/* Returns link address text pointer from link address code,*/
/* type of int												*/
/* returns null, if illegal code							*/
/*_____________________________________________________________________________________________________________*/
int			LinkAddrN(int n)
			{
			return(n & 0xff);
			}
/*_____________________________________________________________________________________________________________*/
/* Returns link address text pointer from link address code,*/
/* type of int												*/
/* returns null, if illegal code							*/
/*_____________________________________________________________________________________________________________*/
char		*LinkAddrText(int a, char *p)
			{
			ltype *q;
			int		i,n=a;
			if(!n || !p)
				return(NULL);
			for(i=n>>8, q=LTYPES; q && (i>1); --i, q=q->next);
			if(!q)
				return(NULL);
			sprintf(p,"%2s-%02d",q->t,n & 0xff);
			translate(p,'.',' ');
			if(RightToLeft)
				strswap(p,"-");
			return(p);
			}
/*_____________________________________________________________________________________________________________*/
/* Returns type pointer from link address code,				*/
/* type of int												*/
/* returns null, if illegal code							*/
/*_____________________________________________________________________________________________________________*/
ltype		*LinkAddrType(int a)
			{
			ltype	*q;
			int		i,n=a;
			if(!(n>>8))
				return(NULL);
			for(i=n>>8, q=LTYPES; q && (i>1); --i, q=q->next);
			return(q);
			}
/*_____________________________________________________________________________________________________________*/
/* Returns pointer to library element from link address		*/
/* returns null, if element  not found or void				*/
/*_____________________________________________________________________________________________________________*/
lib			*FindLib(int a)
			{
			ltype	*q;
			int		i,n=a;
			if(!n)
				return(NULL);

			for(i=n>>8, q=LTYPES; q && (i>1); --i, q=q->next);
			if(!q)
				return(NULL);
			return(checklib(Ln,q,n & 0xff));
			}
/*_____________________________________________________________________________________________________________*/
/* vrne kazalec na novo vpisanega tipa na sklad LTYPES     	*/
/* v primeru napake vrne NULL !!!							*/
/*_____________________________________________________________________________________________________________*/
ltype		*addtype(char *c)
			{
			char	cc[16];
			struct	ltype	*p,*q,**r;

			r = &LTYPES;
			p=memalloc(sizeof(ltype));
			strncpy(cc,c,LIBTYPE_LEN);
			cc[LIBTYPE_LEN]=null;
			sprintf(p->t,"%*s",LIBTYPE_LEN,cc);
			p->next=NULL;
			p->typtxt=NULL;

			if(!*r)
				{
				*r=p;
				return(p);
				}
			for(q=*r;strncmp(p->t,q->t,LIBTYPE_LEN);q=q->next)
		   		{
				if(!q->next)
					{
					q->next=p;
					return(p);
					}
				}
			memfree(p);
			return(q);
			}
/*_____________________________________________________________________________________________________________*/
/* preko elementa knjiznice vpise nov tekst. deskriptor   */
/* na pripadajoci sklad                                   */
/* V primeru napake ali NULL ltype vrne NULL              */
/*_____________________________________________________________________________________________________________*/
libtext		*addtxt(lib *l,char *c)
			{
			char	cc[LIBTEXT_LEN+2];
			libtext	*p,*q;

			if(!l->ltype)
				return(NULL);

			p=memalloc(sizeof(libtext));
			strncpy(cc,c,LIBTEXT_LEN);
			cc[LIBTEXT_LEN]=null;
			sprintf(p->t,"%*s",LIBTEXT_LEN,cc);
			p->next=NULL;
			if(!l->ltype->typtxt) {
				l->ltype->typtxt=p;
				return(p);
				}
			for(q=l->ltype->typtxt; strncmp(p->t,q->t,LIBTEXT_LEN); q=q->next)
				{
				if(!q->next) {
					q->next=p;
					return(p);
					}
				 }
			memfree(p);
			return(q);
			}
/*_____________________________________________________________________________________________________________*/
/* izracun predikcije ob casu v l->xc na l->xc->x,y,z	 */
/* vsebina lib mora biti v grid koordinatah ???      	 */
/*_____________________________________________________________________________________________________________*/
lib		*interpolate(lib *l)
		{
		coord	*p;
		int	i;
		double	*q;
		samples *s=NULL;

		if(!l || !l->xc)
			return(NULL);
		i=targets_n(l->x);
		if(i < 3)
			return(NULL);
		if(i > 4)
			i=4;

		s=init_samples(s,i-1);
		for(p=l->x; p; p=p->next)
			if(p != l->xc)
				add_sample(s,dtime(&l->xc->t,&p->t),p->x);
		q=solve(s);
		if(q)	{
				l->xc->x = *q;
				s=init_samples(s,i-1);
				for(p=l->x; p; p=p->next)
					if(p != l->xc)
						add_sample(s,dtime(&l->xc->t,&p->t),p->y);
				q=solve(s);
				if(q)	{
						l->xc->y = *q;
						s=init_samples(s,i-1);
						for(p=l->x; p; p=p->next)
							if(p != l->xc)
								add_sample(s,dtime(&l->xc->t,&p->t),p->z);
							q=solve(s);
								if(q)	{
										l->xc->z = *q;
										freesamples(s);
										return(l);
										}
						}
				}
		freesamples(s);
		return(NULL);
		}
/*_____________________________________________________________________________________________________________*/
double	dtime(rtime *t1, rtime *t2)
		{
		double w=calc_week(t1)-calc_week(t2);
		double s=calc_secs(t1)-calc_secs(t2);
		return(w*7.0*24.0*3600.0 + s);
		}
/*_____________________________________________________________________________________________________________*/
void	addtime(rtime *t1, rtime *t2)
		{
		double w=calc_week(t1)+calc_week(t2);
		double s=calc_secs(t1)+calc_secs(t2);
		calc_rtime(w,s,t2);
		}
/*_____________________________________________________________________________________________________________*/
void	addsecs(rtime *t, double i)
		{
		double w=calc_week(t);
		double s=calc_secs(t) + i;
		calc_rtime(w,s,t);
		}
/*_____________________________________________________________________________________________________________*/
