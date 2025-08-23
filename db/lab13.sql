-- 1
CREATE OR REPLACE PROCEDURE GET_TEACHERS(PCODE IN TEACHER.PULPIT%TYPE) IS
BEGIN
  DBMS_OUTPUT.PUT_LINE('Преподаватели кафедры ' || PCODE || ':');
  DBMS_OUTPUT.PUT_LINE('--------------------------------');
  
  FOR t IN (SELECT TEACHER_NAME FROM TEACHER WHERE PULPIT = PCODE) LOOP
    DBMS_OUTPUT.PUT_LINE(t.TEACHER_NAME);
  END LOOP;
  
  IF SQL%ROWCOUNT = 0 THEN
    DBMS_OUTPUT.PUT_LINE('На кафедре нет преподавателей');
  END IF;
  
  DBMS_OUTPUT.PUT_LINE('--------------------------------');
END GET_TEACHERS;
/

select * from PULPIT
-- Анонимный блок для демонстрации
BEGIN
  GET_TEACHERS('Программирование');
  GET_TEACHERS('Несуществующая кафедра');
END;
/

-- 2
CREATE OR REPLACE FUNCTION GET_NUM_TEACHERS(PCODE IN TEACHER.PULPIT%TYPE) 
RETURN NUMBER IS
  v_count NUMBER;
BEGIN
  SELECT COUNT(*) INTO v_count
  FROM TEACHER
  WHERE PULPIT = PCODE;
  
  RETURN v_count;
END GET_NUM_TEACHERS;
/

-- Анонимный блок для демонстрации
DECLARE
  v_num NUMBER;
BEGIN
  v_num := GET_NUM_TEACHERS('Базы данных');
  DBMS_OUTPUT.PUT_LINE('Количество преподавателей на кафедре "Базы данных": ' || v_num);
  
  v_num := GET_NUM_TEACHERS('Несуществующая кафедра');
  DBMS_OUTPUT.PUT_LINE('Количество преподавателей на несуществующей кафедре: ' || v_num);
END;
/

-- 3
-- Процедура для вывода преподавателей факультета
CREATE OR REPLACE PROCEDURE GET_TEACHERS(FCODE IN FACULTY.FACULTY_NAME%TYPE) IS
BEGIN
  DBMS_OUTPUT.PUT_LINE('Преподаватели факультета ' || FCODE || ':');
  DBMS_OUTPUT.PUT_LINE('--------------------------------');
  
  FOR t IN (
    SELECT T.TEACHER_NAME
    FROM TEACHER T
    JOIN PULPIT P ON T.PULPIT = P.PULPIT_NAME
    JOIN FACULTY F ON P.FACULTY = F.FACULTY_NAME
    WHERE F.FACULTY_NAME = FCODE
  ) LOOP
    DBMS_OUTPUT.PUT_LINE(t.TEACHER_NAME);
  END LOOP;
  
  IF SQL%ROWCOUNT = 0 THEN
    DBMS_OUTPUT.PUT_LINE('На факультете нет преподавателей');
  END IF;
  
  DBMS_OUTPUT.PUT_LINE('--------------------------------');
END GET_TEACHERS;
/

-- Процедура для вывода дисциплин кафедры
CREATE OR REPLACE PROCEDURE GET_SUBJECTS(PCODE IN SUBJECT.PULPIT%TYPE) IS
BEGIN
  DBMS_OUTPUT.PUT_LINE('Дисциплины кафедры ' || PCODE || ':');
  DBMS_OUTPUT.PUT_LINE('--------------------------------');
  
  FOR s IN (SELECT SUBJECT_NAME FROM SUBJECT WHERE PULPIT = PCODE) LOOP
    DBMS_OUTPUT.PUT_LINE(s.SUBJECT_NAME);
  END LOOP;
  
  IF SQL%ROWCOUNT = 0 THEN
    DBMS_OUTPUT.PUT_LINE('На кафедре нет дисциплин');
  END IF;
  
  DBMS_OUTPUT.PUT_LINE('--------------------------------');
