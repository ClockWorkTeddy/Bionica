using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    abstract class MobileCreature : Creature
    {
        public int Speed { get; set; }
        public int Saturation { get; set; }
        public Point PreviousLocation { get; set; }
        public delegate void Eating(MobileCreature mob_creature);
        public event Eating Eat;
        public delegate List<Point> Mover(MobileCreature mob_creature);
        public event Mover GetFreePoints;
        public delegate void Hunger(Creature creature);
        public event Hunger Starving;

        public MobileCreature(Point location, Point max_age_range, int saturation) : base (location, max_age_range)
        {
            PreviousLocation = location;
            Saturation = saturation;
        }

        public override void Move()
        {
            PreviousLocation = Location;
            List<Point> free_points = GetFreePoints?.Invoke(this);
            Point step = GetStep(free_points);
            Location = new Point(Location.X + step.X * Speed, Location.Y + step.Y * Speed);
        }

        private Point GetStep(List<Point> free_points)
        {
            Random rnd = new Random();
            int number = rnd.Next(free_points.Count);

            return free_points[number];
        }

        public override void Next()
        {
            Hungering();
            Ageing();
        }

        private void Hungering()
        {
            Saturation--;

            if (Saturation < 0)
                Starving?.Invoke(this);
        }

        public override void Eats()
        {
            Eat?.Invoke(this);
        }

    }
}
