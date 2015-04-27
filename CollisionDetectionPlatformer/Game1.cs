#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace CollisionDetectionPlatformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TileMap tileMap;
        //Player player;
        //SimplePlayer player;
        CollisionHandlingPlayer player;
        CollisionHandler collisionHandler;
        CollisionHandler2 collisionHandler2;
        SimpleCollisionHandler simpleCollisionHandler;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            tileMap = new TileMap(32, new Point(10,10));
            //player = new SimplePlayer(new Vector2(74, 62));
            player = new CollisionHandlingPlayer(new Vector2(74, 62));
            collisionHandler = new CollisionHandler();
            collisionHandler2 = new CollisionHandler2();
            simpleCollisionHandler = new SimpleCollisionHandler();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            /*
            Vector2 pos = collisionHandler2.CheckCollision(player.BoundingBox, tileMap);
            if(pos != Vector2.Zero)
                Player.Playa._position = pos;
             * */
            //simpleCollisionHandler.Collide(player, tileMap);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            tileMap.Render(spriteBatch);
            player.Render(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
