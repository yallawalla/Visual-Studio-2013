#ifndef POINT_HH
#define POINT_HH

using namespace std ;

class Point {
public:
	Point(double X = 0, double Y = 0, double Z = 0) {
		cout << "Point constructor" << this << endl;
		x = X;
		y = Y;
		z = Z;
	}
	~Point() {
		cout << "Point destructor" << this << endl;
	}
	// h)
// Accessors - return regular or const reference to private data member
    double& get_x() { return x ;}
    const double& get_x() const { return x ;}
    double& get_y() { return y ;}
    const double& get_y() const { return y ;}
    double& get_z() { return z ;}
    const double& get_z() const { return z ;}
// Modifiers
    void set_x(double X) { x=X ;}
    void set_y(double Y) { y=Y ;}
    void set_z(double Z) { z=Z ;}
// i) Set position in single call
    void set_position(double X,double Y, double Z) {
        set_x(X) ;
        set_y(Y) ;
        set_z(Z) ;
    }

private:
    double x,y,z ;

} ;
// b)
// Relevance of copy constructor and destructor same as in CaloCell.hh
// j)
// Every accessor member function has its const twin, same argument as in CaloCell.hh 



#endif