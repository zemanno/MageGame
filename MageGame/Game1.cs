#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using SpriteClasses;
#endregion
public enum MagicType {
    fireball, waterball, poisonball, lightball, shadowball, toxicball, rockball
}
public enum GameState {
    Start, Play, GameOver, Lose
}
namespace MageGame {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Sprite background, startScreen, endScreen, loseScreen, Mage, Mage2;
        List<Sprite> magic, ShadowBat, ShadowWalker;
        SoundEffect startSong, backSong;
        SoundEffectInstance BackSong;
        Random random;
        Vector2 textRight;
        MouseState prevMouseState, prevClick;
        KeyboardState prevKeyState;
        SpriteFont arial;
        GameState gameState = GameState.Start;
        float timeSince, timeSince2, timeSince3;
        const float SPAWN_TIME = 3f, SPAWN_TIME2 = 6f, TIME_BETWEEN = 0.03f;
        public const int FLOOR = 443;
        const int SCORE = 1;
        int totalScore, missed;
        MagicType magicType = MagicType.fireball;
        Vector2 gravityForce = new Vector2(0.0f, 200.0f);

        public Game1() : base() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 612;
            graphics.PreferredBackBufferHeight = 483;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            missed = 0;
            totalScore = 0;
            timeSince = 0;
            timeSince2 = 0;
            timeSince3 = 0;
            random = new Random();
            magic = new List<Sprite>();
            ShadowBat = new List<Sprite>();
            ShadowWalker = new List<Sprite>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = new Sprite(Content.Load<Texture2D>("images/background"), new Vector2(0, 0), Vector2.Zero, false, 0.0f, 1f, SpriteEffects.None, null);
            startScreen = new Sprite(Content.Load<Texture2D>("images/StartScreen"), new Vector2(0, 0), Vector2.Zero, false, 0.0f, 1f, SpriteEffects.None, null);
            endScreen = new Sprite(Content.Load<Texture2D>("images/WinScreen"), new Vector2(0, 0), Vector2.Zero, false, 0.0f, 1f, SpriteEffects.None, null);
            loseScreen = new Sprite(Content.Load<Texture2D>("images/EndScreen"), new Vector2(0, 0), Vector2.Zero, false, 0.0f, 1f, SpriteEffects.None, null);
            Mage = new Mage(Content, "images/MageRed", new Vector2(250, FLOOR - 31));
            Mage2 = new Player2(Content, new Vector2(350, FLOOR - 31));
            ShadowBat.Add(new ShadowBat(Content, random, GraphicsDevice));

            backSong = Content.Load<SoundEffect>("Audio/HeroicAge");
            BackSong = backSong.CreateInstance();
            BackSong.IsLooped = true;
            BackSong.Play();

