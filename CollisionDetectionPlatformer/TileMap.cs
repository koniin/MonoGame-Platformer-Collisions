using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionDetectionPlatformer
{
    public class TileMap
    {
        public static TileMap Map { get; private set;}

        private int _tileSize;
        public Tile[,] Tiles { get; set; }
        private Point _gridSize;
        public TileMap(int tileSize, Point gridSize)
        {
            Map = this;
            _tileSize = tileSize;
            _gridSize = gridSize;
            Tiles = new Tile[gridSize.X, gridSize.Y];
            for (int x = 0; x < gridSize.X; x++)
            {
                for (int y = 0; y < gridSize.Y; y++)
                {
                    Tiles[x, y] = new Tile(string.Format("{0}:{1}", x * _tileSize, y * _tileSize))
                    {
                        Position = new Vector2(x, y)
                    };
                    if (x == 0 || x == gridSize.X - 1 || y == 0 || y == gridSize.Y - 1)
                        Tiles[x, y].IsSolid = true;
                }
            }
        }

        /*
        Assuming that there are no slopes and one-way platforms, the algorithm is straightforward:
	
        Decompose movement into X and Y axes, step one at a time. If you’re planning on implementing slopes afterwards, 
        step X first, then Y. Otherwise, the order shouldn’t matter much. 
        Then, for each axis:
            Get the coordinate of the forward-facing edge, e.g. : If walking left, the x coordinate of left of bounding box. 
            If walking right, x coordinate of right side. If up, y coordinate of top, etc.
		
            Figure which lines of tiles the bounding box intersects with – this will give you a minimum and maximum tile 
            value on the OPPOSITE axis. For example, if we’re walking left, perhaps the 
            player intersects with horizontal rows 32, 33 and 34 (that is, tiles with y = 32 * TS, y = 33 * TS, and y = 34 * TS, 
            where TS = tile size).
		
            Scan along those lines of tiles and towards the direction of movement until you find the closest static obstacle. 
            Then loop through every moving obstacle, and determine which is the closest obstacle that is actually on your path.
		
            The total movement of the player along that direction is then the minimum between the distance to closest obstacle, 
            and the amount that you wanted to move in the first place.
		
            Move player to the new position. With this new position, step the other coordinate, if still not done.
	
        */
        
        public Vector2 TryMovePlayer(Vector2 position, Vector2 moveAmount, Vector2 direction)
        {
            // direction.X => positive is moving right, negative is moving left
            // direction.Y => positive is moving down, negative is moving up

            // First find tiles in Y where we are moving to
            // Step X first
            if (direction.X != 0)
            {
                // For larger objects we need more points (same size sprite yields max 2 tiles)
                // Get the coordinate of the forward-facing edge, e.g. : If walking left, 
                // the x coordinate of left of bounding box. 
                // If walking right, x coordinate of right side. If up, y coordinate of top, etc.
                float boxEdge = direction.X > 0 ? position.X + _tileSize : position.X;
                Vector2 topPoint = new Vector2(boxEdge, position.Y);
                Vector2 bottomPoint = new Vector2(boxEdge, position.Y + _tileSize);

                /*
                    Figure which lines of tiles the bounding box intersects with – this will give you a minimum and maximum tile 
                    value on the OPPOSITE axis. For example, if we’re walking left, perhaps the 
                    player intersects with horizontal rows 32, 33 and 34 (that is, tiles with y = 32 * TS, y = 33 * TS, and y = 34 * TS, 
                    where TS = tile size).
                */
                var tileTop = PositionToTile(topPoint);
                var tileBottom = PositionToTile(bottomPoint);

                // -- debug 
                tileTop.Intersected = true;
                tileBottom.Intersected = true;

                // --

                /*
                    Scan along those lines of tiles and towards the direction of movement until you find the closest static obstacle. 
                    NOT IMPLEMENTED -> Then loop through every moving obstacle, and determine which is the closest obstacle that is actually on your path.
                */
                var solid1 = GetNextSolidX(topPoint, (int)direction.X);
                var solid2 = GetNextSolidX(bottomPoint, (int)direction.X);

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
                    var movementX = Math.Min(topPoint.X - Math.Min(solid1.X + _tileSize, solid2.X + _tileSize), moveAmount.X);
                    if (movementX < moveAmount.X)
                        movementX = -movementX;
                    return new Vector2(movementX, 0);
                }
            }
            return Vector2.Zero;
        }
        
        public Tile PositionToTile(Vector2 position)
        {
            return PositionToTile(position.X, position.Y);
        }

        private Tile PositionToTile(float x, float y)
        {
            int tileX = (int)(x / _tileSize);
            int tileY = (int)(y / _tileSize);
            return Tiles[tileY, tileX];
        }

        public Tile GetTile(Vector2 position)
        {
            return Tiles[(int)position.X, (int)position.Y];
        }

        public Tile PositionToTile2(Vector2 position)
        {
            return PositionToTile(position.X, position.Y);
        }

        private Tile PositionToTile2(float x, float y)
        {
            int tileX = (int)(x / _tileSize);
            int tileY = (int)(y / _tileSize);
            return Tiles[tileX, tileY];
        }

        public Vector2 GetNextSolidX(Vector2 pos, int direction)
        {
            for (int x = (int)(pos.X / _tileSize); x < _gridSize.X; x += direction)
            {
                if (Tiles[x, (int)(pos.Y / _tileSize)].IsSolid)
                {
                    return new Vector2(x * _tileSize, pos.Y);
                }
            }
            return Vector2.Zero;
        }

        public Vector2 GetNextSolidY(Vector2 pos, int direction)
        {
            for (int y = (int)(pos.Y / _tileSize); y < _gridSize.Y; y += direction)
            {
                if (Tiles[(int)(pos.X / _tileSize), y].IsSolid)
                {
                    return new Vector2(pos.X, y);
                }
            }
            return Vector2.Zero;
        }

        private Texture2D pointTexture;
        private Dictionary<Color, Texture2D> textures = new Dictionary<Color, Texture2D>();

        public void Render(SpriteBatch batch)
        {
            for (int x = 0; x < _gridSize.X; x++)
            {
                for (int y = 0; y < _gridSize.Y; y++)
                {
                    if (y == 0 && x == 0)
                    {
                        DrawRectangle(batch, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), 1, Color.Cyan);
                    }
                    else if (y == _gridSize.Y - 1 && x == 0)
                    {
                        DrawRectangle(batch, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), 1, Color.Pink);
                    }
                    else
                    {
                        if (Tiles[y, x].Intersected)
                            DrawRectangle(batch, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), 1, Color.Blue);
                        if (Tiles[y, x].IsSolid)
                            DrawRectangle(batch, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), 1, Color.Red);
                        if (Tiles[y, x].IsSolid && Tiles[y, x].Intersected)
                            DrawRectangle(batch, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), 1, Color.Yellow);
                        
                    }
                }
            }
            
        }

        private void DrawRectangle(SpriteBatch batch, Rectangle area, int width, Color color)
        {
            if (!textures.ContainsKey(color)) {
                pointTexture = new Texture2D(batch.GraphicsDevice, 1, 1);
                pointTexture.SetData<Color>(new Color[] { color });
                textures[color] = pointTexture;
            }
            else
            {
                pointTexture = textures[color];
            }


            batch.Draw(pointTexture, new Rectangle(area.X, area.Y, area.Width, width), color);
            batch.Draw(pointTexture, new Rectangle(area.X, area.Y, width, area.Height), color);
            batch.Draw(pointTexture, new Rectangle(area.X + area.Width - width, area.Y, width, area.Height), color);
            batch.Draw(pointTexture, new Rectangle(area.X, area.Y + area.Height - width, area.Width, width), color);
        }
    }
}
