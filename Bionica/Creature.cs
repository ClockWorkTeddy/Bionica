using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    abstract class Creature
    {
        public Point Location { get; set; }
        public Point PreviousLocation { get; set; }
        public int Size { get; set; }
        public int Speed { get; set; }
        public int Code { get; set; }
        public int Age { get; set; }
        public Creature (Point location)
        {
            Location = location;
            PreviousLocation = new Point(0, 0);
            Age = 0;
        }

        public void Move(int block_x, int block_y)
        {
            PreviousLocation = Location;
            Location = new Point(Location.X + GetLocation(block_x), Location.Y + GetLocation(block_y));
        }

        private int GetLocation(int block)
        {
            Random rnd = new Random();
            int random = rnd.Next(100);

            int result = (random < 50 ? -Speed : Speed) + block;

            return result;
        }
    }
}