END GET_SUBJECTS;
/

-- Анонимные блоки для демонстрации
BEGIN
  GET_TEACHERS('Информационные технологии');
  GET_SUBJECTS('Программирование');
END;
/

-- 4
-- Функция для подсчета преподавателей факультета
CREATE OR REPLACE FUNCTION GET_NUM_TEACHERS(FCODE IN FACULTY.FACULTY_NAME%TYPE) 
RETURN NUMBER IS
  v_count NUMBER;
BEGIN
  SELECT COUNT(*) INTO v_count
  FROM TEACHER T
  JOIN PULPIT P ON T.PULPIT = P.PULPIT_NAME
  JOIN FACULTY F ON P.FACULTY = F.FACULTY_NAME
  WHERE F.FACULTY_NAME = FCODE;
  
  RETURN v_count;
END GET_NUM_TEACHERS;
/

-- Функция для подсчета дисциплин кафедры
CREATE OR REPLACE FUNCTION GET_NUM_SUBJECTS(PCODE IN SUBJECT.PULPIT%TYPE) 
RETURN NUMBER IS
  v_count NUMBER;
BEGIN
  SELECT COUNT(*) INTO v_count
  FROM SUBJECT
  WHERE PULPIT = PCODE;
  
  RETURN v_count;
END GET_NUM_SUBJECTS;
/

-- Анонимные блоки для демонстрации
DECLARE
  v_num NUMBER;
BEGIN
  v_num := GET_NUM_TEACHERS('Экономика');
  DBMS_OUTPUT.PUT_LINE('Количество преподавателей на факультете "Экономика": ' || v_num);
  
  v_num := GET_NUM_SUBJECTS('Базы данных');
  DBMS_OUTPUT.PUT_LINE('Количество дисциплин на кафедре "Базы данных": ' || v_num);
END;
/

-- 5
CREATE OR REPLACE PACKAGE TEACHERS AS
  -- Процедура для вывода преподавателей факультета
  PROCEDURE GET_TEACHERS(FCODE IN FACULTY.FACULTY_NAME%TYPE);
  
  -- Процедура для вывода дисциплин кафедры
  PROCEDURE GET_SUBJECTS(PCODE IN SUBJECT.PULPIT%TYPE);
  
  -- Функция для подсчета преподавателей факультета
  FUNCTION GET_NUM_TEACHERS(FCODE IN FACULTY.FACULTY_NAME%TYPE) RETURN NUMBER;
  
  -- Функция для подсчета дисциплин кафедры
  FUNCTION GET_NUM_SUBJECTS(PCODE IN SUBJECT.PULPIT%TYPE) RETURN NUMBER;
END TEACHERS;
/

