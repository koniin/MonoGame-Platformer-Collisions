using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionDetectionPlatformer
{
    public class CollisionHandler2
    {
        public Vector2 CheckCollision(Rectangle bounding, TileMap tileMap)
        {
            Vector2 movement = new Vector2(bounding.X, bounding.Y);

            Rectangle box = new Rectangle((int)movement.X, (int)movement.Y, 32, 32);
            Vector2 topLeftPoint = new Vector2(box.X, box.Y);
            Vector2 topRightPoint = new Vector2(box.X + box.Width, box.Y);
            Vector2 bottomLeftPoint = new Vector2(box.X, box.Y + box.Height);
            Vector2 bottomRightPoint = new Vector2(box.X + box.Width, box.Y + box.Height);

            var tile = tileMap.PositionToTile(topLeftPoint);
            var tileRight = tileMap.PositionToTile(topRightPoint);
            var tileBottomLeft = tileMap.PositionToTile(bottomLeftPoint);
            var tileBottomRight = tileMap.PositionToTile(bottomRightPoint);

            return movement;
        }

    }
}
