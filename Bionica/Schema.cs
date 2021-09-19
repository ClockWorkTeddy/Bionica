using System;
using System.Collections.Generic;
using System.Text;

namespace Bionica
{
    class Schema
    {
        public int[,] Sch = null;
        public List<Creature> Creatures {get; set;}

        public int Size { get; set; }
        public Schema(int size)
        {
            Sch = new int[size, size];
            Creatures = new List<Creature>();
            Size = size;

        }
        private void Clear()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Sch[i, j] = 0;

            Creatures.Clear();
        }
        public void Start()
        {
            Clear();

            int quantity = 20;

            for (int i = 0; i < quantity; i++)
                AddCreature();
        }

        private void AddCreature()
        {
            int i = 0;
            int j = 0;

            Random rnd = new Random();

            do
            {
                i = rnd.Next(0, Size);
                j = rnd.Next(0, Size);
            }
            while (Sch[i, j] != 0);

            Creature creature = new Creature(new System.Drawing.Point(i, j));

            Creatures.Add(creature);

            Place();
        }

        public void Place()
        {
            foreach (Creature creature in Creatures)
            {
                Sch[creature.PreviousLocation.X, creature.PreviousLocation.Y] = 0;
                Sch[creature.Location.X, creature.Location.Y] = 1;
            }
        }

        public void Move()
        {
            foreach (Creature creature in Creatures)
                creature.Move();

            Place();
        }

    }
}
