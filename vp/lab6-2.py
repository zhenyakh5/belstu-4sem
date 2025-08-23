import telebot
from telebot import types

bot = telebot.TeleBot("")

@bot.message_handler(commands=['start'])
def send_welcome(message):
    markup = types.InlineKeyboardMarkup()
    btn1 = types.InlineKeyboardButton("Количество символов", callback_data='length')
    btn2 = types.InlineKeyboardButton("Верхний регистр", callback_data='upper')
    btn3 = types.InlineKeyboardButton("Убрать пробелы", callback_data='nospaces')
    markup.add(btn1, btn2, btn3)
    bot.send_message(message.chat.id, "Выберите действие, а потом отправьте строку:", reply_markup=markup)


@bot.callback_query_handler(func=lambda call: True)
def callback_handler(call):
    if call.data in ['length', 'upper', 'nospaces']:
        bot.send_message(call.message.chat.id, "Отправьте строку для обработки.")
        bot.register_next_step_handler(call.message, process_text, call.data)
    else:
        bot.answer_callback_query(call.id, "Неизвестная команда.")

def process_text(message, action):
    if message.content_type != 'text':
        bot.reply_to(message, "Пожалуйста, отправьте текст.")
        return

    text = message.text
    if action == 'length':
        result = f"Количество символов: {len(text)}"
    elif action == 'upper':
        result = f"Верхний регистр: {text.upper()}"
    elif action == 'nospaces':
        result = f"Без пробелов: {text.replace(' ', '')}"

    bot.send_message(message.chat.id, result)

bot.polling()