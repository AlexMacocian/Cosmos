using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Structures
{
    public class CelestialBody
    {
        public int id;
        public double posX, posY, vX, vY, aX, aY, size, mass;
        public Barnes_Hut.BHNode containingNode;
        private bool outOfBounds;
        private volatile bool markedToRemove;
        private double sizepercmass;

        public bool OutOfBounds
        {
            get
            {
                return outOfBounds;
            }
        }

        public bool MarkedToRemove
        {
            get
            {
                return markedToRemove;
            }
        }

        public CelestialBody(int id, double posX, double posY, double mass, double size)
        {
            this.id = id;
            vX = 0;
            vY = 0;
            aX = 0;
            aY = 0;
            this.posX = posX;
            this.posY = posY;
            this.mass = mass;
            this.size = size;
            sizepercmass = size / mass;
        }

        public void ApplyForce(double fx, double fy)
        {
            aX += fx / mass;
            aY += fy / mass;
        }

        public bool Collides(CelestialBody otherBody)
        {
            double dx, dy, r;
            dx = this.posX - otherBody.posX;
            dy = this.posY - otherBody.posY;
            r = this.size / 2 + otherBody.size / 2;
            if (dx * dx + dy * dy <= r * r)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Conflicts(CelestialBody otherBody)
        {
            double dx, dy, r;
            dx = this.posX - otherBody.posX;
            dy = this.posY - otherBody.posY;
            r = this.size / 2 + otherBody.size / 2;
            if (dx * dx + dy * dy <= r * r / 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Attract(CelestialBody otherBody, out double fx, out double fy)
        {
            fx = this.posX - otherBody.posX;
            fy = this.posY - otherBody.posY;
            double distancesqrd = fx * fx + fy * fy;
            double strength = Constants.G * mass * otherBody.mass / distancesqrd;
            double length = Math.Sqrt(distancesqrd);
            fx /= distancesqrd;
            fy /= distancesqrd;
            fx *= strength;
            fy *= strength;
        }

        public void ResolveConflict(CelestialBody otherBody)
        {
            if(otherBody.mass < this.mass)
            {
                double totalMass = otherBody.mass + this.mass;
                this.mass = totalMass;
                otherBody.MarkToRemove();
            }
            else
            {
                MarkToRemove();
                double totalMass = otherBody.mass + this.mass;
                otherBody.mass = totalMass;
            }
        }

        public void ResolveCollisionWithAbsorption(CelestialBody otherBody)
        {
            if (otherBody.mass < this.mass)
            {
                double massToGive = otherBody.mass * Constants.COLLISION_ABSORPTION_MULTIPLIER;
                otherBody.mass -= massToGive;
                this.mass += massToGive;
                if(otherBody.mass < 1)
                {
                    double totalMass = otherBody.mass + this.mass;
                    this.mass = totalMass;
                    otherBody.MarkToRemove();
                }

            }
            else
            {
                double massToGive = this.mass * Constants.COLLISION_ABSORPTION_MULTIPLIER;
                this.mass -= massToGive;
                otherBody.mass += massToGive;
                if (this.mass < 1)
                {
                    double totalMass = otherBody.mass + this.mass;
                    otherBody.mass = totalMass;
                    MarkToRemove();
                }
            }
        }

        public virtual void Update()
        {
            if (aX > Constants.MAX_ACCELERATION)
            {
                aX = Constants.MAX_ACCELERATION;
            }
            else if (aX < -Constants.MAX_ACCELERATION)
            {
                aX = -Constants.MAX_ACCELERATION;
            }
            if (aY > Constants.MAX_ACCELERATION)
            {
                aY = Constants.MAX_ACCELERATION;
            }
            else if (aY < -Constants.MAX_ACCELERATION)
            {
                aY = -Constants.MAX_ACCELERATION;
            }
            vX += aX;
            vY += aY;
            posX += vX * Constants.TIME_CONSTANT;
            posY += vY * Constants.TIME_CONSTANT;
            aX = 0;
            aY = 0;
            if(posX > Galaxy.Instance.Width / 2)//OVERFLOW RIGHT
            {
                outOfBounds = true;
            }
            else if(posX < -Galaxy.Instance.Width / 2)//OVERFLOW LEFT
            {
                outOfBounds = true;
            }

            if(posY > Galaxy.Instance.Height / 2)//OVERFLOW BOT
            {
                outOfBounds = true;
            }
            else if(posY < -Galaxy.Instance.Height / 2)//OVERFLOW TOP
            {
                outOfBounds = true;
            }
            size = mass * sizepercmass;
            if (containingNode != null)
            {
                if (!containingNode.NodeFits(this) && !outOfBounds)
                {
                    containingNode.ReinsertChild();
                }
            }
        }

        /// <summary>
        /// Operation which marks this body for future removal. Cannot be reset.
        /// </summary>
        public void MarkToRemove()
        {
            markedToRemove = true;
        }
    }
}
