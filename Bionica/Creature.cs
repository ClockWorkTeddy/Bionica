using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    class Creature
    {
        public Point Location { get; set; }
        public Point PreviousLocation { get; set; }
        public int Size { get; set; }
        public Creature (Point location)
        {
            this.Location = location;
            this.Size = 1;
        }

        public void Move(int block_x, int block_y)
        {
            this.PreviousLocation = this.Location;
            this.Location = new Point(this.Location.X + GetLocation(block_x), this.Location.Y + GetLocation(block_y));

        }

        private int GetLocation(int block)
        {
            Random rnd = new Random();
            int random = rnd.Next(100);

            int result = (random < 50 ? -1 : 1) + block;

            return result;
        }
    }
}
