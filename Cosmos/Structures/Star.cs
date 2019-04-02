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

        public Class starClass;


        public Star(int id, double posX, double posY, double mass, double size, Class starClass) : base(id, posX, posY, mass, size)
        {
            this.starClass = starClass;
        }

        /// <summary>
        /// Generate a star
        /// </summary>
        /// <param name="randomValue">Value between 0 and 1</param>
        /// <returns></returns>
        public static Star GenerateRandomStar(int id, double randomValue, double posX, double posY)
        {
            Class starClass;
            double size = Constants.SUN_SIZE;
            double mass = Constants.SUN_MASS;
            if(randomValue <= Constants.O_CLASS_CUTOFF) //CLASS O STAR, >= 16 M
            {
                double scale = 16 + (randomValue / Constants.O_CLASS_CUTOFF) * 4;
                mass *= scale;
                size *= scale;
                starClass = Class.O;
            }
            else if(randomValue <= Constants.B_CLASS_CUTOFF + Constants.O_CLASS_CUTOFF) //CLASS B STAR, 2.1 -> 16M
            {
                double scale = 2.1 + (randomValue / (Constants.B_CLASS_CUTOFF + Constants.O_CLASS_CUTOFF)) * 13.9;
                mass *= scale;
                size *= scale;
                starClass = Class.B;
            }
            else if(randomValue <= Constants.A_CLASS_CUTOFF + Constants.B_CLASS_CUTOFF + Constants.O_CLASS_CUTOFF)//CLASS A 1.4 -> 2.1M
            {
                double scale = 1.4 + (randomValue / (Constants.A_CLASS_CUTOFF + Constants.B_CLASS_CUTOFF + Constants.O_CLASS_CUTOFF)) * 0.7;
                mass *= scale;
                size *= scale;
                starClass = Class.A;
            }
            else if(randomValue <= Constants.F_CLASS_CUTOFF + Constants.A_CLASS_CUTOFF + Constants.B_CLASS_CUTOFF +
                Constants.O_CLASS_CUTOFF) //CLASS F 1.04 -> 1.4M
            {
                double scale = 1.04 + (randomValue / (Constants.F_CLASS_CUTOFF + Constants.A_CLASS_CUTOFF + Constants.B_CLASS_CUTOFF +
                Constants.O_CLASS_CUTOFF)) * 0.3;
                mass *= scale;
                size *= scale;
                starClass = Class.F;
            }
            else if(randomValue <= Constants.G_CLASS_CUTOFF + Constants.F_CLASS_CUTOFF + Constants.A_CLASS_CUTOFF +
                Constants.B_CLASS_CUTOFF + Constants.O_CLASS_CUTOFF) //CLASS G 0.8 -> 1.04M
            {
                double scale = 0.8 + (randomValue / (Constants.G_CLASS_CUTOFF + Constants.F_CLASS_CUTOFF + Constants.A_CLASS_CUTOFF +
                Constants.B_CLASS_CUTOFF + Constants.O_CLASS_CUTOFF)) * 0.24;
                mass *= scale;
                size *= scale;
                starClass = Class.G;
            }
            else if(randomValue <= Constants.K_CLASS_CUTOFF + Constants.G_CLASS_CUTOFF + Constants.F_CLASS_CUTOFF +
                Constants.A_CLASS_CUTOFF + Constants.B_CLASS_CUTOFF + Constants.O_CLASS_CUTOFF)
            {
                double scale = 0.45 + (randomValue / (Constants.K_CLASS_CUTOFF + Constants.G_CLASS_CUTOFF + Constants.F_CLASS_CUTOFF +
                Constants.A_CLASS_CUTOFF + Constants.B_CLASS_CUTOFF + Constants.O_CLASS_CUTOFF)) * 0.35;
                mass *= scale;
                size *= scale;
                starClass = Class.K;
            }
            else
            {
                double scale = 0.08 + (randomValue / (Constants.M_CLASS_CUTOFF + Constants.K_CLASS_CUTOFF + Constants.G_CLASS_CUTOFF + Constants.F_CLASS_CUTOFF +
                Constants.A_CLASS_CUTOFF + Constants.B_CLASS_CUTOFF + Constants.O_CLASS_CUTOFF)) * 0.37;
                mass *= scale;
                size *= scale;
                starClass = Class.M;
            }

            Star star = new Star(id, posX, posY, mass, size, starClass);
            return star;
        }
    }
}
