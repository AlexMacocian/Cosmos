using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cosmos.Structures
{
    class Galaxy
    {
        public volatile bool requestUpdate;
        public static Galaxy Instance;
        public static Random Rand = new Random();

        public object quadLock = new object();
        public double maxDepth = 0;
        private volatile int toRemove = 0;
        private volatile int iterations = 0;
        private int width, height;
        private int minX, minY, maxX, maxY;
        private bool running = false, calculating = false;
        volatile public List<CelestialBody> Bodies;
        public int Count;
        public Barnes_Hut.BHNode root;
        public int N;

        public int Iterations
        {
            get
            {
                return iterations;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        public int ToRemove
        {
            get
            {
                return toRemove;
            }
        }

        public Galaxy(int width, int height, int systems, int minBodsPerSys, int maxBodsPerSys)
        {
            this.width = width;
            this.height = height;
            root = new Barnes_Hut.BHNode(int.MaxValue, int.MaxValue);
            Bodies = new List<CelestialBody>(systems * maxBodsPerSys);
            Count = N;
            double totalMass = 0;
            for (int i = 0; i < systems; i++)
            {
                double posX, posY;
                double theta = Rand.NextDouble() * (Math.PI * 2);//GET VALUE BETWEEN 0 AND 2PI
                double radius = Rand.Next(0, Math.Min(width, height) / 2);
                posX = radius * Math.Cos(theta);
                posY = radius * Math.Sin(theta);
                double randomValue = Rand.Next(0, 100000000);
                randomValue /= 100000000;
                Star star = Star.GenerateRandomStar(i, randomValue, posX, posY);
                Bodies.Add(star);
                totalMass += star.mass;
                double bodies = Rand.Next(minBodsPerSys, maxBodsPerSys);
                for(int j = 0; j < bodies; j++)
                {
                    double rand = Rand.NextDouble();
                    double mass2 = Constants.EARTH_SIZE * (0.2 + rand * 50);
                    double d2 = mass2;
                    double theta2 = Rand.NextDouble() * (Math.PI * 2);
                    double posX2, posY2;
                    double radius2 = Rand.Next((int)star.size, (int)(star.size * 1.4));
                    posX2 = radius2 * Math.Cos(theta2);
                    posY2 = radius2 * Math.Sin(theta2);
                    posX2 += posX;
                    posY2 += posY;
                    CelestialBody satellite = new CelestialBody(systems + (i * j), posX2, posY2, mass2, d2);
                    Bodies.Add(satellite);
                    GenerateOrbitalAcceleration(star, satellite);
                    totalMass += mass2;
                }
            }
            //Spawn black hole in middle
            //double px = 0, py = 0;
            //double m = 10e15 * N;
            //Bodies.Insert(0, new CelestialBody(0, px, py, m, m / 200));
            //Galaxy.Instance = this;
            //for (int i = 1; i < N; i++) //Apply orbital velocity
            //{
            //    GenerateOrbitalAcceleration(Bodies[i], m);
            //    Bodies[i].Update();
            //}
            this.N = Bodies.Count;
        }

        public Galaxy(int width, int height, int N)
        {
            this.N = N;
            this.width = width;
            this.height = height;
            double galaxyradius;
            if(width > height)
            {
                galaxyradius = height / 2;
            }
            else
            {
                galaxyradius = width / 2;
            }
            root = new Barnes_Hut.BHNode(int.MaxValue, int.MaxValue);
            Bodies = new  List<CelestialBody>(N);
            Count = N;
            double totalMass = 0;
            for(int i = 1; i < N; i++)
            {
                double mass, density;
                double posX, posY;
                double theta = Rand.NextDouble() * (Math.PI * 2);//GET VALUE BETWEEN 0 AND 2PI
                double radius = Math.Max(50, Math.Log(galaxyradius)) + Rand.NextDouble() * galaxyradius / 10;
                posX = radius * Math.Cos(theta);
                posY = radius * Math.Sin(theta);
                if(i % 100 == 0) //Spawn star once in 100
                {
                    mass = Rand.Next(1, 10);
                    density = Rand.Next(1, 10);
                    density *= 10e8;
                    mass *= 10e9;
                    
                }
                else
                {
                    mass = Rand.Next(1, 10);
                    density = Rand.Next(1, 10);
                    density /= 10;
                    
                }
                Bodies.Add(new CelestialBody(i, posX, posY, mass, density));
                totalMass += mass;
            }
            //Spawn black hole in middle
            double px = 0, py = 0;
            double m = 10e15 * N;
            Bodies.Insert(0, new CelestialBody(0, px, py, m, m / 200));
            Galaxy.Instance = this;
            for (int i = 1; i < N; i++) //Apply orbital velocity
            {
                GenerateOrbitalAcceleration(Bodies[i], m);
                Bodies[i].Update();
            }
        }

        public void UpdateGalaxy()
        {
            foreach(CelestialBody body in Bodies)
            {
                foreach(CelestialBody otherBody in Bodies)
                {
                    if (body.id != otherBody.id)
                    {
                        if (!body.Collides(otherBody))
                        {
                            double fx = 0, fy = 0;
                            otherBody.Attract(body, out fx, out fy);
                            body.ApplyForce(fx, fy);
                        }
                    }
                }
                body.Update();
                if (body.OutOfBounds && !body.MarkedToRemove)
                {
                    body.MarkToRemove();
                }
            }
            iterations++;
            //RECREATE LIST WITHOUT REMOVED OBJECTS
            if (toRemove > 500)
            {
                List<CelestialBody> newList = new List<CelestialBody>(N - toRemove);
                foreach (CelestialBody body in Bodies)
                {
                    if (!body.OutOfBounds)
                    {
                        newList.Add(body);
                    }
                }
                N = newList.Count;
                Bodies = newList;
                toRemove = 0;
            }
        }

        public void UpdateGalaxyThreaded()
        {
            if (!running)
            {
                running = true;
                Thread t = new Thread(() => {
                    while (true)
                    {
                        UpdateGalaxy();
                    }
                });
                t.Start();
            }
        }

        public void UpdateGalaxyOptimized()
        {
            //RECREATE LIST WITHOUT REMOVED OBJECTS
            if (toRemove > 0)
            {
                List<CelestialBody> newList = new List<CelestialBody>(N - toRemove);
                foreach (CelestialBody body in Bodies)
                {
                    if (!body.MarkedToRemove)
                    {
                        newList.Add(body);
                    }
                }
                N = newList.Count;
                Bodies = newList;
                Interlocked.Exchange(ref toRemove, 0);
            }

            if (Galaxy.Instance.Iterations % 50 == 0 || toRemove > 500) //RECREATE TREE STRUCTURE
            {
                while (!Monitor.TryEnter(quadLock))
                {

                }
                root.Reset();
                Monitor.Exit(quadLock);
                GC.Collect();

            }
            else
            {
                while (!Monitor.TryEnter(quadLock))
                {

                }
                root.Clear();
                Monitor.Exit(quadLock);
            }



            while (!Monitor.TryEnter(quadLock))
            {

            }
            root.RootInsertBatchThreaded(Bodies);

            Monitor.Exit(quadLock);

            Parallel.ForEach(Bodies, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 }, (body) =>
             {
                 if (!body.MarkedToRemove)
                 {
                     root.CalculateForce(body);
                 }
             });

            Parallel.ForEach(Bodies, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 }, (body) =>
            {
                if (body.MarkedToRemove)
                {
                    Interlocked.Increment(ref toRemove);//INCREMENT THE NUMBER OF NODES THAT WILL BE REMOVED
                }
                else
                {
                    body.Update();
                    if (body.OutOfBounds)
                        if (!body.MarkedToRemove)
                        {
                            body.MarkToRemove();
                            Interlocked.Increment(ref toRemove);//INCREMENT THE NUMBER OF NODES THAT WILL BE REMOVED
                        }
                }
            });
            Interlocked.Increment(ref iterations);
            requestUpdate = false;
        }

        public void UpdateGalaxyOptimizedThreaded()
        {
            if (!running)
            {
                running = true;
                Thread t = new Thread(() => {
                    Interlocked.Exchange(ref iterations, 0);
                    root.InsertBatch(Bodies);
                    while (running)
                    {           
                        if (requestUpdate)
                        {
                            UpdateGalaxyOptimizedWithSkipIterations();
                        }
                    }
                });
                t.Start();
                requestUpdate = true;
            }
            if (!requestUpdate)
            {
                requestUpdate = true;
            }
        }

        public void UpdateGalaxyOptimizedWithSkipIterations()
        {
            //RECREATE LIST WITHOUT REMOVED OBJECTS
            if (toRemove > 0)
            {
                List<CelestialBody> newList = new List<CelestialBody>(N - toRemove);
                foreach (CelestialBody body in Bodies)
                {
                    if (!body.MarkedToRemove)
                    {
                        newList.Add(body);
                    }
                }
                N = newList.Count;
                Bodies = newList;
                Interlocked.Exchange(ref toRemove, 0);
            }

            if (iterations % Constants.ITERATIONS_PER_CALCULATION == 0)
            {
                if (!calculating)
                {
                    Task.Factory.StartNew(() =>
                    {
                        if (Galaxy.Instance.Iterations % 50 == 0 || toRemove > 500) //RECREATE TREE STRUCTURE
                        {
                            while (!Monitor.TryEnter(quadLock))
                            {

                            }
                            root.Reset();
                            Monitor.Exit(quadLock);
                            GC.Collect();

                        }
                        else
                        {
                            while (!Monitor.TryEnter(quadLock))
                            {

                            }
                            root.Clear();
                            Monitor.Exit(quadLock);
                        }


                        while (!Monitor.TryEnter(quadLock))
                        {

                        }
                        root.RootInsertBatchThreaded(Bodies);

                        Monitor.Exit(quadLock);
                        calculating = true;
                        Parallel.ForEach(Bodies, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 }, (body) =>
                        {
                            if (!body.MarkedToRemove)
                            {
                                root.CalculateForce(body);
                            }
                        });
                        calculating = false;
                    });
                }
                else
                {
                    Constants.ITERATIONS_PER_CALCULATION++;
                }
            }

            Parallel.ForEach(Bodies, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 }, (body) =>
            {
                if (body.MarkedToRemove)
                {
                    Interlocked.Increment(ref toRemove);//INCREMENT THE NUMBER OF NODES THAT WILL BE REMOVED
                }
                else
                {
                    body.Update();
                    if (body.OutOfBounds)
                        if (!body.MarkedToRemove)
                        {
                            body.MarkToRemove();
                            Interlocked.Increment(ref toRemove);//INCREMENT THE NUMBER OF NODES THAT WILL BE REMOVED
                        }
                }
            });
            Interlocked.Increment(ref iterations);
            requestUpdate = false;
        }

        public void Stop()
        {
            running = false;
        }

        private void GenerateOrbitalAcceleration(CelestialBody body, double m)
        {
            double ax = 0, ay = 0;
            ax = -body.posX;
            ay = -body.posY;

            double distance = Math.Sqrt(ax * ax + ay * ay);

            double temp = ax;
            ax = ay;
            ay = -temp;

            ax /= distance;
            ay /= distance;

            double strength = Math.Sqrt(Constants.G * m * Math.Sqrt(m) / distance);

            ax *= strength;
            ay *= strength;

            body.ApplyForce(ax, ay);
        }

        private void GenerateOrbitalAcceleration(CelestialBody body, CelestialBody satellite)
        {
            double ax = 0, ay = 0;
            ax = body.posX - satellite.posX;
            ay = body.posY - satellite.posY;

            double distance = Math.Sqrt(ax * ax + ay * ay);

            double temp = ax;
            ax = ay;
            ay = -temp;

            ax /= distance;
            ay /= distance;

            double strength = Math.Sqrt(Constants.G * body.mass / distance / distance);

            ax *= strength;
            ay *= strength;

            satellite.ApplyForce(ax, ay);
        }
    }
}
