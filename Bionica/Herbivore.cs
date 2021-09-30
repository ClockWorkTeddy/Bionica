using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    class Herbivore : Creature
    {
        private static Point max_age_range = new Point(500, 800);
        public static int SizeDef { get; } = 2;
        public Herbivore(Point location) : base (location, max_age_range)
        {
            Size = 2;
            Code = 2;
            Speed = 1;
        }
    }
}
