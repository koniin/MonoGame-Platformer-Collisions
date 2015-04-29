using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionDetectionPlatformer
{
    class PreCheckingPlayer
    {
        public PreCheckingPlayer()
        {
            Velocity = Vector2.Zero;
        }

        public PreCheckingPlayer(Vector2 position)
        {
            Position = position;
        }

        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 500.0f;
        private const float MaxFallSpeed = 450.0f;
        private const float JumpControlPower = 0.14f; 

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;
        
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

        private float movement;
        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        public void Update(float dt)
        {
            GetInput();

            ApplyPhysics(dt);

            Move(dt);

            movement = 0.0f;
            isJumping = false;
        }

        private void GetInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (//gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A))
            {
                movement = -1.0f;
            }
            else if (//gamePadState.IsButtonDown(Buttons.DPadRight) ||
                     keyboardState.IsKeyDown(Keys.Right) ||
                     keyboardState.IsKeyDown(Keys.D))
            {
                movement = 1.0f;
            }

            isJumping =
                //gamePadState.IsButtonDown(JumpButton) ||
                keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W);
        }

        private void ApplyPhysics(float dt)
        {
            Vector2 previousPosition = Position;

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            velocity.X += movement * MoveAcceleration * dt;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * dt, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, dt);

            // Apply pseudo-drag horizontally.
            if (IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            // Prevent the player from running faster than his top speed.            
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);
        }

        private void Move(float dt)
        {
            int directionX = Math.Sign(velocity.X);
            int directionY = Math.Sign(velocity.Y);
            int newX = (int)Math.Floor(position.X); // round down if float?
            int newY = (int)Math.Floor(position.Y); // round down if float?

            Rectangle boundingBox = new Rectangle(newX, newY, 32, 32);
            var tileMap = TileMap.Map;

            isOnGround = false;

            // Move X first
            if (velocity.X > 0)
            {
                for (int x = 1; x <= Math.Abs(velocity.X * dt); x++)
                {
                    boundingBox.X += directionX;
                    Tile t1 = tileMap.PositionToTile(boundingBox.Right, boundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(boundingBox.Right, boundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(boundingBox))
                    {
                        // Add slope handling
                        // Reset velocity x if hit
                        break;
                    }
                    newX = boundingBox.X;
                }
            }
            if (velocity.X < 0)
            {
                for (int x = 1; x <= Math.Abs(velocity.X * dt); x++)
                {
                    boundingBox.X += directionX;
                    Tile t1 = tileMap.PositionToTile(boundingBox.Left, boundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(boundingBox.Left, boundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(boundingBox))
                    {
                        // Add slope handling
                        // Reset velocity x if hit
                        break;
                    }
                    newX = boundingBox.X;
                }
            }

            // Move Y
            if (velocity.Y > 0)
            {
                for (int y = 1; y < Math.Abs(velocity.Y * dt); y++)
                {
                    boundingBox.Y += directionY;
                    // Bottom tiles
                    Tile t1 = tileMap.PositionToTile(newX, boundingBox.Bottom);
                    Tile t2 = tileMap.PositionToTile(newX + boundingBox.Width - 1, boundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(boundingBox))
                    {
                        // Add slope handling
                        // Reset velocity y if hit on bottom
                        // directionY == 1 ? Velocity.Y = 0
                        isOnGround = true;
                        break;
                    }
                    newY = boundingBox.Y;
                }
            }
            if (velocity.Y < 0)
            {
                for (int y = 1; y < Math.Abs(velocity.Y * dt); y++)
                {
                    boundingBox.Y += directionY;
                    // Bottom tiles
                    Tile t1 = tileMap.PositionToTile(newX, boundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(newX + boundingBox.Width - 1, boundingBox.Top);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(boundingBox))
                    {
                        // Add slope handling
                        // Reset velocity y if hit on bottom
                        // directionY == 1 ? Velocity.Y = 0
                        //isJumping = false;
                        break;
                    }
                    newY = boundingBox.Y;
                }
            }

            if (position.Y == newY)
                isOnGround = true;

            // Update current position
            position.X = newX;
            position.Y = newY;

            /*
            // Apply velocity.
            Position += velocity * dt;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
             * */
        }

        private float DoJump(float velocityY, float dt)
        {
            // If the player wants to jump
            if (isJumping)
            {
                // Begin or continue a jump
                if ((!wasJumping && IsOnGround) || jumpTime > 0.0f)
                {
                    /*
                    if (jumpTime == 0.0f)
                        jumpSound.Play();
                    */
                    jumpTime += dt;
                    //sprite.PlayAnimation(jumpAnimation);
                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
            }
            wasJumping = isJumping;

            return velocityY;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            Renderer.Render(spriteBatch, (int)position.X, (int)position.Y);
        }
    }
}
