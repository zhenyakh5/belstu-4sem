import nltk
nltk.download('punkt')
nltk.download('stopwords')
nltk.download('punkt_tab')

import matplotlib.pyplot as plt
from wordcloud import WordCloud, STOPWORDS
from nltk.stem import SnowballStemmer
from nltk.tokenize import word_tokenize
import string
import nltk
from PIL import Image
import numpy as np

with open('text.txt', 'r', encoding='utf-8') as file:
    text = file.read()

stemmer = SnowballStemmer('english')

stop_words = set(nltk.corpus.stopwords.words('english'))
stop_words.update(['would', 'could', 'said', 'oh'])

def process_text(text):
    text = text.translate(str.maketrans('', '', string.punctuation))
    tokens = word_tokenize(text.lower())
    processed_words = [stemmer.stem(word) for word in tokens if word not in stop_words and len(word) > 2]
    return ' '.join(processed_words)

processed_text = process_text(text)

mask = np.array(Image.open('image.png'))

wordcloud = WordCloud(
    background_color='white',
    mask=mask,
    contour_width=1,
    contour_color='black',
    max_words=100,
    colormap='viridis'
).generate(processed_text)

# Визуализация
plt.figure(figsize=(10, 8))
plt.imshow(wordcloud, interpolation='bilinear')
plt.axis('off')
plt.show()

# Сохранение изображения
wordcloud.to_file('wordcloud_note.png')