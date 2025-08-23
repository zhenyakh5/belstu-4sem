import telebot
from telebot import types

bot = telebot.TeleBot("")

@bot.message_handler(commands=['start', 'help'])
def send_welcome(message):
    help_text = """
    Привет! Я бот, который умеет:
    - Отвечать на команды (/start, /help)
    - Реагировать на слова: 'привет', 'пока'
    - Обрабатывать фото, файлы и стикеры
    - Отвечать на непонятные сообщения
    """
    bot.reply_to(message, help_text)

@bot.message_handler(func=lambda message: True)
def handle_text(message):
    text = message.text.lower()
    if text == 'привет':
        bot.reply_to(message, "Привет! Как дела?")
    elif text == 'пока':
        bot.reply_to(message, "До свидания!")
    else:
        bot.reply_to(message, "Я не понимаю. Введите /help для справки.")

@bot.message_handler(content_types=['photo'])
def handle_photo(message):
    bot.reply_to(message, "Классное фото! 📸")

@bot.message_handler(content_types=['document'])
def handle_file(message):
    bot.reply_to(message, "Файл получен! 📁")

@bot.message_handler(content_types=['sticker'])
def handle_sticker(message):
    bot.reply_to(message, "Крутой стикер! 😊")

@bot.message_handler(func=lambda message: True)
def handle_unknown(message):
    bot.reply_to(message, "Я не понимаю. Введите /help для справки.")

bot.polling()