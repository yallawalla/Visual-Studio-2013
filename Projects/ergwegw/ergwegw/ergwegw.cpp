#include "stdafx.h"


#include "stdio.h"

using namespace System;


void crta(int x1,int y1,int x2,int y2)
{
	if(x2-x1 > 1 || y2-y1>1) {
		crta(x1,y1,(x1+x2)/2,(y1+y2)/2);
		crta((x1+x2)/2,(y1+y2)/2,x2,y2);
	}
	else
	{
		printf("%d,%d\r\n",x1,y1);
	}
}



int main(array<System::String ^> ^args)
{

	crta(1,1,5,3);
	getchar();

    return 0;
}
