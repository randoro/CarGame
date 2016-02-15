using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarGame
{
    class MoveState : FSMState
    {
        FSMAIControl control;

        public MoveState(FSMAIControl control) 
        {
            this.control = control;
        }

        public override void Update(GameTime GameTime) 
        {
            if (control.CanMoveNearBy(control.moveDirection))
            {
                if (control.moveDirection == 1)
                {
                    Game1.controlCar.MoveRight();
                }
                else
                {
                    Game1.controlCar.MoveLeft();
                }
            }
            
        }

        public override void Init() 
        {

        }

        public override FSMStateEnum CheckTransitions()
        {
            if (!control.isAtbestlane)
            {
                return FSMStateEnum.checkPathState;
            }
            return FSMStateEnum.idleBestState;
        }

        public override FSMStateEnum GetID()
        {
            return FSMStateEnum.moveState;
        }
    }
}
