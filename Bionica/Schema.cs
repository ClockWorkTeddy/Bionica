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
            herb.RemoveCreature += RemoveCreature;
        }

        private void AddPlant()
        {
            Plant plant = new Plant(GetLocation(Plant.SizeDef));
            Plants.Add(plant);
            SetCode(plant.Location, plant.Size, plant.Code);
            plant.RemoveCreature += RemoveCreature;
        }

        private void RemoveCreature(Creature creature)
        {
            SetCode(creature.Location, creature.Size, 0);

            if (creature is Plant)
                Creatures["Plants"].Remove(creature);
            else if (creature is Herbivore)
                Creatures["Herbivores"].Remove(creature);
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
            while (!CheckPlace(i, j, creature_size, 0));

            return new Point(i, j);
        }

        private bool FreeSpace(int i, int j, int size)
        {
            int summ = 0;

            for (int p = i; p < i + size; p++)
                for (int q = j; q < j + size; q++)
                    summ += Sch[q, p];

            return summ != 0;
        }

        public void Place()
        {
            foreach (List<Creature> list in Creatures.Values)
                foreach (Creature creature in list)
                {
                    SetCode(creature.PreviousLocation, creature.Size, 0);
                    SetCode(creature.Location, creature.Size, creature.Code);
                }

            Epoche++;
        }

        public void Move()
        {
            foreach (List<Creature> list in Creatures.Values)
                for (int i = 0; i < list.Count; i++)
                {
                    int block_x = GetBlock(list[i].Location.X, list[i].Size);
                    int block_y = GetBlock(list[i].Location.Y, list[i].Size);

                    list[i].Move(block_x, block_y);
                }

            AddPlants();
            Place();
        }

        private List<Point> GetFreeSpace(Creature creature)
        {
            List<Point> FreeSpace = new List<Point>();

            int x = creature.Location.Y;
            int y = creature.Location.X;
            int size = creature.Size;

            for (int i = x - size; i < x + size; i += size)
                for (int j = y - size; y < j + size; i += size)
                    if (CheckPlace(i, j, size, 1))
                        FreeSpace.Add(new Point(i - x, j - y));

            return FreeSpace;
        }

        private bool CheckPlace(int i, int j, int size, int min_code)
        {
            int result = 0;

            for (int p = i; p < i + size; p++)
                for (int q = j; q < j + size; q++)
                    if (Sch[q, p] > min_code)
                        result++;

            return result == 0;
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
