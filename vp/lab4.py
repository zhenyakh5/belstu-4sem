import requests
from bs4 import BeautifulSoup
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns

def parse_books():
    url = "http://books.toscrape.com/"
    response = requests.get(url)
    soup = BeautifulSoup(response.text, "html.parser")

    books = []
    for book in soup.find_all("article", class_="product_pod"):
        title = book.h3.a["title"]
        price = book.find("p", class_="price_color").text.replace("Â£", "")
        books.append({"Название": title, "Цена": float(price)})

    return books

def save_to_csv(data, filename="books.csv"):
    df = pd.DataFrame(data)
    df.to_csv(filename, index=False, encoding="utf-8")

books_data = parse_books()
save_to_csv(books_data)
print("Данные успешно сохранены в books.csv")

df = pd.read_csv("books.csv")

df_sorted = df.sort_values(by="Цена", ascending=False)
print("Первые 5 значений:")
print(df_sorted.head())

print("\nОсновные статистические метрики:")
print(df["Цена"].describe())

plt.figure(figsize=(10, 6))
sns.histplot(df["Цена"], bins=30, kde=True, color="blue")
plt.title("Распределение цен на книги")
plt.xlabel("Цена (£)")
plt.ylabel("Количество книг")
plt.show()

plt.figure(figsize=(10, 6))
sns.boxplot(x=df["Цена"], color="orange")
plt.title("Box-plot цен на книги")
plt.xlabel("Цена (£)")
plt.show()