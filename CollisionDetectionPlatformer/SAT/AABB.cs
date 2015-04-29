using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionDetectionPlatformer.SAT
{
        public class AABB
        {
            private List<Vector> edges = new List<Vector>();
            private List<Vector> points = new List<Vector>();

            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

            public AABB(int x, int y, int width, int height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;

                points.Add(new Vector(X, Y));
                points.Add(new Vector(X + width, Y));
                points.Add(new Vector(X + width, Y + height));
                points.Add(new Vector(X, Y + height));

                BuildEdges();
            }

            public void BuildEdges()
            {
                Vector p1;
                Vector p2;
                edges.Clear();
                for (int i = 0; i < Points.Count; i++)
                {
                    p1 = Points[i];
                    if (i + 1 >= Points.Count)
                    {
                        p2 = Points[0];
                    }
                    else
                    {
                        p2 = Points[i + 1];
                    }
                    edges.Add(p2 - p1);
                }
            }

            public Vector Center
            {
                get
                {
                    float totalX = 0;
                    float totalY = 0;
                    for (int i = 0; i < Points.Count; i++)
                    {
                        totalX += Points[i].X;
                        totalY += Points[i].Y;
                    }

                    return new Vector(totalX / (float)Points.Count, totalY / (float)Points.Count);
                }
            }

            public void Offset(Vector v)
            {
                Offset(v.X, v.Y);
            }

            public void Offset(float x, float y)
            {
                X += (int)x;
                Y += (int)y;
                for (int i = 0; i < points.Count; i++)
                {
                    Vector p = points[i];
                    points[i] = new Vector(p.X + x, p.Y + y);
                }
            }

            public List<Vector> Points
            {
                get { return points; }
            }

            public List<Vector> Edges
            {
                get { return edges; }
            }
        
    }
}
