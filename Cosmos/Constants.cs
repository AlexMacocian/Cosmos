using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos
{
    class Constants
    {
        public static double G = 6.67408e-10;
        public static double TIME_CONSTANT = 10e7;
        public static double ITERATIONS_PER_CALCULATION = 2;
        public static double MAX_ACCELERATION = 10e6;
        public static int BH_MAX_DEPTH = 100;
        public static double Theta = 0.95;
        public static double ZOOM_LEVEL = 1;
        public static double EARTH_SIZE = 6;
        public static double SUN_MASS = 10e5;
        public static double SUN_SIZE = 10e2 * EARTH_SIZE;
        public static double O_CLASS_CUTOFF = 0.03;
        public static double B_CLASS_CUTOFF = 0.13;
        public static double A_CLASS_CUTOFF = 0.6;
        public static double F_CLASS_CUTOFF = 3;
        public static double G_CLASS_CUTOFF = 7.6;
        public static double K_CLASS_CUTOFF = 12.1;
        public static double M_CLASS_CUTOFF = 76.45;
    }
}
