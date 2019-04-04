using Cosmos.Engine;
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
        private volatile bool updating = false;
        volatile private List<CelestialBody> Bodies;
        public List<Star> Stars;
        public List<Planet> Planets;
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
            Planets = new List<Planet>();
            Stars = new List<Star>();
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
                Stars.Add(star);
                totalMass += star.mass;
                double bodies = Rand.Next(minBodsPerSys, maxBodsPerSys);
                for(int j = 0; j < bodies; j++)
                {
                    double randomValue2 = Rand.Next(0, 100000000);
                    randomValue2 /= 100000000;
                    double theta2 = Rand.NextDouble() * (Math.PI * 2);
                    double posX2, posY2;
                    double radius2 = Rand.Next((int)(star.size * Constants.MIN_DISTANCE_RATIO), (int)(star.size * Constants.MAX_DISTANCE_RATIO));
                    posX2 = radius2 * Math.Cos(theta2);
                    posY2 = radius2 * Math.Sin(theta2);
                    posX2 += posX;
                    posY2 += posY;
                    Planet planet = Planet.GeneratePlanet(systems + (i * j), randomValue2, posX2, posY2, star, theta2);
                    Planets.Add(planet);
                    star.OrbitingPlanets.Add(planet);
                    totalMass += planet.mass;
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

        public CelestialBody GetBody(int index)
        {
            return Bodies[index];
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
                        if (requestUpdate && !updating)
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
            updating = true;
            //RECREATE LIST WITHOUT REMOVED OBJECTS
            if (toRemove > 0)
            {
                List<CelestialBody> newList = new List<CelestialBody>(N - toRemove);
                List<Planet> newPlanetList = new List<Planet>();
                List<Star> newStarList = new List<Star>();
                foreach (Star body in Stars)
                {
                    if (!body.MarkedToRemove)
                    {
                        newList.Add(body);
                        newStarList.Add(body as Star);
                    }
                }
                foreach (Planet body in Planets)
                {
                    if (!body.MarkedToRemove)
                    {
                        newPlanetList.Add(body as Planet);
                    }
                }
                N = newList.Count;
                Bodies = newList;
                Planets = newPlanetList;
                Stars = newStarList;
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
                    if(body == Camera.Instance.Following)
                    {
                        Camera.Instance.Follow(body);
                    }
                    if (body is Star)
                    {
                        foreach (Planet planet in (body as Star).OrbitingPlanets)
                        {
                            planet.Update();
                        }
                    }
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
            updating = false;
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
