-- 1
CREATE TABLE T_RANGE (
    id NUMBER,
    name VARCHAR2(100),
    value NUMBER,
    created_date DATE
)
PARTITION BY RANGE (value) (
    PARTITION p_low VALUES LESS THAN (100),
    PARTITION p_medium VALUES LESS THAN (500),
    PARTITION p_high VALUES LESS THAN (1000),
    PARTITION p_max VALUES LESS THAN (MAXVALUE)
);

-- 2
CREATE TABLE T_INTERVAL (
    id NUMBER,
    event_name VARCHAR2(100),
    event_date DATE,
    details VARCHAR2(500)
)
PARTITION BY RANGE (event_date)
INTERVAL (NUMTOYMINTERVAL(1, 'MONTH'))
(
    PARTITION p_initial VALUES LESS THAN (TO_DATE('01-01-2026', 'DD-MM-YYYY'))
);

-- 3
CREATE TABLE T_HASH (
    id NUMBER,
    username VARCHAR2(50),
    email VARCHAR2(100),
    registration_date DATE
)
PARTITION BY HASH (username)
PARTITIONS 4;

-- 4
CREATE TABLE T_LIST (
    id NUMBER,
    product_code CHAR(5),
    product_name VARCHAR2(100),
    price NUMBER(10,2)
)
PARTITION BY LIST (product_code) (
    PARTITION p_electronics VALUES ('ELEC1', 'ELEC2', 'ELEC3'),
    PARTITION p_clothing VALUES ('CLOTH', 'SHOES'),
    PARTITION p_other VALUES (DEFAULT)
);

-- 5
-- Вставка данных в T_RANGE
INSERT INTO T_RANGE VALUES (1, 'Item 1', 50, SYSDATE);
INSERT INTO T_RANGE VALUES (2, 'Item 2', 150, SYSDATE);
INSERT INTO T_RANGE VALUES (3, 'Item 3', 600, SYSDATE);
INSERT INTO T_RANGE VALUES (4, 'Item 4', 1500, SYSDATE);
COMMIT;

-- Вставка данных в T_INTERVAL
INSERT INTO T_INTERVAL VALUES (1, 'Event 1', TO_DATE('15-DEC-2022', 'DD-MON-YYYY'), 'Initial event');
INSERT INTO T_INTERVAL VALUES (2, 'Event 2', TO_DATE('20-JAN-2023', 'DD-MON-YYYY'), 'First 2023 event');
INSERT INTO T_INTERVAL VALUES (3, 'Event 3', TO_DATE('15-FEB-2023', 'DD-MON-YYYY'), 'February event');
COMMIT;

-- Вставка данных в T_HASH
INSERT INTO T_HASH VALUES (1, 'user1', 'user1@example.com', SYSDATE);
INSERT INTO T_HASH VALUES (2, 'user2', 'user2@example.com', SYSDATE);
INSERT INTO T_HASH VALUES (3, 'user3', 'user3@example.com', SYSDATE);
INSERT INTO T_HASH VALUES (4, 'user4', 'user4@example.com', SYSDATE);
COMMIT;

-- Вставка данных в T_LIST
INSERT INTO T_LIST VALUES (1, 'ELEC1', 'Smartphone', 599.99);
INSERT INTO T_LIST VALUES (2, 'CLOTH', 'T-Shirt', 19.99);
INSERT INTO T_LIST VALUES (3, 'BOOK1', 'Novel', 12.99); -- Попадет в секцию DEFAULT
COMMIT;

-- Проверка распределения данных
SELECT table_name, partition_name, high_value, num_rows 
FROM user_tab_partitions 
WHERE table_name IN ('T_RANGE', 'T_INTERVAL', 'T_HASH', 'T_LIST');

SELECT * FROM T_RANGE PARTITION (p_low);
SELECT * FROM T_RANGE PARTITION (p_medium);
SELECT * FROM T_RANGE PARTITION (p_high);
SELECT * FROM T_RANGE PARTITION (p_max);

-- 6
ALTER TABLE T_RANGE ENABLE ROW MOVEMENT;
ALTER TABLE T_INTERVAL ENABLE ROW MOVEMENT;
ALTER TABLE T_HASH ENABLE ROW MOVEMENT;
ALTER TABLE T_LIST ENABLE ROW MOVEMENT;

UPDATE T_RANGE SET value = 75 WHERE id = 2; -- Переместится в p_low
UPDATE T_RANGE SET value = 750 WHERE id = 1; -- Переместится в p_high

UPDATE T_INTERVAL SET event_date = TO_DATE('15-MAR-2023', 'DD-MON-YYYY') WHERE id = 1;

UPDATE T_HASH SET username = 'newuser1' WHERE id = 1;

UPDATE T_LIST SET product_code = 'SHOES' WHERE id = 1; -- Переместится в p_clothing
COMMIT;

-- 7
-- Объединяем две секции в T_RANGE
ALTER TABLE T_RANGE MERGE PARTITIONS p_low, p_medium INTO PARTITION p_low_medium;

SELECT partition_name, high_value FROM user_tab_partitions WHERE table_name = 'T_RANGE';

-- 8
ALTER TABLE T_RANGE SPLIT PARTITION p_low_medium 
AT (250) INTO (PARTITION p_low, PARTITION p_medium);

SELECT partition_name, high_value FROM user_tab_partitions WHERE table_name = 'T_RANGE';

-- 9
CREATE TABLE T_RANGE_NONPART AS SELECT * FROM T_RANGE WHERE 1=0;

ALTER TABLE T_RANGE EXCHANGE PARTITION p_high WITH TABLE T_RANGE_NONPART;

SELECT * FROM T_RANGE_NONPART;
SELECT * FROM T_RANGE PARTITION (p_high);

-- 10
SELECT table_name, partitioning_type, partition_count 
FROM user_part_tables;

SELECT partition_name, high_value, num_rows 
FROM user_tab_partitions 
WHERE table_name = 'T_RANGE';

SELECT * FROM T_LIST PARTITION (p_electronics);

SELECT partition_name 
FROM user_tab_partitions 
WHERE table_name = 'T_HASH' AND rownum = 1;

SELECT * FROM T_HASH PARTITION (SYS_P1144); -- Подставить реальное имя секции