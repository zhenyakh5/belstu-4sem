-- ОБЩИЕ
-- РЕГИСТРАЦИЯ
DECLARE
  v_user_id NUMBER;
BEGIN
  SYSTEM.register_user(
    p_name      => 'Евгений Харченко',
    p_email     => 'zhkharchenkooooo@gmail.com',
    p_password  => 'zhenya130905',
    p_user_id   => v_user_id
  );
  DBMS_OUTPUT.PUT_LINE('Создан пользователь с ID: ' || v_user_id);
END;

SELECT * FROM Users;

-- ПОЛУЧИТЬ СПИСОК ДОСТУПНЫХ НОМЕРОВ
DECLARE
    v_cursor SYS_REFCURSOR;
    v_room_id NUMBER;
    v_room_type VARCHAR2(50);
    v_price NUMBER(10,2);
    v_description VARCHAR2(500);
    v_capacity NUMBER(2);
    v_status VARCHAR2(50);
    v_periods VARCHAR2(1000);
BEGIN
    v_cursor := SYSTEM.get_available_rooms(
        TO_DATE('2025-05-27', 'YYYY-MM-DD'),
        TO_DATE('2025-05-31', 'YYYY-MM-DD')
    );
    
    DBMS_OUTPUT.PUT_LINE('ID | Тип | Цена | Статус | Занятые периоды');
    DBMS_OUTPUT.PUT_LINE('------------------------------------------');
    
    LOOP
        FETCH v_cursor INTO v_room_id, v_room_type, v_price, 
                            v_description, v_capacity, v_status, v_periods;
        EXIT WHEN v_cursor%NOTFOUND;
        
        DBMS_OUTPUT.PUT_LINE(
            RPAD(v_room_id, 3) || ' | ' || 
            RPAD(v_room_type, 10) || ' | ' || 
            RPAD(v_price, 6) || ' | ' ||
            RPAD(v_status, 25) || ' | ' ||
            NVL(v_periods, '-')
        );
    END LOOP;
    
    CLOSE v_cursor;
END;
/

SELECT * FROM Rooms;
SELECT * FROM Bookings;

-- ДЛЯ ПОЛЬЗОВАТЕЛЯ
-- ЗАБРОНИРОВАТЬ НОМЕР
DECLARE
    v_booking_id NUMBER;
BEGIN
    SYSTEM.book_room(
        p_user_id => 114405,
        p_room_id => 42,
        p_check_in => TO_DATE('2025-06-20', 'YYYY-MM-DD'),
        p_check_out => TO_DATE('2025-06-23', 'YYYY-MM-DD'),
        p_booking_id => v_booking_id
    );
    DBMS_OUTPUT.PUT_LINE('Бронирование создано с ID: ' || v_booking_id);
END;

select * from Users

SELECT * FROM Rooms;
SELECT * FROM Bookings;

-- ОТМЕНИТЬ БРОНЬ
BEGIN
    SYSTEM.cancel_booking(
        p_user_id => 114403,
        p_booking_id => 82
    );
    DBMS_OUTPUT.PUT_LINE('Бронирование отменено');
END;

SELECT * FROM Rooms;
SELECT * FROM Bookings;

-- ДОБАВИТЬ ОТЗЫВ
BEGIN
    SYSTEM.add_review(
        p_user_id => 114403,
        p_room_id => 41,
        p_rating => 5,
        p_comment => 'Отличный номер, прекрасный сервис!'
    );
    DBMS_OUTPUT.PUT_LINE('Отзыв добавлен');
END;

SELECT * FROM Reviews;

-- ПРОСМОТР ВСЕХ УСЛУГ
DECLARE
    v_cursor SYS_REFCURSOR;
    v_service_id NUMBER;
    v_service_name VARCHAR2(100);
    v_price NUMBER(10,2);
    v_description VARCHAR2(500);
    v_status VARCHAR2(50);
