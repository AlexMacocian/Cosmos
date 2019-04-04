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
        public static double EARTH_SIZE = 1000;
        public static double EARTH_MASS = 1;
        public static double SUN_MASS = 10e5;
        public static double SUN_SIZE = 10e2 * EARTH_SIZE;
        public static double STAR_O_CLASS_CUTOFF = 0.03;
        public static double STAR_B_CLASS_CUTOFF = 0.13;
        public static double STAR_A_CLASS_CUTOFF = 0.6;
        public static double STAR_F_CLASS_CUTOFF = 3;
        public static double STAR_G_CLASS_CUTOFF = 7.6;
        public static double STAR_K_CLASS_CUTOFF = 12.1;
        public static double STAR_M_CLASS_CUTOFF = 76.45;
        public static double PLANET_ASTEROIDIAN_CUTOFF = 45.5;
        public static double PLANET_MERCURIAN_CUTOFF = 10.5;
        public static double PLANET_SUBTERRAN_CUTOFF = 8;
        public static double PLANET_TERRAN_CUTOFF = 11;
        public static double PLANET_SUPERTERRAN_CUTOFF = 15;
        public static double PLANET_NEPTUNIAN_CUTOFF = 7.5;
        public static double PLANET_JOVIAN_CUTOFF = 2.5;
        /// <summary>
        /// Ratio between distance to star and star radius
        /// </summary>
        public static double HABITABLE_ZONE_DISTANCE_RATIO = 1.6;
        /// <summary>
        /// Maximum value of the ratio between distance to star and star ratio.
        /// </summary>
        public static double MAX_DISTANCE_RATIO = 2.5;
        /// <summary>
        /// Minimum value of ratio between distance to star and star ratio.
        /// </summary>
        public static double MIN_DISTANCE_RATIO = 1.0001;
        /// <summary>
        /// PERCENTAGE OF MASS LOST TO THE BIGGER BODY DURING COLLISION
        /// </summary>
        public static double COLLISION_ABSORPTION_MULTIPLIER = 0.05;
    }
}
