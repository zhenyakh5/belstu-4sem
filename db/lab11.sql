-- 1
DECLARE
    v_teacher_name TEACHER.TEACHER_NAME%TYPE;
    v_pulpit TEACHER.PULPIT%TYPE;
BEGIN
    SELECT TEACHER_NAME, PULPIT INTO v_teacher_name, v_pulpit
    FROM TEACHER
    WHERE TEACHER_NAME = 'Иванов Иван Иванович';
    
    DBMS_OUTPUT.PUT_LINE('Преподаватель: ' || v_teacher_name || ', кафедра: ' || v_pulpit);
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Преподаватель не найден');
END;
/

-- 2
DECLARE
    v_teacher_name TEACHER.TEACHER_NAME%TYPE;
    v_pulpit TEACHER.PULPIT%TYPE;
BEGIN
    SELECT TEACHER_NAME, PULPIT INTO v_teacher_name, v_pulpit
    FROM TEACHER
    WHERE PULPIT LIKE '%Програм%';
    
    DBMS_OUTPUT.PUT_LINE('Преподаватель: ' || v_teacher_name || ', кафедра: ' || v_pulpit);
EXCEPTION
    WHEN TOO_MANY_ROWS THEN
        DBMS_OUTPUT.PUT_LINE('Найдено несколько преподавателей');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: ' || SQLERRM || ', код: ' || SQLCODE);
END;
/

-- 3
DECLARE
    v_teacher_name TEACHER.TEACHER_NAME%TYPE;
BEGIN
    SELECT TEACHER_NAME INTO v_teacher_name
    FROM TEACHER
    WHERE PULPIT = 'Программирование';
    
    DBMS_OUTPUT.PUT_LINE('Преподаватель: ' || v_teacher_name);
EXCEPTION
    WHEN TOO_MANY_ROWS THEN
        DBMS_OUTPUT.PUT_LINE('Найдено несколько преподавателей на кафедре Программирование');
END;
/

-- 4
DECLARE
    v_auditorium_name AUDITORIUM.AUDITORIUM_NAME%TYPE;
    v_capacity AUDITORIUM.AUDITORIUM_CAPACITY%TYPE;
BEGIN
    SELECT AUDITORIUM_NAME, AUDITORIUM_CAPACITY INTO v_auditorium_name, v_capacity
    FROM AUDITORIUM
    WHERE AUDITORIUM_NAME = '999';
    
    DBMS_OUTPUT.PUT_LINE('Аудитория: ' || v_auditorium_name || ', вместимость: ' || v_capacity);
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Аудитория не найдена');
        DBMS_OUTPUT.PUT_LINE('Количество обработанных строк: ' || SQL%ROWCOUNT);
END;
/

-- 5
BEGIN
    -- Нарушение внешнего ключа
    INSERT INTO TEACHER VALUES ('Новый преподаватель', 'Несуществующая кафедра');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка вставки: ' || SQLERRM);
END;
/

BEGIN
    -- Нарушение первичного ключа
    INSERT INTO FACULTY VALUES ('Информационные технологии');
EXCEPTION
    WHEN DUP_VAL_ON_INDEX THEN
        DBMS_OUTPUT.PUT_LINE('Факультет уже существует');
END;
/

BEGIN
    -- Нарушение NOT NULL
    INSERT INTO AUDITORIUM VALUES (NULL, 30, 'ЛК');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: ' || SQLERRM);
END;
/

-- 6
DECLARE
    CURSOR c_teachers IS
        SELECT TEACHER_NAME, PULPIT
        FROM TEACHER;
    
    v_teacher_name TEACHER.TEACHER_NAME%TYPE;
    v_pulpit TEACHER.PULPIT%TYPE;
BEGIN
    OPEN c_teachers;
    LOOP
        FETCH c_teachers INTO v_teacher_name, v_pulpit;
        EXIT WHEN c_teachers%NOTFOUND;
        DBMS_OUTPUT.PUT_LINE('Преподаватель: ' || v_teacher_name || ', кафедра: ' || v_pulpit);
    END LOOP;
    CLOSE c_teachers;
END;
/

-- 7
DECLARE
    CURSOR c_subjects IS
        SELECT SUBJECT_NAME, PULPIT
        FROM SUBJECT;
    
    r_subject c_subjects%ROWTYPE;
BEGIN
    OPEN c_subjects;
    FETCH c_subjects INTO r_subject;
    WHILE c_subjects%FOUND LOOP
        DBMS_OUTPUT.PUT_LINE('Предмет: ' || r_subject.SUBJECT_NAME || ', кафедра: ' || r_subject.PULPIT);
        FETCH c_subjects INTO r_subject;
    END LOOP;
    CLOSE c_subjects;
END;
/

