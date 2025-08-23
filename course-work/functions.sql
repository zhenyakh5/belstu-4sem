CREATE OR REPLACE FUNCTION get_available_rooms(
    p_check_in DATE,
    p_check_out DATE,
    p_user_id NUMBER DEFAULT NULL
) RETURN SYS_REFCURSOR AS
    v_cursor SYS_REFCURSOR;
    v_is_blocked NUMBER := 0;
BEGIN
    IF p_user_id IS NOT NULL THEN
        BEGIN
            SELECT is_blocked INTO v_is_blocked
            FROM Users
            WHERE user_id = p_user_id;
            
            IF v_is_blocked = 1 THEN
                DBMS_OUTPUT.PUT_LINE('Предупреждение: Пользователь заблокирован, но может просматривать доступные номера');
            END IF;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                NULL;
        END;
    END IF;

    OPEN v_cursor FOR
    SELECT 
        r.room_id, 
        rt.room_type_name, 
        r.price_per_night, 
        r.description, 
        r.capacity,
        CASE 
            WHEN r.is_available = 0 THEN 'Номер временно недоступен'
            WHEN EXISTS (
                SELECT 1 FROM Bookings b
                WHERE b.room_id = r.room_id
                AND b.status NOT IN ('Отменено', 'Выполнено')
                AND NOT (p_check_out <= b.check_in_date OR p_check_in >= b.check_out_date)
            ) THEN 'Занят на запрошенные даты'
            ELSE 'Доступен'
        END AS availability_status,
        (
            SELECT LISTAGG(
                TO_CHAR(b.check_in_date, 'DD.MM.YYYY') || ' - ' || 
                TO_CHAR(b.check_out_date, 'DD.MM.YYYY'), 
                ', '
            ) WITHIN GROUP (ORDER BY b.check_in_date)
            FROM Bookings b
            WHERE b.room_id = r.room_id
            AND b.status NOT IN ('Отменено', 'Выполнено')
            AND b.check_out_date > SYSDATE
        ) AS occupied_periods
    FROM Rooms r
    JOIN RoomTypes rt ON r.room_type_id = rt.room_type_id
    GROUP BY 
        r.room_id, 
        rt.room_type_name, 
        r.price_per_night, 
        r.description, 
        r.capacity, 
        r.is_available,
        CASE 
            WHEN r.is_available = 0 THEN 'Номер временно недоступен'
            WHEN EXISTS (
                SELECT 1 FROM Bookings b
                WHERE b.room_id = r.room_id
                AND b.status NOT IN ('Отменено', 'Выполнено')
                AND NOT (p_check_out <= b.check_in_date OR p_check_in >= b.check_out_date)
            ) THEN 'Занят на запрошенные даты'
            ELSE 'Доступен'
        END
    ORDER BY 
        CASE 
            WHEN EXISTS (
                SELECT 1 FROM Bookings b
                WHERE b.room_id = r.room_id
                AND b.status NOT IN ('Отменено', 'Выполнено')
                AND NOT (p_check_out <= b.check_in_date OR p_check_in >= b.check_out_date)
            ) THEN 1
            WHEN r.is_available = 0 THEN 2
            ELSE 0
        END,
        r.price_per_night;
    
    RETURN v_cursor;
END;
/

CREATE OR REPLACE FUNCTION get_room_reviews(
    p_room_id IN NUMBER,
    p_user_id IN NUMBER DEFAULT NULL
) RETURN SYS_REFCURSOR AS
    v_cursor SYS_REFCURSOR;
    v_is_blocked NUMBER := 0;
BEGIN
    IF p_user_id IS NOT NULL THEN
        BEGIN
            SELECT is_blocked INTO v_is_blocked
            FROM Users
            WHERE user_id = p_user_id;
            
            IF v_is_blocked = 1 THEN
                DBMS_OUTPUT.PUT_LINE('Предупреждение: Пользователь заблокирован, но может просматривать отзывы');
            END IF;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                NULL;
        END;
    END IF;

    OPEN v_cursor FOR
    SELECT u.name, r.rating, r.comment_text, r.created_at
    FROM Reviews r
    JOIN Users u ON r.user_id = u.user_id
    WHERE r.room_id = p_room_id
    ORDER BY r.created_at DESC;
    RETURN v_cursor;
END;
/

CREATE OR REPLACE FUNCTION get_user_bookings(
    p_user_id IN NUMBER
) RETURN SYS_REFCURSOR AS
    v_cursor SYS_REFCURSOR;
    v_is_blocked NUMBER;
BEGIN
    SELECT is_blocked INTO v_is_blocked
    FROM Users
    WHERE user_id = p_user_id;
    
    IF v_is_blocked = 1 THEN
        DBMS_OUTPUT.PUT_LINE('Предупреждение: Пользователь заблокирован, но может просматривать свои бронирования');
    END IF;

    OPEN v_cursor FOR
    SELECT 
        b.booking_id, 
        r.room_id, 
        rt.room_type_name,
        b.check_in_date, 
        b.check_out_date, 
        b.status, 
        b.total_price,
        (SELECT LISTAGG(rs.service_name || ' (' || bs.quantity || 'x)', ', ') 
         WITHIN GROUP (ORDER BY rs.service_name)
         FROM BookingServices bs
         JOIN RoomServices rs ON bs.service_id = rs.service_id
         WHERE bs.booking_id = b.booking_id) AS services_list
    FROM Bookings b
    JOIN Rooms r ON b.room_id = r.room_id
    JOIN RoomTypes rt ON r.room_type_id = rt.room_type_id
    WHERE b.user_id = p_user_id
    ORDER BY b.check_in_date DESC;
    RETURN v_cursor;
END;
/

CREATE OR REPLACE FUNCTION get_all_services
RETURN SYS_REFCURSOR AS
    v_cursor SYS_REFCURSOR;
BEGIN
    OPEN v_cursor FOR
    SELECT 
        service_id,
        service_name,
        price,
        description,
        'Доступна' AS availability_status
    FROM 
        RoomServices
    ORDER BY 
        service_name;
    
    RETURN v_cursor;
END;
/