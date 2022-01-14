// mcarlo.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <time.h>
#include <math.h>
#include <iostream>
#include <stdlib.h>
using namespace std;

// The function to integrate
double f(const double& x, const double& y)
{
	double numer = exp(-2 * x - 3 * y);
	double denom = sqrt(x*x + y*y + 1);
	double retval = numer / denom;
	return retval;
}
void integrate()
{
	// Declares local constants
	const double area = atan(1); // pi/4
	// Inits local variables
	int n = 0;
	double sum = 0, x = 0, y = 0;
	// Seeds random number generator
	srand(time(NULL));
	// Gets the value of N
	cout << "What is N? ";
	cin >> n;
	// Loops the given number of times
	for (int i = 0; i<n; i++)
	{
		// Loops until criteria met
		while (true)
		{
			// Generates random points between 0 and 1
			x = static_cast<double>(rand()) / RAND_MAX;
			y = static_cast<double>(rand()) / RAND_MAX;
			// Checks if the points are suitable
			if ((x*x + y*y) <= 1)
			{
				// If so, break out of the while loop
				break;
			}
		}
		// Updates our sum with the given points
		sum += f(x, y);
	}
	// Integral = area times mean value < f > of f
	sum = area * sum / n;
	cout << "The integral is " << sum << endl;
}

int _tmain(int argc, _TCHAR* argv[])
{
	integrate();
	return 0;
}

