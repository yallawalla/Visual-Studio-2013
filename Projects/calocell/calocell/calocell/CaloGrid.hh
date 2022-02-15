#ifndef CALOGRID_HH
#define CALOGRID_HH

#include "CaloCell.hh"

using namespace std ;

class CaloGrid {
public:

    CaloGrid(int Nx = 3,int Ny = 3) {           //r) added default values for grid size otherwise we get an error trying to instantiate them in Calorimeter class
        cout << "CaloGrid constructor" << this << endl ;
        nx = Nx ;
        ny = Ny ;
        len = Nx*Ny ;
        calocells = new CaloCell[len] ;
    }

    CaloGrid(const CaloGrid& other) : nx(other.nx) , ny(other.ny) , len(other.len) {
        cout << "CaloGrid copy constructor" << this << endl ;
        calocells = new CaloCell[len] ;
        int i ;
        for (i = 0 ; i < len ; i++) {
            calocells[i] = other.calocells[i] ;
        }
    }

    ~CaloGrid() {
        cout << "CaloGrid destructor" << this << endl ;
        delete[] calocells ;
    }

// o)
    CaloCell* cell(int x , int y) {
/*
Taking into account the grid points are mapped to the array index in the following way:
e.g. nx = 2, ny = 3
    i   (x,y)
    0   (0,0)
    1   (1,0)   
    2   (0,1)
    3   (1,1)
    4   (0,2)
    5   (1,2)
*/
        int i = y*nx + x ;
// if index is out of range we return a null pointer
        if (i >= len) {
            return NULL ;
        }
// else we return the pointer to the i-th element
        else {
            return &calocells[i] ;
        }
    }
// p)
// we don't duplicate the code from non_const version, essentially we force the const function to call the non_const function then we convert the pointer back to a const CaloCell pointer  
    const CaloCell* cell(int x , int y) const {
        CaloCell* p_ = const_cast<CaloCell*>(cell(x,y)) ;
        const CaloCell* p = const_cast<const CaloCell*> (p_);
        return p ;
    }
private:
    int nx,ny ;
    int len ;
    CaloCell* calocells ;

} ;




#endif