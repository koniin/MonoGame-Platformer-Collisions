using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionDetectionPlatformer
{
    public class CollisionHandler
    {
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
                Vector2 topPoint = new Vector2(box.X + (box.Width), box.Y);
                Vector2 bottomPoint = new Vector2(box.X + (box.Width), box.Y + box.Height);

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
                var solid1 = tileMap.GetNextSolidX(topPoint, 1);
                var solid2 = tileMap.GetNextSolidX(bottomPoint, 1);

                /*
                    The total movement of the player along that direction is then the minimum between the distance to 
                    closest obstacle,  and the amount that you wanted to move in the first place.
                */
                var movementX = Math.Min(Math.Min(solid1.X, solid2.X) - topPoint.X, moveAmount.X);
                return new Vector2(movementX, 0);
            }
            return Vector2.Zero;
        }
    }
}
