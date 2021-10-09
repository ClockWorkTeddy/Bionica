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
        public delegate void Breed(Point location);
        public event Breed Breeding;

        public MobileCreature(Point location, Point max_age_range, int saturation) : base (location, max_age_range)
        {
            PreviousLocation = location;
            Saturation = saturation;
        }

        public void Move()
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
            int qnt = 1;
            for (int i = 0; i < qnt; i++)
                Move();
            for (int i = 0; i < qnt; i++)
                Eats();
            for (int i = 0; i < qnt; i++)
                Hungering();
            for (int i = 0; i < qnt; i++)
                Ageing();

            if (this.Saturation > 100)
            {
                for (int i = 0; i < qnt; i++)
                    Breeds();
                this.Saturation -= 50;
            }
        }

        private void Hungering()
        {
            Saturation--;

            if (Saturation < 0)
                Starving?.Invoke(this);
        }

        public void Eats()
        {
            Eat?.Invoke(this);
        }

        public void Breeds()
        {
            Breeding?.Invoke(this.PreviousLocation);
        }
    }
}
