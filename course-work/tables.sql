CREATE TABLE Roles (
    role_id NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    role_name VARCHAR2(50) NOT NULL
) TABLESPACE hotel_db_data;

INSERT INTO ROLES (role_name) VALUES ('Пользователь');
INSERT INTO ROLES (role_name) VALUES ('Менеджер отеля');
INSERT INTO ROLES (role_name) VALUES ('Специальный администратор');

CREATE TABLE Users (
    user_id NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR2(100) NOT NULL,
    email VARCHAR2(100) UNIQUE NOT NULL,
    password VARCHAR2(255) NOT NULL,
    role_id NUMBER NOT NULL,
    created_at TIMESTAMP DEFAULT SYSTIMESTAMP,
    is_blocked NUMBER(1) DEFAULT 0 CHECK (is_blocked IN (0, 1)),
    CONSTRAINT fk_user_role FOREIGN KEY (role_id) REFERENCES Roles(role_id)
) TABLESPACE hotel_db_data;

CREATE TABLE RoomTypes (
    room_type_id NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    room_type_name VARCHAR2(50) NOT NULL
) TABLESPACE hotel_db_data;

INSERT INTO RoomTypes(room_type_name) VALUES ('Эконом');
INSERT INTO RoomTypes(room_type_name) VALUES ('Стандарт');
INSERT INTO RoomTypes(room_type_name) VALUES ('Люкс');
INSERT INTO RoomTypes(room_type_name) VALUES ('Делюкс');
INSERT INTO RoomTypes(room_type_name) VALUES ('Президентский');

CREATE TABLE Rooms (
    room_id NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    room_type_id NUMBER NOT NULL,
    price_per_night NUMBER(10,2) NOT NULL,
    description VARCHAR2(500),
    capacity NUMBER(2) NOT NULL,
    is_available NUMBER(1) DEFAULT 1 CHECK (is_available IN (0, 1)),
    CONSTRAINT fk_room_type FOREIGN KEY (room_type_id) REFERENCES RoomTypes(room_type_id)
) TABLESPACE hotel_db_data;

CREATE TABLE Bookings (
    booking_id NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    user_id NUMBER NOT NULL,
    room_id NUMBER NOT NULL,
    check_in_date DATE NOT NULL,
    check_out_date DATE NOT NULL,
    status VARCHAR2(25) DEFAULT 'Ожидание' CHECK (status IN ('Ожидание', 'Подтверждено', 'Отменено', 'Выполнено')),
    total_price NUMBER(10,2) NOT NULL,
    created_at TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_booking_user FOREIGN KEY (user_id) REFERENCES Users(user_id),
    CONSTRAINT fk_booking_room FOREIGN KEY (room_id) REFERENCES Rooms(room_id),
    CONSTRAINT chk_dates CHECK (check_out_date > check_in_date)
) TABLESPACE hotel_db_data;

CREATE TABLE RoomServices (
    service_id NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    service_name VARCHAR2(100) NOT NULL,
    price NUMBER(10,2) NOT NULL,
    description VARCHAR2(500)
) TABLESPACE hotel_db_data;

CREATE TABLE BookingServices (
    booking_service_id NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    booking_id NUMBER NOT NULL,
    service_id NUMBER NOT NULL,
    quantity NUMBER DEFAULT 1,
    CONSTRAINT fk_bs_booking FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id),
    CONSTRAINT fk_bs_service FOREIGN KEY (service_id) REFERENCES RoomServices(service_id)
) TABLESPACE hotel_db_data;

CREATE TABLE Reviews (
    review_id NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    user_id NUMBER NOT NULL,
    room_id NUMBER NOT NULL,
    rating NUMBER(1) NOT NULL CHECK (rating BETWEEN 1 AND 5),
    comment_text VARCHAR2(1000),
    created_at TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_review_user FOREIGN KEY (user_id) REFERENCES Users(user_id),
    CONSTRAINT fk_review_room FOREIGN KEY (room_id) REFERENCES Rooms(room_id)
) TABLESPACE hotel_db_data;