-- ОБЩИЕ ПРОЦЕДУРЫ
CREATE OR REPLACE PROCEDURE register_user(
    p_name IN VARCHAR2,
    p_email IN VARCHAR2,
    p_password IN VARCHAR2,
    p_role_name IN VARCHAR2 DEFAULT 'Пользователь',
    p_user_id OUT NUMBER
) AS
    v_role_id NUMBER;
BEGIN
    SELECT role_id INTO v_role_id 
    FROM Roles 
    WHERE LOWER(role_name) = LOWER(p_role_name);
    
    INSERT INTO Users (name, email, password, role_id)
    VALUES (p_name, p_email, p_password, v_role_id)
    RETURNING user_id INTO p_user_id;
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN ROLLBACK; RAISE;
END;
/

-- ДЛЯ ОБЫЧНОГО ПОЛЬЗОВАТЕЛЯ
CREATE OR REPLACE PROCEDURE book_room(
    p_user_id IN NUMBER,
    p_room_id IN NUMBER,
    p_check_in IN DATE,
    p_check_out IN DATE,
    p_booking_id OUT NUMBER
) AS
    v_price_per_night NUMBER(10,2);
    v_is_available NUMBER;
    v_conflict_count NUMBER;
    v_current_date DATE := TRUNC(SYSDATE);
    v_is_blocked NUMBER;
BEGIN
    SELECT is_blocked INTO v_is_blocked
    FROM Users
    WHERE user_id = p_user_id;
    
    IF v_is_blocked = 1 THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: Пользователь заблокирован и не может совершать бронирования');
        RAISE_APPLICATION_ERROR(-20007, 'Пользователь заблокирован');
    END IF;

    BEGIN
        SELECT price_per_night, is_available 
        INTO v_price_per_night, v_is_available
        FROM Rooms 
        WHERE room_id = p_room_id;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RAISE_APPLICATION_ERROR(-20001, 'Номер с ID ' || p_room_id || ' не найден');
    END;

    IF p_check_in < v_current_date THEN
        RAISE_APPLICATION_ERROR(-20005, 'Дата заезда не может быть в прошлом');
    END IF;
    
    IF p_check_out < v_current_date THEN
        RAISE_APPLICATION_ERROR(-20006, 'Дата выезда не может быть в прошлом');
    END IF;

    IF p_check_in >= p_check_out THEN
        RAISE_APPLICATION_ERROR(-20004, 'Дата выезда должна быть позже даты заезда');
    END IF;

    IF v_is_available = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Номер в данный момент занят');
    END IF;

    SELECT COUNT(*) INTO v_conflict_count
    FROM Bookings
    WHERE room_id = p_room_id
    AND status NOT IN ('Отменено', 'Выполнено')
    AND NOT (p_check_out <= check_in_date OR p_check_in >= check_out_date);

    IF v_conflict_count > 0 THEN
        RAISE_APPLICATION_ERROR(-20003, 'Номер уже забронирован на указанные даты');
    END IF;

    INSERT INTO Bookings (user_id, room_id, check_in_date, check_out_date, total_price)
    VALUES (p_user_id, p_room_id, p_check_in, p_check_out, 
           v_price_per_night * (p_check_out - p_check_in))
    RETURNING booking_id INTO p_booking_id;
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

CREATE OR REPLACE PROCEDURE cancel_booking(
    p_user_id IN NUMBER,
    p_booking_id IN NUMBER
) AS
    v_room_id NUMBER;
BEGIN
    SELECT room_id INTO v_room_id FROM Bookings 
    WHERE booking_id = p_booking_id AND user_id = p_user_id;
    
    UPDATE Bookings SET status = 'Отменено' 
    WHERE booking_id = p_booking_id;
    
    UPDATE Rooms SET is_available = 1 WHERE room_id = v_room_id;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN ROLLBACK; RAISE;
END;
/

CREATE OR REPLACE PROCEDURE add_review(
    p_user_id IN NUMBER,
    p_room_id IN NUMBER,
    p_rating IN NUMBER,
    p_comment IN VARCHAR2 DEFAULT NULL
) AS
    v_has_booking NUMBER;
    v_is_blocked NUMBER;
