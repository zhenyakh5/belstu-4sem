import random 

def task1():
    print ("\nЗадание 1 \n")

    name = input("Введите имя: ")
    surname = input("Введите фамилию: ")

    print(f"\nИмя и фамилия с заглавных букв: {name.capitalize()} {surname.capitalize()}")
    print(f"Инициалы: {name.capitalize()[0]}. {surname.capitalize()[0]}.")

def task2():
    print ("\nЗадание 2 \n")

    print(f"Список из квадратов чисел в диапазоне от 10 до 19:")
    
    squares = [x ** 2 for x in range(10, 20)]
    print(squares)

    print(f"\nСумма всех чисел в списке: {sum(squares)}")

    for i in squares:
        if i % 2 == 0:
            squares.remove(i)

    print(f"Список без четных чисел: {squares}")
    print(f"Количество оставшихся элементов в списке: {len(squares)}\n")

def task3():
    print ("\nЗадание 3 \n")
    
    while True:
        n = input("Введите количество элементов: ")

        if IsInt(n):
            n = int(n)
            break
        else:
            print("Некорректный ввод")

    A = []
    for i in range(n):
        A.append(random.randint(1, 100))

    print(f"Полученный список А: {A}")

    B = []
    currentSum = 0

    for num in A:
        B.append(currentSum)
        currentSum += num

    print(f"Полученный список В из сумм элементов списка А: {B}")


def IsInt(a):
    try:
        int(a)
        return True
    except ValueError:
        return False

task1()
task2()
task3()