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

        public static void Render(SpriteBatch batch, int x, int y)
        {
            DrawRectangle(batch, new Rectangle(x, y, 32, 32), 1, Color.Gray);
        }

        private static void DrawRectangle(SpriteBatch batch, Rectangle area, int width, Color color)
        {
            if (pointTexture == null)
            {
                pointTexture = new Texture2D(batch.GraphicsDevice, 1, 1);
                pointTexture.SetData<Color>(new Color[] { color });
            }

            batch.Draw(pointTexture, new Rectangle(area.X, area.Y, area.Width, width), color);
            batch.Draw(pointTexture, new Rectangle(area.X, area.Y, width, area.Height), color);
            batch.Draw(pointTexture, new Rectangle(area.X + area.Width - width, area.Y, width, area.Height), color);
            batch.Draw(pointTexture, new Rectangle(area.X, area.Y + area.Height - width, area.Width, width), color);
        }
    }
}
