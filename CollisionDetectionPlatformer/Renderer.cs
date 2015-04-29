using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionDetectionPlatformer
{
    public static class Renderer
    {
        private static Texture2D pointTexture;
        private static Dictionary<Color, Texture2D> textures = new Dictionary<Color, Texture2D>();

        public static void Render(SpriteBatch batch, int x, int y)
        {
            DrawRectangle(batch, new Rectangle(x, y, 32, 32), 1, Color.Gray);
        }

        public static void Render(SpriteBatch batch, int x, int y, Color color)
        {
            DrawRectangle(batch, new Rectangle(x, y, 32, 32), 1, color);
        }

        private static void DrawRectangle(SpriteBatch batch, Rectangle area, int width, Color color)
        {
            if (!textures.ContainsKey(color))
            {
                textures[color] = new Texture2D(batch.GraphicsDevice, 1, 1);
                textures[color].SetData<Color>(new Color[] { color });
            }

            batch.Draw(textures[color], new Rectangle(area.X, area.Y, area.Width, width), color);
            batch.Draw(textures[color], new Rectangle(area.X, area.Y, width, area.Height), color);
            batch.Draw(textures[color], new Rectangle(area.X + area.Width - width, area.Y, width, area.Height), color);
            batch.Draw(textures[color], new Rectangle(area.X, area.Y + area.Height - width, area.Width, width), color);
        }
    }
}