-- 8
DECLARE
    CURSOR c_auditoriums(p_min NUMBER, p_max NUMBER) IS
        SELECT AUDITORIUM_NAME, AUDITORIUM_CAPACITY
        FROM AUDITORIUM
        WHERE AUDITORIUM_CAPACITY BETWEEN p_min AND p_max
        ORDER BY AUDITORIUM_CAPACITY;
    
    -- Способ 1: LOOP-цикл
    PROCEDURE print_auditoriums_loop(p_min NUMBER, p_max NUMBER) IS
        v_name AUDITORIUM.AUDITORIUM_NAME%TYPE;
        v_capacity AUDITORIUM.AUDITORIUM_CAPACITY%TYPE;
    BEGIN
        DBMS_OUTPUT.PUT_LINE('Аудитории с вместимостью от ' || p_min || ' до ' || p_max || ':');
        FOR r_aud IN c_auditoriums(p_min, p_max) LOOP
            DBMS_OUTPUT.PUT_LINE(r_aud.AUDITORIUM_NAME || ' - ' || r_aud.AUDITORIUM_CAPACITY);
        END LOOP;
    END;
    
    -- Способ 2: WHILE-цикл
    PROCEDURE print_auditoriums_while(p_min NUMBER, p_max NUMBER) IS
        v_name AUDITORIUM.AUDITORIUM_NAME%TYPE;
        v_capacity AUDITORIUM.AUDITORIUM_CAPACITY%TYPE;
    BEGIN
        DBMS_OUTPUT.PUT_LINE('Аудитории с вместимостью от ' || p_min || ' до ' || p_max || ':');
        FOR r_aud IN c_auditoriums(p_min, p_max) LOOP
            DBMS_OUTPUT.PUT_LINE(r_aud.AUDITORIUM_NAME || ' - ' || r_aud.AUDITORIUM_CAPACITY);
        END LOOP;
    END;
    
    -- Способ 3: FOR-цикл
    PROCEDURE print_auditoriums_for(p_min NUMBER, p_max NUMBER) IS
    BEGIN
        DBMS_OUTPUT.PUT_LINE('Аудитории с вместимостью от ' || p_min || ' до ' || p_max || ':');
        FOR r_aud IN c_auditoriums(p_min, p_max) LOOP
            DBMS_OUTPUT.PUT_LINE(r_aud.AUDITORIUM_NAME || ' - ' || r_aud.AUDITORIUM_CAPACITY);
        END LOOP;
    END;

BEGIN
    print_auditoriums_loop(0, 20);
    print_auditoriums_while(21, 30);
    print_auditoriums_for(31, 60);
    print_auditoriums_loop(61, 80);
    print_auditoriums_while(81, 1000);
END;
/

-- 9
DECLARE
    TYPE t_auditorium_cursor IS REF CURSOR;
    c_auditorium t_auditorium_cursor;
    
    v_name AUDITORIUM.AUDITORIUM_NAME%TYPE;
    v_capacity AUDITORIUM.AUDITORIUM_CAPACITY%TYPE;
    v_type AUDITORIUM.AUDITORIUM_TYPE%TYPE;
BEGIN
    -- Открываем курсор с параметром
    OPEN c_auditorium FOR
        SELECT AUDITORIUM_NAME, AUDITORIUM_CAPACITY, AUDITORIUM_TYPE
        FROM AUDITORIUM
        WHERE AUDITORIUM_TYPE = :p_type;
    
    DBMS_OUTPUT.PUT_LINE('Аудитории типа ' || :p_type || ':');
    LOOP
        FETCH c_auditorium INTO v_name, v_capacity, v_type;
        EXIT WHEN c_auditorium%NOTFOUND;
        DBMS_OUTPUT.PUT_LINE(v_name || ' - ' || v_capacity || ' мест, тип: ' || v_type);
    END LOOP;
    CLOSE c_auditorium;
END;
/

-- 10
DECLARE
    CURSOR c_auditoriums(p_min NUMBER, p_max NUMBER) IS
        SELECT AUDITORIUM_NAME, AUDITORIUM_CAPACITY
        FROM AUDITORIUM
        WHERE AUDITORIUM_CAPACITY BETWEEN p_min AND p_max
        FOR UPDATE;
BEGIN
    FOR r_aud IN c_auditoriums(40, 80) LOOP
        UPDATE AUDITORIUM
        SET AUDITORIUM_CAPACITY = r_aud.AUDITORIUM_CAPACITY * 0.9
        WHERE CURRENT OF c_auditoriums;
        
        DBMS_OUTPUT.PUT_LINE('Обновлена аудитория ' || r_aud.AUDITORIUM_NAME || 
                            ': было ' || r_aud.AUDITORIUM_CAPACITY || 
                            ', стало ' || r_aud.AUDITORIUM_CAPACITY * 0.9);
    END LOOP;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Обновление завершено');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Ошибка: ' || SQLERRM);
END;
/