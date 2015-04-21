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
                                bounds = new Rectangle(box.X, box.Y + (int)Math.Round(depth.Y), 32, 32);
                            }
                            else // if (collision == TileCollision.Impassable) // Ignore platforms.
                            {
                                //return new Vector2(box.X + depth.X, box.Y);
                                // Resolve the collision along the X axis.
                                // Position = new Vector2(Position.X + depth.X, Position.Y);

                                // Perform further collisions with the new bounds.
                                // bounds = BoundingRectangle;
                                bounds = new Rectangle(box.X + (int)Math.Round(depth.X), box.Y, 32, 32);
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

            //if (tileTopRight.IsSolid)
            //{
            //    Rectangle tileBounds = new Rectangle((int)tileBottomRight.Position.Y * 32, (int)tileBottomRight.Position.X * 32, 32, 32);
            //    Vector2 depth = RectangleExtensions.GetIntersectionDepth(box, tileBounds);
            //    if (depth != Vector2.Zero)
            //    {

            //    }
            //}
            //if (tileBottomRight.IsSolid)
            //{
            //    Rectangle tileBounds = new Rectangle((int)tileBottomRight.Position.Y * 32, (int)tileBottomRight.Position.X * 32, 32, 32);
            //    Vector2 depth = RectangleExtensions.GetIntersectionDepth(box, tileBounds);
            //    if (depth != Vector2.Zero)
            //    {
            //        //float absDepthX = Math.Abs(depth.X);
            //        //float absDepthY = Math.Abs(depth.Y);
            //        //// Y collision
            //        //if (absDepthY < absDepthX)  // || collision == TileCollision.Platform)
            //        //{
            //        //    /*
            //        //    // If we crossed the top of a tile, we are on the ground.
            //        //    if (previousBottom <= tileBounds.Top)
            //        //        isOnGround = true;

            //        //    // Ignore platforms, unless we are on the ground.
            //        //    if (collision == TileCollision.Impassable || IsOnGround)
            //        //    {
            //        //        // Resolve the collision along the Y axis.
            //        //        Position = new Vector2(Position.X, Position.Y + depth.Y);

            //        //        // Perform further collisions with the new bounds.
            //        //        bounds = BoundingRectangle;
            //        //    }
            //        //     * */
            //        //}
            //        //else
            //        //{
            //            // Resolve the collision along the X axis.
            //            return new Vector2(box.X + depth.X, box.Y);

            //            // Perform further collisions with the new bounds.
            //            //bounds = BoundingRectangle;
            //        //}
            //    }
            //}
            //if (tileBottomLeft.IsSolid)
            //{
            //    Rectangle tileBounds = new Rectangle((int)tileBottomLeft.Position.Y * 32, (int)tileBottomLeft.Position.X * 32, 32, 32);
            //    Vector2 depth = RectangleExtensions.GetIntersectionDepth(box, tileBounds);
            //    if (depth != Vector2.Zero)
            //    {
            //        //float absDepthX = Math.Abs(depth.X);
            //        //float absDepthY = Math.Abs(depth.Y);

            //        //// Y collision
            //        //if (absDepthY < absDepthX)  // || collision == TileCollision.Platform)
            //        //{
            //        //    /*
            //        //    // If we crossed the top of a tile, we are on the ground.
            //        //    if (previousBottom <= tileBounds.Top)
            //        //        isOnGround = true;

            //        //    // Ignore platforms, unless we are on the ground.
            //        //    if (collision == TileCollision.Impassable || IsOnGround)
            //        //    {
            //        //        // Resolve the collision along the Y axis.
            //        //        Position = new Vector2(Position.X, Position.Y + depth.Y);

            //        //        // Perform further collisions with the new bounds.
            //        //        bounds = BoundingRectangle;
            //        //    }
            //        //     * */
            //        //}
            //        //else
            //        //{
            //            // Resolve the collision along the X axis.
            //            return new Vector2(box.X + depth.X, box.Y);

            //            // Perform further collisions with the new bounds.
            //            //bounds = BoundingRectangle;
            //        //}
            //    }
            //}
            // return Vector2.Zero;

            // if we are 
            // get distance to move back
        }
        
        /// <summary>
        /// Calculate the amount to move to get close to the next solids.
        /// Assumes top left coordinates of objects.
        /// </summary>
        /// <param name="box">is the moving object</param>
        /// <param name="tileMap">is the map containing solids</param>
        /// <param name="moveAmount">is the amount we want to move</param>
        /// <param name="direction">is the direction we are moving (e.g Vector(1,0) for right and no Y movement</param>
        /// <returns>the amount to move</returns>
        public Vector2 TryMovePlayer(Rectangle box, TileMap tileMap, Vector2 moveAmount, Vector2 direction)
        {
            /// First do X collisions (so we can handle slopes?)

            // First find tiles in X where and if we are moving to
            if (direction.X != 0)
            {
                // For larger objects we need more points (same size or smaller sprite yields max 2 tiles)
                // Get the coordinate of the forward-facing edge, e.g. : If walking right, 
                // the x coordinate of right of bounding box. 
                float boxEdge = direction.X > 0 ? box.X + box.Width : box.X;
                Vector2 topPoint = new Vector2(boxEdge, box.Y);
                Vector2 bottomPoint = new Vector2(boxEdge, box.Y + box.Height);

                /*
                    Figure which lines of tiles the bounding box intersects with – this will give you a minimum and maximum tile 
                    value on the OPPOSITE axis. For example, if we’re walking left, perhaps the 
                    player intersects with horizontal rows 32, 33 and 34 (that is, tiles with y = 32 * TS, y = 33 * TS, and y = 34 * TS, 
                    where TS = tile size).
                */
                var tileTop = tileMap.PositionToTile(topPoint);
                var tileBottom = tileMap.PositionToTile(bottomPoint);

                // -- debug 
                tileTop.Intersected = true;
                tileBottom.Intersected = true;

                // --

                /*
                    Scan along those lines of tiles and towards the direction of movement until you find the closest static obstacle. 
                    NOT IMPLEMENTED -> Then loop through every moving obstacle, and determine which is the closest obstacle that is actually on your path.
                */
                var solid1 = tileMap.GetNextSolidX(topPoint, (int)direction.X);
                var solid2 = tileMap.GetNextSolidX(bottomPoint, (int)direction.X);

                /*
                    The total movement of the player along that direction is then the minimum between the distance to 
                    closest obstacle,  and the amount that you wanted to move in the first place.
                */
                if (direction.X > 0)
                {
                    var movementX = Math.Min(Math.Min(solid1.X, solid2.X) - topPoint.X, moveAmount.X);
                    return new Vector2(movementX, 0);
                }
                else
                {
                    var movementX = Math.Min(topPoint.X - Math.Min(solid1.X + box.Width, solid2.X + box.Width), moveAmount.X);
                    if (movementX < moveAmount.X)
                        movementX = -movementX;
                    return new Vector2(movementX, 0);
                }
            }
            return Vector2.Zero;
        }
    }
}
