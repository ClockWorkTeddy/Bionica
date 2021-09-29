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
        public delegate void Death(Creature creature);
        public event Death RemoveCreature;
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

        public void Move(List<Point> free_points)
        {
            Ageing();
            PreviousLocation = Location;
            Point step = GetStep(free_points);
            Location = new Point(Location.X + step.X * Speed, Location.Y + step.Y * Speed);
        }

        private Point GetStep(List<Point> free_points)
        {
            Random rnd = new Random();
            int number = rnd.Next(free_points.Count);
            
            return free_points[number];
        }

        private void Ageing()
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
