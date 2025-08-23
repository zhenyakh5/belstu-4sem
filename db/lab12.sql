SELECT 
    TEACHER_NAME AS NAME,
    TO_CHAR(BIRTHDAY, 'Day') AS BIRTH_DAY
FROM 
    TEACHER
WHERE 
    TO_CHAR(BIRTHDAY, 'D') = '1'
    OR TO_CHAR(BIRTHDAY, 'Day') LIKE 'Monday%';

-- 3
CREATE OR REPLACE VIEW TEACHERS_NEXT_MONTH_BIRTH AS
SELECT 
    TEACHER_NAME,
    TO_CHAR(BIRTHDAY, 'DD/MM/YYYY') AS BIRTHDAY_FORMATTED
FROM 
    TEACHER
WHERE 
    TO_CHAR(BIRTHDAY, 'MM') = TO_CHAR(ADD_MONTHS(SYSDATE, 1), 'MM');

SELECT * FROM TEACHERS_NEXT_MONTH_BIRTH;

-- 4
CREATE OR REPLACE VIEW TEACHERS_BY_BIRTH_MONTH AS
SELECT 
    TO_CHAR(BIRTHDAY, 'Month') AS MONTH_NAME,
    COUNT(*) AS TEACHER_COUNT
FROM 
    TEACHER
GROUP BY 
    TO_CHAR(BIRTHDAY, 'Month')
ORDER BY 
    TO_DATE('01 ' || TO_CHAR(BIRTHDAY, 'Month') || ' 2000', 'DD Month YYYY');

-- Проверка
SELECT * FROM TEACHERS_BY_BIRTH_MONTH;

-- 5
DECLARE
    CURSOR jubilee_teachers IS
        SELECT 
            TEACHER_NAME,
            EXTRACT(YEAR FROM SYSDATE) + 1 - EXTRACT(YEAR FROM BIRTHDAY) AS AGE_NEXT_YEAR
        FROM 
            TEACHER
        WHERE 
            MOD(EXTRACT(YEAR FROM SYSDATE) + 1 - EXTRACT(YEAR FROM BIRTHDAY), 5) = 0;
    
    v_teacher_name TEACHER.TEACHER_NAME%TYPE;
    v_age NUMBER;
BEGIN
    OPEN jubilee_teachers;
    DBMS_OUTPUT.PUT_LINE('Преподаватели с юбилеем в следующем году:');
    DBMS_OUTPUT.PUT_LINE('-----------------------------------------');
    
    LOOP
        FETCH jubilee_teachers INTO v_teacher_name, v_age;
        EXIT WHEN jubilee_teachers%NOTFOUND;
        
        DBMS_OUTPUT.PUT_LINE(v_teacher_name || ' - исполняется ' || v_age || ' лет');
    END LOOP;
    
    CLOSE jubilee_teachers;
END;

-- 6
DECLARE
    CURSOR salary_stats IS
        SELECT 
            P.PULPIT_NAME,
            F.FACULTY_NAME,
            FLOOR(AVG(T.SALARY)) AS AVG_SALARY
        FROM 
            TEACHER T
            JOIN PULPIT P ON T.PULPIT = P.PULPIT_NAME
            JOIN FACULTY F ON P.FACULTY = F.FACULTY_NAME
        GROUP BY 
            ROLLUP(F.FACULTY_NAME, P.PULPIT_NAME)
        ORDER BY 
            F.FACULTY_NAME, P.PULPIT_NAME;
    
    v_pulpit PULPIT.PULPIT_NAME%TYPE;
    v_faculty FACULTY.FACULTY_NAME%TYPE;
    v_avg_salary NUMBER;
BEGIN
    OPEN salary_stats;
    DBMS_OUTPUT.PUT_LINE('Средняя зарплата по кафедрам и факультетам:');
    DBMS_OUTPUT.PUT_LINE('--------------------------------------------');
    
    LOOP
        FETCH salary_stats INTO v_pulpit, v_faculty, v_avg_salary;
        EXIT WHEN salary_stats%NOTFOUND;
        
        IF v_pulpit IS NULL AND v_faculty IS NULL THEN
            DBMS_OUTPUT.PUT_LINE('Общая средняя зарплата по всем факультетам: ' || v_avg_salary);
        ELSIF v_pulpit IS NULL THEN
            DBMS_OUTPUT.PUT_LINE('Средняя зарплата по факультету ' || v_faculty || ': ' || v_avg_salary);
        ELSE
            DBMS_OUTPUT.PUT_LINE('Кафедра ' || v_pulpit || ': ' || v_avg_salary);
        END IF;
    END LOOP;
    
    CLOSE salary_stats;
END;

-- 7
DECLARE
    v_numerator NUMBER := 100;
    v_denominator NUMBER := 0;
    v_result NUMBER;
    
    e_custom_error EXCEPTION;
BEGIN
    BEGIN
        IF v_denominator = 0 THEN
            RAISE e_custom_error;
        END IF;
        
        v_result := v_numerator / v_denominator;
        DBMS_OUTPUT.PUT_LINE('Результат: ' || v_result);
    EXCEPTION
        WHEN ZERO_DIVIDE THEN
            DBMS_OUTPUT.PUT_LINE('Ошибка: деление на ноль (ZERO_DIVIDE)');
        WHEN e_custom_error THEN
            DBMS_OUTPUT.PUT_LINE('Ошибка: делитель равен нулю (пользовательское исключение)');
    END;
END;

-- 8
DECLARE
    v_teacher_name TEACHER.TEACHER_NAME%TYPE;
    v_teacher_id NUMBER := 999; -- Несуществующий ID
BEGIN
    BEGIN
        SELECT TEACHER_NAME INTO v_teacher_name
        FROM TEACHER
        WHERE ROWNUM = v_teacher_id;
        
        DBMS_OUTPUT.PUT_LINE('Найден преподаватель: ' || v_teacher_name);
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            DBMS_OUTPUT.PUT_LINE('Преподаватель не найден!');
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Произошла ошибка: ' || SQLERRM);
    END;
END;


-- 9
DECLARE
    e_main_exception EXCEPTION;
    e_nested_exception EXCEPTION;
    PRAGMA EXCEPTION_INIT(e_main_exception, -20001);
    PRAGMA EXCEPTION_INIT(e_nested_exception, -20001);
    
    v_test NUMBER := 1;
BEGIN
    BEGIN
        IF v_test = 1 THEN
            RAISE e_nested_exception;
        END IF;
    EXCEPTION
        WHEN e_nested_exception THEN
            DBMS_OUTPUT.PUT_LINE('Обработка во вложенном блоке');
            RAISE; -- Пробрасываем исключение дальше
    END;
EXCEPTION
    WHEN e_main_exception THEN
        DBMS_OUTPUT.PUT_LINE('Обработка в основном блоке');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Необработанное исключение: ' || SQLERRM);
END;

-- 10
DECLARE
    v_max_salary NUMBER;
BEGIN
    BEGIN
        SELECT MAX(SALARY) INTO v_max_salary
        FROM TEACHER;
        
        DBMS_OUTPUT.PUT_LINE('Максимальная зарплата: ' || v_max_salary);
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            DBMS_OUTPUT.PUT_LINE('Ошибка NO_DATA_FOUND сработала');
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Другая ошибка: ' || SQLERRM);
    END;
    
    DBMS_OUTPUT.PUT_LINE('Результат: ' || NVL(TO_CHAR(v_max_salary), 'NULL'));
END;