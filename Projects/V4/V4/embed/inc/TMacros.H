#ifndef __TMacros_h_
#define __TMacros_h_

#define ISMASK(Value, Mask) (((Value) & (Mask))!=0)
#define SETMASK(Value, Mask) Value|=(Mask)
#define CLEARMASK(Value, Mask) Value&=~(Mask)
#define INVERTMASK(Value, Mask) Value^=(Mask)

#define ARRAY_SIZE(ARR) (sizeof ARR/sizeof ARR[0])
#define BITSIZEOF(ITEM) (8*sizeof(ITEM))

#define IS_IN_RANGE(VAL,MIN,MAX) ((VAL)>=(MIN) && (VAL)<=(MAX))

#define MIN(VAL1,VAL2) ((VAL1)<(VAL2) ? (VAL1) : (VAL2))
#define MAX(VAL1,VAL2) ((VAL1)<(VAL2) ? (VAL2) : (VAL1))
#define ZERO(O) memset(&(O),0,sizeof(O))
#define FILL(O,P) memset(&(O),(P),sizeof(O))
#define COPY(D,S) memcpy(&(D),&(S),sizeof(D))
#define COMPARE(P1,P2) memcmp(&(P1),&(P2),sizeof(P1))
#define EQUAL(P1,P2) (0==COMPARE(P1,P2))

#endif  
