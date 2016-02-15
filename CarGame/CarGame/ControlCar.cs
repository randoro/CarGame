using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarGame
{
    public class ControlCar : Car
    {
        public ControlCar(int lane)
        {
            this.lane = lane;
            this.position = new Vector2(78 * lane, 720 - 110);
            sourceRect = new Rectangle((int)position.X, (int)position.Y, 60, 110);
        }

        public override void Update(GameTime gameTime)
        {
            position.X = 78 * lane;
            position.Y = 720 - 110;
            sourceRect.X = (int)position.X;
            sourceRect.Y = (int)position.Y;

        }


        public void MoveRight()
        {
            if (lane != Globals.lanes - 1)
            {
                lane++;
            }
        }

        public void MoveLeft()
        {
            if (lane != 0)
            {
                lane--;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.cars, position, new Rectangle(0, 0, 60, 110), Color.White);
        }

    }
}
