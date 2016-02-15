using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarGame
{
    class IdleState : FSMState
    {
        FSMAIControl control;

        public IdleState(FSMAIControl control) 
        {
            this.control = control;
        }

        

        public override void Update(GameTime GameTime) 
        {




        }

        public override FSMStateEnum CheckTransitions()
        {
            if (!(control.LaneEmpty(Game1.controlCar.lane)) && !(Game1.controlCar.lane == control.bestlane))
            {
                return FSMStateEnum.checkPathState;
            }
            return FSMStateEnum.idleBestState;
        }

        public override FSMStateEnum GetID()
        {
            return FSMStateEnum.idleBestState;
        }

    }
}
