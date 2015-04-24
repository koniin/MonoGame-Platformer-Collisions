using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionDetectionPlatformer
{
    public class SimplePlayer
    {
        private const float Gravity = 300f;
        private const float ACCEL = 900f;
        private const float FRICTION = 300f;
        private const float MAXDX = 300f;
        private const float MAXDY = 500f;

        bool wasLeft = false;
        bool wasRight = false;
        public bool falling = false;
        public bool jumping = false;

        public float dx;
        public float dy;
        float ddx = 0;
        float ddy = Gravity;

        public int x;
        public int y;
        
        public SimplePlayer(Vector2 position)
        {
            x = (int)position.X;
            y = (int)position.Y;
        }

        public void Update(float dt)
        {
            bool left = false;
            bool right = false;

            KeyboardState currentKeyBoardState = Keyboard.GetState();
            if (currentKeyBoardState.IsKeyDown(Keys.Up))
            {
            }
            if (currentKeyBoardState.IsKeyDown(Keys.Left))
            {
                left = true;
            }
            if (currentKeyBoardState.IsKeyDown(Keys.Right))
            {
                right = true;
            }


            wasLeft = dx < 0;
            wasRight = dx > 0;

            ddx = 0;
            ddy = Gravity;

            if (left)
                ddx = ddx - ACCEL;     // player wants to go left
            else if (wasLeft)
                ddx = ddx + FRICTION;  // player was going left, but not any more

            if (right)
                ddx = ddx + ACCEL;     // player wants to go right
            else if (wasRight)
                ddx = ddx - FRICTION;  // player was going right, but not any more

            /*
            if (player.jump && !player.jumping && !falling)
            {
                player.ddy = player.ddy - JUMP;     // apply an instantaneous (large) vertical impulse
                player.jumping = true;
            }
             * */

            y = (int)Math.Floor(y + (dt * dy));
            x = (int)Math.Floor(x + (dt * dx));
            dx = MathHelper.Clamp(dx + (dt * ddx), -MAXDX, MAXDX);
            dy = MathHelper.Clamp(dy + (dt * ddy), -MAXDY, MAXDY);

            if ((wasLeft && (dx > 0)) || (wasRight && (dx < 0)))
            {
                dx = 0; // clamp at zero to prevent friction from making us jiggle side to side
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            Renderer.Render(spriteBatch, x, y);
        }
    }
}