BEGIN
    v_cursor := SYSTEM.get_all_services();
    
    DBMS_OUTPUT.PUT_LINE('СПИСОК ВСЕХ ДОСТУПНЫХ УСЛУГ');
    DBMS_OUTPUT.PUT_LINE('─────────────────────────────────────────────────────');
    DBMS_OUTPUT.PUT_LINE(RPAD('ID', 5) || RPAD('НАЗВАНИЕ УСЛУГИ', 25) || 
                        RPAD('ЦЕНА', 10) || 'ОПИСАНИЕ');
    DBMS_OUTPUT.PUT_LINE('─────────────────────────────────────────────────────');
    
    LOOP
        FETCH v_cursor INTO v_service_id, v_service_name, v_price, v_description, v_status;
        EXIT WHEN v_cursor%NOTFOUND;
        
        DBMS_OUTPUT.PUT_LINE(
            RPAD(v_service_id, 5) || 
            RPAD(v_service_name, 25) || 
            RPAD(v_price, 10) || 
            v_description
        );
    END LOOP;
    
    DBMS_OUTPUT.PUT_LINE('─────────────────────────────────────────────────────');
    DBMS_OUTPUT.PUT_LINE('Всего услуг: ' || v_cursor%ROWCOUNT);
    
    CLOSE v_cursor;
END;

-- ДОБАВЛЕНИЕ УСЛУГИ К БРОНИРОВАНИЯ
BEGIN
    SYSTEM.add_service(
        p_booking_id => 84,
        p_service_id => 41,
        p_quantity => 1
    );
    DBMS_OUTPUT.PUT_LINE('Услуга добавлена к бронированию');
END;

SELECT * FROM RoomServices;
SELECT * FROM Bookings;
SELECT * FROM BookingServices;

-- ПОЛУЧИТЬ ОТЗЫВЫ
SELECT * FROM Reviews;

DECLARE
    v_cursor SYS_REFCURSOR;
    v_name VARCHAR2(100);
    v_rating NUMBER;
    v_comment VARCHAR2(500);
    v_created_at DATE;
BEGIN
    v_cursor := SYSTEM.get_room_reviews(
        p_room_id => 1
    );
    
    LOOP
        FETCH v_cursor INTO v_name, v_rating, v_comment, v_created_at;
        EXIT WHEN v_cursor%NOTFOUND;
        DBMS_OUTPUT.PUT_LINE('Пользователь: ' || v_name || ', Оценка: ' || v_rating || 
                            ', Дата: ' || TO_CHAR(v_created_at, 'DD.MM.YYYY'));
        DBMS_OUTPUT.PUT_LINE('Комментарий: ' || v_comment);
        DBMS_OUTPUT.PUT_LINE('---------------------');
    END LOOP;
    
    CLOSE v_cursor;
END;

-- МЕНЕДЖЕР ОТЕЛЯ
-- ИЗМЕНЕНИЯ СТАТУСА БРОНИРОВАНИЯ
BEGIN
    SYSTEM.change_booking_status(
        p_booking_id => 102,
        p_new_status => 'Отменено'
    );
    DBMS_OUTPUT.PUT_LINE('Статус бронирования изменен');
END;

SELECT * FROM Bookings;

-- УДАЛЕНИЕ УСЛУГИ ИЗ БРОНИРОВАНИЯ
BEGIN
    SYSTEM.remove_service_from_booking(
        p_booking_id => 43,
        p_service_id => 4
    );
    DBMS_OUTPUT.PUT_LINE('Услуга удалена из бронирования');
END;

SELECT * FROM Bookings;
SELECT * FROM BookingServices;

-- ОБНОВЛЕНИЕ ЦЕНЫ НОМЕРА
BEGIN
    SYSTEM.update_room_price(
        p_room_id => 4,
        p_new_price => 15000
    );
    DBMS_OUTPUT.PUT_LINE('Цена номера обновлена');
END;

SELECT * FROM Rooms;

-- ПОЛУЧЕНИЕ БРОНИРОВАНИЙ ПОЛЬЗОВАТЕЛЯ
SELECT * FROM Bookings;

DECLARE
    v_cursor SYS_REFCURSOR;
    v_booking_id NUMBER;
    v_room_id NUMBER;
    v_room_type_name VARCHAR2(100);
    v_check_in DATE;
    v_check_out DATE;
    v_status VARCHAR2(50);
    v_total_price NUMBER;
    v_services VARCHAR2(1000);
