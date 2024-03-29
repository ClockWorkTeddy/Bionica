﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    class Herbivore : MobileCreature
    {
        private static Point max_age_range = new Point(250, 300);
        private static int saturation = 100;
        public static int SizeDef { get; } = 2;
        public Herbivore(Point location) : base (location, max_age_range, saturation)
        {
            Size = SizeDef;
            Code = 2;
            Speed = 1;
        }
    }
}
