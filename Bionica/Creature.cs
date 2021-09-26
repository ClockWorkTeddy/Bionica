using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    abstract class Creature
    {
        private int max_age = 0;

        public Point Location { get; set; }
        public Point PreviousLocation { get; set; }
        public int Size { get; set; }
        public int Speed { get; set; }
        public int Code { get; set; }
        public int Age { get; set; }
        public bool IsAlive { get; set; } = true;

        public Creature (Point location, Point max_age_range)
        {
            Location = location;
            PreviousLocation = location;
            Age = 0;
            max_age = GetMaxAge(max_age_range);
        }

        private int GetMaxAge(Point max_age_range)
        {
            Random rnd = new Random();
            int max_age = rnd.Next(max_age_range.X, max_age_range.Y);

            return max_age;
        }

        public void Move(int block_x, int block_y)
        {
            PreviousLocation = Location;
            Location = new Point(Location.X + GetLocation(block_x), Location.Y + GetLocation(block_y));
            Ageing();
        }

        private int GetLocation(int block)
        {
            Random rnd = new Random();
            int random = rnd.Next(100);

            int result = (random < 50 ? -Speed : Speed) + block * Speed;

            return result;
        }

        private void Ageing()
        {
            Age++;

            if (Age >= max_age)
                IsAlive = false;
        }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(Location.ToString()))
                return base.ToString();
            else
                return Location.ToString();
        }
    }
}
