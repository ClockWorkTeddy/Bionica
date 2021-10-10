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
        public int Size { get; set; }
        public int Code { get; set; }
        public int Age { get; set; }
        public int Hash { get; set; }
        public bool Alive { get; set; }
        public delegate void CreatureHandler(Creature creature);
        public event CreatureHandler RemoveCreature;
        public Creature (Point location, Point max_age_range)
        {
            Location = location;
            Age = 0;
            max_age = GetMaxAge(max_age_range);
            Hash = Location.X * 1000 + Location.Y;

            Alive = true;
        }

        private int GetMaxAge(Point max_age_range)
        {
            Random rnd = new Random();
            int max_age = rnd.Next(max_age_range.X, max_age_range.Y);

            return max_age;
        }

        public virtual void Next()
        {
            Ageing();
        }
        protected void Ageing()
        {
            Age++;

            if (Age > max_age)
                RemoveCreature?.Invoke(this);
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
