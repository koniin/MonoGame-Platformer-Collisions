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
        public Tile(string name)
        {
            Name = name;
        }
    }
}
