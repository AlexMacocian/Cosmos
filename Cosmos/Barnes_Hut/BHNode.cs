using Cosmos.Engine;
using Cosmos.Structures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Cosmos.Structures.Galaxy;

namespace Cosmos.Barnes_Hut
{
    public class BHNode
    {
        #region Private fields
        List<CelestialBody> topLeftBods = new List<CelestialBody>();
        List<CelestialBody> topRightBods = new List<CelestialBody>();
        List<CelestialBody> botLeftBods = new List<CelestialBody>();
        List<CelestialBody> botRightBods = new List<CelestialBody>();

        private BHNode topLeft, topRight, botLeft, botRight;
        private BHNode parent = null;
        private bool hasChildrenNodes = false, hasChildBody = false;
        private CelestialBody childBody;
        private double cx, cy; //centre of node
        private double cmx, cmy; //centre of mass 
        private double width, height, depth = 0, totalMass;
        private enum Location
        {
            TopLeft,
            TopRight,
            BotLeft,
            BotRight,
            Root
        }
        private Location nodeLocation = Location.Root;
        #endregion
        #region Public fields
        public object treelock = new object();
        #endregion
        #region Public properties
        public double CenterX
        {
            get
            {
                return cx;
            }
        }
        public double CenterY
        {
            get
            {
                return cy;
            }
        }
        public double Width
        {
            get
            {
                return width;
            }
        }
        public double Height
        {
            get
            {
                return height;
            }
        }
        public BHNode TopLeft
        {
            get
            {
                return topLeft;
            }
        }
        public BHNode TopRight
        {
            get
            {
                return topRight;
            }
        }
        public BHNode BotLeft
        {
            get
            {
                return botLeft;
            }
        }
        public BHNode BotRight
        {
            get
            {
                return botRight;
            }
        }
        public bool HasChildrenNodes
        {
            get
            {
                return hasChildrenNodes;
            }
        }
        public bool HasChildBody
        {
            get
            {
                return hasChildBody;
            }
        }
        public CelestialBody ChildBody
        {
            get
            {
                return childBody;
            }
            set
            {
                childBody = value;
                if(childBody != null)
                {
                    childBody.containingNode = this;
                }
            }
        }
        public BHNode Parent
        {
            get
            {
                return parent;
            }
        }

