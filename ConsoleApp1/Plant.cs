using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Plant: Entity
    {
        protected Item seed;
        protected Item product;

        public bool harvestable;

        public Plant(Item seed, Item product, int lifeSpan, string name) : base(lifeSpan, name)
        {
            this.seed = seed;
            this.product = product;

            harvestable = false;
        }

        protected override void Expire()
        {
            harvestable = true;
        }

        public void Harvest()
        {
            Farm.instance.RemovePlant(this);
        }

        public static void HarvestBunch(List<Plant> plants, string name)
        {
            List<Plant> harvestablePlants = plants.FindAll(plants => plants.harvestable && plants.name == name);
            
            Primes primes = new Primes();

            int quantity = harvestablePlants.Count;

            if (quantity <= 0) return;

            Plant exemplar = harvestablePlants.First();

            Console.WriteLine($"How many {name} you want to replant (max: {quantity})");

            bool continueFlag;
            int replantQuantity;

            do
            {
                int.TryParse(Console.ReadLine(), out replantQuantity);

                continueFlag = replantQuantity >= 0 && replantQuantity <= quantity;

                if(!continueFlag)
                {
                    Console.WriteLine("Insert a valid quantity:");
                }
            } while (!continueFlag);

            int replants = 0;

            bool haltReplant = false;

            do
            {
                Plant crop = exemplar.Clone();

                haltReplant = !Farm.instance.TryAddPlant(crop);

                if(!haltReplant) replants++;
            } while (!haltReplant && replants <= replantQuantity);

            if(replantQuantity - replants > 0)
            {
                int seedSelledValue = exemplar.seed.value * (replantQuantity - replants);
                Farm.instance.AddMoney(seedSelledValue);
                Console.WriteLine($"Space limit reached rest {replantQuantity - replants} {name}'s seeds where selled by {seedSelledValue}");
            }

            int products = primes.Prime(quantity - replantQuantity);

            if (products <= 0) return;

            int value = products * exemplar.product.value;

            Farm.instance.AddMoney(value);
            Console.WriteLine($"{products} harvested, returning {value} coins");

            harvestablePlants.ForEach(plant => plant.Harvest());
        }

        public override Plant Clone()
        {
            return new Plant(seed, product, lifeSpan, name);
        }
    }
}
