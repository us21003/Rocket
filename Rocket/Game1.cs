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

        int spriteXRocket;
        int spriteYRocket;
        int spriteYRocketOnStartup;
        int moveXRocket;
        int moveYRocket;
        int numberOfSpaceRocks = 5;
        int spriteYComet = 0;
        int spriteXComet = 900;
        int[] spaceRockY;

        bool gameIsRunning;

        #region Game1
        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this._graphics.PreferredBackBufferWidth = 950;
            this._graphics.PreferredBackBufferHeight = 650;
            this.IsMouseVisible = false;
            this.CreateRocks();
            this.LoadOnRandomX();
            this.MoveComets();
        }
        #endregion
        #region myMethods

        #region LoadRandomPosition

        private void LoadOnRandomX() {
            Random r = new Random();
            spriteYRocketOnStartup = r.Next(0, 615);
        }

        #endregion

        #region CreateRocks
        private void CreateRocks() {
            Random r = new Random();
            //rockMovesY = new int[numberOfSpaceRocks];
            spaceRockY = new int[numberOfSpaceRocks];

            for(int x = 0; x < numberOfSpaceRocks; x++) {
                //rockMovesY[x] = r.Next(0, 900);
                spaceRockY[x] = r.Next(0, 650);
            }
        }
        #endregion

        #region moveComets

        private void MoveComets() {
            Random r = new Random();
            spriteYComet = r.Next(0, 600);
        }

        private void RelocateComets() {
            //
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

            if (gameTime.TotalGameTime.Milliseconds == 0) {
                spriteXComet-=50;
            }

            foreach (Keys key in pressedKey) {
                if (key == Keys.Up) {
                    moveYRocket = -10;
                    moveXRocket = 0;

                    spriteYRocket = spriteYRocket + moveYRocket;
                }

                if (key == Keys.Down) {
                    moveYRocket = 10;
                    moveXRocket = 0;

                    spriteYRocket = spriteYRocket + moveYRocket;
                }

                if (key == Keys.Left) {
                    moveYRocket = 0;
                    moveXRocket = -10;

                    spriteXRocket = spriteXRocket + moveXRocket;
                }

                if (key == Keys.Right) {
                    moveYRocket = 0;
                    moveXRocket = 10;

                    spriteXRocket = spriteXRocket + moveXRocket;
                }

                if(key == Keys.Escape) {
                    this.Exit();
                }

            }

            if(spriteXRocket < 0) {
                spriteXRocket = 0;
            }

            if(spriteYRocket < 0) {
                spriteYRocket = 0;
            }

            //Prevent to go aoutside the screen
            if (spriteXRocket + moveXRocket > 300) {
                spriteXRocket = 300;
            }
            if (spriteYRocket + moveYRocket > 610) {
                spriteYRocket = 610;
            }

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(mySpace, new Rectangle(0,0,950,650), Color.White);
            
            for(int x = 0; x < numberOfSpaceRocks; x++) {
                _spriteBatch.Draw(mySpacialRock, new Rectangle(spriteXComet, spaceRockY[x], 70, 40), Color.White);
            }
            _spriteBatch.Draw(myRocket, new Rectangle(spriteXRocket, spriteYRocket, 70, 40), Color.White);


            _spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion

    }
}
