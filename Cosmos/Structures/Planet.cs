using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Structures
{
    public class Planet : CelestialBody
    {
        public enum Class
        {
            Asteroidian,
            Mercurian,
            Subterran,
            Terran,
            Superterran,
            Neptunian,
            Jovian
        }
        public enum Habitability
        {
            hP, //hypopsychroplanet < -50C
            P, //psychroplanet < 0C
            M, //mesoplanet 0C <=> 50C
            T, //thermoplanet  > 50C
            hT //hyperthermoplanet >100C
        }
        public enum Rotation
        {
            Clockwise,
            CounterClockwise
        }

        private bool inOrbit;
        private double theta = 0, distanceToStar = 0;
        private Star star;
        public double temperature;
        public Class PlanetClass;
        public Habitability PlanetHabitability;
        public Rotation RotationType;

        public Star Star
        {
            get => star;
            set
            {
                star = value;
                if(star != null)
                {
                    double dx, dy;
                    dx = star.posX - posX;
                    dy = star.posY - posY;
                    distanceToStar = Helper.Fast_Sqrt((float)(dx * dx + dy * dy));
                    theta = Math.Acos(posX / distanceToStar);
                }
            }
        }

        public Planet(int id, double posX, double posY, double mass, double size, Class planetClass, Habitability habitability) : base(id, posX, posY, mass, size)
        {
            this.PlanetClass = planetClass;
            this.PlanetHabitability = habitability;
        }

        public Planet(int id, double posX, double posY, double mass, double size, Class planetClass, Habitability habitability, Star star, double thetaAngle, double distanceToStar, double temperature, Rotation rotationType) : base(id, posX, posY, mass, size)
        {
            this.PlanetClass = planetClass;
            this.PlanetHabitability = habitability;
            this.star = star;
            this.temperature = temperature;
            this.theta = thetaAngle;
            this.distanceToStar = distanceToStar;
            this.RotationType = rotationType;
        }

        public override void Update()
        {
            base.Update();
            if (RotationType == Rotation.Clockwise)
            {
                theta += 10e-13 * Constants.TIME_CONSTANT;
                if (theta > 2 * Math.PI)
                {
                    theta = 0;
                }
            }
            else
            {
                theta -= 10e-13 * Constants.TIME_CONSTANT;
                if (theta < 0)
                {
                    theta = 2 * Math.PI;
                }
            }
            posX = star.posX + distanceToStar * Math.Sin(theta);
            posY = star.posY + distanceToStar * Math.Cos(theta);
        }

        /// <summary>
        /// Generate a planet based on a random value
        /// </summary>
        /// <param name="id">ID of celestial body</param>
        /// <param name="randomValue">Random value between 0 and 1</param>
        /// <param name="posX">X coordinate of planet</param>
        /// <param name="posY">Y coordinate of planet</param>
        /// <param name="star">Star that this planet orbits</param>
        /// <returns></returns>
        public static Planet GeneratePlanet(int id, double randomValue, double posX, double posY, Star star)
        {
            Class planetClass;
            Habitability habitability;
            randomValue *= 100;
            #region MASS AND SIZE CALCULATIONS
            double size = Constants.EARTH_SIZE;
            double mass = Constants.EARTH_MASS;
            if (randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF)
            {
                size *= 0.1 + (randomValue / Constants.PLANET_ASTEROIDIAN_CUTOFF) * 0.2;
                mass *= (randomValue / Constants.PLANET_ASTEROIDIAN_CUTOFF) * 0.00001;
                planetClass = Class.Asteroidian;
            }
            else if(randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF)
            {
                size *= 0.3 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF)) * 0.4;
                mass *= 0.00001 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF)) * 0.1;
                planetClass = Class.Mercurian;
            }
            else if(randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                Constants.PLANET_SUBTERRAN_CUTOFF)
            {
                size *= 0.5 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF + 
                    Constants.PLANET_SUBTERRAN_CUTOFF)) * 0.7;
                mass *= 0.1 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF + 
                    Constants.PLANET_SUBTERRAN_CUTOFF)) * 0.5;
                planetClass = Class.Subterran;
            }
            else if(randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                Constants.PLANET_SUBTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF)
            {
                size *= 0.8 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF + 
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF)) * 1.1;
                mass *= 0.5 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF + 
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF)) * 1.5;
                planetClass = Class.Terran;
            }
            else if(randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                Constants.PLANET_SUBTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF)
            {
                size *= 1.3 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF)) * 2;
                mass *= 2 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF)) * 8;
                planetClass = Class.Superterran;
            }
            else if(randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF + 
                Constants.PLANET_SUBTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF +
                Constants.PLANET_NEPTUNIAN_CUTOFF)
            {
                size *= 2.1 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF +
                    Constants.PLANET_NEPTUNIAN_CUTOFF)) * 3.6;
                mass *= 10 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF + 
                    Constants.PLANET_NEPTUNIAN_CUTOFF)) * 40;
                planetClass = Class.Neptunian;
            }
            else
            {
                size *= 3.5 + (randomValue / 100) * 6.5;
                mass *= 50 + (randomValue / 100) * 150;
                planetClass = Class.Jovian;
            }
            #endregion
            #region HABITABILITY CALCULATIONS
            double dx, dy;
            dx = star.posX - posX;
            dy = star.posY - posY;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            double temperature;
            if(distance / star.size < Constants.HABITABLE_ZONE_DISTANCE_RATIO)
            {
                temperature = (Constants.HABITABLE_ZONE_DISTANCE_RATIO - distance / star.size) * 1000;
            }
            else
            {
                temperature = (distance / star.size - Constants.HABITABLE_ZONE_DISTANCE_RATIO) * -250;
            }

            if(temperature < -50)
            {
                habitability = Habitability.hP;
            }
            else if(temperature < 0)
            {
                habitability = Habitability.P;
            }
            else if(temperature < 50)
            {
                habitability = Habitability.M;
            }
            else if(temperature < 100)
            {
                habitability = Habitability.T;
            }
            else
            {
                habitability = Habitability.hT;
            }
            #endregion
            double sin = star.posX * posY - posX * star.posY;
            double cos = star.posX * posX + star.posY * posY;
            double theta = Math.Atan2(sin, cos);
            Rotation rot = Rotation.CounterClockwise;
            if (randomValue < 50)
            {
                rot = Rotation.Clockwise;
            }
            return new Planet(id, posX, posY, mass, size, planetClass, habitability, star, theta, distance, temperature, rot);
        }

        /// <summary>
        /// Generate a planet based on a random value
        /// </summary>
        /// <param name="id">ID of celestial body</param>
        /// <param name="randomValue">Random value between 0 and 1</param>
        /// <param name="posX">X coordinate of planet</param>
        /// <param name="posY">Y coordinate of planet</param>
        /// <param name="star">Star that this planet orbits</param>
        /// <param name="theta">Angle between star and planet</param>
        /// <returns></returns>
        public static Planet GeneratePlanet(int id, double randomValue, double posX, double posY, Star star, double theta)
        {
            Class planetClass;
            Habitability habitability;
            randomValue *= 100;
            #region MASS AND SIZE CALCULATIONS
            double size = Constants.EARTH_SIZE;
            double mass = Constants.EARTH_MASS;
            if (randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF)
            {
                size *= 0.1 + (randomValue / Constants.PLANET_ASTEROIDIAN_CUTOFF) * 0.2;
                mass *= (randomValue / Constants.PLANET_ASTEROIDIAN_CUTOFF) * 0.00001;
                planetClass = Class.Asteroidian;
            }
            else if (randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF)
            {
                size *= 0.3 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF)) * 0.4;
                mass *= 0.00001 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF)) * 0.1;
                planetClass = Class.Mercurian;
            }
            else if (randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                Constants.PLANET_SUBTERRAN_CUTOFF)
            {
                size *= 0.5 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUBTERRAN_CUTOFF)) * 0.7;
                mass *= 0.1 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUBTERRAN_CUTOFF)) * 0.5;
                planetClass = Class.Subterran;
            }
            else if (randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                Constants.PLANET_SUBTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF)
            {
                size *= 0.8 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF)) * 1.1;
                mass *= 0.5 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF)) * 1.5;
                planetClass = Class.Terran;
            }
            else if (randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                Constants.PLANET_SUBTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF)
            {
                size *= 1.3 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF)) * 2;
                mass *= 2 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF)) * 8;
                planetClass = Class.Superterran;
            }
            else if (randomValue < Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                Constants.PLANET_SUBTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF +
                Constants.PLANET_NEPTUNIAN_CUTOFF)
            {
                size *= 2.1 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF +
                    Constants.PLANET_NEPTUNIAN_CUTOFF)) * 3.6;
                mass *= 10 + (randomValue / (Constants.PLANET_ASTEROIDIAN_CUTOFF + Constants.PLANET_MERCURIAN_CUTOFF +
                    Constants.PLANET_SUPERTERRAN_CUTOFF + Constants.PLANET_TERRAN_CUTOFF + Constants.PLANET_SUPERTERRAN_CUTOFF +
                    Constants.PLANET_NEPTUNIAN_CUTOFF)) * 40;
                planetClass = Class.Neptunian;
            }
            else
            {
                size *= 3.5 + (randomValue / 100) * 6.5;
                mass *= 50 + (randomValue / 100) * 150;
                planetClass = Class.Jovian;
            }
            #endregion
            #region HABITABILITY CALCULATIONS
            double dx, dy;
            dx = star.posX - posX;
            dy = star.posY - posY;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            double temperature;
            if (distance / star.size < Constants.HABITABLE_ZONE_DISTANCE_RATIO)
            {
                temperature = (Constants.HABITABLE_ZONE_DISTANCE_RATIO - distance / star.size) * 1000;
            }
            else
            {
                temperature = (distance / star.size - Constants.HABITABLE_ZONE_DISTANCE_RATIO) * -250;
            }

            if (temperature < -50)
            {
                habitability = Habitability.hP;
            }
            else if (temperature < 0)
            {
                habitability = Habitability.P;
            }
            else if (temperature < 50)
            {
                habitability = Habitability.M;
            }
            else if (temperature < 100)
            {
                habitability = Habitability.T;
            }
            else
            {
                habitability = Habitability.hT;
            }
            #endregion
            Rotation rot = Rotation.CounterClockwise;
            if (randomValue < 50)
            {
                rot = Rotation.Clockwise;
            }
            return new Planet(id, posX, posY, mass, size, planetClass, habitability, star, theta, distance, temperature, rot);
        }
    }
}
