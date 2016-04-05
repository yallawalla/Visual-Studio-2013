using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class Runge
    {
        public delegate double[] Function(double dt, double x, double[] y);
        public double[] err = new double[3];
        public double[] solve(double x, double[] y, double dt, Function f, double[] emax)
        {
            double[] k1, k2, k3, k4, k5;

            k1 = f(dt,x, y);
            k2 = f(dt,x, y.Zip(k1, (m, n) => (m + n / 2)).ToArray());
            k3 = f(dt,x, y.Zip(k2, (m, n) => (m + n / 2)).ToArray());
            k4 = f(dt,x, y.Zip(k3, (m, n) => (m + n)).ToArray());

            k5 = y.Zip(k1, (m, n) => (m + n)).ToArray();
            k5 = f(dt, x, k5.Zip(k2, (m, n) => (m + 2*n)).ToArray());

            err = k3.Zip(k2, (m, n) => (m/3 - n/3)).ToArray();
            err = err.Zip(k4, (m, n) => (m + n / 6)).ToArray();
            err = err.Zip(k5, (m, n) => (m - n / 6)).ToArray();

            //if (emax.Max(element => Math.Abs(element)) > 0 && err.Max(element => Math.Abs(element)) > 0)
            //{
            //    double fmin=emax.Zip(err, (m, n) => (Math.Pow(Math.Abs(m / n), 0.25))).ToArray().Min();   
            //    if (fmin > 1.2)
            //        dt = dt * fmin * 1.2;
            //    if (fmin < 1)
            //    {
            //        dt = dt * fmin * 0.8;
            //        return (solve(x, y,ref dt, f, emax));
            //    }
            //}


            if (err.Max(element => Math.Abs(element)) > 1e13)
            {
                y = solve(x, y, dt/2, f, emax);
                y = solve(x, y, dt/2, f, emax);
            }

            y = y.Zip(k1, (m, n) => (m + n / 6)).ToArray();
            y = y.Zip(k2, (m, n) => (m + n / 3)).ToArray();
            y = y.Zip(k3, (m, n) => (m + n / 3)).ToArray();
            y = y.Zip(k4, (m, n) => (m + n / 6)).ToArray();
            return y;
        }
    }
}
/*
Public Sub Integrate()
  
  Dim r As Integer
  Dim emax(0 To n - 1) As Double
  Dim h As Double, v As Double
  Dim yp(0 To n - 1) As Double, p(0 To 4, 0 To n - 1) As Double, err As Double, fmin As Double
  Dim mi As Integer, ni As Integer
  Dim ro As Double, y(0 To n - 1) As Double

  emax(0) = 1
  emax(1) = 0.1
  emax(2) = 50
  emax(3) = 1
  
  y(VX) = mvarMuzzleVelocity * Cos(mvarElevation)
  y(TANTH) = Tan(mvarElevation)
  y(ALT) = mvarHeight
  y(TIM) = 0
  
  r = 0
  While r <> mvarRange

    If r + mvarRangeStep > mvarRange Then
      mvarRangeStep = mvarRange - r
    End If
    
    For mi = 0 To 4
      Select Case mi
        Case 0
          For ni = 0 To n - 1
            yp(ni) = y(ni)
          Next ni
        Case 1, 2
          For ni = 0 To n - 1
            yp(ni) = y(ni) + p(mi - 1, ni) / 2#
          Next ni
        Case 3
          For ni = 0 To n - 1
            yp(ni) = y(ni) + p(mi - 1, ni)
          Next ni
        Case 4
          For ni = 0 To ni - 1
            yp(ni) = y(ni) + 2# * p(1, ni) + p(0, ni)
          Next ni
      End Select

      h = yp(ALT)
      v = Sqr(1 + yp(TANTH) * yp(TANTH)) * yp(VX)
      ro = mvarAir.Ventzel(h, v)
      p(mi, TIM) = mvarRangeStep / yp(VX)
      p(mi, VX) = -C_ro * ro * mvarRangeStep * mvarFactor
      p(mi, ALT) = mvarRangeStep * yp(TANTH)
      p(mi, TANTH) = -mvarRangeStep * M_G / yp(VX) / yp(VX)
    Next mi
    
    If mvarVariableStep = True Then
      fmin = 100#
      For ni = 0 To n - 1
        err = Abs((p(3, ni) + 2# * p(2, ni) - 2# * p(1, ni) - p(4, ni)) / 6#)
        If err > 0 Then
          If fmin > Sqr(Sqr(emax(ni) / err)) Then
            fmin = Sqr(Sqr(emax(ni) / err))
          End If
        End If
      Next ni
      If fmin < 1# Then
        mvarRangeStep = 0.8 * fmin * mvarRangeStep
        Integrate
        Exit Sub
      End If
      r = r + mvarRangeStep
      If fmin > 1.2 Then
        mvarRangeStep = 0.8 * fmin * mvarRangeStep
      End If
    Else
      r = r + mvarRangeStep
    End If
    For ni = 0 To n - 1
      y(ni) = y(ni) + (p(0, ni) + 2# * (p(1, ni) + p(2, ni)) + p(3, ni)) / 6#
    Next ni
  Wend
    
  mvarHeight = y(ALT)
  mvarElevation = Atn(y(TANTH))
  mvarMuzzleVelocity = y(VX) / Cos(mvarElevation)
  mvarTime = y(TIM)
End Sub
*/
