using ConsoleApp1;

Plant Trigo = new Plant(new Item{ name = "wheat's seed", value = 5}, new Item { name = "wheat", value = 10 }, 3, "wheat");
Animal Oveja = new Animal(new Item { name = "wool", value = 10 }, new Item { name = "meat", value = 20 }, 5, "sheep");

Farm granja = new Farm(5, 5, 10, 100, 5, 80,new List<Plant> { Trigo, Trigo.Clone(), Trigo.Clone()}, new List<Animal>() { Oveja, Oveja.Clone()});

granja.Step();