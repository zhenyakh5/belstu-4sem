def uppercase(func):
    def wrapper(*args, **kwargs):
        result = func(*args, **kwargs)
        if isinstance(result, str):
            return result.upper()
        return result
    return wrapper

def count_calls(func):
    def wrapper(*args, **kwargs):
        wrapper.call_count += 1
        return func(*args, **kwargs)
    wrapper.call_count = 0
    return wrapper

def html_tag(tag):
    def decorator(func):
        def wrapper(*args, **kwargs):
            result = func(*args, **kwargs)
            return f"<{tag}>{result}</{tag}>"
        return wrapper
    return decorator

@uppercase
def greet_upper(name):
    return f"Hello, {name}!"

print(greet_upper("Tom"))  # Вывод: HELLO, TOM!

@count_calls
def greet_count(name):
    print(f"Hello, {name}!")

greet_count("Tom")
greet_count("Denis") 
print(f"Функция greet_count вызвана {greet_count.call_count} раз(а).")  # Вывод: Функция greet_count вызвана 2 раз(а).

@html_tag("div")
def get_text():
    return "Hello, World!"

print(get_text())  # Вывод: <div>Hello, World!</div>