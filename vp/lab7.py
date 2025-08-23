import pygame
import random
import sys
from pygame.locals import *

# Инициализация Pygame
pygame.init()
pygame.mixer.init()

# Константы
WINDOW_WIDTH = 800
WINDOW_HEIGHT = 600
GRID_SIZE = 20
GRID_WIDTH = WINDOW_WIDTH // GRID_SIZE
GRID_HEIGHT = WINDOW_HEIGHT // GRID_SIZE
FPS = 10

# Цвета
WHITE = (255, 255, 255)
BLACK = (0, 0, 0)
GREEN = (0, 255, 0)
RED = (255, 0, 0)
BLUE = (0, 0, 255)
YELLOW = (255, 255, 0)
PURPLE = (128, 0, 128)

# Направления
UP = (0, -1)
DOWN = (0, 1)
LEFT = (-1, 0)
RIGHT = (1, 0)

# Создание окна
screen = pygame.display.set_mode((WINDOW_WIDTH, WINDOW_HEIGHT))
pygame.display.set_caption('Змейка')
clock = pygame.time.Clock()

# Загрузка звуков
try:
    eat_sound = pygame.mixer.Sound('eat.wav')
    game_over_sound = pygame.mixer.Sound('game_over.wav')
except:
    # Если звуковые файлы не найдены, создаем заглушки
    eat_sound = pygame.mixer.Sound(buffer=bytearray(100))
    game_over_sound = pygame.mixer.Sound(buffer=bytearray(100))

