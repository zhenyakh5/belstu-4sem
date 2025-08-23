from datetime import datetime, timedelta

def word_frequency(text):
    words = text.split()
    frequency = {}
    for word in words:
        word = word.lower().strip('.,!?')
        if word in frequency:
            frequency[word] += 1
        else:
            frequency[word] = 1
    return frequency

logs = [
    ("192.168.1.1", "200 OK", 1543),
    ("192.168.1.2", "404 Not Found", 234),
    ("192.168.1.1", "500 Internal Server Error", 542),
    ("192.168.1.3", "200 OK", 876),
    ("192.168.1.2", "200 OK", 1324),
]

ip_counts = {}
for log in logs:
    ip = log[0]
    if ip in ip_counts:
        ip_counts[ip] += 1
    else:
        ip_counts[ip] = 1

status_counts = {}
for log in logs:
    status = log[1]
    if status in status_counts:
        status_counts[status] += 1
    else:
        status_counts[status] = 1
most_frequent_status = max(status_counts, key=status_counts.get)

total_data = sum(log[2] for log in logs)

service_a = {"Анна", "Иван", "Мария", "Сергей", "Алексей"}
service_b = {"Мария", "Иван", "Дмитрий", "Ольга", "Светлана"}
service_c = {"Сергей", "Ольга", "Александр", "Иван", "Анна"}

all_services_users = service_a & service_b & service_c
only_one_service_users = (service_a | service_b | service_c) - (service_a & service_b) - (service_b & service_c) - (service_a & service_c)
unique_users = {
    "service_a": len(service_a - service_b - service_c),
    "service_b": len(service_b - service_a - service_c),
    "service_c": len(service_c - service_a - service_b)
}
largest_unique_base = max(unique_users, key=unique_users.get)

tasks = {
    "Задача 1": "2025-02-10",
    "Задача 2": "2025-02-15",
    "Задача 3": "2025-02-05",
    "Задача 4": "2025-02-20",
    "Задача 5": "2025-02-12",
    "Задача 6": "2025-02-01",
    "Задача 7": "2025-02-18",
    "Задача 8": "2025-02-22",
    "Задача 9": "2025-02-08",
    "Задача 10": "2025-02-14",
}

today = datetime.today()
expired_tasks = [task for task, deadline in tasks.items() if datetime.strptime(deadline, "%Y-%m-%d") < today]
urgent_tasks = [task for task, deadline in tasks.items() if today < datetime.strptime(deadline, "%Y-%m-%d") < today + timedelta(days=3)]

def add_task(task_name, deadline):
    if task_name in tasks:
        return "Задача с таким названием уже существует."
    try:
        deadline_date = datetime.strptime(deadline, "%Y-%m-%d")
        if deadline_date < today:
            return "Дата дедлайна не может быть в прошлом."
        tasks[task_name] = deadline
        return "Задача успешно добавлена."
    except ValueError:
        return "Неверный формат даты. Используйте формат YYYY-MM-DD."

text = "Hello world! Hello everyone. World is beautiful."
print("Частота слов:", word_frequency(text))

print("Количество запросов от каждого IP:", ip_counts)
print("Самый частый HTTP-статус:", most_frequent_status)
print("Общий объем переданных данных:", total_data)

print("Пользователи во всех трех сервисах:", all_services_users)
print("Пользователи только в одном сервисе:", only_one_service_users)
print("Сервис с самой большой уникальной базой пользователей:", largest_unique_base)

print("Задачи с истекшим дедлайном:", expired_tasks)
print("Задачи, до окончания которых осталось меньше 3-х дней:", urgent_tasks)
print(add_task("Новая задача", "2025-02-25"))
