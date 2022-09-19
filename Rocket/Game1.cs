using System;
using System.Diagnostics.SymbolStore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rocket {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D myRocket; //cohete
        private Texture2D mySpace; //Background
        private Texture2D mySpacialRock; //cometa
        private SpriteFont myFont;
        private Texture2D gaveOver;

        int spriteX;
        int spriteY;
        int moveX;
        int moveY;
        int numberOfSpaceRocks = 5;
        int points;
        int maxPoint;
        int[] rockMovesY;
        int[] spaceRockY;
        Vector2 position;

        #region Game1
        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this._graphics.PreferredBackBufferWidth = 800;
            this._graphics.PreferredBackBufferHeight = 525;
            this.IsMouseVisible = false;
            this.CreateRocks();
        }
        #endregion
        #region myMethods
        #region CreateRocks
        private void CreateRocks() {
            rockMovesY = new int[numberOfSpaceRocks];
            spaceRockY = new int[numberOfSpaceRocks];

            for(int x = 0; x < numberOfSpaceRocks; x++) {
                rockMovesY[x] = 450;
                spaceRockY[x] = 0;
            }
        }
        #endregion
        #endregion



        #region Initialize
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }
        #endregion

        #region LoadContent
        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            myRocket = Content.Load<Texture2D>("rocket");
            mySpace = Content.Load<Texture2D>("space");
            myFont = Content.Load<SpriteFont>("myFont");
            mySpacialRock = Content.Load<Texture2D>("spaceRock");
            // TODO: use this.Content to load your game content here
        }
        #endregion

        #region Update
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] pressedKey = keyboardState.GetPressedKeys();

            foreach(Keys key in pressedKey) {
                if (key == Keys.Up) {
                    moveY = -5;
                    moveX = 0;

                    spriteY = spriteY + moveY;
                }

                if (key == Keys.Down) {
                    moveY = 5;
                    moveX = 0;

                    spriteY = spriteY + moveY;
                }

                if (key.Equals(Keys.Left)) {
                    moveY = 0;
                    moveX = -5;

                    spriteX = spriteX + moveX;
                }

                if (key.Equals(Keys.Right)) {
                    moveY = 0;
                    moveX = 5;

                    spriteX = spriteX + moveX;
                }

            }

            if(spriteX < 0) {
                spriteX = 0;
            }

            if(spriteY < 0) {
                spriteY = 0;
            }

            if (spriteX+moveX>_graphics.GraphicsDevice.Viewport.Width) {
                spriteX = _graphics.GraphicsDevice.Viewport.Width-moveX;
            }
            if (spriteY + moveY > _graphics.GraphicsDevice.Viewport.Height) {
                spriteY = _graphics.GraphicsDevice.Viewport.Height - moveY;
            }

            position.X = spriteX;
            position.Y = spriteY;

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(mySpace, new Rectangle(0,0,800,525), Color.White);
            _spriteBatch.Draw(myRocket, new Rectangle(0, 0, 70, 40), Color.White);
            _spriteBatch.Draw(mySpacialRock, new Rectangle(230, 150, 70, 40), Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion
    }
}
