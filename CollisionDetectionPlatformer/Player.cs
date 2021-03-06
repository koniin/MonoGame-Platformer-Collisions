﻿using Microsoft.Xna.Framework;
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
        public static Player Playa
        {
            get;
            private set;
        }

        private Texture2D pointTexture;
        private KeyboardState lastKeyBoardState;

        private const float MoveAcceleration = 300f;
        private const float MaxMoveSpeed = 500f;
        private const float GroundDragFactor = 0.97f;
        private const float Gravity = 300f;

        public Rectangle BoundingBox { 
            get {
                return new Rectangle((int)_position.X, (int)_position.Y, 32, 32);
            }
        }

        public Vector2 Direction;
        public Vector2 _position;
        public Vector2 Velocity;

        public bool IsOnGround { get; set; }

        public Player(Vector2 position)
        {
            Playa = this;
            _position = position;
            Velocity = new Vector2();
        }

        public void Update(float deltaTime)
        {
            Vector2 lastPos = _position;
            Direction = Vector2.Zero;

            KeyboardState currentKeyBoardState = Keyboard.GetState();
            if (lastKeyBoardState.IsKeyDown(Keys.Up) && currentKeyBoardState.IsKeyUp(Keys.Up)) {
            }
            if (currentKeyBoardState.IsKeyDown(Keys.Up))
            {
                IsOnGround = false;
                Velocity.Y = -150;
            }
            if (currentKeyBoardState.IsKeyDown(Keys.Left))
            {
                Direction = -Vector2.UnitX;
            }
            if (currentKeyBoardState.IsKeyDown(Keys.Right))
            {
                Direction = Vector2.UnitX;
            }

            Velocity.X = Direction.X * MoveAcceleration;

            if (!IsOnGround)
            {
                Velocity.Y += Gravity * deltaTime;
            }
            else
            {
                Velocity.Y = 0;
            }

            _position += Velocity * deltaTime;
            
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

        public int PreviousBottom { get; set; }
    }
}
