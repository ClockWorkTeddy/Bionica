using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    class Plant : Creature
    {           
        private static Point max_age_range = new Point(20, 30);
        public Plant(Point location) : base (location, max_age_range)
        {
            Size = 1;
            Speed = 0;
            Code = 1;
        }
    }
}
