#ifndef CALOCELL_HH
#define CALOCELL_HH

using namespace std ;

class CaloCell {

public:
    CaloCell(double Energy = 0,int Id = 0) {          // n) Assigned default values so we can initialize an array of calocells in CaloGrid
        energy = Energy ;
        ID = Id ;
        cout << "CaloCell constructor" << this << endl ;
    }
// c , d)
// Accessors - return reference to private data member
// implemented both reference and const reference so we can instantiate const CaloCell
    double& get_energy() {
        return energy ;
    }
    const double& get_energy() const {
        return energy ;
    }
    
    int& get_ID() {
        return ID ;
    }
    const int& get_ID() const {
        return ID ;
    }
// Modifiers
    void set_energy(float new_energy) {
        energy = new_energy ;
        return ;
    }

    void set_ID(int new_ID) {
        ID = new_ID ;
        return ;
    }

private:
    double energy ;
    int ID ;

} ;

#endif

// a) private members double energy and int ID added
// b) CaloCell is not a very complex object with a lot of parameters that we would like to assign to many of its copied instances. 
// Since the default copy constructor makes a copy that can be modified without affecting the original and vice versa (checked in main), there is no need for a custom copy constructor
// Implementaton of a custom destructor is also not necessary because there is no fear of memory leakage or any other pathological behaviour