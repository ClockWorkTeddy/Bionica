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
        public List<Creature> Plants { get; set; } = new List<Creature>();
        public List<Creature> Herbivores { get; set; } = new List<Creature>();
        public int Size { get; set; }
        public Schema(int size)
        {
            Sch = new int[size, size];
            Creatures = new Dictionary<string, List<Creature>>();
            Creatures.Add("Plants", Plants);
            Creatures.Add("Herbivores", Herbivores);
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
            AddHerbivore();
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
            double fertility = rnd.Next(min_fert, max_fert) / (Size * 10.0);

            return fertility;
        }
        private void AddHerbivore()
        {
            Herbivore herb = new Herbivore(GetLocation(Herbivore.SizeDef));
            Herbivores.Add(herb);
            SetCode(herb.Location, herb.Size, herb.Code);
        }

        private void AddPlant()
        {
            Plant plant = new Plant(GetLocation(Plant.SizeDef));
            Plants.Add(plant);
            SetCode(plant.Location, plant.Size, plant.Code);
        }

        private void SetCode(Point location, int size, int code)
        {
            for (int i = location.X; i < location.X + size; i++)
                for (int j = location.Y; j < location.Y + size; j++)
                    Sch[j, i] = code;
        }

        private Point GetLocation(int creature_size)
        {
            Random rnd = new Random();

            int i;
            int j;
            do
            {
                i = rnd.Next(0, Size - creature_size);
                j = rnd.Next(0, Size - creature_size);
            }
            while (FreeSpace(i, j, creature_size));

            return new Point(i, j);
        }

        private bool FreeSpace(int i, int j, int size)
        {
            int summ = 0;

            for (int p = i; p < i + size; p++)
                for (int q = j; q < j + size; q++)
                    summ += Sch[j, i];

            return summ != 0;
        }

        public void Place()
        {
            foreach (List<Creature> list in Creatures.Values)
                foreach (Creature creature in list)
                {
                    SetCode(creature.PreviousLocation, creature.Size, 0);

                    if (creature.IsAlive)
                        SetCode(creature.Location, creature.Size, creature.Code);
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
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    result.Append(Sch[j, i] + ",");
                result.Append("\n");
            }

            return result.ToString();
        }
    }
}
