using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rocket {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D myRocketLeft; //cohete
        private Texture2D myRocketRight;
        private Texture2D myRocketUp;
        private Texture2D myRocketDown;
        private Texture2D mySpace; //Background
        private Texture2D mySpacialRock; //cometa
        private Texture2D myDiamond;
        private Texture2D collision;
        private SpriteFont myFont;
        private Texture2D gaveOver;

        Random r = new Random();
        Rectangle[] cometPosition;
        Rectangle rocketPosition;
        Rectangle diamondPosition;

        int spriteXRocket; //Posicion en el eje x del cometa
        int spriteYRocket; //Posicion en el eje y del cometa
        int moveXRocket; //Movimiento en x del cometa
        int moveYRocket; //Movimiento en y del cometa
        int numberOfSpaceRocks = 15; //Cantidad de obstaculos a agregar
        int spriteYComet = 0; //Posicion en el eje y donde apareceran
        int spriteXComet = 900; //Posicion en el eje x donde apareceran
        int diamondX; //Posicion en x del diamante
        int diamondY; //Lo mismo, pero en y
        int spriteRocketAddress = 2;//0=Arriba,1=Abajo,2=Izquierda,3=Derecha
        int points = 0; //Puntos por cada diamante capturado
        int collisionX = 0; //Posicion en x donde hubo colision
        int collisionY = 0; //Lo mismo, pero en y
        int[] spaceRockX; //Arreglo de posiciones de cada cometa en el eje x
        int[] spaceRockY; //Lo mismo, pero en y

        bool hasCrashed = false;

        #region Game1
        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this._graphics.PreferredBackBufferWidth = 950;
            this._graphics.PreferredBackBufferHeight = 650;
            this.IsMouseVisible = false;
            this.CreateRocks();
            this.MoveComets();
            this.LoadDiamond();
        }
        #endregion
        #region myMethods

        #region LoadDyamond

        //Indicar en donde hacer aparecer el diamante, usando un metodo aleatorio
        private void LoadDiamond() {
            diamondX = r.Next(0, 900);
            diamondY = r.Next(0, 625);

            diamondPosition = new Rectangle(diamondX, diamondY, 50, 50);
        }

        #endregion

        #region CreateRectangles
        //Crear rectangulos a aprtir de las posiciones de los cometas y sus dimensiones
        private void CreateRectangles(int positionX, int positionY) {
            cometPosition = new Rectangle[numberOfSpaceRocks];
            
            for(int x = 0; x < numberOfSpaceRocks; x++) {
                cometPosition[x] = new Rectangle(positionX, positionY, 70, 40);
                this.RocketHasCrashed(cometPosition, positionX, positionY);
            }
        }

        #endregion

        #region CreateRocks
        //Dar la posicion y el numero de cometas
        private void CreateRocks() {
            spaceRockX = new int[numberOfSpaceRocks];
            spaceRockY = new int[numberOfSpaceRocks];

            for(int i = 0; i < numberOfSpaceRocks; i++) {
                spaceRockX[i] = r.Next(800, 900);
                spaceRockY[i] = r.Next(0, 625);

                //LLamamos a la funcion para agregarles un margen de rectangulo
                CreateRectangles(spaceRockX[i], spaceRockY[i]);
            }
        }
        #endregion

        #region hasCrashed
        //funcion que comprueba si el cohete ha chocado contra un cometa
        private void RocketHasCrashed(Rectangle[] cometPosition, int positionX, int positionY) {
            for(int x = 0; x < numberOfSpaceRocks; x++) {
                if (rocketPosition.Intersects(cometPosition[x])) {
                    hasCrashed = true;
                    //guardamos las posiciones donde ha colisionado, para, posteriormente, colocar una textura de colision ahi
                    collisionX = positionX;
                    collisionY = positionY;
                }
            }
        }

        //Comprobamos si se ha capturado al diamante (interceptado)
        private void RocketHasTheDiamond(Rectangle rocketPosition) {
            if (rocketPosition.Intersects(diamondPosition)) {
                points++;
                //Si se ha capturado, hacemos aparecer uno nuevo en una nueva posicion aleatoria diferente
                this.LoadDiamond();
            }
        }

        #endregion

        #region moveComets
        //Mover los cometas; los hacemos aparecer en una posicin aleatoria en Y
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
            //Cargando texturas...
            myRocketUp = Content.Load<Texture2D>("rocket0");
            myRocketDown = Content.Load<Texture2D>("rocket1");
            myRocketLeft = Content.Load<Texture2D>("rocket2");
            myRocketRight = Content.Load<Texture2D>("rocket3");
            mySpace = Content.Load<Texture2D>("space");
            myFont = Content.Load<SpriteFont>("myFont");
            mySpacialRock = Content.Load<Texture2D>("spaceRock2");
            myDiamond = Content.Load<Texture2D>("dyamond");
            collision = Content.Load<Texture2D>("collision");
            // TODO: use this.Content to load your game content here
        }
        #endregion

        #region Update
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] pressedKey = keyboardState.GetPressedKeys();

            //Hacer que los cometas se muevan cada segundo en -50
            if (gameTime.TotalGameTime.Milliseconds == 0) {
                spriteXComet-=50;

                //"Movimiento infinito": una vez fuera de la pantalla, vuelven a aparecer en el inicio, pero en posiciones diferentes (aleatorias).
                if(spriteXComet < 0) {
                    spriteXComet = 900;
                    for(int x = 0; x < numberOfSpaceRocks; x++) {
                        spaceRockX[x] = r.Next(450, 900);
                        spaceRockY[x] = r.Next(0, 625);
                    }
                }
            }

            //Captura de eventos de teclado
            foreach (Keys key in pressedKey) {
                if (key == Keys.Up) {
                    moveYRocket = -5;
                    moveXRocket = 0;

                    spriteYRocket = spriteYRocket + moveYRocket;
                    //Capturamos la direccino tomada para cambiar la textura luego
                    spriteRocketAddress = 0;
                }

                if (key == Keys.Down) {
                    moveYRocket = 5;
                    moveXRocket = 0;

                    spriteYRocket = spriteYRocket + moveYRocket;
                    spriteRocketAddress = 1;
                }

                if (key == Keys.Left) {
                    moveYRocket = 0;
                    moveXRocket = -5;

                    spriteXRocket = spriteXRocket + moveXRocket;
                    spriteRocketAddress = 2;
                }

                if (key == Keys.Right) {
                    moveYRocket = 0;
                    moveXRocket = 5;

                    spriteXRocket = spriteXRocket + moveXRocket;
                    spriteRocketAddress = 3;
                }

                if(key == Keys.Escape) {
                    this.Exit();
                }
            }

            //Asignar limites a los margenes
            if(spriteXRocket < 0) {
                spriteXRocket = 0;
            }

            if(spriteYRocket < 0) {
                spriteYRocket = 0;
            }

            //Asignar limites al limite: evitamos que llegue al limite completo para que no se vea bugueado
            if (spriteXRocket + moveXRocket > 860) {
                spriteXRocket = 860;
            }
            if (spriteYRocket + moveYRocket > 600) {
                spriteYRocket = 600;
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
            _spriteBatch.Draw(myDiamond, new Rectangle(diamondX, diamondY, 50, 50), Color.White);

            //Movemos los cometas y hacemos aparecerlos: iniciaran en una posicion definida aleatoriamente menos la posicion inicial del cometa,
            //para que luzca que se mueve desde el vacio del espacio (?
            for (int i = 0; i < numberOfSpaceRocks; i++) {
                _spriteBatch.Draw(mySpacialRock, new Rectangle(spaceRockX[i] - spriteXComet, spaceRockY[i], 70, 40), Color.White);
            }

            //Arreglo de posiciones: hacemos cargar todas las rocas en posiciones diferentes y que se muevan constantemente 
            for(int x = 0; x < numberOfSpaceRocks; x++) {
                //Dependiendo del la direccion, cargamos una textura diferente
                switch (spriteRocketAddress) {
                    case 0:
                        _spriteBatch.Draw(myRocketUp, new Rectangle(900 - spriteXRocket, spriteYRocket, 40, 70), Color.White);
                        rocketPosition = new Rectangle(spriteXRocket, spriteYRocket, myRocketUp.Width, myRocketUp.Height);
                        break;
                    case 1:
                        _spriteBatch.Draw(myRocketDown, new Rectangle(spriteXRocket, spriteYRocket, 40, 70), Color.White);
                        rocketPosition = new Rectangle(spriteXRocket, 325 + spriteYRocket, myRocketUp.Width, myRocketUp.Height);
                        break;
                    case 2:
                        _spriteBatch.Draw(myRocketLeft, new Rectangle(spriteXRocket, spriteYRocket, 70, 40), Color.White);
                        rocketPosition = new Rectangle(spriteXRocket, 325 + spriteYRocket, myRocketUp.Width, myRocketUp.Height);
                        break;
                    case 3:
                        _spriteBatch.Draw(myRocketRight, new Rectangle(spriteXRocket, spriteYRocket, 70, 40), Color.White);
                        rocketPosition = new Rectangle(spriteXRocket, 325 + spriteYRocket, myRocketUp.Width, myRocketUp.Height);
                        break;
                }
                //Obtenemos la posicion del diamante y vamos comparando con la del cometa (working with some bugs)
                this.RocketHasTheDiamond(rocketPosition);

                //Comprobamos si no ha colisionado (not working).
                if (hasCrashed) {                    
                    _spriteBatch.Draw(collision, new Rectangle(collisionX, collisionY, 100, 100), Color.White);
                    _spriteBatch.DrawString(myFont, "Game over\nPoints earned: " + points, new Vector2(_graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight/2) ,Color.White);
                    System.Threading.Thread.Sleep(5000);
                    this.Exit(); //Aca iba a ir alguna animacion, pero al no cargar el evento de colision, no puse na'a
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion

    }
}
