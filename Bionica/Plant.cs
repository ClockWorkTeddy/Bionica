using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    class Plant : Creature
    {
        public Plant(Point location) : base (location)
        {
            Size = 1;
            Speed = 0;
            Code = 1;
        }
    }
}
