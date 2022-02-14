// calocell.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include "Calorimeter.hh"
using namespace std;


int _tmain(int argc, _TCHAR* argv[])
{
	//e)
	//


	double E = 3.5;
	const int id = 0;
	CaloCell C(E, id);
	cout << C.get_energy() << endl;
	cout << C.get_ID() << endl;
	CaloCell C_(C);

	C.set_energy(5.6);
	C.set_ID(8);
	C_.set_energy(6.6);
	C_.set_ID(9);
	const CaloCell CC(4.4, 5);

	cout << C.get_energy() << endl;
	cout << C.get_ID() << endl;
	cout << C_.get_energy() << endl;
	cout << C_.get_ID() << endl;


	cout << CC.get_energy() << endl;
	cout << CC.get_ID() << endl;
	// k)
	Point P;
	Point P_(P);
	const Point CP1;
	const Point CP2(6.4, 645, 532);

	cout << P.get_x() << " " << P.get_y() << " " << P.get_z() << endl;
	cout << P_.get_x() << " " << P_.get_y() << " " << P_.get_z() << endl;
	cout << CP1.get_x() << " " << CP1.get_y() << " " << CP1.get_z() << endl;
	cout << CP2.get_x() << " " << CP2.get_y() << " " << CP2.get_z() << endl;

	P.set_x(3);
	P.set_y(112.1);
	P.set_z(32.2);
	P_.set_x(25);
	P_.set_y(122.1);
	P_.set_z(3.252);


	cout << P.get_x() << " " << P.get_y() << " " << P.get_z() << endl;
	cout << P_.get_x() << " " << P_.get_y() << " " << P_.get_z() << endl;
	P.set_position(4.324, 5.234, 345.11);
	P_.set_position(44, 5234.25, 3.11);
	cout << P.get_x() << " " << P.get_y() << " " << P.get_z() << endl;
	cout << P_.get_x() << " " << P_.get_y() << " " << P_.get_z() << endl;
	// q)
	int nx = 2;
	int ny = 2;
	CaloGrid c_g(nx, ny);
	const CaloGrid c__(nx, ny);
	CaloGrid c_g_(c_g);
	CaloCell* p = c_g.cell(1, 1);

	const CaloCell* p_ = c_g.cell(1, 0);
	cout << p->get_energy() << endl;
	p->set_ID(5);

	cout << p->get_ID() << endl;
	cout << p_->get_energy() << endl;
	cout << p_->get_ID() << endl;

	// u) 
	Calorimeter calo(2, 2);
	Calorimeter c_calo(calo);
	Point point1 = calo.position();
	CaloGrid grid1 = calo.grid();
	CaloCell *cell1 = grid1.cell(1, 0);
	Point point2 = c_calo.position();
	CaloGrid grid2 = c_calo.grid();
	CaloCell *cell2 = grid2.cell(1, 0);

	cout << cell1->get_ID() << endl;
	cout << cell2->get_ID() << endl;
	cout << point1.get_x() << point1.get_y() << point1.get_z() << endl;
	cout << point2.get_x() << point2.get_y() << point2.get_z() << endl;
	cell1->set_ID(10);
	point1.set_position(1, 1, 1);
	cell2->set_ID(1);
	point2.set_position(1, 0, 1);
	cout << cell1->get_ID() << endl;
	cout << cell2->get_ID() << endl;
	cout << point1.get_x() << point1.get_y() << point1.get_z() << endl;
	cout << point2.get_x() << point2.get_y() << point2.get_z() << endl;

	const Calorimeter calo_const(2, 2);
	const Point point_c = calo_const.position();
	const CaloGrid grid_c = calo_const.grid();

	cout << point_c.get_x() << point_c.get_y() << point_c.get_z() << endl;

	return 0;
}
// Below I will write only the output relevant to a specific question, the end code should output it all together
// e)
/*
CaloCell constructor0x6233fff9a0
3.5
0
5.6
8
6.6
9
CaloCell constructor0x6233fff980
4.4
5
*/
// k)
/*
Point constructor0x780d9ff900
Point constructor0x780d9ff8c0
Point constructor0x780d9ff8a0
0 0 0
0 0 0
0 0 0
6.4 645 532
3 112.1 32.2
25 1232.1 3.252
4.324 5.234 345.11
44 5234.25 3.11
*/
// n)
/*
CaloGrid.hh: In copy constructor 'CaloGrid::CaloGrid(const CaloGrid&)':
CaloGrid.hh:18:39: error: no matching function for call to 'CaloCell::CaloCell()'
18 |         calocells = new CaloCell[nx*ny] ;
|                                       ^
In file included from main4.cc:3:
CaloCell.hh:9:5: note: candidate: 'CaloCell::CaloCell(double, int)'
9 |     CaloCell(double Energy ,int Id ) {
|     ^~~~~~~~
CaloCell.hh:9:5: note:   candidate expects 2 arguments, 0 provided
CaloCell.hh:6:7: note: candidate: 'constexpr CaloCell::CaloCell(const CaloCell&)'
6 | class CaloCell {
|       ^~~~~~~~
CaloCell.hh:6:7: note:   candidate expects 1 argument, 0 provided
*/
// 
// q)
/*
CaloCell constructor0x27fad071750
CaloCell constructor0x27fad071760
CaloCell constructor0x27fad071770
CaloCell constructor0x27fad071780
CaloGrid constructor0x89e1bff6b0
CaloCell constructor0x27fad071ac0
CaloCell constructor0x27fad071ad0
CaloCell constructor0x27fad071ae0
CaloCell constructor0x27fad071af0
CaloGrid copy constructor0x89e1bff690
CaloCell constructor0x27fad071da0
CaloCell constructor0x27fad071dc0
0
5
0
0
CaloGrid destructor0x89e1bff690
CaloGrid destructor0x89e1bff6b0
*/
// u)
/*
Point constructor0x3fe7dffcc0
CaloCell constructor0x21fd2a12490
CaloCell constructor0x21fd2a124a0
CaloCell constructor0x21fd2a124b0
CaloCell constructor0x21fd2a124c0
CaloCell constructor0x21fd2a124d0
CaloCell constructor0x21fd2a124e0
CaloCell constructor0x21fd2a124f0
CaloCell constructor0x21fd2a12500
CaloCell constructor0x21fd2a12510
CaloGrid constructor0x3fe7dffcd8
Point constructor0x3fe7dffab0
CaloCell constructor0x21fd2b41b40
CaloCell constructor0x21fd2b41b50
CaloCell constructor0x21fd2b41b60
CaloCell constructor0x21fd2b41b70
CaloGrid constructor0x3fe7dffad0
CaloGrid destructor0x3fe7dffad0
Calorimeter constructor0x3fe7dffca0
CaloCell constructor0x21fd2b41b40
CaloCell constructor0x21fd2b41b50
CaloCell constructor0x21fd2b41b60
CaloCell constructor0x21fd2b41b70
CaloGrid copy constructor0x3fe7dffc88
CaloCell constructor0x21fd2a12530
CaloCell constructor0x21fd2a12540
CaloCell constructor0x21fd2a12550
CaloCell constructor0x21fd2a12560
CaloGrid copy constructor0x3fe7dffc10
CaloCell constructor0x21fd2a12580
CaloCell constructor0x21fd2a12590
CaloCell constructor0x21fd2a125a0
CaloCell constructor0x21fd2a125b0
CaloGrid copy constructor0x3fe7dffbd0
0
0
000
000
10
1
111
101
Point constructor0x3fe7dffba0
CaloCell constructor0x21fd2a125d0
CaloCell constructor0x21fd2a125e0
CaloCell constructor0x21fd2a125f0
CaloCell constructor0x21fd2a12600
CaloCell constructor0x21fd2a12610
CaloCell constructor0x21fd2a12620
CaloCell constructor0x21fd2a12630
CaloCell constructor0x21fd2a12640
CaloCell constructor0x21fd2a12650
CaloGrid constructor0x3fe7dffbb8
Point constructor0x3fe7dffab0
CaloCell constructor0x21fd2a12670
CaloCell constructor0x21fd2a12680
CaloCell constructor0x21fd2a12690
CaloCell constructor0x21fd2a126a0
CaloGrid constructor0x3fe7dffad0
CaloGrid destructor0x3fe7dffad0
Calorimeter constructor0x3fe7dffb80
CaloCell constructor0x21fd2a12670
CaloCell constructor0x21fd2a12680
CaloCell constructor0x21fd2a12690
CaloCell constructor0x21fd2a126a0
CaloGrid copy constructor0x3fe7dffb40
000
CaloGrid destructor0x3fe7dffb40

*/