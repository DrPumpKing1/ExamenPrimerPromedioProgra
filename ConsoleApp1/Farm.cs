using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Farm
    {
        public static Farm instance;

        public int cropSpaces;
        public int animalSpaces;
        public List<Plant> plants;
        public List<Animal> animals;

        private List<string> plantsSpecies;
        private List<string> animalsSpecies;

        private List<Item> inventory;

        public int money;

        public int expansionCrop;
        public int expansionCropCost;
        public int expansionAnimal;
        public int expansionAnimalCost;

        public Farm(int cropSpaces, int animalSpaces, int expansionCrop, int expansionCropCost, int expansionAnimal, int expansionAnimalCost, List<Plant> plants, List<Animal> animals) 
        {
            this.cropSpaces = cropSpaces;
            this.animalSpaces = animalSpaces;

            this.plants = new List<Plant>();
            this.animals = new List<Animal>();

            plantsSpecies = new List<string>();
            animalsSpecies = new List<string>();

            List<Plant> tempPlants = plants;
            List<Animal> tempAnimal = animals;

            tempPlants.ForEach(p => TryAddPlant(p));
            tempAnimal.ForEach(a => TryAddAnimal(a));

            money = 0;
            inventory = new List<Item>();

            this.expansionAnimal = expansionAnimal;
            this.expansionCrop = expansionCrop;
            this.expansionCropCost = expansionCropCost;
            this.expansionAnimalCost = expansionAnimalCost;

            instance = this;
        }

        public void Step()
        {
            //Step all plants and animals
            plants.ForEach(p => p.Step());
            animals.ForEach(p => p.Step());

            //Harvest Plants
            List<string> plantSpeciesCopy = new List<string>(plantsSpecies);
            foreach(string plantSpecies in plantSpeciesCopy)
            {
                Plant.HarvestBunch(plants, plantSpecies);
            }

            //Reproduce Animals
            List<string> animalSpeciesCopy = new List<string>(animalsSpecies);
            foreach(string animalSpecies in animalSpeciesCopy)
            {
                Animal.ReproduceKind(animals, animalSpecies);
            }


            List<Animal> deathAnimals = new List<Animal>();
            //Butcher death animals
            animals.ForEach(a =>
            {
                if (a.death)
                {
                    a.Butchery();
                    deathAnimals.Add(a);
                }
            });

            deathAnimals.ForEach(a => a.DeathRemove());

            //Sell goods
            SellGoods();

            //ask to expand
            Console.WriteLine($"Expand farmlands by {expansionCrop} crops for {expansionCropCost}? (Y/N) (money: {money})");
            string answer  = Console.ReadLine();
            if(answer == "Y") 
            {
                if (TrySpendMoney(expansionCropCost))
                {
                    ExpandCropSpaces(expansionCrop);
                }
                else Console.WriteLine("Not enough Money");
            }

            Console.WriteLine($"Expand corral by {expansionAnimal} spaces for {expansionAnimalCost}? (Y/N) (money: {money})");
            answer = Console.ReadLine();
            if (answer == "Y")
            {
                if (TrySpendMoney(expansionAnimalCost))
                {
                    ExpandAnimalSpaces(expansionAnimal);
                }
                else Console.WriteLine("Not enough Money");
            }

            Console.WriteLine($"Money: {money} coins");
            plants.ForEach(p => Console.WriteLine($"You've one {p.name} age {p.lifeSpan - p.life}"));
            animals.ForEach(p => Console.WriteLine($"You've one {p.name} age {p.lifeSpan - p.life}"));

            Step();
        }

        public void ExpandCropSpaces(int moreSpaces)
        {
            cropSpaces += moreSpaces;
        }

        public void ExpandAnimalSpaces(int moreSpaces)
        {
            animalSpaces += moreSpaces;
        }

        public bool TryAddPlant(Plant plant)
        {
            if (plants.Count >= cropSpaces) return false;

            plants.Add(plant);
            if(!plantsSpecies.Contains(plant.name)) plantsSpecies.Add(plant.name);

            return true;
        }

        public bool TryAddAnimal(Animal animal)
        {
            if (animals.Count >= animalSpaces) return false;

            animals.Add(animal);
            if(!animalsSpecies.Contains(animal.name)) animalsSpecies.Add(animal.name);

            return true;
        }

        public void RemovePlant(Plant plant)
        {
            if (!plants.Contains(plant)) return;
            
            plants.Remove(plant);
        }

        public void RemoveAnimal(Animal animal)
        {
            if(!animals.Contains(animal)) return;
            
            animals.Remove(animal);
        }

        public void AddMoney(int quantity)
        {
            money += quantity;
        }

        public bool TrySpendMoney(int quantity)
        {
            if(money < quantity) return false;

            money -= quantity;

            return true;
        }

        public void AddInventory(Entity sender, Item item)
        {
            Console.WriteLine($"{sender.name} send {item.name}");
            inventory.Add(new Item { name = item.name, value = item.value});
        }

        public void SellGoods()
        {
            int value = 0;

            List<Item> itemsToRemove = new List<Item>();

            inventory.ForEach(item => {
                value += item.value;
                itemsToRemove.Add(item);        
            });

            itemsToRemove.ForEach(item => inventory.Remove(item));

            AddMoney(value);

            Console.WriteLine($"Sell all inventory goods for {value} coins");
        }
    }
}
