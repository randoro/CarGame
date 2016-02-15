using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarGame
{

    public enum FSMStateEnum { none, idleBestState, checkPathState, moveState }

    class FSMachine
    {
        List<LinkedList<FallingCar>> queueList;
        List<FSMState> states;
        public FSMState currentState = null;
        FSMState defaultState = null;
        FSMState goalState = null;
        FSMStateEnum goalStateID = FSMStateEnum.none;

        public FSMachine(List<LinkedList<FallingCar>> queueList)
        {
            this.queueList = queueList;
            states = new List<FSMState>();
            //currentState = FSMState.idleBestState;
        }


        public void UpdateMachine(GameTime gameTime)
        {
            //don't do anything if you have no states
            if (states.Count == 0)
                return;

            //don't do anything if there's no current 
            //state, and no default state
            if (currentState == null)
                currentState = defaultState;
            if (currentState == null)
                return;

            //check for transitions, and then update
            FSMStateEnum oldStateID = currentState.GetID();
            goalStateID = currentState.CheckTransitions();

            //switch if there was a transition
            if (goalStateID != oldStateID)
            {
                if (TransitionState(goalStateID))
                {
                    currentState.Exit();
                    currentState = goalState;
                    currentState.Enter();
                }
            }
            currentState.Update(gameTime);	
        }

        public void AddState(FSMState newState)
        {
            states.Add(newState);
        }

        public void SetDefaultState(FSMState state) 
        {
            defaultState = state; 
        }

        void SetGoalID(FSMStateEnum goal) 
        {
            goalStateID = goal; 
        }

        bool TransitionState(FSMStateEnum goal)
        {
            //don't do anything if you have no states
            if (states.Count == 0)
                return false;

            //determine if we have state of type 'goal'
            //in the list, and switch to it, otherwise, quit out
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].GetID() == goal)
                {
                    goalState = states[i];
                    return true;
                }
            }
            return false;
        }


        public void Reset()
        {
            if (currentState != null)
                currentState.Exit();
            currentState = defaultState;

            //init all the states
            for (int i = 0; i < states.Count; i++)
                states[i].Init();

            //and now enter the m_defaultState, if any
            if (currentState != null)
                currentState.Enter();

        }


    }
}
