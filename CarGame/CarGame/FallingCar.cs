using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarGame
{
    class FallingCar : Car
    {
        protected float fallingVelocity;
        public float TTL;
        Color carColor = Color.White;
        bool crashed = false;

        public FallingCar(int lane, float fallingVelocity)
        {
            this.lane = lane;
            this.fallingVelocity = fallingVelocity;
            this.position = new Vector2(78*lane, 50);
            sourceRect = new Rectangle((int)position.X, (int)position.Y, 60, 100);
        }

        public override void Update(GameTime gameTime)
        {
            position.Y += fallingVelocity;
            sourceRect.X = (int)position.X;
            sourceRect.Y = (int)position.Y;
            TTL = getTTLAfterCar();

            if (sourceRect.Intersects(Game1.controlCar.sourceRect))
            {
                if (!crashed)
                {
                    carColor = Color.Red;
                    crashed = true;
                    Game1.crashes++;
                }
            }

        }

        public float getTTLAfterCar()
        {
            return ((Game1.controlCar.position.Y + 110) - position.Y) / fallingVelocity;
        }

        public float getTTLBeforeCar()
        {
            return ((Game1.controlCar.position.Y) - position.Y) / fallingVelocity;
        }

        public float getTTLCarLength()
        {
            return (110.0f / fallingVelocity);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.cars, position, new Rectangle(78, 0, 60, 110), carColor);
            spriteBatch.DrawString(Game1.font, TTL.ToString(), position, Color.Black);
        }

    }
}
