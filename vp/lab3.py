class Animal:
    def __init__(self, name, age):
        self.name = name
        self._age = age

    @property
    def age (self):
        return self._age

    @age.setter
    def age(self, new_age):
        if new_age > self._age:
            self._age = new_age
            return self._age

    def move(self):
        print("Я двигаюсь")

    def __str__(self):
        return f"{self.name}, возраст: {self.age}"

class Mammal(Animal):
    def __init__(self, name, age, species, speed):
        super().__init__(name, age)
        self.species = species
        self.speed = speed

    def move(self):
        print("Я бегаю")

    def __str__(self):
        return f"Млекопитающее: {super().__str__()}, вид: {self.species}, скорость: {self.speed} км/ч"

class Bird(Animal):
    def __init__(self, name, age, species, speed):
        super().__init__(name, age)
        self.species = species
        self.speed = speed

    def move(self):
        print("Я летаю")

    def __str__(self):
        return f"Птица: {super().__str__()}, вид: {self.species}, скорость: {self.speed} км/ч"

class Fish(Animal):
    def __init__(self, name, age, species, speed):
        super().__init__(name, age)
        self.species = species
        self.speed = speed

    def move(self):
        print("Я плаваю")

    def __str__(self):
        return f"Рыба: {super().__str__()}, вид: {self.species}, скорость: {self.speed} км/ч"

animals = [
    Mammal("Лев", 5, "Хищник", 80),
    Bird("Орел", 3, "Хищная птица", 120),
    Fish("Тунец", 2, "Лучеперая рыба", 70),
    Bird("Воробей", 1, "Воробьинообразные", 30),
    Mammal("Слон", 10, "Травоядное", 25)
]

for animal in animals:
    print(animal)

print("\nКто умеет летать:")
for animal in animals:
    if isinstance(animal, Bird):
        print(animal.name)

print("\nКто умеет плавать:")
for animal in animals:
    if isinstance(animal, Fish):
        print(animal.name)

oldest_animal = max(animals, key=lambda x: x.age)
print("\nСамое старое животное:", oldest_animal.name)

fastest_bird = max([animal for animal in animals if isinstance(animal, Bird)], key=lambda x: x.speed)
print("\nСамая быстрая птица:", fastest_bird.name)

