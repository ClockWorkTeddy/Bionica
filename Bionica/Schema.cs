using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bionica
{
    class Schema
    {
        private int Square;
        public int[,] Sch = null;
        public int Epoche { get; set; }
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
            Square = Size * Size;
        }
        private void Clear()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Sch[i, j] = 0;

            foreach (List<Creature> list in Creatures.Values)
                list.Clear();

            Epoche = -1;
        }
        public void Start()
        {
            Clear();
            AddPlants();
            Place();
        }

        private void AddPlants()
        {
            double fertility = GetFertility();
            int plants_qnt = (int)((Square - Plants.Count) * fertility + 0.5);

            for (int i = 0; i < plants_qnt; i++)
                AddPlant();
        }

        private double GetFertility()
        {
            Random rnd = new Random();
            int min_fert = 1;
            int max_fert = 6;
            double fertility = rnd.Next(min_fert, max_fert) / 10000.0;

            return fertility;
        }

        private void AddPlant()
        {
            Plant plant = new Plant(GetLocation());
            Plants.Add(plant);
            Sch[plant.Location.X, plant.Location.Y] = plant.Code;
        }

        private Point GetLocation()
        {
            Random rnd = new Random();

            int i;
            int j;
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

                    if (creature.IsAlive)
                        Sch[creature.Location.X, creature.Location.Y] = creature.Code;
                }

            Epoche++;
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

            AddPlants();
            Place();
            Death();
        }

        private void Death()
        {
            string[] keys = new string[Creatures.Keys.Count];
            Creatures.Keys.CopyTo(keys, 0);

            for (int i = 0; i < Creatures.Values.Count; i++)
                Creatures[keys[i]].RemoveAll(x => !x.IsAlive);
        }

        public int GetBlock(int location, int size)
        {
            int block = 0;

            if (location == Size - size)
                block = -1;
            else if (location == size)
                block = 1;

            return block;
        }

        public override string ToString()
        {
            if (Sch == null)
                return base.ToString();
            else
                return GetSchString();

        }

        private string GetSchString()
        {
            string result = "";

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    result += Sch[j, i];
                result += "\n";
            }

            return result;
        }
    }
}
