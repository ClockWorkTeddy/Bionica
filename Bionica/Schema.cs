using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    class Schema
    {
        public int[,] Sch = null;
        public Dictionary<string, List<Creature>> Creatures {get; set;}
        public List<Creature> Plants { get; set; }
        public int Size { get; set; }
        public Schema(int size)
        {
            Sch = new int[size, size];
            Creatures = new Dictionary<string, List<Creature>>();
            Plants = new List<Creature>();
            Creatures.Add("Plants", Plants);
            
            Size = size;
        }
        private void Clear()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Sch[i, j] = 0;

            foreach (List<Creature> list in Creatures.Values)
                list.Clear();
        }
        public void Start()
        {
            Clear();

            int plants_qnt = 200;

            for (int i = 0; i < plants_qnt; i++)
                AddPlant();
        }


        private void AddPlant()
        {
            Plant plant = new Plant(GetLocation());
            Plants.Add(plant);
            Place();
        }

        private Point GetLocation()
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

            return new Point(i, j);
        }

        public void Place()
        {
            foreach (List<Creature> list in Creatures.Values)
                foreach (Creature creature in list)
                {
                    Sch[creature.PreviousLocation.X, creature.PreviousLocation.Y] = 0;
                    Sch[creature.Location.X, creature.Location.Y] = creature.Code;
                }
        }

        public void Move()
        {
            foreach (List<Creature> list in Creatures.Values)
                foreach (Creature creature in list)
            {
                int block_x = GetBlock(creature.Location.X, creature.Size);
                int block_y = GetBlock(creature.Location.Y, creature.Size);

                creature.Move(block_x, block_y);
            }

            Place();
        }

        public int GetBlock(int Location, int size)
        {
            int block = 0;

            if (Location >= Size - size)
                block = -1;
            else if (Location <= size)
                block = 1;

            return block;
        }

    }
}
