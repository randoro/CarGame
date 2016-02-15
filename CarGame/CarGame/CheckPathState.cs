using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarGame
{
    class CheckPathState : FSMState
    {
        FSMAIControl control;

        public CheckPathState(FSMAIControl control) 
        {
            this.control = control;
        }

        public override void Update(GameTime GameTime) 
        {

        }


        public override FSMStateEnum CheckTransitions()
        {
            if (control.canMove)
            {
                return FSMStateEnum.moveState;
            }

            return FSMStateEnum.idleBestState;
        }

        public override FSMStateEnum GetID()
        {
            return FSMStateEnum.checkPathState;
        }
    }
}
