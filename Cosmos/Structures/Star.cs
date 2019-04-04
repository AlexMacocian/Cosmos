using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Structures
{
    public class Star : CelestialBody
    {
        //CLASSIFICATION FOR TEMPERATURE: O, B, A, F, G, K, M
        //A SUBDIVISION FROM 0-9 FOR TEMPERATURE
        //FOR SIZE BASED ON ROMAN NUMERALS: 0, Ia, Ib, II, III, IV, V, VI, VII

        public enum Class
        {
            O,
            B,
            A,
            F,
            G,
            K,
            M
        }

        public Class StarClass;
        public List<Planet> OrbitingPlanets;


        public Star(int id, double posX, double posY, double mass, double size, Class starClass) : base(id, posX, posY, mass, size)
        {
            this.StarClass = starClass;
            OrbitingPlanets = new List<Planet>();
        }

        public override void Update()
        {
            base.Update();
            if(mass > Constants.SUN_MASS * 16)
            {
                StarClass = Class.O;
            }
            else if(mass > Constants.SUN_MASS * 2.1)
            {
                StarClass = Class.B;
            }
            else if(mass > Constants.SUN_MASS * 1.4)
            {
                StarClass = Class.A;
            }
            else if(mass > Constants.SUN_MASS * 1.04)
            {
                StarClass = Class.F;
            }
            else if(mass > Constants.SUN_MASS * 0.8)
            {
                StarClass = Class.G;
            }
            else if(mass > Constants.SUN_MASS * 0.45)
            {
                StarClass = Class.K;
            }
            else
            {
                StarClass = Class.M;
            }
        }

        /// <summary>
        /// Generate a star
        /// </summary>
        /// <param name="randomValue">Value between 0 and 1</param>
        /// <param name="id">ID of celestial body</param>
        /// <param name="posX">X coordinate</param>
        /// <param name="posY">Y coordinate</param>
        /// <returns></returns>
        public static Star GenerateRandomStar(int id, double randomValue, double posX, double posY)
        {
            Class starClass;
            double size = Constants.SUN_SIZE;
            double mass = Constants.SUN_MASS;
            randomValue *= 100;
            if(randomValue <= Constants.STAR_O_CLASS_CUTOFF) //CLASS O STAR, >= 16 M
            {
                double massscale = 6 + (randomValue / Constants.STAR_O_CLASS_CUTOFF) * 4;
                double sizescale = 3 + (randomValue / Constants.STAR_O_CLASS_CUTOFF) * 2;
                mass *= massscale;
                size *= sizescale;
                starClass = Class.O;
            }
            else if(randomValue <= Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF) //CLASS B STAR, 2.1 -> 16M
            {
                double massscale = 2.1 + (randomValue / (Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)) * 13.9;
                double sizescale = 1.9 + (randomValue / (Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)) * 1.1;
                mass *= massscale;
                size *= sizescale;
                starClass = Class.B;
            }
            else if(randomValue <= Constants.STAR_A_CLASS_CUTOFF + Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)//CLASS A 1.4 -> 2.1M
            {
                double massscale = 1.4 + (randomValue / (Constants.STAR_A_CLASS_CUTOFF + Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)) * 0.7;
                double sizescale = 1.4 + (randomValue / (Constants.STAR_A_CLASS_CUTOFF + Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)) * 0.5;
                mass *= massscale;
                size *= sizescale;
                starClass = Class.A;
            }
            else if(randomValue <= Constants.STAR_F_CLASS_CUTOFF + Constants.STAR_A_CLASS_CUTOFF + Constants.STAR_B_CLASS_CUTOFF +
                Constants.STAR_O_CLASS_CUTOFF) //CLASS F 1.04 -> 1.4M
            {
                double massscale = 1.04 + (randomValue / (Constants.STAR_F_CLASS_CUTOFF + Constants.STAR_A_CLASS_CUTOFF + Constants.STAR_B_CLASS_CUTOFF +
                Constants.STAR_O_CLASS_CUTOFF)) * 0.3;
                double sizescale = 1.04 + (randomValue / (Constants.STAR_F_CLASS_CUTOFF + Constants.STAR_A_CLASS_CUTOFF + Constants.STAR_B_CLASS_CUTOFF +
                Constants.STAR_O_CLASS_CUTOFF)) * 0.26;
                mass *= massscale;
                size *= sizescale;
                starClass = Class.F;
            }
            else if(randomValue <= Constants.STAR_G_CLASS_CUTOFF + Constants.STAR_F_CLASS_CUTOFF + Constants.STAR_A_CLASS_CUTOFF +
                Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF) //CLASS G 0.8 -> 1.04M
            {
                double massscale = 0.8 + (randomValue / (Constants.STAR_G_CLASS_CUTOFF + Constants.STAR_F_CLASS_CUTOFF + Constants.STAR_A_CLASS_CUTOFF +
                Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)) * 0.24;
                double sizescale = 0.8 + (randomValue / (Constants.STAR_G_CLASS_CUTOFF + Constants.STAR_F_CLASS_CUTOFF + Constants.STAR_A_CLASS_CUTOFF +
                Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)) * 0.24;
                mass *= massscale;
                size *= sizescale;
                starClass = Class.G;
            }
            else if(randomValue <= Constants.STAR_K_CLASS_CUTOFF + Constants.STAR_G_CLASS_CUTOFF + Constants.STAR_F_CLASS_CUTOFF +
                Constants.STAR_A_CLASS_CUTOFF + Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)
            {
                double massscale = 0.45 + (randomValue / (Constants.STAR_K_CLASS_CUTOFF + Constants.STAR_G_CLASS_CUTOFF + Constants.STAR_F_CLASS_CUTOFF +
                Constants.STAR_A_CLASS_CUTOFF + Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)) * 0.35;
                double sizescale = 0.45 + (randomValue / (Constants.STAR_K_CLASS_CUTOFF + Constants.STAR_G_CLASS_CUTOFF + Constants.STAR_F_CLASS_CUTOFF +
                Constants.STAR_A_CLASS_CUTOFF + Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)) * 0.35;
                mass *= massscale;
                size *= sizescale;
                starClass = Class.K;
            }
            else
            {
                double massscale = 0.08 + (randomValue / (Constants.STAR_M_CLASS_CUTOFF + Constants.STAR_K_CLASS_CUTOFF + Constants.STAR_G_CLASS_CUTOFF + Constants.STAR_F_CLASS_CUTOFF +
                Constants.STAR_A_CLASS_CUTOFF + Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)) * 0.37;
                double sizescale = 0.08 + (randomValue / (Constants.STAR_M_CLASS_CUTOFF + Constants.STAR_K_CLASS_CUTOFF + Constants.STAR_G_CLASS_CUTOFF + Constants.STAR_F_CLASS_CUTOFF +
                Constants.STAR_A_CLASS_CUTOFF + Constants.STAR_B_CLASS_CUTOFF + Constants.STAR_O_CLASS_CUTOFF)) * 0.37;
                mass *= massscale;
                size *= sizescale;
                starClass = Class.M;
            }

            Star star = new Star(id, posX, posY, mass, size, starClass);
            return star;
        }
    }
}
