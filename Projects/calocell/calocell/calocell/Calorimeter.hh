
#ifndef CALORIMETER_HH
#define CALORIMETER_HH


using namespace std ;
#include "Point.hh"
#include "CaloGrid.hh"
#include <iostream>

class Calorimeter {
public:
    Calorimeter(int Nx, int Ny , double X = 0 , double Y = 0 , double Z = 0) {
        nx = Nx ; 
        ny = Ny ;
        x = X ;
        y = Y ;
        z = Z ;
        point = Point(x,y,z) ;
        calogrid = CaloGrid(nx,ny) ;
        cout << "Calorimeter constructor " << this << endl ;
    }
/*
    Calorimeter(const Calorimeter &other) {
        cout << "Calorimeter copy constructor" << &other << endl ;
    }

    ~Calorimeter() {
        cout << "Calorimeter destructor" << this << endl ;

    }
*/
// t)
    CaloGrid& grid() { return calogrid ;}
    const CaloGrid& grid() const { return calogrid ;}
    Point& position() { return point ;}
    const Point& position() const { return point ;}

private:
    double x,y,z ;
    int nx,ny ;
    Point point ;
    CaloGrid calogrid ;
} ;

#endif