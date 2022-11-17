using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    class Herbivore : MobileCreature
    {
        private static Point max_age_range = new Point(250, 300);
        private static int saturation = 50;
        public static int SizeDef { get; } = 2;
        public Herbivore(Point location, int code) : base (location, max_age_range, saturation)
        {
            Size = SizeDef;
            Code = code;
            Speed = 1;
        }
    }
}
