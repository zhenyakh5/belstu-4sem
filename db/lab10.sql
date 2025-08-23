-- 1
BEGIN
  NULL;
END;
/

-- 2
BEGIN
  DBMS_OUTPUT.PUT_LINE('Hello World!');
END;
/
-- 3 and 4
SELECT KEYWORD FROM V$RESERVED_WORDS WHERE LENGTH != 1;

-- 5
DECLARE
  -- Целочисленные переменные
  v_num1 NUMBER := 10;
  v_num2 NUMBER := 3;
  v_result NUMBER;
  
  -- С фиксированной точкой
  v_fixed1 NUMBER(5,2) := 123.45;
  v_fixed2 NUMBER(5,-2) := 12345; -- Округление до сотен
  
  -- С плавающей точкой
  v_float NUMBER := 1.5E2; -- 1.5 * 10^2 = 150
  
  -- Дата
  v_date DATE := SYSDATE;
  v_date2 DATE := TO_DATE('2023-01-15', 'YYYY-MM-DD');
  
  -- Символьные переменные
  v_char CHAR(10) := 'ABC';
  v_varchar VARCHAR2(50) := 'Пример текста'; -- Увеличено до 50
  v_nchar NCHAR(10) := N'Unicode';
  v_nvarchar NVARCHAR2(50) := N'Юникод текст'; -- Увеличено до 50
  
  -- Логические
  v_bool1 BOOLEAN := TRUE;
  v_bool2 BOOLEAN := FALSE;
  v_bool3 BOOLEAN := NULL;
BEGIN
  v_result := v_num1 + v_num2;
  DBMS_OUTPUT.PUT_LINE('Сумма: ' || v_result);
  
  v_result := v_num1 - v_num2;
  DBMS_OUTPUT.PUT_LINE('Разность: ' || v_result);
  
  v_result := v_num1 * v_num2;
  DBMS_OUTPUT.PUT_LINE('Произведение: ' || v_result);
  
  v_result := v_num1 / v_num2;
  DBMS_OUTPUT.PUT_LINE('Деление: ' || v_result);
  
  v_result := MOD(v_num1, v_num2);
  DBMS_OUTPUT.PUT_LINE('Остаток: ' || v_result);
  
  DBMS_OUTPUT.PUT_LINE('Фиксированная точка: ' || v_fixed1);
  DBMS_OUTPUT.PUT_LINE('Округление: ' || v_fixed2);
  DBMS_OUTPUT.PUT_LINE('Экспонента: ' || v_float);
  DBMS_OUTPUT.PUT_LINE('Дата: ' || TO_CHAR(v_date, 'DD.MM.YYYY'));
  
  DBMS_OUTPUT.PUT_LINE('Символьные: ' || RTRIM(v_char) || ', ' || v_varchar);
  
  IF v_bool1 THEN
    DBMS_OUTPUT.PUT_LINE('Логическая переменная TRUE');
  END IF;
  
  IF NOT v_bool2 THEN
    DBMS_OUTPUT.PUT_LINE('Логическая переменная FALSE');
  END IF;
END;
/

-- 6
DECLARE
  c_pi CONSTANT NUMBER := 3.14159;
  c_message CONSTANT VARCHAR2(50) := 'Это константа';
  c_flag CONSTANT CHAR(1) := 'Y';
BEGIN
  DBMS_OUTPUT.PUT_LINE(c_message);
  DBMS_OUTPUT.PUT_LINE('Значение PI: ' || c_pi);
  DBMS_OUTPUT.PUT_LINE('Флаг: ' || c_flag);
  
  c_pi := 3.14; -- Ошибка, нельзя изменить константу
END;
/

-- 7
DECLARE
  v_teacher_name TEACHER.TEACHER_NAME%TYPE;
  v_pulpit_name PULPIT.PULPIT_NAME%TYPE;
