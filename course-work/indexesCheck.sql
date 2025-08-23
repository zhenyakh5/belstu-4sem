BEGIN
    FOR i IN 1..100000 LOOP
        INSERT INTO Users (name, email, password, role_id, created_at, is_blocked)
        VALUES (
            'User' || DBMS_RANDOM.STRING('L', 5) || i,
            LOWER(DBMS_RANDOM.STRING('L', 5) || i || '@example.com'),
            DBMS_RANDOM.STRING('X', 20),
            1,
            SYSTIMESTAMP - DBMS_RANDOM.VALUE(0, 365),
            CASE WHEN DBMS_RANDOM.VALUE(0, 1) > 0.95 THEN 0 ELSE 0 END
        );
        
    END LOOP;
    COMMIT;
END;
/

select * from Users where is_blocked = 0;
CREATE INDEX idx_users_is_blocked ON Users(is_blocked) TABLESPACE hotel_db_data;
