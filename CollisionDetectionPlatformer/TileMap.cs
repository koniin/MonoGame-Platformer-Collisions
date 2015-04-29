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
                    if (y == gridSize.Y - 2 && x == 5)
                    {
                        Tiles[x, y].IsSolid = true;
                    }

                    if (y == gridSize.Y - 4 && x == _gridSize.X - 3)
                    {
                        Tiles[x, y].IsSolid = true;
                    }
                    if (y == 4 && x >= 0 && x < 6)
                    {
                        Tiles[x, y].IsSolid = true;
                    }
                }
            }
        }
                
        public Tile PositionToTile(Vector2 position)
        {
            return PositionToTile(position.X, position.Y);
        }

        public Tile PositionToTile(float x, float y)
        {
            int tileX = (int)(x / _tileSize);
            int tileY = (int)(y / _tileSize);
            return Tiles[tileX, tileY];
        }

        public Tile GetTile(int x, int y)
        {
            return Tiles[x, y];
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
                        DrawRectangle(batch, y, x, _tileSize, 1, Color.Cyan);
                    }
                    else if (y == _gridSize.Y - 1 && x == 0)
                    {
                        DrawRectangle(batch, y, x, _tileSize, 1, Color.Pink);
                    }
                    else
                    {
                        if (Tiles[y, x].Intersected)
                            DrawRectangle(batch, y, x, _tileSize, 1, Color.Blue);
                        if (Tiles[y, x].IsSolid)
                            DrawRectangle(batch, y, x, _tileSize, 1, Color.Red);
                        if (Tiles[y, x].IsSolid && Tiles[y, x].Intersected)
                            DrawRectangle(batch, y, x, _tileSize, 1, Color.Yellow);
                        
                    }
                }
            }
            
        }

        private void DrawRectangle(SpriteBatch batch, int x, int y, int tileSize, int width, Color color)
        {
            DrawRectangle(batch, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), 1, color);
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