BEGIN
    SELECT is_blocked INTO v_is_blocked
    FROM Users
    WHERE user_id = p_user_id;
    
    IF v_is_blocked = 1 THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: Пользователь заблокирован и не может оставлять отзывы');
        RAISE_APPLICATION_ERROR(-20008, 'Пользователь заблокирован');
    END IF;

    SELECT COUNT(*) INTO v_has_booking
    FROM Bookings
    WHERE user_id = p_user_id AND room_id = p_room_id AND status = 'Выполнено';
    
    IF v_has_booking = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'Нельзя оставить отзыв без завершенного бронирования');
    END IF;
    
    INSERT INTO Reviews (user_id, room_id, rating, comment_text)
    VALUES (p_user_id, p_room_id, p_rating, p_comment);
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN ROLLBACK; RAISE;
END;
/

CREATE OR REPLACE PROCEDURE add_service(
    p_booking_id IN NUMBER,
    p_service_id IN NUMBER,
    p_quantity IN NUMBER DEFAULT 1
) AS
    v_room_price NUMBER(10,2);
    v_nights NUMBER;
    v_services_total NUMBER(10,2);
BEGIN
    INSERT INTO BookingServices (booking_id, service_id, quantity)
    VALUES (p_booking_id, p_service_id, p_quantity);
    
    SELECT r.price_per_night, (b.check_out_date - b.check_in_date)
    INTO v_room_price, v_nights
    FROM Bookings b
    JOIN Rooms r ON b.room_id = r.room_id
    WHERE b.booking_id = p_booking_id;
    
    SELECT NVL(SUM(rs.price * bs.quantity), 0)
    INTO v_services_total
    FROM BookingServices bs
    JOIN RoomServices rs ON bs.service_id = rs.service_id
    WHERE bs.booking_id = p_booking_id;
    
    UPDATE Bookings
    SET total_price = (v_room_price * v_nights) + v_services_total
    WHERE booking_id = p_booking_id;
    
    COMMIT;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20001, 'Бронирование или услуга не найдены');
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

-- ДЛЯ МЕНЕДЖЕРА ОТЕЛЯ:
CREATE OR REPLACE PROCEDURE change_booking_status(
    p_booking_id IN NUMBER,
    p_new_status IN VARCHAR2
) AS
    v_room_id Rooms.room_id%TYPE;
    v_active_bookings NUMBER;
BEGIN
    SELECT room_id INTO v_room_id FROM Bookings WHERE booking_id = p_booking_id;

    UPDATE Bookings
    SET status = p_new_status
    WHERE booking_id = p_booking_id;

    IF p_new_status IN ('Отменено', 'Выполнено') THEN
        SELECT COUNT(*) INTO v_active_bookings
        FROM Bookings
        WHERE room_id = v_room_id AND status = 'Подтверждено' AND booking_id != p_booking_id;

        IF v_active_bookings = 0 THEN
            UPDATE Rooms SET is_available = 1 WHERE room_id = v_room_id;
        END IF;

    ELSIF p_new_status = 'Подтверждено' THEN
        UPDATE Rooms SET is_available = 0 WHERE room_id = v_room_id;
    END IF;

    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;

CREATE OR REPLACE PROCEDURE update_room_price(
    p_room_id IN NUMBER,
    p_new_price IN NUMBER
) AS
BEGIN
    UPDATE Rooms SET price_per_night = p_new_price 
    WHERE room_id = p_room_id;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN ROLLBACK; RAISE;
END;

CREATE OR REPLACE PROCEDURE remove_service_from_booking(
    p_booking_id IN NUMBER,
    p_service_id IN NUMBER
) AS
BEGIN
    DELETE FROM BookingServices
    WHERE booking_id = p_booking_id AND service_id = p_service_id;
    
    UPDATE Bookings b
    SET total_price = (
        SELECT r.price_per_night * (b.check_out_date - b.check_in_date) + 
               NVL(SUM(rs.price * bs.quantity), 0)
        FROM Rooms r
        LEFT JOIN BookingServices bs ON b.booking_id = bs.booking_id
        LEFT JOIN RoomServices rs ON bs.service_id = rs.service_id
        WHERE r.room_id = b.room_id
        GROUP BY r.price_per_night, b.check_out_date, b.check_in_date
    )
    WHERE booking_id = p_booking_id;
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

