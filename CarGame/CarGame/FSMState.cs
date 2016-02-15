using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarGame
{
    public abstract class FSMState
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public abstract void Update(GameTime GameTime);
        public virtual void Init() { }
        public abstract FSMStateEnum CheckTransitions();
        public abstract FSMStateEnum GetID();

    }
}