BEGIN
  -- Получаем информацию о преподавателе
  SELECT TEACHER_NAME, PULPIT INTO v_teacher_name, v_pulpit_name
  FROM TEACHER 
  WHERE ROWNUM = 1;
  
  DBMS_OUTPUT.PUT_LINE('Преподаватель: ' || v_teacher_name);
  DBMS_OUTPUT.PUT_LINE('Кафедра: ' || v_pulpit_name);
  
  -- Выводим информацию об аудиториях
  DBMS_OUTPUT.PUT_LINE('Аудитории типа ЛК:');
  FOR aud_rec IN (SELECT AUDITORIUM_NAME, AUDITORIUM_CAPACITY 
                  FROM AUDITORIUM 
                  WHERE AUDITORIUM_TYPE = 'ЛК') LOOP
    DBMS_OUTPUT.PUT_LINE(aud_rec.AUDITORIUM_NAME || ' (вместимость: ' || aud_rec.AUDITORIUM_CAPACITY || ')');
  END LOOP;
END;
/

-- 8
DECLARE
  r_subject SUBJECT%ROWTYPE;
BEGIN
  SELECT * INTO r_subject 
  FROM SUBJECT 
  WHERE SUBJECT_NAME = 'Основы программирования';
  
  DBMS_OUTPUT.PUT_LINE('Предмет: ' || r_subject.SUBJECT_NAME || 
                      ', Кафедра: ' || r_subject.PULPIT);
END;
/

-- 9
DECLARE
  v_num NUMBER := 10;
BEGIN
  -- Простой IF
  IF v_num > 0 THEN
    DBMS_OUTPUT.PUT_LINE('Число положительное');
  END IF;
  
  -- IF-ELSE
  IF v_num MOD 2 = 0 THEN
    DBMS_OUTPUT.PUT_LINE('Число четное');
  ELSE
    DBMS_OUTPUT.PUT_LINE('Число нечетное');
  END IF;
  
  -- IF-ELSIF-ELSE
  IF v_num < 0 THEN
    DBMS_OUTPUT.PUT_LINE('Отрицательное');
  ELSIF v_num = 0 THEN
    DBMS_OUTPUT.PUT_LINE('Ноль');
  ELSE
    DBMS_OUTPUT.PUT_LINE('Положительное');
  END IF;
END;
/

-- 10
DECLARE
  v_grade CHAR(1) := 'B';
  v_result VARCHAR2(20);
BEGIN
  -- Простой CASE
  v_result := CASE v_grade
                WHEN 'A' THEN 'Отлично'
                WHEN 'B' THEN 'Хорошо'
                WHEN 'C' THEN 'Удовлетворительно'
                ELSE 'Неудовлетворительно'
              END;
  DBMS_OUTPUT.PUT_LINE('Оценка: ' || v_result);
  
  -- Поисковый CASE
  CASE 
    WHEN v_grade = 'A' THEN DBMS_OUTPUT.PUT_LINE('90-100 баллов');
    WHEN v_grade = 'B' THEN DBMS_OUTPUT.PUT_LINE('75-89 баллов');
    WHEN v_grade = 'C' THEN DBMS_OUTPUT.PUT_LINE('60-74 балла');
    ELSE DBMS_OUTPUT.PUT_LINE('Меньше 60 баллов');
  END CASE;
END;
/

-- 11
DECLARE
  v_counter NUMBER := 1;
BEGIN
  LOOP
    DBMS_OUTPUT.PUT_LINE('Итерация: ' || v_counter);
    v_counter := v_counter + 1;
    EXIT WHEN v_counter > 5;
  END LOOP;
END;
/

-- 12
DECLARE
  v_counter NUMBER := 1;
BEGIN
  WHILE v_counter <= 5 LOOP
    DBMS_OUTPUT.PUT_LINE('Итерация: ' || v_counter);
    v_counter := v_counter + 1;
  END LOOP;
END;
/

-- 13
BEGIN
  FOR i IN 1..5 LOOP
    DBMS_OUTPUT.PUT_LINE('Итерация: ' || i);
  END LOOP;
  
  -- Обратный порядок
  FOR i IN REVERSE 1..5 LOOP
    DBMS_OUTPUT.PUT_LINE('Обратная итерация: ' || i);
  END LOOP;
END;
/