-- ДЛЯ СПЕЦИАЛЬНЫХ АДМИНИСТРАТОРОВ:
CREATE OR REPLACE PROCEDURE toggle_user_block(
    p_user_id IN NUMBER,
    p_block IN NUMBER
) AS
BEGIN
    UPDATE Users SET is_blocked = p_block
    WHERE user_id = p_user_id;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN ROLLBACK; RAISE;
END;
/

CREATE OR REPLACE PROCEDURE assign_manager_role(
    p_user_id IN NUMBER
) AS
BEGIN
    UPDATE Users 
    SET role_id = (SELECT role_id FROM Roles WHERE role_name = 'Менеджер отеля')
    WHERE user_id = p_user_id;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN ROLLBACK; RAISE;
END;
/

CREATE OR REPLACE PROCEDURE delete_user(
    p_user_id IN NUMBER
) AS
BEGIN
    DELETE FROM BookingServices
    WHERE booking_id IN (SELECT booking_id FROM Bookings WHERE user_id = p_user_id);

    DELETE FROM Reviews WHERE user_id = p_user_id;

    DELETE FROM Bookings WHERE user_id = p_user_id;

    DELETE FROM Users WHERE user_id = p_user_id;
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN 
        ROLLBACK; 
        RAISE;
END;
/

CREATE OR REPLACE PROCEDURE add_room(
    p_room_type_id IN NUMBER,
    p_price_per_night IN NUMBER,
    p_description IN VARCHAR2,
    p_capacity IN NUMBER,
    p_room_id OUT NUMBER
) AS
BEGIN
    INSERT INTO Rooms (room_type_id, price_per_night, description, capacity)
    VALUES (p_room_type_id, p_price_per_night, p_description, p_capacity)
    RETURNING room_id INTO p_room_id;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

CREATE OR REPLACE PROCEDURE update_room(
    p_room_id IN NUMBER,
    p_room_type_id IN NUMBER DEFAULT NULL,
    p_price_per_night IN NUMBER DEFAULT NULL,
    p_description IN VARCHAR2 DEFAULT NULL,
    p_capacity IN NUMBER DEFAULT NULL,
    p_is_available IN NUMBER DEFAULT NULL
) AS
BEGIN
    UPDATE Rooms
    SET room_type_id = NVL(p_room_type_id, room_type_id),
        price_per_night = NVL(p_price_per_night, price_per_night),
        description = NVL(p_description, description),
        capacity = NVL(p_capacity, capacity),
        is_available = NVL(p_is_available, is_available)
    WHERE room_id = p_room_id;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

CREATE OR REPLACE PROCEDURE delete_room(
    p_room_id IN NUMBER
) AS
    v_booking_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_booking_count
    FROM Bookings
    WHERE room_id = p_room_id AND status NOT IN ('Отменено', 'Выполнено');
    
    IF v_booking_count > 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'Нельзя удалить номер с активными бронированиями');
    END IF;
    
    DELETE FROM Rooms WHERE room_id = p_room_id;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

CREATE OR REPLACE PROCEDURE add_new_service(
    p_service_name IN VARCHAR2,
    p_price IN NUMBER,
    p_description IN VARCHAR2 DEFAULT NULL,
    p_service_id OUT NUMBER
) AS
BEGIN
    INSERT INTO RoomServices (service_name, price, description)
    VALUES (p_service_name, p_price, p_description)
    RETURNING service_id INTO p_service_id;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

CREATE OR REPLACE PROCEDURE update_service(
    p_service_id IN NUMBER,
    p_service_name IN VARCHAR2 DEFAULT NULL,
    p_price IN NUMBER DEFAULT NULL,
    p_description IN VARCHAR2 DEFAULT NULL
) AS
BEGIN
    UPDATE RoomServices
    SET service_name = NVL(p_service_name, service_name),
        price = NVL(p_price, price),
        description = NVL(p_description, description)
    WHERE service_id = p_service_id;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

CREATE OR REPLACE PROCEDURE delete_service(
    p_service_id IN NUMBER
) AS
    v_booking_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_booking_count
    FROM BookingServices
    WHERE service_id = p_service_id;
    
    IF v_booking_count > 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Нельзя удалить услугу, связанную с бронированиями');
    END IF;
    
    DELETE FROM RoomServices WHERE service_id = p_service_id;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/