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
        }
        #endregion
        #region Private methods
        /// <summary>
        /// Private constructor to be called when constructing the data structure
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="center"></param>
        private BHNode(double width, double height, double cx, double cy, double depth, BHNode parent)
        {
            this.width = width;
            this.height = height;
            this.cx = cx;
            this.cy = cy;
            this.depth = depth;
            this.parent = parent;
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
                    topLeft.Insert(body);
                }
                else //Bot
                {
                    botLeft.Insert(body);
                }
            }
            else //Right
            {
                if (body.posY < cy) //Top
                {
                    topRight.Insert(body);
                }
                else //Bot
                {
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
                    childBody = body;
                    hasChildBody = true;
                    cmx = childBody.posX;
                    cmy = childBody.posY;
                    totalMass = childBody.mass;
                    childBody.containingNode = this;
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
                        topLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy - height / 4, depth + 1, this);
                    if (topRight == null)
                        topRight = new BHNode(width / 2, height / 2, cx + width / 4, cy - height / 4, depth + 1, this);
                    if (botLeft == null)
                        botLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy + height / 4, depth + 1, this);
                    if (botRight == null)
                        botRight = new BHNode(width / 2, height / 2, cx + width / 4, cy + height / 4, depth + 1, this);
                    hasChildrenNodes = true;
                }
                //Send child body inward
                SendInward(childBody);

                hasChildBody = false;

                //Send body inward
                SendInward(body);
            }
            else //Node has no body and no children, set body
            {
                childBody = body;
                hasChildBody = true;
                childBody.containingNode = this;
            }
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
                    childBody = bodies[0];
                    cmx = childBody.posX;
                    cmy = childBody.posY;
                    totalMass = childBody.mass;
                    hasChildBody = true;
                    childBody.containingNode = this;
                }
                return;
            }

            if (bodies.Count > 1)
            {
                if (!hasChildrenNodes)
                {
                    if (topLeft == null)
                        topLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy - height / 4, depth + 1, this);
                    if (topRight == null)
                        topRight = new BHNode(width / 2, height / 2, cx + width / 4, cy - height / 4, depth + 1, this);
                    if (botLeft == null)
                        botLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy + height / 4, depth + 1, this);
                    if (botRight == null)
                        botRight = new BHNode(width / 2, height / 2, cx + width / 4, cy + height / 4, depth + 1, this);
                }
                if (hasChildBody)
                {
                    //Send the body inward
                    SendInward(childBody);
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
                            topLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy - height / 4, depth + 1, this);
                        if (topRight == null)
                            topRight = new BHNode(width / 2, height / 2, cx + width / 4, cy - height / 4, depth + 1, this);
                        if (botLeft == null)
                            botLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy + height / 4, depth + 1, this);
                        if (botRight == null)
                            botRight = new BHNode(width / 2, height / 2, cx + width / 4, cy + height / 4, depth + 1, this);
                    }
                    SendInward(childBody);
                    SendInward(bodies[0]);
                    hasChildrenNodes = true;
                }
                else //No current body, set childbody as body
                {
                    childBody = bodies[0];
                    hasChildBody = true;
                    childBody.containingNode = this;
                    cmx = childBody.posX;
                    cmy = childBody.posY;
                    totalMass = childBody.mass;
                }
            }
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
                    childBody = bodies[0];
                    cmx = childBody.posX;
                    cmy = childBody.posY;
                    totalMass = childBody.mass;
                    hasChildBody = true;
                    childBody.containingNode = this;
                }
                return;
            }

            if (bodies.Count > 1)
            {
                if (!hasChildrenNodes)
                {
                    if (topLeft == null)
                        topLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy - height / 4, depth + 1, this);
                    if (topRight == null)
                        topRight = new BHNode(width / 2, height / 2, cx + width / 4, cy - height / 4, depth + 1, this);
                    if (botLeft == null)
                        botLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy + height / 4, depth + 1, this);
                    if (botRight == null)
                        botRight = new BHNode(width / 2, height / 2, cx + width / 4, cy + height / 4, depth + 1, this);
                }
                if (hasChildBody)
                {
                    //Send the body inward
                    SendInward(childBody);
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
                            topLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy - height / 4, depth + 1, this);
                        if (topRight == null)
                            topRight = new BHNode(width / 2, height / 2, cx + width / 4, cy - height / 4, depth + 1, this);
                        if (botLeft == null)
                            botLeft = new BHNode(width / 2, height / 2, cx - width / 4, cy + height / 4, depth + 1, this);
                        if (botRight == null)
                            botRight = new BHNode(width / 2, height / 2, cx + width / 4, cy + height / 4, depth + 1, this);
                    }
                    SendInward(childBody);
                    SendInward(bodies[0]);
                    hasChildrenNodes = true;
                }
                else //No current body, set childbody as body
                {
                    childBody = bodies[0];
                    hasChildBody = true;
                    childBody.containingNode = this;
                    cmx = childBody.posX;
                    cmy = childBody.posY;
                    totalMass = childBody.mass;
                }
            }
        }

        public void DrawOutline(SpriteBatch spritebatch, Texture2D pixeltext, int screenwidth, int screenheight, RectangleD screenBounds)
        {
            Rectangle boundrect = new Rectangle((int)(cx - width / 2), (int)(cy - height / 2), (int)(width), (int)(height));
            if (!screenBounds.Intersects(boundrect))
            {
                return;
            }
            spritebatch.Draw(pixeltext, new Microsoft.Xna.Framework.Rectangle(boundrect.Left, boundrect.Top, boundrect.Width, (int)(1 / Camera.Instance.Zoom)), Microsoft.Xna.Framework.Color.Green);
            spritebatch.Draw(pixeltext, new Microsoft.Xna.Framework.Rectangle(boundrect.Left, boundrect.Bottom, boundrect.Width, (int)(1 / Camera.Instance.Zoom)), Microsoft.Xna.Framework.Color.Green);
            spritebatch.Draw(pixeltext, new Microsoft.Xna.Framework.Rectangle(boundrect.Left, boundrect.Top, (int)(1 / Camera.Instance.Zoom), boundrect.Height), Microsoft.Xna.Framework.Color.Green);
            spritebatch.Draw(pixeltext, new Microsoft.Xna.Framework.Rectangle(boundrect.Right, boundrect.Top, (int)(1 / Camera.Instance.Zoom), boundrect.Height), Microsoft.Xna.Framework.Color.Green);

            lock (treelock)
            {
                if (hasChildrenNodes)
                {
                    topLeft.DrawOutline(spritebatch, pixeltext, screenwidth, screenheight, screenBounds);
                    topRight.DrawOutline(spritebatch, pixeltext, screenwidth, screenheight, screenBounds);
                    botLeft.DrawOutline(spritebatch, pixeltext, screenwidth, screenheight, screenBounds);
                    botRight.DrawOutline(spritebatch, pixeltext, screenwidth, screenheight, screenBounds);
                }
            }

            spritebatch.Draw(pixeltext, new Microsoft.Xna.Framework.Rectangle((int)cmx - 1, (int)cmy - 1, 2, 2), Helper.GetColorByDepth(this.depth));
        }

        public void Reset()
        {
            if (hasChildrenNodes)
            {
                topLeft.Clear();
                topRight.Clear();
                botLeft.Clear();
                botRight.Clear();
                hasChildrenNodes = false;
                lock(treelock){
                    topLeft = topRight = botLeft = botRight = null;
                }
            }
            hasChildBody = false;
            cmx = 0;
            cmy = 0;
            totalMass = 0;
        }

        public void Clear()
        {
            if (hasChildrenNodes)
            {
                topLeft.Clear();
                topRight.Clear();
                botLeft.Clear();
                botRight.Clear();
                hasChildrenNodes = false;
            }
            hasChildBody = false;
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
                if (childBody.id != body.id)
                {
                    if (!body.Collides(childBody))
                    {
                        double fx = 0, fy = 0;
                        childBody.Attract(body, out fx, out fy);
                        body.ApplyForce(fx * Constants.ITERATIONS_PER_CALCULATION, fy * Constants.ITERATIONS_PER_CALCULATION);
                    }
                    else
                    {
                        if (body.Collides(childBody))
                        {
                            body.ResolveCollisionWithAbsorption(childBody);
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
                        topLeft.CalculateForce(body);
                        topRight.CalculateForce(body);
                        botLeft.CalculateForce(body);
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
                FindContainingNode(childBody);
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
        #endregion
    }
}
