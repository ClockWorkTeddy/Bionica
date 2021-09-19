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
        public Creature (Point location)
        {
            this.Location = location;
        }

        public void Move()
        {
            this.PreviousLocation = this.Location;
            this.Location = new Point(this.Location.X + GetLocation(), this.Location.Y + GetLocation());

        }

        private int GetLocation()
        {
            Random rnd = new Random();
            int random = rnd.Next(100);

            int result = random < 50 ? -1 : 1;

            return result;
        }
    }
}