# Класс спрайта змейки
class Snake(pygame.sprite.Sprite):
    def __init__(self):
        super().__init__()
        self.positions = [(GRID_WIDTH // 2, GRID_HEIGHT // 2)]
        self.direction = RIGHT
        self.next_direction = RIGHT
        self.length = 1
        self.score = 0
        self.speed = FPS
        
        # Создаем поверхность для спрайта
        self.image = pygame.Surface((GRID_SIZE, GRID_SIZE))
        self.image.fill(GREEN)
        self.rect = self.image.get_rect()
        self.rect.topleft = self.positions[0][0] * GRID_SIZE, self.positions[0][1] * GRID_SIZE
        
    def update(self):
        # Изменяем направление
        self.direction = self.next_direction
        
        # Получаем новую позицию головы
        head_x, head_y = self.positions[0]
        dir_x, dir_y = self.direction
        new_x = (head_x + dir_x) % GRID_WIDTH
        new_y = (head_y + dir_y) % GRID_HEIGHT
        new_position = (new_x, new_y)
        
        # Проверяем столкновение с собой
        if new_position in self.positions[:-1]:
            self.kill()
            return
        
        # Добавляем новую позицию в начало списка
        self.positions.insert(0, new_position)
        
        # Если длина змейки больше нужной, удаляем последний сегмент
        if len(self.positions) > self.length:
            self.positions.pop()
        
        # Обновляем позицию спрайта
        self.rect.topleft = self.positions[0][0] * GRID_SIZE, self.positions[0][1] * GRID_SIZE
    
    def change_direction(self, direction):
        # Запрещаем разворот на 180 градусов
        if (direction[0] * -1, direction[1] * -1) != self.direction:
            self.next_direction = direction
    
    def grow(self):
        self.length += 1
        self.score += 10
        # Увеличиваем скорость каждые 50 очков
        if self.score % 50 == 0 and self.speed < FPS * 2:
            self.speed += 1
    
    def draw(self, surface):
        for i, (x, y) in enumerate(self.positions):
            # Голова змейки другого цвета
            if i == 0:
                color = BLUE
            else:
                # Чередуем цвета для сегментов тела
                color = GREEN if i % 2 == 0 else YELLOW
                
            rect = pygame.Rect(x * GRID_SIZE, y * GRID_SIZE, GRID_SIZE, GRID_SIZE)
            pygame.draw.rect(surface, color, rect)
            pygame.draw.rect(surface, BLACK, rect, 1)

# Класс спрайта еды
class Food(pygame.sprite.Sprite):
    def __init__(self):
        super().__init__()
        self.image = pygame.Surface((GRID_SIZE, GRID_SIZE))
        self.image.fill(RED)
        self.rect = self.image.get_rect()
        self.position = (0, 0)
        self.randomize_position()
        
    def randomize_position(self):
        self.position = (random.randint(0, GRID_WIDTH - 1), random.randint(0, GRID_HEIGHT - 1))
        self.rect.topleft = self.position[0] * GRID_SIZE, self.position[1] * GRID_SIZE

# Класс врага (препятствия)
class Enemy(pygame.sprite.Sprite):
    def __init__(self):
        super().__init__()
        self.image = pygame.Surface((GRID_SIZE, GRID_SIZE))
        self.image.fill(PURPLE)
        self.rect = self.image.get_rect()
        self.position = (0, 0)
        self.randomize_position()
        self.direction = random.choice([UP, DOWN, LEFT, RIGHT])
        self.move_counter = 0
        
    def randomize_position(self):
        self.position = (random.randint(0, GRID_WIDTH - 1), random.randint(0, GRID_HEIGHT - 1))
        self.rect.topleft = self.position[0] * GRID_SIZE, self.position[1] * GRID_SIZE
        
    def update(self):
        self.move_counter += 1
        if self.move_counter >= 3:  # Меняем направление каждые 3 хода
            self.direction = random.choice([UP, DOWN, LEFT, RIGHT])
            self.move_counter = 0
            
        # Двигаем врага
        x, y = self.position
        dx, dy = self.direction
        new_x = (x + dx) % GRID_WIDTH
        new_y = (y + dy) % GRID_HEIGHT
        self.position = (new_x, new_y)
        self.rect.topleft = new_x * GRID_SIZE, new_y * GRID_SIZE

# Функция отображения счета
def draw_score(surface, score):
    font = pygame.font.Font(None, 36)
    score_text = font.render(f'Очки: {score}', True, WHITE)
    surface.blit(score_text, (10, 10))

# Функция отображения Game Over
def draw_game_over(surface):
    font = pygame.font.Font(None, 72)
    game_over_text = font.render('GAME OVER', True, RED)
    text_rect = game_over_text.get_rect(center=(WINDOW_WIDTH // 2, WINDOW_HEIGHT // 2))
    surface.blit(game_over_text, text_rect)
    
    font = pygame.font.Font(None, 36)
    restart_text = font.render('Нажмите R для перезапуска', True, WHITE)
    restart_rect = restart_text.get_rect(center=(WINDOW_WIDTH // 2, WINDOW_HEIGHT // 2 + 50))
    surface.blit(restart_text, restart_rect)

# Основная функция игры
def main():
    # Создаем группы спрайтов
    all_sprites = pygame.sprite.Group()
    food_group = pygame.sprite.GroupSingle()
    enemies_group = pygame.sprite.Group()
    
    # Создаем змейку
    snake = Snake()
    all_sprites.add(snake)
    
    # Создаем еду
    food = Food()
    food_group.add(food)
    all_sprites.add(food)
    
    # Создаем врагов
    for _ in range(3):
        enemy = Enemy()
        enemies_group.add(enemy)
        all_sprites.add(enemy)
    
    running = True
    game_over = False
    
    while running:
        for event in pygame.event.get():
            if event.type == QUIT:
                running = False
            elif event.type == KEYDOWN:
                if not game_over:
                    if event.key == K_UP:
                        snake.change_direction(UP)
                    elif event.key == K_DOWN:
                        snake.change_direction(DOWN)
                    elif event.key == K_LEFT:
                        snake.change_direction(LEFT)
                    elif event.key == K_RIGHT:
                        snake.change_direction(RIGHT)
                else:
                    if event.key == K_r:
                        # Перезапуск игры
                        return main()
        
        if not game_over:
            # Обновляем змейку
            snake.update()
            
            # Обновляем врагов
            enemies_group.update()
            
            # Проверяем столкновение змейки с едой
            if pygame.sprite.collide_rect(snake, food):
                snake.grow()
                food.randomize_position()
                # Проверяем, чтобы еда не появилась на змейке или врагах
                while (food.position in snake.positions or 
                       pygame.sprite.spritecollideany(food, enemies_group)):
                    food.randomize_position()
                eat_sound.play()
            
            # Проверяем столкновение змейки с врагами
            if pygame.sprite.spritecollideany(snake, enemies_group):
                game_over = True
                game_over_sound.play()
            
            # Проверяем, жива ли змейка (метод update мог убить ее)
            if not snake.alive():
                game_over = True
                game_over_sound.play()
        
        # Отрисовка
        screen.fill(BLACK)
        
        # Отрисовка сетки
        for x in range(0, WINDOW_WIDTH, GRID_SIZE):
            pygame.draw.line(screen, (50, 50, 50), (x, 0), (x, WINDOW_HEIGHT))
        for y in range(0, WINDOW_HEIGHT, GRID_SIZE):
            pygame.draw.line(screen, (50, 50, 50), (0, y), (WINDOW_WIDTH, y))
        
        # Отрисовка всех спрайтов
        snake.draw(screen)  # Отрисовываем змейку вручную для разноцветных сегментов
        for sprite in all_sprites:
            if sprite != snake:  # Змейку уже отрисовали
                screen.blit(sprite.image, sprite.rect)
        
        # Отрисовка счета
        draw_score(screen, snake.score)
        
        # Отрисовка Game Over, если игра окончена
        if game_over:
            draw_game_over(screen)
        
        pygame.display.flip()
        
        # Управление скоростью игры
        if not game_over:
            clock.tick(snake.speed)
        else:
            clock.tick(FPS)
    
    pygame.quit()
    sys.exit()

if __name__ == "__main__":
    main()