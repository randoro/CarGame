using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace CarGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D cars;
        public static SpriteFont font;
        List<LinkedList<FallingCar>> queueList;
        Random rand = new Random();
        bool AIControlled = false;
        FSMAIControl AI;

        DateTime roundStart = DateTime.Now;
        DateTime now = DateTime.Now;
        public static int crashes = 0;
        int spawnCounter = 0;
        int spawnAt = (int)(60.0f / Globals.spawnsPerSecond);

        public static ControlCar controlCar;

        public Game1()
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
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;

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
            cars = Content.Load<Texture2D>("cars");
            font = Content.Load<SpriteFont>("font");
            queueList = new List<LinkedList<FallingCar>>();
            for (int i = 0; i < Globals.lanes; i++)
            {
                queueList.Add(new LinkedList<FallingCar>());
            }
            
            controlCar = new ControlCar(0);

            AI = new FSMAIControl(queueList);
            //testCar2 = new FallingCar(0, 0);
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
            KeyMouseReader.Update();

            now = DateTime.Now;
            // Allows the game to exit
            if (KeyMouseReader.KeyPressed(Keys.Escape))
                this.Exit();

            if (KeyMouseReader.KeyPressed(Keys.A) && !AIControlled)
                controlCar.MoveLeft();

            if (KeyMouseReader.KeyPressed(Keys.D) && !AIControlled)
                controlCar.MoveRight();

            if (KeyMouseReader.KeyPressed(Keys.Space))
            {
                AIControlled = !AIControlled;
                crashes = 0;
                roundStart = DateTime.Now;
            }

            if (AIControlled)
            {
                AI.Update(gameTime);
            }

            spawnCounter++;
            if (spawnCounter > spawnAt)
            {
                int tries = 0;
                while (tries < 1)
                {
                    if (SpawnCar())
                    {
                        break;
                    }
                    tries++;

                }
/*
                int randLane = rand.Next(0, Globals.lanes);
                FallingCar newCar = new FallingCar(randLane, (float)(rand.NextDouble() * 7) + 1f);
                float TTLPlusCar = newCar.getTTLBeforeCar();
                if (queueList[randLane].Count > 0)
                {
                    if (TTLPlusCar > queueList[randLane].Last.Value.TTL)
                    {
                        queueList[randLane].AddLast(newCar);
                        spawnCounter = 0;
                    }
                }
                else
                {
                    queueList[randLane].AddLast(newCar);
                    spawnCounter = 0;
                }*/

            }


            foreach(LinkedList<FallingCar> l in queueList) 
            {
                foreach (FallingCar f in l)
                {
                    f.Update(gameTime);
                }

                if (l.First != null && l.First.Value.TTL < 0)
                {
                    l.RemoveFirst();
                }

            }


            controlCar.Update(gameTime);
            //testCar2.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();


            foreach (LinkedList<FallingCar> l in queueList)
            {
                foreach (FallingCar f in l)
                {
                    f.Draw(spriteBatch);
                }
            }

            if (AIControlled)
            {
                AI.Draw(spriteBatch);
            }
            spriteBatch.DrawString(Game1.font, "Crashes: " + crashes, new Vector2(1000, 50), Color.Black);

            spriteBatch.DrawString(Game1.font, (now - roundStart).ToString(), new Vector2(1000, 100), Color.Black);

            controlCar.Draw(spriteBatch);
            //testCar2.Draw(spriteBatch);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


        public bool SpawnCar()
        {
            bool failed = false;

            int randLane = rand.Next(0, Globals.lanes);
            FallingCar newCar = new FallingCar(randLane, (float)(rand.NextDouble() * Globals.maxSpeed) + Globals.minSpeed);
            //float TTLPlusCar = newCar.getTTLBeforeCar();
            float TTLPlusCar = newCar.getTTLBeforeCar() - newCar.getTTLCarLength();
            if (queueList[randLane].Count > 0)
            {
                if (TTLPlusCar < queueList[randLane].Last.Value.TTL)
                {
                    failed = true;
                    return false;
                }
            }

            if (randLane != 0 && queueList[randLane - 1].Count > 0)
            {
                if (TTLPlusCar < queueList[randLane - 1].Last.Value.TTL)
                {
                    failed = true;
                    return false;
                }
            }

            if (randLane != Globals.lanes-1 && queueList[randLane + 1].Count > 0)
            {
                if (TTLPlusCar < queueList[randLane + 1].Last.Value.TTL)
                {
                    failed = true;
                    return false;
                }
            }




            //else
            //{
            //    queueList[randLane].AddLast(newCar);
            //    spawnCounter = 0;
            //}


            if (!failed)
            {
                queueList[randLane].AddLast(newCar);
                spawnCounter = 0;
                return true;
            }
            return false;
        }
    }
}
