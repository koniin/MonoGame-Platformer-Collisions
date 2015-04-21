using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionDetectionPlatformer
{
    class Player
    {
        private Texture2D pointTexture;
        private KeyboardState lastKeyBoardState;

        private const float MoveAcceleration = 150f;
        private const float MaxMoveSpeed = 400f;
        private const float GroundDragFactor = 1f;
        private const float Gravity = 5.81f;

        public Vector2 Direction { get; set; }
        public Vector2 _position;

        private Vector2 velocity;

        public Player(Vector2 position)
        {
            _position = position;
            velocity = new Vector2();
        }

        public void Update(float deltaTime, float dt)
        {
            Vector2 lastPos = _position;

            KeyboardState currentKeyBoardState = Keyboard.GetState();
            if (lastKeyBoardState.IsKeyDown(Keys.Up) && currentKeyBoardState.IsKeyUp(Keys.Up)) {
            }
            if (lastKeyBoardState.IsKeyDown(Keys.Down) && currentKeyBoardState.IsKeyUp(Keys.Down))
            {
            }
            if (currentKeyBoardState.IsKeyDown(Keys.Left))
            {
                Direction = -Vector2.UnitX;
            }
            if (currentKeyBoardState.IsKeyDown(Keys.Right))
            {
                Direction = Vector2.UnitX;
            }

            velocity.X += Direction.X * MoveAcceleration * deltaTime;
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            if (velocity.X != 0)
            {
                var v = TileMap.Map.TryMovePlayer(_position, velocity * deltaTime, new Vector2(Math.Sign(velocity.X), Math.Sign(velocity.Y)));
                //_position += velocity * deltaTime;
                _position += v;
                
            }
            //else
            //{
            //    _position += velocity * deltaTime;
            //}
            velocity.X *= GroundDragFactor;
            
            Direction = Vector2.Zero;
        }

        public void Render(SpriteBatch batch)
        {
            DrawRectangle(batch, new Rectangle((int)_position.X, (int)_position.Y, 32, 32), 1, Color.Gray);
        }

        private void DrawRectangle(SpriteBatch batch, Rectangle area, int width, Color color)
        {
            if (pointTexture == null) {
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
