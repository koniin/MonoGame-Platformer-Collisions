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
        private const float JUMP = 10000f;
        private const float MAXDX = 300f;
        private const float MAXDY = 1200f;

        bool wasLeft = false;
        bool wasRight = false;
        public bool falling = true;
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
            bool jump = false;

            float acceleration = falling ? ACCEL * 0.5f : ACCEL;
            float friction = falling ? FRICTION * 0.5f : FRICTION;

            KeyboardState currentKeyBoardState = Keyboard.GetState();
            if (currentKeyBoardState.IsKeyDown(Keys.Up))
            {
                jump = true;
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
                ddx = ddx - acceleration;     // player wants to go left
            else if (wasLeft)
                ddx = ddx + friction;  // player was going left, but not any more

            if (right)
                ddx = ddx + acceleration;     // player wants to go right
            else if (wasRight)
                ddx = ddx - friction;  // player was going right, but not any more

            
            if (jump && !jumping && !falling)
            {
                ddy = ddy - JUMP;     // apply an instantaneous (large) vertical impulse
                jumping = true;
            }

            x = (int)Math.Floor(x + (dt * dx));
            y = (int)Math.Floor(y + (dt * dy));
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
