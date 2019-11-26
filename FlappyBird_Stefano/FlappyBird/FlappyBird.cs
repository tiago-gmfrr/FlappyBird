using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FlappyBird
{
    public class FlappyBird : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch sb;

        #region Bird
        Bird bird;

        bool oldkState, keysDown;
        KeyboardState kState;
        #endregion

        bool isPlaying, oldIsPlaying;

        #region Environnement
        Background bg;

        Background floor;

        Stopwatch stopwatch;
        #endregion

        #region Pipes
        List<DoublePipe> doublePipes;

        Random r;

        Texture2D pipe;

        int rndPool, millis, viewH, viewW;

        float spawnRate;
        #endregion

        #region Score
        int score;

        SpriteFont sprFont, backFont;
        #endregion

        #region Menu
        Button btnRestart, btnQuit;

        Texture2D logo, gameOver, scoreBoard;

        MouseState mState, oldMState;

        Slider sldPipeGape;
        Rectangle sldRect;

        SpriteFont normal;
        #endregion

        #region Sounds
        SoundEffect soundDead, soundPoint, soundFlap;
        #endregion

        public static bool Restart = false;

        public FlappyBird()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;

            graphics.ApplyChanges();

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
            // TODO: Add your initialization logic here
            IsMouseVisible = false;

            stopwatch = new Stopwatch();

            oldkState = false;

            isPlaying = false;
            oldIsPlaying = false;

            doublePipes = new List<DoublePipe>();

            spawnRate = 750;

            r = new Random();

            score = 0;

            Highscore.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            sb = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            viewH = GraphicsDevice.Viewport.Height - 150;
            viewW = GraphicsDevice.Viewport.Width;

            #region Fonts
            sprFont = Content.Load<SpriteFont>("flappySprFont");
            backFont = Content.Load<SpriteFont>("backFont");

            normal = Content.Load<SpriteFont>("normal");
            #endregion

            #region Pipe
            pipe = Content.Load<Texture2D>("flappyPipe");

            doublePipes.Add(new DoublePipe(pipe, new Vector2(viewW - pipe.Width - 50, viewH / 2)));
            #endregion

            #region Menu
            Texture2D btnRestartT = Content.Load<Texture2D>("btnRestart");
            btnRestart = new Button(new Point(viewW / 2 - btnRestartT.Width - 25,
                viewH - btnRestartT.Height - 25),
                btnRestartT, Content.Load<Texture2D>("btnRestart"),
                Content.Load<Texture2D>("btnRestartHover"), 1);

            Texture2D btnQuitT = Content.Load<Texture2D>("btnQuit");
            btnQuit = new Button(new Point(viewW / 2 + 25,
                viewH - btnQuitT.Height - 25),
                btnQuitT, Content.Load<Texture2D>("btnQuit"),
                Content.Load<Texture2D>("btnQuitHover"), 1);

            logo = Content.Load<Texture2D>("flappyLogo");

            scoreBoard = Content.Load<Texture2D>("ScoreBoard");

            gameOver = Content.Load<Texture2D>("gameOver");

            sldRect = new Rectangle((int)doublePipes[0].X - 150, viewH / 2 - 100, 50, 200);

            sldPipeGape = new Slider(500, 700, DoublePipe.offset, sldRect,
                false, Color.LightGray, Color.Green, Color.DarkGreen);

            sldPipeGape.LoadContent(Content);
            #endregion

            #region Sound
            soundDead = Content.Load<SoundEffect>("flappyDeadSound");
            soundPoint = Content.Load<SoundEffect>("flappyPointSound");
            soundFlap = Content.Load<SoundEffect>("flappyUpSound");
            #endregion

            #region Bird
            Animation birdAnim = new Animation();
            Texture2D birdTexture = Content.Load<Texture2D>("FlappyBird");
            int frameCount = 4;
            float birdScale = .85f;
            birdAnim.Initialize(birdTexture, Vector2.Zero, birdTexture.Width / frameCount,
                birdTexture.Height, frameCount, 100, Color.White, birdScale, true);

            Vector2 birdPos = new Vector2(viewW / 4, viewH / 2);

            bird = new Bird(birdAnim, birdPos, Content.Load<Texture2D>("flappyDead"),
                viewH, birdScale, soundDead, soundFlap);
            #endregion

            #region Environnement
            bg = new Background(Content, "smallFlappyBg", viewW, viewH, -3, 0, 1);

            floor = new Background(Content, "floor", viewW, viewH + 150, -7, viewH, 1);
            #endregion
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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

            // TODO: Add your update logic here
            oldIsPlaying = isPlaying;

            kState = Keyboard.GetState();

            oldkState = keysDown;

            keysDown = AreKeysDown(kState, Keys.Space, Keys.F, Keys.G, Keys.H, Keys.V, Keys.B, Keys.N);

            bird.Update(gameTime, !oldkState && keysDown, isPlaying, out isPlaying);
            
            if (oldIsPlaying != isPlaying)
                gameTime.TotalGameTime = new TimeSpan(0, 0, 0);

            if (!bird.isDead)
            {
                #region Update Environnement
                bg.Update(gameTime);
                floor.Update(gameTime);
                #endregion

                if (isPlaying)
                {
                    millis = gameTime.TotalGameTime.Milliseconds;

                    if (millis != 0 && millis % spawnRate == 0)
                        doublePipes.Add(new DoublePipe(pipe,
                            new Vector2(viewW + pipe.Width / 2,
                            r.Next(viewH / 2 - rndPool, viewH / 2 + rndPool))));

                    for (int i = 0; i < doublePipes.Count; i++)
                    {
                        doublePipes[i].Update();

                        if (!doublePipes[i].isPointAdded && doublePipes[i].X < bird.X)
                        {
                            score++;
                            doublePipes[i].isPointAdded = true;

                            soundPoint.Play();
                        }

                        if (doublePipes[i].IsOut)
                        {
                            doublePipes.RemoveAt(i);
                            i--;
                            continue;
                        }

                        if (doublePipes[i].IsCollinding(bird.Hitbox))
                        {
                            bird.isDead = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (kState.IsKeyDown(Keys.W))
                        sldPipeGape.AddValue(-1f);
                    else if (kState.IsKeyDown(Keys.S))
                        sldPipeGape.AddValue(1f);

                    sldPipeGape.SetValue(Mouse.GetState());
                    DoublePipe.offset = (int)sldPipeGape.Value;
                    rndPool = (viewH - DoublePipe.offset) / 2;
                }

                Mouse.SetPosition(0, 0);
            }
            else
            {
                oldMState = mState;
                mState = Mouse.GetState();

                if (kState.IsKeyDown(Keys.A))
                    Mouse.SetPosition(btnRestart.pos.X, btnRestart.pos.Y);
                else if (kState.IsKeyDown(Keys.D))
                    Mouse.SetPosition(btnQuit.pos.X, btnQuit.pos.Y);

                if (btnRestart.Update(mState, oldMState) ||
                    (keysDown && btnRestart.pos.X == mState.X && btnRestart.pos.Y == mState.Y))
                    RestartGame();

                if (btnQuit.Update(mState, oldMState) ||
                    (keysDown && btnQuit.pos.X == mState.X && btnQuit.pos.Y == mState.Y))
                    Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            sb.Begin();
            bg.Draw(sb);

            // pipes
            foreach (DoublePipe db in doublePipes)
                db.Draw(sb);

            bird.Draw(sb);

            floor.Draw(sb);

            #region Show score
            if (isPlaying && !bird.isDead)
            {
                Vector2 posStr = new Vector2(viewW / 2 - score.ToString().Length * 36 / 2, 10);
                sb.DrawString(backFont, score.ToString(), posStr, Color.White);
                sb.DrawString(sprFont, score.ToString(), posStr, Color.Black);
            }
            #endregion

            if (!isPlaying)
            {
                int logoScale = 2;
                sb.Draw(logo, new Rectangle(GraphicsDevice.Viewport.Width / 2 - logo.Width / logoScale / 2, 75,
                    logo.Width / logoScale, logo.Height / logoScale), Color.White);

                sldPipeGape.Draw(sb);
                string strSld = "Choisissez l'espacement des tuyaux.";
                sb.DrawString(normal, strSld, new Vector2(sldRect.X - strSld.Length * 13, sldRect.Y + sldRect.Height / 2), Color.Black);
            }

            if (bird.isDead)
            {
                int highscore = Highscore.GetHighscore(score);

                int gameOverScale = 2;
                sb.Draw(gameOver, new Rectangle(viewW / 2 - gameOver.Width / gameOverScale / 2, 75,
                    gameOver.Width / gameOverScale, gameOver.Height / gameOverScale), Color.White);

                btnRestart.Draw(sb);
                btnQuit.Draw(sb);

                Rectangle scoreBoardRect = new Rectangle(viewW / 2 - scoreBoard.Width / 3,
                    viewH / 2 - scoreBoard.Height / 3 + 50,
                    (int)(scoreBoard.Width / 1.5), (int)(scoreBoard.Height / 1.5));

                sb.Draw(scoreBoard, scoreBoardRect, Color.White);

                Vector2 posScore = new Vector2(scoreBoardRect.X + scoreBoardRect.Width - 225,
                    scoreBoardRect.Y + (scoreBoardRect.Height / 2) - 115);
                sb.DrawString(backFont, score.ToString(), posScore, Color.White);
                sb.DrawString(sprFont, score.ToString(), posScore, Color.Black);

                Vector2 posHighscore = new Vector2(posScore.X, posScore.Y + 140);
                sb.DrawString(backFont, highscore.ToString(), posHighscore, Color.White);
                sb.DrawString(sprFont, highscore.ToString(), posHighscore, Color.Black);
            }
            sb.End();

            base.Draw(gameTime);
        }

        public void RestartGame()
        {
            Initialize();
            LoadContent();

            stopwatch.Restart();
        }

        bool AreKeysDown(KeyboardState kState, params Keys[] keys)
        {
            foreach (Keys k in keys)
                if (kState.IsKeyDown(k))
                    return true;

            return false;
        }
    }
}