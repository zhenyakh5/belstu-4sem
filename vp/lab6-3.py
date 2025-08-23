import telebot
from telebot import types
import json
import os
from datetime import datetime

bot = telebot.TeleBot("")

DATA_FILE = "transactions.json"

def load_data():
    if not os.path.exists(DATA_FILE):
        return {}
    with open(DATA_FILE, "r", encoding="utf-8") as f:
        return json.load(f)

def save_data(data):
    with open(DATA_FILE, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=4)

def init_user_data(user_id):
    data = load_data()
    if user_id not in data:
        data[user_id] = {
            "balance": 0,
            "transactions": []
        }
        save_data(data)
    return data

@bot.message_handler(commands=['start', 'help'])
def send_welcome(message):
    markup = types.ReplyKeyboardMarkup(resize_keyboard=True)
    
    row1 = [
        types.KeyboardButton("Добавить доход"),
        types.KeyboardButton("Добавить расход")
    ]
    
    row2 = [
        types.KeyboardButton("Показать баланс"),
        types.KeyboardButton("История транзакций")
    ]
    
    markup.add(*row1)
    markup.add(*row2)
    
    bot.send_message(
        message.chat.id,
        "💰 Финансовый менеджер\n\nВыберите действие:",
        reply_markup=markup
    )

@bot.message_handler(func=lambda message: True)
def handle_text(message):
    if message.text == "Добавить доход":
        msg = bot.reply_to(message, "Введите сумму дохода:")
        bot.register_next_step_handler(msg, process_income)
    elif message.text == "Добавить расход":
        msg = bot.reply_to(message, "Введите сумму расхода:")
        bot.register_next_step_handler(msg, process_expense)
    elif message.text == "Показать баланс":
        show_balance(message)
    elif message.text == "История транзакций":
        show_transaction_history(message)

def process_income(message):
    try:
        amount = float(message.text)
        user_id = str(message.chat.id)
        data = load_data()
        
        if user_id not in data:
            data[user_id] = {"balance": 0, "transactions": []}
        
        data[user_id]["balance"] += amount
        transaction = {
            "type": "income",
            "amount": amount,
            "date": datetime.now().strftime("%Y-%m-%d %H:%M"),
            "description": "Доход"
        }
        data[user_id]["transactions"].append(transaction)
        save_data(data)
        
        bot.send_message(
            message.chat.id,
            f"✅ Доход +{amount:.2f} успешно добавлен!\n"
            f"Новый баланс: {data[user_id]['balance']:.2f}"
        )
    except ValueError:
        bot.reply_to(message, "❌ Ошибка! Пожалуйста, введите число.")

def process_expense(message):
    try:
        amount = float(message.text)
        user_id = str(message.chat.id)
        data = load_data()
        
        if user_id not in data:
            data[user_id] = {"balance": 0, "transactions": []}
        
        data[user_id]["balance"] -= amount
        transaction = {
            "type": "expense",
            "amount": amount,
            "date": datetime.now().strftime("%Y-%m-%d %H:%M"),
            "description": "Расход"
        }
        data[user_id]["transactions"].append(transaction)
        save_data(data)
        
        bot.send_message(
            message.chat.id,
            f"✅ Расход -{amount:.2f} успешно добавлен!\n"
            f"Новый баланс: {data[user_id]['balance']:.2f}"
        )
    except ValueError:
        bot.reply_to(message, "❌ Ошибка! Пожалуйста, введите число.")

def show_balance(message):
    user_id = str(message.chat.id)
    data = load_data()
    
    if user_id in data:
        balance = data[user_id]["balance"]
        bot.send_message(
            message.chat.id,
            f"💰 Ваш текущий баланс: {balance:.2f}"
        )
    else:
        bot.reply_to(message, "У вас пока нет операций.")

def show_transaction_history(message):
    user_id = str(message.chat.id)
    data = load_data()
    
    if user_id not in data or not data[user_id]["transactions"]:
        bot.reply_to(message, "История транзакций пуста.")
        return
    
    transactions = data[user_id]["transactions"][-10:]
    history_text = "📜 История транзакций:\n\n"
    
    for tx in reversed(transactions):
        emoji = "⬆️" if tx["type"] == "income" else "⬇️"
        history_text += (
            f"{emoji} {tx['date']}\n"
            f"{tx['description']}: {tx['amount']:.2f}\n"
            f"————————————\n"
        )
    
    history_text += f"\n💰 Текущий баланс: {data[user_id]['balance']:.2f}"
    
    bot.send_message(message.chat.id, history_text)

bot.polling()