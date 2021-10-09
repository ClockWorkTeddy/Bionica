using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    class Plant : Creature
    {           
        private static Point max_age_range = new Point(5000, 7500);
        public static int SizeDef { get; } = 1;
        public Plant(Point location) : base (location, max_age_range)
        {
            Size = SizeDef;
            Code = 1;
        }
    }
}
