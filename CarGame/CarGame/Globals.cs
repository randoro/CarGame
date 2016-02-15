using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarGame
{
    public static class Globals
    {
        public const int lanes = 8;
        public static Random rand = new Random();
        public const float spawnsPerSecond = 4f;
        public static float minSpeed = 5;
        public static float maxSpeed = 10; //plus one
    }
}