            arial = Content.Load<SpriteFont>("Fonts/Arial");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() { }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            switch (gameState) {
                case GameState.Start:
                    updateSplash();
                    break;
                case GameState.GameOver:
                    updateSplash();
                    break;
                case GameState.Lose:
                    updateSplash();
                    break;
                case GameState.Play:
                    Mage.Update(gameTime, GraphicsDevice);
                    Mage2.Update(gameTime, GraphicsDevice);
                    foreach (Sprite enemy in ShadowBat) {
                        for (int j = 0; j < magic.Count; j++) {
                            if (enemy.CollisionSprite(magic[j])) {
                                enemy.Alive = false;
                                magic.RemoveAt(j);
                                totalScore += SCORE;
                                if (totalScore >= 100) gameState = GameState.GameOver;
                            }
                        }
                    }
                    foreach (Sprite enemy in ShadowWalker) {
                        if (enemy.CollisionSprite(Mage)) {
                            enemy.Alive = false;
                            Mage.Alive = false;
                            gameState = GameState.Lose;
                        }
                        for (int j = 0; j < magic.Count; j++) {
                            if (enemy.CollisionSprite(magic[j])) {
                                enemy.Alive = false;
                                magic.RemoveAt(j);
                                totalScore += SCORE;
                                if (totalScore >= 100) gameState = GameState.GameOver;
                            }
                        }
                    }
                    for (int i = 0; i < ShadowBat.Count; i++) {
                        if (ShadowBat[i].Velocity.X > 0) ShadowBat[i].SpriteEffect = SpriteEffects.FlipHorizontally;
                        else ShadowBat[i].SpriteEffect = SpriteEffects.None;
                        ShadowBat[i].Update(gameTime, GraphicsDevice);
                    }
                    float elapsed = (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                    timeSince += elapsed;
                    if (timeSince >= SPAWN_TIME) {
                        ShadowBat.Add(new ShadowBat(Content, random, GraphicsDevice));
                        timeSince = 0;
                    }
                    for (int i = 0; i < ShadowWalker.Count; i++) {
                        if (ShadowWalker[i].Velocity.X > 0) ShadowWalker[i].SpriteEffect = SpriteEffects.None;
                        else ShadowWalker[i].SpriteEffect = SpriteEffects.FlipHorizontally;
                        ShadowWalker[i].Update(gameTime, GraphicsDevice);
                    }
                    timeSince2 += elapsed;
                    if (timeSince2 >= SPAWN_TIME2)
                    {
                        int temp = random.Next(0, 2);
                        if (temp == 0) ShadowWalker.Add(new ShadowWalker(Content, GraphicsDevice, new Vector2(0, 422), new Vector2(50, 0)));
                        else if (temp == 1) ShadowWalker.Add(new ShadowWalker(Content, GraphicsDevice, new Vector2(GraphicsDevice.Viewport.Width, 422), new Vector2(-50, 0)));
                        timeSince2 = 0;
                    }
                    for (int i = 0; i < magic.Count; i++) {
                        magic[i].Update(gameTime);
                        float time = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                        if (magic[i].SpriteEffect == SpriteEffects.FlipVertically) {
                            //the force of gravity acting on the ball
                            magic[i].Force = gravityForce;
                            // sets the acceleration of the ball 
                            magic[i].acceleration = magic[i].Force / 1;
                            //sets the velocity of th ball
                            magic[i].Velocity = magic[i].InitialVelocity + magic[i].acceleration * time;
                            //changes the balls position
                            magic[i].Position += (magic[i].InitialVelocity * time) + (0.5f * magic[i].acceleration * time * time);
                            if (magic[i].Position.Y >= 438) {
                                //sets the y velocity of the ball to change direction each time the ball hits the above mentiones height
                                magic[i].Velocity = new Vector2(magic[i].InitialVelocity.X, magic[i].InitialVelocity.Y * -1);
                            }
                            magic[i].InitialVelocity = magic[i].Velocity;
                        }
                        if (magic[i].SpriteEffect == SpriteEffects.FlipHorizontally) {
                            for (int j = 0; j < ShadowBat.Count; j++) {
                                if ((ShadowBat[j].Position.X - magic[i].Position.X) <= 90f && (ShadowBat[j].Position.X - magic[i].Position.X) >= -90 && ShadowBat[j].Velocity.X > 0f)
                                {
                                    if ((magic[i].Position.Y - ShadowBat[j].Position.Y) <= 90f && (magic[i].Position.Y - ShadowBat[j].Position.Y) >= 0)
                                    ShadowBat[j].Velocity = new Vector2(-200, 0);
                                    
                                }
                                else if ((ShadowBat[j].Position.X - magic[i].Position.X) <= 90f && (ShadowBat[j].Position.X - magic[i].Position.X) >= -90 && ShadowBat[j].Velocity.X < 0f)
                                {
                                    if ((magic[i].Position.Y - ShadowBat[j].Position.Y) <= 90f && (magic[i].Position.Y - ShadowBat[j].Position.Y) >= 0)
                                        ShadowBat[j].Velocity = new Vector2(200, 0);
                                }
                            }
                        }
                        if (magic[i].isOffScreen(magic[i], GraphicsDevice)) {
                            missed += 1;
                            magic.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < ShadowBat.Count; i++) 
                        if (ShadowBat[i].Alive == false)ShadowBat.RemoveAt(i);
                    for (int i = 0; i < ShadowWalker.Count; i++)
                        if (ShadowWalker[i].Alive == false)
                            ShadowWalker.RemoveAt(i);
                    UpdateInput(elapsed);
                break;
                base.Update(gameTime);
            }
        }
        private void UpdateInput(float elapsed)
        {
            bool keyPressed = false;
            KeyboardState keyState = Keyboard.GetState();
            MouseState currMouseState = Mouse.GetState();
            MouseState currClick = Mouse.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (keyState.IsKeyDown(Keys.D1)) {
                magicType = MagicType.fireball;
                Mage.Image = Content.Load<Texture2D>("Images/MageRed");
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.D2)) {
                magicType = MagicType.waterball;
                Mage.Image = Content.Load<Texture2D>("Images/MageBlue");
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.D3)) {
                magicType = MagicType.poisonball;
                Mage.Image = Content.Load<Texture2D>("Images/MageGreen");
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.D4)) {
                magicType = MagicType.lightball;
                Mage.Image = Content.Load<Texture2D>("Images/MageWhite");
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.D5)) {
                magicType = MagicType.shadowball;
                Mage.Image = Content.Load<Texture2D>("Images/MageBlack");
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.D6)) {
                magicType = MagicType.toxicball;
                Mage.Image = Content.Load<Texture2D>("Images/MagePurple");
                keyPressed = true;
            }
            //checks to see if any key or button relating to left is pressed
            if (keyState.IsKeyDown(Keys.A) || gamePadState.DPad.Left == ButtonState.Pressed || gamePadState.ThumbSticks.Left.X < -0.5f) {
                Mage.Left();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Left)) {
                Mage2.Left();
                keyPressed = true;
            }
            //checks to see if any key or button relating to right is pressed
            if (keyState.IsKeyDown(Keys.D) || gamePadState.DPad.Right == ButtonState.Pressed || gamePadState.ThumbSticks.Left.X > 0.5f)
            {
                Mage.Right();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                Mage2.Right();
                keyPressed = true;
            }
            if (currClick.LeftButton == ButtonState.Pressed && prevClick.LeftButton != ButtonState.Pressed)
                attack(magicType, elapsed);
            if (keyState.IsKeyDown(Keys.Up) && keyState != prevKeyState)
                attack(MagicType.rockball, elapsed);
            prevKeyState = keyState;
            prevMouseState = currMouseState;
            prevClick = currClick;
            //checks to see if no keys are pressed
            if (!keyPressed)
            {
                Mage2.Idle();
                Mage.Idle();
            }
        }
        private void updateSplash() {
            bool keyPressed = false;
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            //checks to see if any key or button relating to up is pressed
            if (gameState == GameState.Start) {
                if (keyState.IsKeyDown(Keys.Enter) || gamePadState.Buttons.Start == ButtonState.Pressed) {
                    gameState = GameState.Play;
                    keyPressed = true;
                }
            }
            if (gameState == GameState.GameOver) {
                if (keyState.IsKeyDown(Keys.Enter) || gamePadState.Buttons.Start == ButtonState.Pressed) {
                    gameState = GameState.Play;
                    BackSong.Stop();
                    Initialize();
                    keyPressed = true;
                }
            }
            if (gameState == GameState.Lose) {
                if (keyState.IsKeyDown(Keys.Enter) || gamePadState.Buttons.Start == ButtonState.Pressed) {
                    gameState = GameState.Play;
                    BackSong.Stop();
                    Initialize();
                    keyPressed = true;
                }
            }
            if (keyState.IsKeyDown(Keys.Escape)) {
                Exit();
                keyPressed = true;
            }           
        }
        protected void attack(MagicType type, float elapsed) {
            if (type == MagicType.fireball) {
                //finds the position and quadrant 
                Vector2 position = new Vector2(prevMouseState.X - Mage.Position.X, prevMouseState.Y - Mage.Position.Y);
                //normalizes the vector
                float length = (float)Math.Sqrt(Math.Pow((prevMouseState.X - Mage.Position.X), 2) + Math.Pow((prevMouseState.Y - Mage.Position.Y), 2));
                //finds the directon of the vector
                Vector2 direction = position / length;
                magic.Add(new MagicAttack(Content, "images/Fireball", Mage.Position, new Vector2 (direction.X * 230f, direction.Y * 230f), new Vector2(13, 15), new Vector2(1, 1), 0.0f, SpriteEffects.None));
            }
            if (type == MagicType.waterball) {
                //finds the position and quadrant 
                Vector2 position = new Vector2(prevMouseState.X - Mage.Position.X, prevMouseState.Y - Mage.Position.Y);
                //normalizes the vector
                float length = (float)Math.Sqrt(Math.Pow((prevMouseState.X - Mage.Position.X), 2) + Math.Pow((prevMouseState.Y - Mage.Position.Y), 2));
                //finds the directon of the vector
                Vector2 direction = position / length;
                magic.Add(new MagicAttack(Content, "images/Waterball", Mage.Position, new Vector2(direction.X * 200f, direction.Y * 200f), new Vector2(15, 15), new Vector2(1, 1), 1.0f, SpriteEffects.None));
            }
            if (type == MagicType.poisonball) {
                //finds the position and quadrant 
                Vector2 position = new Vector2(prevMouseState.X - Mage.Position.X, prevMouseState.Y - Mage.Position.Y);
                //normalizes the vector
                float length = (float)Math.Sqrt(Math.Pow((prevMouseState.X - Mage.Position.X), 2) + Math.Pow((prevMouseState.Y - Mage.Position.Y), 2));
                //finds the directon of the vector
                Vector2 direction = position / length;
                magic.Add(new MagicAttack(Content, "images/PoisonBall", Mage.Position, new Vector2(direction.X * 150f, direction.Y * 150f), new Vector2(15, 16), new Vector2(1, 1), 1.5f, SpriteEffects.None));
            }
            if (type == MagicType.lightball) {
                //finds the position and quadrant 
                Vector2 position = new Vector2(prevMouseState.X - Mage.Position.X, prevMouseState.Y - Mage.Position.Y);
                //normalizes the vector
                float length = (float)Math.Sqrt(Math.Pow((prevMouseState.X - Mage.Position.X), 2) + Math.Pow((prevMouseState.Y - Mage.Position.Y), 2));
                //finds the directon of the vector
                Vector2 direction = position / length;
                magic.Add(new MagicAttack(Content, "images/LightBall", Mage.Position, new Vector2(direction.X * 200f, direction.Y * 200f), new Vector2(32, 32), new Vector2(1, 1), 3.0f, SpriteEffects.FlipHorizontally));
            }
            if (type == MagicType.shadowball) {
                //finds the position and quadrant 
                Vector2 position = new Vector2(prevMouseState.X - Mage.Position.X, prevMouseState.Y - Mage.Position.Y);
                //normalizes the vector
                float length = (float)Math.Sqrt(Math.Pow((prevMouseState.X - Mage.Position.X), 2) + Math.Pow((prevMouseState.Y - Mage.Position.Y), 2));
                //finds the directon of the vector
                Vector2 direction = position / length;
                magic.Add(new MagicAttack(Content, "images/ShadowBall", Mage.Position, new Vector2(direction.X * 190f, direction.Y * 190f), new Vector2(32, 32), new Vector2(1, 1), -3.0f, SpriteEffects.None));
            }
            if (type == MagicType.toxicball) {
                //finds the position and quadrant 
                Vector2 position = new Vector2(prevMouseState.X - Mage.Position.X, prevMouseState.Y - Mage.Position.Y);
                //normalizes the vector
                float length = (float)Math.Sqrt(Math.Pow((prevMouseState.X - Mage.Position.X), 2) + Math.Pow((prevMouseState.Y - Mage.Position.Y), 2));
                //finds the directon of the vector
                Vector2 direction = position / length;
                magic.Add(new MagicAttack(Content, "images/ToxicBall", Mage.Position, new Vector2(direction.X * 180f, direction.Y * 180f), new Vector2(20, 20), new Vector2(1, 1), -2.0f, SpriteEffects.FlipVertically));
            }
            if (type == MagicType.rockball) {
                timeSince3 += elapsed;
                if (timeSince3 >= TIME_BETWEEN) {
                    magic.Add(new MagicAttack(Content, "images/RockBall", Mage2.Position, new Vector2(0, -180f), new Vector2(20, 20), new Vector2(1, 1), -4.0f, SpriteEffects.None));
                    timeSince3 = 0;
                }
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            switch (gameState) {
                case GameState.Start:
                    spriteBatch.Begin();
                    startScreen.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.GameOver:
                    spriteBatch.Begin();
                    BackSong.Stop();
                    endScreen.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.Lose:
                    spriteBatch.Begin();
                    BackSong.Stop();
                    loseScreen.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.Play:
                    spriteBatch.Begin();
                    background.Draw(spriteBatch);
                    Mage.Draw(spriteBatch);
                    Mage2.Draw(spriteBatch);
                    for (int i = 0; i < magic.Count; i++)        magic[i].Draw(spriteBatch);
                    for (int i = 0; i < ShadowBat.Count; i++)    ShadowBat[i].Draw(spriteBatch);
                    for (int i = 0; i < ShadowWalker.Count; i++) ShadowWalker[i].Draw(spriteBatch);
                    string pointString = totalScore.ToString();
                    pointString = "" + totalScore;
                    pointString = "Enemies Destroyed: " + pointString;
                    spriteBatch.DrawString(arial, pointString, textRight, Color.White);
                    string shotsString = missed.ToString();
                    shotsString = "" + missed;
                    shotsString = "Shots Missed: " + missed;
                    spriteBatch.DrawString(arial, shotsString, new Vector2(0, 18), Color.White);
                    spriteBatch.End();
                    break;
            }
            base.Draw(gameTime);
        }
    }
}