        private Location NodeLocation { get => nodeLocation; set => nodeLocation = value; }
        #endregion
        #region Private methods
        /// <summary>
        /// Private constructor to be called when constructing the data structure
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="center"></param>
        private BHNode(double width, double height, double cx, double cy, double depth, BHNode parent, Location location)
        {
            this.width = width;
            this.height = height;
            this.cx = cx;
            this.cy = cy;
            this.depth = depth;
            this.parent = parent;
            this.nodeLocation = location;
        }
        /// <summary>
        /// Method determines to which quadrant to put the body
        /// </summary>
        /// <param name="body"></param>
        private void SendInward(CelestialBody body)
        {
            if (body.posX < cx) //Left
            {
                if (body.posY < cy) //Top
                {
                    if (topLeft == null)
                        topLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy - height / 4, depth + 1, this, Location.TopLeft);
                    topLeft.Insert(body);
                }
                else //Bot
                {
                    if (botLeft == null)
                        botLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy + height / 4, depth + 1, this, Location.BotLeft);
                    botLeft.Insert(body);
                }
            }
            else //Right
            {
                if (body.posY < cy) //Top
                {
                    if (topRight == null)
                        topRight = new BHNode(width / 2, height / 2, cx + width / 4, cy - height / 4, depth + 1, this, Location.TopRight);
                    topRight.Insert(body);
                }
                else //Bot
                {
                    if (botRight == null)
                        botRight = new BHNode(width / 2, height / 2, cx + width / 4, cy + height / 4, depth + 1, this, Location.BotRight);
                    botRight.Insert(body);
                }
            }
        }
        private void FindContainingNode(CelestialBody body)
        {
            //REMOVE BODY FROM WEIGHT CENTER CALCULATION
            double tx = body.posX;
            double ty = body.posY;

            tx *= body.mass;
            ty *= body.mass;

            cmx *= totalMass;
            cmy *= totalMass;

            cmx -= tx;
            cmy -= ty;

            totalMass -= body.mass;

            cmx /= totalMass;
            cmy /= totalMass;

            if (NodeFits(body))
            {
                Insert(body);
            }
            else
            {
                if (parent != null)
                {
                    parent.FindContainingNode(body);
                    if(!hasChildBody && !hasChildrenNodes)
                    {
                        switch (nodeLocation)
                        {
                            case Location.BotLeft:
                                this.parent.botLeft = null;
                                break;
                            case Location.BotRight:
                                this.parent.botRight = null;
                                break;
                            case Location.TopLeft:
                                this.parent.topLeft = null;
                                break;
                            case Location.TopRight:
                                this.parent.topRight = null;
                                break;
                        }
                        if( this.parent.botLeft == null && 
                            this.parent.botRight ==  null &&
                            this.parent.topLeft == null &&
                            this.parent.topRight == null
                            )
                        {
                            this.parent.hasChildrenNodes = false;
                        }
                    }
                }
            }
        }
        #endregion
        #region Public methods
        /// <summary>
        /// Constructor to be called only for root
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public BHNode(double width, double height)
        {
            cx = 0; cy = 0;
            this.height = height;
            this.width = width;
        }

        /// <summary>
        /// Insert one body into the tree
        /// </summary>
        /// <param name="body"></param>
        public void Insert(CelestialBody body)
        {
            if (!NodeFits(body))
            {
                return;
            }
            if(depth > Constants.BH_MAX_DEPTH)
            {
                if(!hasChildBody)
                {
                    ChildBody = body;
                    hasChildBody = true;
                    cmx = ChildBody.posX;
                    cmy = ChildBody.posY;
                    totalMass = ChildBody.mass;
                }
                return;
            }

            if (hasChildrenNodes) //Node is subdivided, find child node to contain body
            {
                SendInward(body);
            }
            else if(hasChildBody)//Node doesn't have children nodes so we must subdivide
            {
                //Generate child nodes
                lock (treelock)
                {
                    if (topLeft == null)
                        topLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy - height / 4, depth + 1, this, Location.TopLeft);
                    if (topRight == null)
                        topRight = new BHNode(width / 2, height / 2, cx + width / 4, cy - height / 4, depth + 1, this, Location.TopRight);
                    if (botLeft == null)
                        botLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy + height / 4, depth + 1, this, Location.BotLeft);
                    if (botRight == null)
                        botRight = new BHNode(width / 2, height / 2, cx + width / 4, cy + height / 4, depth + 1, this, Location.BotRight);
                    hasChildrenNodes = true;
                }
                //Send child body inward
                SendInward(ChildBody);

                hasChildBody = false;

                //Send body inward
                SendInward(body);
            }
            else //Node has no body and no children, set body
            {
                ChildBody = body;
                hasChildBody = true;
            }

            CleanUp();

            double tx = body.posX;
            double ty = body.posY;

            tx *= body.mass;
            ty *= body.mass;

            cmx *= totalMass;
            cmy *= totalMass;

            cmx += tx;
            cmy += ty;

            totalMass += body.mass;

            cmx /= totalMass;
            cmy /= totalMass;
        }

        /// <summary>
        /// Inserts multiple bodies at the same time to improve insertion performance
        /// </summary>
        /// <param name="bodies"></param>
        public void InsertBatch(List<CelestialBody> bodies)
        {
            if (depth >= Constants.BH_MAX_DEPTH) //Reached max depth
            {
                if (!hasChildBody && bodies.Count > 0)
                {
                    ChildBody = bodies[0];
                    cmx = ChildBody.posX;
                    cmy = ChildBody.posY;
                    totalMass = ChildBody.mass;
                    hasChildBody = true;
                }
                return;
            }

            if (bodies.Count > 1)
            {
                if (!hasChildrenNodes)
                {
                    if (topLeft == null)
                        topLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy - height / 4, depth + 1, this, Location.TopLeft);
                    if (topRight == null)
                        topRight = new BHNode(width / 2, height / 2, cx + width / 4, cy - height / 4, depth + 1, this, Location.TopRight);
                    if (botLeft == null)
                        botLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy + height / 4, depth + 1, this, Location.BotLeft);
                    if (botRight == null)
                        botRight = new BHNode(width / 2, height / 2, cx + width / 4, cy + height / 4, depth + 1, this, Location.BotRight);
                }
                if (hasChildBody)
                {
                    //Send the body inward
                    SendInward(ChildBody);
                    hasChildBody = false;
                }

                topLeftBods.Clear();
                botLeftBods.Clear();
                topRightBods.Clear();
                botRightBods.Clear();

                if (bodies.Count > 0)
                {
                    cmx *= totalMass;
                    cmy *= totalMass;
                }

                foreach (CelestialBody body in bodies) // Split bodies into lists
                {
                    double dx = cx - body.posX;
                    double dy = cy - body.posY;
                    if (!(Math.Abs(dx) - (width / 2) < 0) ||
                        !(Math.Abs(dy) - (height / 2) < 0)) //Body is not in boundary
                    {
                        continue;
                    }

                    if (body.posX < cx) //Left
                    {
                        if (body.posY < cy) //Top
                        {
                            topLeftBods.Add(body);
                        }
                        else //Bot
                        {
                            botLeftBods.Add(body);
                        }
                    }
                    else //Right
                    {
                        if (body.posY < cy) //Top
                        {
                            topRightBods.Add(body);
                        }
                        else //Bot
                        {
                            botRightBods.Add(body);
                        }
                    }
                    double tx = body.posX;
                    double ty = body.posY;
                    tx *= body.mass;
                    ty *= body.mass;

                    cmx += tx;
                    cmy += ty;

                    totalMass += body.mass;
                }

                cmx /= totalMass;
                cmy /= totalMass;

                //Insert recursively into quadrants
                topLeft.InsertBatch(topLeftBods);
                topRight.InsertBatch(topRightBods);
                botLeft.InsertBatch(botLeftBods);
                botRight.InsertBatch(botRightBods);
                hasChildrenNodes = true;
            }
            else if (bodies.Count == 1) //Just one body to add
            {
                if (hasChildBody) //If already have a body
                {
                    if (!hasChildrenNodes)
                    {
                        if (topLeft == null)
                            topLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy - height / 4, depth + 1, this, Location.TopLeft);
                        if (topRight == null)
                            topRight = new BHNode(width / 2, height / 2, cx + width / 4, cy - height / 4, depth + 1, this, Location.TopRight);
                        if (botLeft == null)
                            botLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy + height / 4, depth + 1, this, Location.BotLeft);
                        if (botRight == null)
                            botRight = new BHNode(width / 2, height / 2, cx + width / 4, cy + height / 4, depth + 1, this, Location.BotRight);
                    }
                    SendInward(ChildBody);
                    SendInward(bodies[0]);
                    hasChildrenNodes = true;
                }
                else //No current body, set childbody as body
                {
                    ChildBody = bodies[0];
                    hasChildBody = true;
                    cmx = ChildBody.posX;
                    cmy = ChildBody.posY;
                    totalMass = ChildBody.mass;
                }
            }
            CleanUp();
        }

        /// <summary>
        /// An implementation of InsertBatch optimized for root to parallelize the construction of the tree using 4 threads
        /// </summary>
        /// <param name="bodies"></param>
        public void RootInsertBatchThreaded(List<CelestialBody> bodies)
        {
            if (depth >= Constants.BH_MAX_DEPTH) //Reached max depth
            {
                if (!hasChildBody && bodies.Count > 0)
                {
                    ChildBody = bodies[0];
                    cmx = ChildBody.posX;
                    cmy = ChildBody.posY;
                    totalMass = ChildBody.mass;
                    hasChildBody = true;
                }
                return;
            }

            if (bodies.Count > 1)
            {
                if (!hasChildrenNodes)
                {
                    if (topLeft == null)
                        topLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy - height / 4, depth + 1, this, Location.TopLeft);
                    if (topRight == null)
                        topRight = new BHNode(width / 2, height / 2, cx + width / 4, cy - height / 4, depth + 1, this, Location.TopRight);
                    if (botLeft == null)
                        botLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy + height / 4, depth + 1, this, Location.BotLeft);
                    if (botRight == null)
                        botRight = new BHNode(width / 2, height / 2, cx + width / 4, cy + height / 4, depth + 1, this, Location.BotRight);
                }
                if (hasChildBody)
                {
                    //Send the body inward
                    SendInward(ChildBody);
                    hasChildBody = false;
                }

                topLeftBods.Clear();
                botLeftBods.Clear();
                topRightBods.Clear();
                botRightBods.Clear();

                if (bodies.Count > 0)
                {
                    cmx *= totalMass;
                    cmy *= totalMass;
                }

                foreach (CelestialBody body in bodies) // Split bodies into lists
                {
                    double dx = cx - body.posX;
                    double dy = cy - body.posY;
                    if (!(Math.Abs(dx) - (width / 2) < 0) ||
                        !(Math.Abs(dy) - (height / 2) < 0)) //Body is not in boundary
                    {
                        continue;
                    }

                    if (body.posX < cx) //Left
                    {
                        if (body.posY < cy) //Top
                        {
                            topLeftBods.Add(body);
                        }
                        else //Bot
                        {
                            botLeftBods.Add(body);
                        }
                    }
                    else //Right
                    {
                        if (body.posY < cy) //Top
                        {
                            topRightBods.Add(body);
                        }
                        else //Bot
                        {
                            botRightBods.Add(body);
                        }
                    }
                    double tx = body.posX;
                    double ty = body.posY;
                    tx *= body.mass;
                    ty *= body.mass;

                    cmx += tx;
                    cmy += ty;

                    totalMass += body.mass;
                }

                cmx /= totalMass;
                cmy /= totalMass;

                //Insert recursively into quadrants
                Parallel.Invoke(new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 }, () =>
                {
                    topLeft.InsertBatch(topLeftBods);
                }, () =>
                {
                    topRight.InsertBatch(topRightBods);
                }, () =>
                {
                    botLeft.InsertBatch(botLeftBods);
                }, () =>
                {
                    botRight.InsertBatch(botRightBods);
                });                
                hasChildrenNodes = true;
            }
            else if (bodies.Count == 1) //Just one body to add
            {
                if (hasChildBody) //If already have a body
                {
                    if (!hasChildrenNodes)
                    {
                        if (topLeft == null)
                            topLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy - height / 4, depth + 1, this, Location.TopLeft);
                        if (topRight == null)
                            topRight = new BHNode(width / 2, height / 2, cx + width / 4, cy - height / 4, depth + 1, this, Location.TopRight);
                        if (botLeft == null)
                            botLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy + height / 4, depth + 1, this, Location.BotLeft);
                        if (botRight == null)
                            botRight = new BHNode(width / 2, height / 2, cx + width / 4, cy + height / 4, depth + 1, this, Location.BotRight);
                    }
                    SendInward(ChildBody);
                    SendInward(bodies[0]);
                    hasChildrenNodes = true;
                }
                else //No current body, set childbody as body
                {
                    ChildBody = bodies[0];
                    hasChildBody = true;
                    cmx = ChildBody.posX;
                    cmy = ChildBody.posY;
                    totalMass = ChildBody.mass;
                }
            }
            CleanUp();
        }

        public void DrawOutline(SpriteBatch spritebatch, Texture2D pixeltext, int screenwidth, int screenheight, RectangleD screenBounds, Vector3D translate)
        {
            RectangleD boundrect = new RectangleD((cx - width / 2), (cy - height / 2), (width), (height));
            if (!screenBounds.Intersects(boundrect))
            {
                return;
            }
            {
                double size = boundrect.Width * Camera.Instance.Zoom;
                double posX = (boundrect.X * Camera.Instance.Zoom) + translate.X;
                double posY = (boundrect.Y * Camera.Instance.Zoom) + translate.Y;
                spritebatch.Draw(pixeltext, new Microsoft.Xna.Framework.Rectangle((int)posX, (int)posY, (int)size, 1), Microsoft.Xna.Framework.Color.Green);
            }
            {
                double size = boundrect.Width * Camera.Instance.Zoom;
                double posX = (boundrect.X * Camera.Instance.Zoom) + translate.X;
                double posY = ((boundrect.Y + boundrect.Height) * Camera.Instance.Zoom) + translate.Y;
                spritebatch.Draw(pixeltext, new Microsoft.Xna.Framework.Rectangle((int)posX, (int)posY, (int)size, 1), Microsoft.Xna.Framework.Color.Green);
            }
            {
                double size = boundrect.Height * Camera.Instance.Zoom;
                double posX = (boundrect.X * Camera.Instance.Zoom) + translate.X;
                double posY = (boundrect.Y * Camera.Instance.Zoom) + translate.Y;
                spritebatch.Draw(pixeltext, new Microsoft.Xna.Framework.Rectangle((int)posX, (int)posY, 1, (int)size), Microsoft.Xna.Framework.Color.Green);
            }
            {
                double size = boundrect.Height * Camera.Instance.Zoom;
                double posX = ((boundrect.X + boundrect.Width) * Camera.Instance.Zoom) + translate.X;
                double posY = (boundrect.Y * Camera.Instance.Zoom) + translate.Y;
                spritebatch.Draw(pixeltext, new Microsoft.Xna.Framework.Rectangle((int)posX, (int)posY, 1, (int)size), Microsoft.Xna.Framework.Color.Green);
            }
            lock (treelock)
            {
                if (hasChildrenNodes)
                {
                    if(topLeft != null)
                        topLeft.DrawOutline(spritebatch, pixeltext, screenwidth, screenheight, screenBounds, translate);
                    if(topRight != null)
                        topRight.DrawOutline(spritebatch, pixeltext, screenwidth, screenheight, screenBounds, translate);
                    if(botLeft != null)
                        botLeft.DrawOutline(spritebatch, pixeltext, screenwidth, screenheight, screenBounds, translate);
                    if(botRight != null)
                        botRight.DrawOutline(spritebatch, pixeltext, screenwidth, screenheight, screenBounds, translate);
                }
            }

            spritebatch.Draw(pixeltext, new Microsoft.Xna.Framework.Rectangle((int)cmx - 1, (int)cmy - 1, 2, 2), Helper.GetColorByDepth(this.depth));
        }

        public void Reset()
        {
            if (hasChildrenNodes)
            {
                if(topLeft != null)
                    topLeft.Reset();
                if(topRight != null)
                    topRight.Reset();
                if(botLeft != null)
                    botLeft.Reset();
                if(botRight != null)
                    botRight.Reset();
                hasChildrenNodes = false;
                lock(treelock){
                    topLeft = topRight = botLeft = botRight = null;
                }
            }
            hasChildBody = false;
            childBody = null;
            cmx = 0;
            cmy = 0;
            totalMass = 0;
        }

        public void Clear()
        {
            if (hasChildrenNodes)
            {
                if(topRight != null)
                    topLeft.Clear();
                if(topRight != null)
                    topRight.Clear();
                if(botLeft != null)
                    botLeft.Clear();
                if(botRight != null)
                    botRight.Clear();
                hasChildrenNodes = false;
            }
            hasChildBody = false;
            childBody = null;
            cmx = 0;
            cmy = 0;
            totalMass = 0;
        }

        public void CalculateForce(CelestialBody body)
        {
            if (body.MarkedToRemove)
            {
                return;
            }
            if(hasChildBody)
            {
                if (ChildBody.id != body.id)
                {
                    if (!body.Collides(ChildBody))
                    {
                        double fx = 0, fy = 0;
                        ChildBody.Attract(body, out fx, out fy);
                        body.ApplyForce(fx * Constants.ITERATIONS_PER_CALCULATION, fy * Constants.ITERATIONS_PER_CALCULATION);
                    }
                    else
                    {
                        if (body.Collides(ChildBody))
                        {
                            body.ResolveCollisionWithAbsorption(ChildBody);
                        }
                    }
                }
            }
            else
            {
                if (hasChildrenNodes)
                {
                    double dx, dy;
                    dx = cx - body.posX;
                    dy = cy - body.posY;
                    double r = dx * dx + dy * dy;
                    if (width * width / r < Constants.Theta * Constants.Theta)
                    {
                        //Calculate attraction
                        double distance = dx * dx + dy * dy;
                        double strength = (Constants.G * body.mass * totalMass) / distance;
                        double length = Math.Sqrt(distance);
                        dx /= length;
                        dy /= length;
                        dx *= strength;
                        dy *= strength;
                        body.ApplyForce(dx * Constants.ITERATIONS_PER_CALCULATION, dy * Constants.ITERATIONS_PER_CALCULATION);
                    }
                    else
                    {
                        if(topLeft != null)
                            topLeft.CalculateForce(body);
                        if(topRight != null)
                            topRight.CalculateForce(body);
                        if(botLeft != null)
                            botLeft.CalculateForce(body);
                        if(botRight != null)
                            botRight.CalculateForce(body);
                    }
                }
            }
        }

        public void SetDimensions(double cx, double cy, int width, int height)
        {
            this.cx = cx;
            this.cy = cy;
            this.width = width;
            this.height = height;
        }

        public void ReinsertChild()
        {
            if (hasChildBody)
            {
                hasChildBody = false;
                FindContainingNode(ChildBody);
            }
        }

        public bool NodeFits(CelestialBody body)
        {
            double dx = cx - body.posX;
            double dy = cy - body.posY;
            if (!(Math.Abs(dx) - (width / 2) < 0) ||
                !(Math.Abs(dy) - (height / 2) < 0)) //Body is not in boundary
            {
                return false;
            }
            return true;
        }

        private void CleanUp()
        {
            //Clean up after insertion
            if (topLeft != null && !topLeft.HasChildBody && !topLeft.HasChildrenNodes)
            {
                topLeft = null;
            }
            if (topRight != null && !topRight.HasChildBody && !topRight.HasChildrenNodes)
            {
                topRight = null;
            }
            if (botLeft != null && !botLeft.HasChildBody && !botLeft.HasChildrenNodes)
            {
                botLeft = null;
            }
            if (botRight != null && !botRight.HasChildBody && !botRight.HasChildrenNodes)
            {
                botRight = null;
            }
        }
        #endregion
    }
}