BEGIN
    v_cursor := SYSTEM.get_user_bookings(
        p_user_id => 114405
    );
    
    LOOP
        FETCH v_cursor INTO v_booking_id, v_room_id, v_room_type_name, v_check_in, v_check_out, v_status, v_total_price, v_services;
        EXIT WHEN v_cursor%NOTFOUND;
        DBMS_OUTPUT.PUT_LINE('Бронирование #' || v_booking_id || ', Номер: ' || v_room_id || 
                            ', Тип: ' || v_room_type_name);
        DBMS_OUTPUT.PUT_LINE('Даты: ' || TO_CHAR(v_check_in, 'DD.MM.YYYY') || ' - ' || 
                            TO_CHAR(v_check_out, 'DD.MM.YYYY'));
        DBMS_OUTPUT.PUT_LINE('Статус: ' || v_status || ', Сумма: ' || v_total_price);
        DBMS_OUTPUT.PUT_LINE('Услуги: ' || NVL(v_services, 'нет'));
        DBMS_OUTPUT.PUT_LINE('---------------------');
    END LOOP;
    
    CLOSE v_cursor;
END;


-- ДЛЯ АДМИНИСТРАТОРОВ
-- БЛОКИРОВКА/РАЗБЛОКИРОВКА ПОЛЬЗОВАТЕЛЕЙ
BEGIN
    SYSTEM.toggle_user_block(
        p_user_id => 114364,
        p_block => 0
    );
    DBMS_OUTPUT.PUT_LINE('Статус блокировки пользователя изменен');
END;

SELECT * FROM Users;

-- НАЗНАЧИТЬ МЕНЕДЖЕРОМ
BEGIN
    SYSTEM.assign_manager_role(
        p_user_id => 114402
    );
    DBMS_OUTPUT.PUT_LINE('Пользователю назначена роль менеджера');
END;

SELECT * FROM Users;

-- УДАЛИТЬ ПОЛЬЗОВАТЕЛЯ (Надо сделать удаление пользователей, у которых имеются бронирования или отзывы)
BEGIN
    SYSTEM.delete_user(
        p_user_id => 114381
    );
    DBMS_OUTPUT.PUT_LINE('Пользователь удален');
END;

SELECT * FROM Users;

-- ДОБАВИТЬ НОМЕР
DECLARE
    v_room_id NUMBER;
BEGIN
    SYSTEM.add_room(
        p_room_type_id => 2,
        p_price_per_night => 300,
        p_description => 'Стандартный двухместный номер с видом на реку',
        p_capacity => 2,
        p_room_id => v_room_id
    );
    DBMS_OUTPUT.PUT_LINE('Добавлен новый номер с ID: ' || v_room_id);
END;
/

SELECT * FROM Rooms;

-- ОТРЕДАКТИРОВАТЬ НОМЕР
BEGIN
    SYSTEM.update_room(
        p_room_id => 41,
        p_description => 'Обычный двухместный номер'
    );
    DBMS_OUTPUT.PUT_LINE('Данные номера обновлены');
END;
/

SELECT * FROM Rooms;

-- УДАЛИТЬ НОМЕР
BEGIN
    delete_room(
        p_room_id => 21
    );
    DBMS_OUTPUT.PUT_LINE('Номер успешно удален');
END;
/

SELECT * FROM Rooms;

-- ДОБАВИТЬ НОВУЮ УСЛУГУ
DECLARE
    v_service_id NUMBER;
BEGIN
    SYSTEM.add_new_service(
        p_service_name => 'Обед',
        p_price => 800,
        p_description => 'Обэд с доставкой в номер',
        p_service_id => v_service_id
    );
    DBMS_OUTPUT.PUT_LINE('Добавлена новая услуга с ID: ' || v_service_id);
END;
/

SELECT * FROM RoomServices;

-- ОТРЕДАКТИРОВАТЬ УСЛУГУ
BEGIN
    SYSTEM.update_service(
        p_service_id => 1,
        p_price => 900,
        p_description => 'Расширенный континентальный завтрак с доставкой в номер'
    );
    DBMS_OUTPUT.PUT_LINE('Данные услуги обновлены');
END;
/

SELECT * FROM RoomServices;

-- УДАЛИТЬ УСЛУГУ
BEGIN
    SYSTEM.delete_service(
        p_service_id => 21
    );
    DBMS_OUTPUT.PUT_LINE('Услуга успешно удалена');
END;
/

SELECT * FROM RoomServices;