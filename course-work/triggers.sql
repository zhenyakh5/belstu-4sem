CREATE OR REPLACE TRIGGER trg_check_booking_dates
BEFORE INSERT OR UPDATE ON Bookings
FOR EACH ROW
DECLARE
    v_current_date DATE := TRUNC(SYSDATE);
BEGIN
    IF :NEW.check_in_date < v_current_date THEN
        RAISE_APPLICATION_ERROR(-20010, 'Дата заезда не может быть в прошлом');
    END IF;
    
    IF :NEW.check_out_date < v_current_date THEN
        RAISE_APPLICATION_ERROR(-20011, 'Дата выезда не может быть в прошлом');
    END IF;
    
    IF :NEW.check_in_date >= :NEW.check_out_date THEN
        RAISE_APPLICATION_ERROR(-20012, 'Дата выезда должна быть позже даты заезда');
    END IF;
END;
/

CREATE OR REPLACE TRIGGER trg_check_room_availability
BEFORE INSERT OR UPDATE OF room_id, check_in_date, check_out_date ON Bookings
FOR EACH ROW
DECLARE
    v_is_available NUMBER;
    v_conflict_count NUMBER;
BEGIN
    BEGIN
        SELECT is_available INTO v_is_available
        FROM Rooms WHERE room_id = :NEW.room_id;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RAISE_APPLICATION_ERROR(-20013, 'Номер с ID ' || :NEW.room_id || ' не найден');
    END;
    
    SELECT COUNT(*) INTO v_conflict_count
    FROM Bookings
    WHERE room_id = :NEW.room_id
    AND booking_id != NVL(:NEW.booking_id, -1)
    AND status NOT IN ('Отменено', 'Выполнено')
    AND NOT (:NEW.check_out_date <= check_in_date OR :NEW.check_in_date >= check_out_date);
    
    IF v_conflict_count > 0 THEN
        RAISE_APPLICATION_ERROR(-20014, 'Номер уже забронирован на указанные даты');
    END IF;
END;
/

CREATE OR REPLACE TRIGGER trg_check_review
BEFORE INSERT ON Reviews
FOR EACH ROW
DECLARE
    v_has_completed_booking NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_has_completed_booking
    FROM Bookings
    WHERE user_id = :NEW.user_id
    AND room_id = :NEW.room_id
    AND status = 'Выполнено';
    
    IF v_has_completed_booking = 0 THEN
        RAISE_APPLICATION_ERROR(-20015, 'Вы не можете оставить отзыв о номере, в котором не останавливались');
    END IF;
    
    IF :NEW.rating NOT BETWEEN 1 AND 5 THEN
        RAISE_APPLICATION_ERROR(-20016, 'Рейтинг должен быть от 1 до 5');
    END IF;
END;
/

CREATE OR REPLACE TRIGGER trg_prevent_room_deletion
BEFORE DELETE ON Rooms
FOR EACH ROW
DECLARE
    v_active_bookings NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_active_bookings
    FROM Bookings
    WHERE room_id = :OLD.room_id
    AND status NOT IN ('Отменено', 'Выполнено');
    
    IF v_active_bookings > 0 THEN
        RAISE_APPLICATION_ERROR(-20017, 'Нельзя удалить номер с активными бронированиями');
    END IF;
END;
/

CREATE OR REPLACE TRIGGER trg_prevent_service_deletion
BEFORE DELETE ON RoomServices
FOR EACH ROW
DECLARE
    v_used_services NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_used_services
    FROM BookingServices
    WHERE service_id = :OLD.service_id;
    
    IF v_used_services > 0 THEN
        RAISE_APPLICATION_ERROR(-20018, 'Нельзя удалить услугу, связанную с бронированиями');
    END IF;
END;
/

CREATE OR REPLACE TRIGGER check_booking_status_change
BEFORE UPDATE OF status ON Bookings
FOR EACH ROW
BEGIN
        IF (:OLD.status = 'Подтверждено' AND :NEW.status = 'Ожидание') THEN
            RAISE_APPLICATION_ERROR(-20019, 'Нельзя вернуть подтвержденное бронирование в статус "Ожидание"');
        
        ELSIF (:OLD.status = 'Отменено' AND :NEW.status IN ('Ожидание', 'Подтверждено')) THEN
            RAISE_APPLICATION_ERROR(-20020, 'Отмененное бронирование нельзя восстановить');
            
        ELSIF (:OLD.status = 'Выполнено' AND :NEW.status IN ('Ожидание', 'Подтверждено', 'Отменено')) THEN
            RAISE_APPLICATION_ERROR(-20021, 'Выполненное бронирование нельзя изменить');
            
        ELSIF (:NEW.status = 'Подтверждено' AND :OLD.status IN ('Отменено', 'Выполнено')) THEN
            RAISE_APPLICATION_ERROR(-20022, 'Нельзя подтвердить отмененное или выполненное бронирование');
            
        ELSIF (:NEW.status = 'Отменено' AND :OLD.status = 'Выполнено') THEN
            RAISE_APPLICATION_ERROR(-20023, 'Нельзя отменить выполненное бронирование');
        END IF;
END;
/