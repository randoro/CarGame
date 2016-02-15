using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarGame
{
    class FSMAIControl
    {
        FSMachine fsm;
        List<LinkedList<FallingCar>> queueList;
        public int bestlane = 0;
        public bool isAtbestlane = true;
        public bool canMove = false;
        public int moveDirection = 0;

        public FSMAIControl(List<LinkedList<FallingCar>> queueList)
        {
            fsm = new FSMachine(queueList);
            this.queueList = queueList;
            IdleState idle = new IdleState(this);
            fsm.AddState(idle);
            fsm.AddState(new CheckPathState(this));
            fsm.AddState(new MoveState(this));
            fsm.SetDefaultState(idle);
            fsm.Reset();

        }


        public void Update(GameTime gameTime)
        {

            bestlane = FindBestLane();
            canMove = CanMoveToBestLane();
            moveDirection = MoveDirection();

            fsm.UpdateMachine(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.font, "BestLane: "+bestlane.ToString(), new Vector2(30, 30), Color.Black);
            spriteBatch.DrawString(Game1.font, "CurrentState: " + fsm.currentState.GetID().ToString(), new Vector2(30, 50), Color.Black);
            
        }

        public int FindBestLane()
        {
            int longestTTLID = 0;
            float longestTTL = 0;
            List<int> emptyLanes = new List<int>();
            for (int i = 0; i < Globals.lanes; i++)
			{
                if (queueList[i].Count == 0)
                {
                    emptyLanes.Add(i);
                    //if (i > almostHighestEmpty && i < 1 + Globals.lanes / 2)
                    //{
                    //    almostHighestEmpty = i;
                    //}
                }
                else
                {
                    float newTTL = queueList[i].First.Value.TTL;
                    if (longestTTL < newTTL)
                    {
                        longestTTL = newTTL;
                        longestTTLID = i;
                    }
                }
                

			}
            if (emptyLanes.Count != 0)
            {
                return emptyLanes.Aggregate((x, y) => Math.Abs(x - Game1.controlCar.lane) < Math.Abs(y - Game1.controlCar.lane) ? x : y);
            }

            return longestTTLID;
        }


        public bool CanMoveToBestLane()
        {
            int currentLane = Game1.controlCar.lane;
            if (bestlane == currentLane)
            {
                isAtbestlane = true;
                return false;
            }
            isAtbestlane = false;

            if (bestlane > currentLane)
            {
                for (int i = 0; i < bestlane - currentLane; i++)
                {
                    float leftCarLength = 110;
                    float leftTTL = 1000; //Single.MaxValue;
                    if (queueList[currentLane + i].Count != 0)
                    {
                        leftTTL = queueList[currentLane + i].First.Value.TTL;
                        leftCarLength = queueList[currentLane + i].First.Value.getTTLCarLength();
                    }

                    float rightCarLength = 110;
                    float rightTTL = 1000;//Single.MaxValue;
                    if (queueList[currentLane + i + 1].Count != 0)
                    {
                        rightTTL = queueList[currentLane + i + 1].First.Value.TTL;
                        rightCarLength = queueList[currentLane + i + 1].First.Value.getTTLCarLength();
                    }
                    if (Math.Abs(leftTTL - rightTTL) < 110)
                    {
                        if (leftTTL < leftCarLength || rightTTL < rightCarLength)
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < currentLane - bestlane; i++)
                {
                    float leftCarLength = 110;
                    float leftTTL = 1000; // Single.MaxValue;
                    if (queueList[currentLane - i - 1].Count != 0)
                    {
                        leftTTL = queueList[currentLane - i - 1].First.Value.TTL;
                        leftCarLength = queueList[currentLane - i - 1].First.Value.getTTLCarLength();
                    }

                    float rightCarLength = 110;
                    float rightTTL = 1000; // Single.MaxValue;
                    if (queueList[currentLane - i].Count != 0)
                    {
                        rightTTL = queueList[currentLane - i].First.Value.TTL;
                        rightCarLength = queueList[currentLane - i].First.Value.getTTLCarLength();
                    }

                    if (Math.Abs(leftTTL - rightTTL) < 110)
                    {
                        if (leftTTL < leftCarLength || rightTTL < rightCarLength)
                        {
                            return false;
                        }
                    }

                }
            }

            return true;
        }


        public int MoveDirection()
        {
            int currentLane = Game1.controlCar.lane;
            if (bestlane == currentLane)
            {
                return 0;
            }

            if (bestlane > currentLane)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }

        public bool LaneEmpty(int lane)
        {
            if (queueList[lane].Count == 0)
            {
                return true;
            }
            return false;
        }

        public bool CanMoveNearBy(int direction)
        {
            int currentLane = Game1.controlCar.lane;

            if (queueList[currentLane + direction].Count == 0)
            {
                return true;
            }
            if ((Game1.controlCar.position.Y - queueList[currentLane + direction].First.Value.position.Y) < 115 )
            {
                return false;
            }

            return true;
        }

    }
}
