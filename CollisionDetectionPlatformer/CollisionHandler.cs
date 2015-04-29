using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionDetectionPlatformer
{
    public class CollisionHandler
    {
        public Vector2 CheckCollision(Rectangle box, TileMap tileMap)
        {
            // foreach tile around - check if we are colliding
            //Vector2 topRightPoint = new Vector2(box.X + box.Width, box.Y);
            //Vector2 bottomRightPoint = new Vector2(box.X + box.Width, box.Y + box.Height);
            //Vector2 topLeftPoint = new Vector2(box.X, box.Y);
            //Vector2 bottomLeftPoint = new Vector2(box.X, box.Y + box.Height);


            Rectangle bounds = box;
            int leftTile = (int)Math.Floor((float)bounds.Left / 32);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / 32)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / 32);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / 32)) - 1;

            bool hasCollision = false;
            Player.Playa.IsOnGround = false;
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    var tile = tileMap.GetTile(new Vector2(x, y));
                    if (tile.IsSolid)
                    {
                        Rectangle tileBounds = new Rectangle((int)tile.Position.X * 32, (int)tile.Position.Y * 32, 32, 32);
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            hasCollision = true;
                            string a = tile.Name;
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            // Resolve the collision along the shallow axis.
                            if (absDepthY < absDepthX) // || collision == TileCollision.Platform)
                            {
                                //      Y collision


                                // If we crossed the top of a tile, we are on the ground.

                                // Needs refactoring
                                if (Player.Playa.PreviousBottom <= tileBounds.Top)
                                    Player.Playa.IsOnGround = true;
                                
                                // Ignore platforms, unless we are on the ground.
                                /*
                                if (collision == TileCollision.Impassable || IsOnGround)
                                {
                                    // Resolve the collision along the Y axis.
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);

                                    // Perform further collisions with the new bounds.
                                    bounds = BoundingRectangle;
                                }*/
                                bounds = new Rectangle(box.X, box.Y + (int)Math.Ceiling(depth.Y), 32, 32);
                            }
                            else // if (collision == TileCollision.Impassable) // Ignore platforms.
                            {
                                //return new Vector2(box.X + depth.X, box.Y);
                                // Resolve the collision along the X axis.
                                // Position = new Vector2(Position.X + depth.X, Position.Y);
                                Player.Playa.Velocity.X = 0;
                                // Perform further collisions with the new bounds.
                                // bounds = BoundingRectangle;
                                bounds = new Rectangle(box.X + (int)Math.Ceiling(depth.X), box.Y, 32, 32);
                            }
                        }
                    }
                }
            }

            // Needs refactoring
            Player.Playa.PreviousBottom = bounds.Bottom;

            if (hasCollision)
                return new Vector2(bounds.X, bounds.Y);
            return Vector2.Zero;
        }
    }
}
