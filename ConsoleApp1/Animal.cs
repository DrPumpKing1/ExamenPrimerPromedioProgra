using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Animal : Entity
    {
        protected Item livingProduct;
        protected Item deathProduct;

        public bool death;
        public int adulthood;
        public bool isAdult => life <= adulthood;

        public Animal(Item livingProduct, Item deathProduct, int lifeSpan, string name) : base(lifeSpan, name)
        {
            this.livingProduct = livingProduct;
            this.deathProduct = deathProduct;
            this.death = false;
            this.adulthood = lifeSpan/2;
        }

        public override void Step()
        {
            base.Step();

            if (isAdult && !death) AdultDo();
        }

        private void AdultDo()
        {
            Farm.instance.AddInventory(this, livingProduct);
        }

        public void Butchery()
        {
            Farm.instance.AddInventory(this, deathProduct);
        }

        public void DeathRemove()
        {
            Farm.instance.RemoveAnimal(this);
        }

        protected override void Expire()
        {
            death = true;
        }

        public static void ReproduceKind(List<Animal> animals, string name)
        {
            List<Animal> reproduceAnimals = animals.FindAll(animal => animal.isAdult && animal.name == name);

            Fibonacci fibo = new Fibonacci();

            if(reproduceAnimals.Count <= 0) return;

            Animal exemplar = reproduceAnimals.First();

            int moreAnimals = fibo.NextFibonacci(reproduceAnimals.Count);

            int reborn = 0;

            bool haltReborn = false;

            do
            {
                Animal baby = exemplar.Clone();

                haltReborn = !Farm.instance.TryAddAnimal(baby);

                if (!haltReborn) reborn++;
            } while (!haltReborn && reborn <= moreAnimals);

            if (moreAnimals - reborn > 0)
            {
                Console.WriteLine($"Space limit reached rest {moreAnimals - reborn} {name}'s babies where gifted");
            }

            if(reborn > 0) Console.WriteLine($"Have new {reborn} baby {name}");
        }

        public override Animal Clone()
        {
            return new Animal(livingProduct, deathProduct, lifeSpan, name);
        }
    }
}
