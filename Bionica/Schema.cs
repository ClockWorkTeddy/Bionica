﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;

namespace Bionica
{
    class Schema
    {
        private int Square;
        private ILookup<Point, Creature> PlantsLookUp = null;
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

        public void Revert()
        {
            Clear();
            Place();
        }

        public void Start()
        {
            Clear();

            AddPlants(0.2);

            int herb_qnt = 10;

            for (int i = 0; i < herb_qnt; i++)
            {
                Point location = GetLocation(Herbivore.SizeDef);
                AddHerbivore(location);
            }

            Place();
        }

        private void AddPlants(double fertility)
        {
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
        private void AddHerbivore(Point location)
        {
            Herbivore herb = new Herbivore(location);
            Herbivores.Add(herb);
            SetCode(herb.Location, herb.Size, herb.Code);
            herb.RemoveCreature += RemoveCreature;
            herb.GetFreePoints += GetFreeSpace;
            herb.Starving += RemoveCreature;
            herb.Eat += Eating;
            herb.Breeding += Breed;
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
            {
                Plants.Remove(creature);
            }
            else if (creature is Herbivore)
            {
                Herbivore herb = creature as Herbivore;
                Herbivores.Remove(creature);
                SetCode((creature as Herbivore).PreviousLocation, creature.Size, 0);
                herb.GetFreePoints -= GetFreeSpace;
                herb.Starving -= RemoveCreature;
                herb.Eat -= Eating;
                herb.Breeding -= Breed;
            }

            creature.RemoveCreature -= RemoveCreature;
        }

        private void SetCode(Point location, int size, int code)
        {
            for (int i = location.X; i < location.X + size; i++)
                for (int j = location.Y; j < location.Y + size; j++)
                    Sch[i, j] = code;
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

                if (creature_size > 1)
                {
                    i = ToSize(i, creature_size);
                    j = ToSize(j, creature_size);
                }
            }
            while (!CheckPlace(i, j, creature_size, 0));

            return new Point(i, j);
        }

        private int ToSize(int value, int size)
        {
            int remainder = value % size;

            if (remainder != 0)
                value += remainder;

            return value;
        }

        public void Place()
        {
            foreach (List<Creature> list in Creatures.Values)
                foreach (Creature creature in list)
                {
                    if (creature is MobileCreature)
                        SetCode((creature as MobileCreature).PreviousLocation, creature.Size, 0);
                    SetCode(creature.Location, creature.Size, creature.Code);
                }

            Epoche++;
        }

        public void Move()
        {
            PlantsLookUp = Plants.ToLookup(x => x.Location);

            foreach (List<Creature> list in Creatures.Values)
                for (int i = 0; i < list.Count; i++)
                    list[i].Next();

            double fertility = GetFertility();
            AddPlants(fertility);
            Place();
        }

        private void Eating(MobileCreature mob_creature)
        {
            int x = mob_creature.Location.X;
            int y = mob_creature.Location.Y;
            int size = mob_creature.Size;

            List<Creature> eaten = new List<Creature>(); ;

            for (int i = x; i < x + size; i++)
                for (int j = y; j < y + size; j++)
                    if (Sch[i, j] == 1)
                        eaten.AddRange(PlantsLookUp[new Point(i, j)]); //Location 527 - 121

            foreach (Plant eaten_plant in eaten)
            { 
                mob_creature.Saturation += 5;
                RemoveCreature(eaten_plant);
            }
        }

        private void Breed(Point location)
        {
            AddHerbivore(location);
        }
        private List<Point> GetFreeSpace(Creature creature)
        {
            List<Point> FreeSpace = new List<Point>();

            int x = creature.Location.X;
            int y = creature.Location.Y;
            int size = creature.Size;

            int min_i = MinValue(x - size);
            int max_i = MaxValue(x, size);
            int min_j = MinValue(y - size);
            int max_j = MaxValue(y, size);

            for (int i = min_i; i <= max_i; i += size)
                for (int j = min_j; j <= max_j; j += size)
                    if ( CheckPlace(i, j, size, 1) && !((x == i) && (y == j)) )
                        FreeSpace.Add(new Point(i - x, j - y));

            return FreeSpace;
        }

        private int MinValue(int value)
        {
            return value < 0 ? 0 : value;
        }

        private int MaxValue(int value, int size)
        {
            return value + size >= Size ? value : value + size;
        }

        private bool CheckPlace(int i, int j, int size, int min_code)
        {
            int result = 0;

            for (int p = i; p < i + size; p += size)
                for (int q = j; q < j + size; q += size)
                    if (Sch[p, q] > min_code)
                        result++;

            return result == 0;
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
                    result.Append(Sch[i, j] + ",");

                result.Append("\n");
            }

            return result.ToString();
        }
    }
}