CREATE OR REPLACE PACKAGE BODY TEACHERS AS
  -- Реализация процедуры GET_TEACHERS
  PROCEDURE GET_TEACHERS(FCODE IN FACULTY.FACULTY_NAME%TYPE) IS
  BEGIN
    DBMS_OUTPUT.PUT_LINE('Преподаватели факультета ' || FCODE || ':');
    DBMS_OUTPUT.PUT_LINE('--------------------------------');
    
    FOR t IN (
      SELECT T.TEACHER_NAME
      FROM TEACHER T
      JOIN PULPIT P ON T.PULPIT = P.PULPIT_NAME
      JOIN FACULTY F ON P.FACULTY = F.FACULTY_NAME
      WHERE F.FACULTY_NAME = FCODE
    ) LOOP
      DBMS_OUTPUT.PUT_LINE(t.TEACHER_NAME);
    END LOOP;
    
    IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('На факультете нет преподавателей');
    END IF;
    
    DBMS_OUTPUT.PUT_LINE('--------------------------------');
  END GET_TEACHERS;
  
  -- Реализация процедуры GET_SUBJECTS
  PROCEDURE GET_SUBJECTS(PCODE IN SUBJECT.PULPIT%TYPE) IS
  BEGIN
    DBMS_OUTPUT.PUT_LINE('Дисциплины кафедры ' || PCODE || ':');
    DBMS_OUTPUT.PUT_LINE('--------------------------------');
    
    FOR s IN (SELECT SUBJECT_NAME FROM SUBJECT WHERE PULPIT = PCODE) LOOP
      DBMS_OUTPUT.PUT_LINE(s.SUBJECT_NAME);
    END LOOP;
    
    IF SQL%ROWCOUNT = 0 THEN
      DBMS_OUTPUT.PUT_LINE('На кафедре нет дисциплин');
    END IF;
    
    DBMS_OUTPUT.PUT_LINE('--------------------------------');
  END GET_SUBJECTS;
  
  -- Реализация функции GET_NUM_TEACHERS
  FUNCTION GET_NUM_TEACHERS(FCODE IN FACULTY.FACULTY_NAME%TYPE) 
  RETURN NUMBER IS
    v_count NUMBER;
  BEGIN
    SELECT COUNT(*) INTO v_count
    FROM TEACHER T
    JOIN PULPIT P ON T.PULPIT = P.PULPIT_NAME
    JOIN FACULTY F ON P.FACULTY = F.FACULTY_NAME
    WHERE F.FACULTY_NAME = FCODE;
    
    RETURN v_count;
  END GET_NUM_TEACHERS;
  
  -- Реализация функции GET_NUM_SUBJECTS
  FUNCTION GET_NUM_SUBJECTS(PCODE IN SUBJECT.PULPIT%TYPE) 
  RETURN NUMBER IS
    v_count NUMBER;
  BEGIN
    SELECT COUNT(*) INTO v_count
    FROM SUBJECT
    WHERE PULPIT = PCODE;
    
    RETURN v_count;
  END GET_NUM_SUBJECTS;
END TEACHERS;
/

-- 6
DECLARE
  v_num_teachers NUMBER;
  v_num_subjects NUMBER;
BEGIN
  DBMS_OUTPUT.PUT_LINE('=== Демонстрация процедуры GET_TEACHERS ===');
  TEACHERS.GET_TEACHERS('Информационные технологии');
  
  DBMS_OUTPUT.PUT_LINE('=== Демонстрация процедуры GET_SUBJECTS ===');
  TEACHERS.GET_SUBJECTS('Программирование');
  
  DBMS_OUTPUT.PUT_LINE('=== Демонстрация функции GET_NUM_TEACHERS ===');
  v_num_teachers := TEACHERS.GET_NUM_TEACHERS('Экономика');
  DBMS_OUTPUT.PUT_LINE('Количество преподавателей на факультете "Экономика": ' || v_num_teachers);
  
  DBMS_OUTPUT.PUT_LINE('=== Демонстрация функции GET_NUM_SUBJECTS ===');
  v_num_subjects := TEACHERS.GET_NUM_SUBJECTS('Базы данных');
  DBMS_OUTPUT.PUT_LINE('Количество дисциплин на кафедре "Базы данных": ' || v_num_subjects);
  
  DBMS_OUTPUT.PUT_LINE('=== Проверка на несуществующих данных ===');
  TEACHERS.GET_TEACHERS('Несуществующий факультет');
  TEACHERS.GET_SUBJECTS('Несуществующая кафедра');
  v_num_teachers := TEACHERS.GET_NUM_TEACHERS('Несуществующий факультет');
  DBMS_OUTPUT.PUT_LINE('Количество преподавателей на несуществующем факультете: ' || v_num_teachers);
  v_num_subjects := TEACHERS.GET_NUM_SUBJECTS('Несуществующая кафедра');
  DBMS_OUTPUT.PUT_LINE('Количество дисциплин на несуществующей кафедре: ' || v_num_subjects);
END;
/