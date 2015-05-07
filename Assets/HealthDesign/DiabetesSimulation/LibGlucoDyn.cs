using System;
namespace AssemblyCSharp
{
	public class LibGlucoDyn
	{
		public static double iob(double g,int idur) {  
			double tot;
			if(g<=0.0) {
				tot=100.0;
			} else if (g>=idur*60.0) {
				tot=0.0;
			} else {
				if(idur==3) {
					tot=-3.203e-7*Math.Pow(g,4)+1.354e-4*Math.Pow(g,3)-1.759e-2*Math.Pow(g,2)+9.255e-2*g+99.951;
				} else if (idur==4) {
					tot=-3.31e-8*Math.Pow(g,4)+2.53e-5*Math.Pow(g,3)-5.51e-3*Math.Pow(g,2)-9.086e-2*g+99.95;
				} else if (idur==5) {
					tot=-2.95e-8*Math.Pow(g,4)+2.32e-5*Math.Pow(g,3)-5.55e-3*Math.Pow(g,2)+4.49e-2*g+99.3;
				} else if (idur==6) {
					tot=-1.493e-8*Math.Pow(g,4)+1.413e-5*Math.Pow(g,3)-4.095e-3*Math.Pow(g,2)+6.365e-2*g+99.7;
				} 
				tot=100.0;//GAMESTART ADDED THIS DEFAULT CASE!!! THIS IS NOT ORIGINALLY PART OF GLUCODYN!!!
			}          
			return(tot);
		}

		public static double intIOB(long x1,long x2, int idur,double g) {
			double integral;
			double dx;
			int nn=50; //nn needs to be even
			int ii=1;
			
			//initialize with first and last terms of simpson series
			dx=(x2-x1)/nn;
			integral=iob((g-x1),idur)+iob(g-(x1+nn*dx),idur);
			
			while(ii<nn-2) {
				integral = integral + 4*iob(g-(x1+ii*dx),idur)+2*iob(g-(x1+(ii+1)*dx),idur);
				ii=ii+2;
			}
			
			integral=integral*dx/3.0;
			return(integral);
			
		}

		public static double cob(double g, int ct){
			double tot;
			if(g<=0) {
				tot=0.0;
			} else if (g>=ct) {
				tot=1.0;
			} else if ((g>0)&&(g<=ct/2.0)) {
				tot=2.0/Math.Pow(ct,2)*Math.Pow(g,2);
			} else {
				tot=-1.0+4.0/ct*(g-Math.Pow(g,2)/(2.0*ct));
			}
			return(tot);
		}

		
		public static double deltatempBGI(double g,double dbdt,double sensf,int idur,long t1,long t2) {
			return -dbdt*sensf*((t2-t1)-1/100*intIOB(t1,t2,idur,g));
		}
		
		public static double deltaBGC(double g,double sensf,double cratio,int camount, int ct) {
			return sensf/cratio*camount*cob(g,ct);
		}
		
		public static double deltaBGI(double g,double unitsInsulinInBolus,double sensf, int idur) {
			return -unitsInsulinInBolus*sensf*(1-iob(g,idur)/100.0);
		}

	}
}