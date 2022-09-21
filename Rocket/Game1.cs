﻿using System;
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
        private Texture2D myDyamond;
        private SpriteFont myFont;
        private Texture2D gaveOver;

        Random r = new Random();

        int spriteXRocket;
        int spriteYRocket;
        int spriteYRocketOnStartup;
        int moveXRocket;
        int moveYRocket;
        int numberOfSpaceRocks = 12;
        int spriteYComet = 0;
        int spriteXComet = 900;
        int level = 0;
        int dyamondX;
        int dyamondY;
        int[] spaceRockX;
        int[] spaceRockY;

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
            this.LoadDyamond();
        }
        #endregion
        #region myMethods

        #region LoadRandomPosition

        private void LoadOnRandomX() {
            spriteYRocketOnStartup = r.Next(0, 615);
        }

        #endregion

        #region LoadDyamond

        private void LoadDyamond() {
            dyamondX = r.Next(700, 900);
            dyamondY = r.Next(0, 625);
        }

        #endregion

        #region CreateRocks
        private void CreateRocks() {
            //rockMovesY = new int[numberOfSpaceRocks];
            spaceRockX = new int[numberOfSpaceRocks];
            spaceRockY = new int[numberOfSpaceRocks];

            for(int x = 0; x < numberOfSpaceRocks; x++) {
                spaceRockX[x] = r.Next(800, 900);
                spaceRockY[x] = r.Next(0, 625);
            }
        }
        #endregion

        #region moveComets

        private void MoveComets() {
            spriteYComet = r.Next(0, 615);
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
            myRocket = Content.Load<Texture2D>("rocket2");
            mySpace = Content.Load<Texture2D>("space");
            myFont = Content.Load<SpriteFont>("myFont");
            mySpacialRock = Content.Load<Texture2D>("spaceRock2");
            myDyamond = Content.Load<Texture2D>("dyamond");
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

                //Movimiento infinito: una vez fuera de la pantalla, vuelven a aparecer.
                if(spriteXComet < 0) {
                    spriteXComet = 900;
                    for(int x = 0; x < numberOfSpaceRocks; x++) {
                        spaceRockX[x] = r.Next(700, 900);
                        spaceRockY[x] = r.Next(0, 625);
                    }
                }
            }

            foreach (Keys key in pressedKey) {
                if (key == Keys.Up) {
                    moveYRocket = -5;
                    moveXRocket = 0;

                    spriteYRocket = spriteYRocket + moveYRocket;
                }

                if (key == Keys.Down) {
                    moveYRocket = 5;
                    moveXRocket = 0;

                    spriteYRocket = spriteYRocket + moveYRocket;
                }

                if (key == Keys.Left) {
                    moveYRocket = 0;
                    moveXRocket = -5;

                    spriteXRocket = spriteXRocket + moveXRocket;
                }

                if (key == Keys.Right) {
                    moveYRocket = 0;
                    moveXRocket = 5;

                    spriteXRocket = spriteXRocket + moveXRocket;
                }

                if(key == Keys.Escape) {
                    this.Exit();
                }
            }

            if(spriteXRocket > 875) {
                spriteXRocket = 875;
            }

            if(spriteYRocket < 0) {
                spriteYRocket = 0;
            }

            //Prevent to go aoutside the screen
            if (spriteXRocket + moveXRocket < 700) {
                spriteXRocket = 700;
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
            _spriteBatch.Draw(myDyamond, new Rectangle(dyamondX, dyamondY, 50, 50), Color.White);

            for (int i = 0; i < numberOfSpaceRocks; i++) {
                _spriteBatch.Draw(mySpacialRock, new Rectangle(spaceRockX[i] - spriteXComet, spaceRockY[i], 70, 40), Color.White);
            }
            _spriteBatch.Draw(myRocket, new Rectangle(spriteXRocket, spriteYRocket, 70, 40), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion

    }
}
