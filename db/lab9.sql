-- 2
CREATE GLOBAL TEMPORARY TABLE temp_table (
    id NUMBER,
    name VARCHAR2(50)
) ON COMMIT DELETE ROWS;

INSERT INTO temp_table (id, name) VALUES (1, 'Test');

-- Данные будут удалены после коммита
COMMIT;

SELECT * FROM temp_table;

-- 3
CREATE SEQUENCE S1
START WITH 1000
INCREMENT BY 10
NOMINVALUE
NOMAXVALUE
NOCYCLE
NOCACHE
NOORDER;

-- Получение значений
SELECT S1.NEXTVAL, S1.CURRVAL FROM dual;

GRANT CREATE SEQUENCE TO KEICORE

-- 4
CREATE SEQUENCE S2
START WITH 10
INCREMENT BY 10
MAXVALUE 100
NOCYCLE;

-- Получение значений
SELECT S2.NEXTVAL FROM dual;

-- 5
CREATE SEQUENCE S3
START WITH -1
INCREMENT BY -10
MINVALUE -101
MAXVALUE -1
NOCYCLE
ORDER;

-- Получение значений
SELECT S3.NEXTVAL FROM dual;

-- 6
CREATE SEQUENCE S4
START WITH 10
INCREMENT BY 1
MAXVALUE 50
CYCLE
CACHE 5
NOORDER;

-- Получение значений
SELECT S4.NEXTVAL FROM dual;

-- 7
SELECT sequence_name
FROM all_sequences
WHERE sequence_owner = 'KEICORE';

-- 8
CREATE TABLE T1 (
    N1 NUMBER(20),
    N2 NUMBER(20),
    N3 NUMBER(20),
    N4 NUMBER(20)
) CACHE
STORAGE (BUFFER_POOL KEEP);

INSERT INTO T1 (N1, N2, N3, N4) VALUES (S1.NEXTVAL, S2.NEXTVAL, S3.NEXTVAL, S4.NEXTVAL);
INSERT INTO T1 (N1, N2, N3, N4) VALUES (S1.NEXTVAL, S2.NEXTVAL, S3.NEXTVAL, S4.NEXTVAL);
INSERT INTO T1 (N1, N2, N3, N4) VALUES (S1.NEXTVAL, S2.NEXTVAL, S3.NEXTVAL, S4.NEXTVAL);
INSERT INTO T1 (N1, N2, N3, N4) VALUES (S1.NEXTVAL, S2.NEXTVAL, S3.NEXTVAL, S4.NEXTVAL);
INSERT INTO T1 (N1, N2, N3, N4) VALUES (S1.NEXTVAL, S2.NEXTVAL, S3.NEXTVAL, S4.NEXTVAL);
INSERT INTO T1 (N1, N2, N3, N4) VALUES (S1.NEXTVAL, S2.NEXTVAL, S3.NEXTVAL, S4.NEXTVAL);
INSERT INTO T1 (N1, N2, N3, N4) VALUES (S1.NEXTVAL, S2.NEXTVAL, S3.NEXTVAL, S4.NEXTVAL);

select * from T1

-- 9
CREATE CLUSTER ABC (
    X NUMBER(10),
    V VARCHAR2(12)
) HASHKEYS 200;

alter user KEICORE quota unlimited on TS_KEI

-- 10
CREATE TABLE A (
    XA NUMBER(10),
    VA VARCHAR2(12),
    additional_column VARCHAR2(10)
) CLUSTER ABC (XA, VA);

-- 11
CREATE TABLE B (
    XB NUMBER(10),
    VB VARCHAR2(12),
    additional_column VARCHAR2(10)
) CLUSTER ABC (XB, VB);
INSERT INTO B (XB, VB, additional_column) VALUES (1, 'a', 'b');
INSERT INTO B (XB, VB, additional_column) VALUES (1, 'a', 'b');
INSERT INTO B (XB, VB, additional_column) VALUES (1, 'a', 'b');
INSERT INTO B (XB, VB, additional_column) VALUES (1, 'a', 'b');
INSERT INTO B (XB, VB, additional_column) VALUES (1, 'a', 'b');

-- 12
CREATE TABLE C (
    XC NUMBER(10),
    VC VARCHAR2(12),
    additional_column VARCHAR2(10)
) CLUSTER ABC (XC, VC);
INSERT INTO C (XC, VC, additional_column) VALUES (1, 'a', 'b');
INSERT INTO C (XC, VC, additional_column) VALUES (1, 'a', 'b');
INSERT INTO C (XC, VC, additional_column) VALUES (1, 'a', 'b');
INSERT INTO C (XC, VC, additional_column) VALUES (1, 'a', 'b');
INSERT INTO C (XC, VC, additional_column) VALUES (1, 'a', 'b');

-- 13
SELECT table_name
FROM all_tables
WHERE cluster_name = 'ABC';

SELECT cluster_name
FROM all_clusters
WHERE cluster_name = 'ABC';

-- 14
CREATE SYNONYM my_synonym FOR C;

-- Применение
SELECT * FROM my_synonym;

grant create synonym to KEICORE

-- 15
CREATE PUBLIC SYNONYM public_synonym FOR B;

-- Применение
SELECT * FROM public_synonym;
grant create public synonym to KEICORE

-- 16
CREATE TABLE AA (
    id NUMBER PRIMARY KEY,
    name VARCHAR2(50)
);

CREATE TABLE BB (
    idy NUMBER,
    a_id NUMBER,
    description VARCHAR2(50),
);

INSERT INTO AA (id, name) VALUES (1, 'Test AA');
INSERT INTO BB (idy, a_id, description) VALUES (1, 1, 'Test BB');

CREATE VIEW V1 AS
SELECT AA.id, AA.name, BB.description
FROM AA
INNER JOIN BB ON AA.id = BB.a_id;

-- Применение
SELECT * FROM V1;

-- 18
CREATE DATABASE LINK my_dblink
CONNECT TO *user* IDENTIFIED BY *pass*
USING 'remote_db';

-- 19
SELECT * FROM remote_table@my_dblink;

INSERT INTO remote_table@my_dblink (column1, column2) VALUES (value1, value2);

UPDATE remote_table@my_dblink SET column1 = value1 WHERE condition;

DELETE FROM remote_table@my_dblink WHERE condition;

EXECUTE remote_procedure@my_dblink;

SELECT remote_function@my_dblink(param) FROM dual;







