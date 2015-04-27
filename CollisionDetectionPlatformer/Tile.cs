using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionDetectionPlatformer
{
    public class Tile
    {
        public bool IsSolid { get; set; }
        public string Name { get; set; }
        public bool Intersected { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X * 32, (int)Position.Y * 32, 32, 32);
            }
        }
        public Tile(string name)
        {
            Name = name;
        }
    }
}
