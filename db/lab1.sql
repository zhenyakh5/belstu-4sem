CREATE TABLE KEI_t (
    column1 NUMBER(3) PRIMARY KEY,
    column2 VARCHAR2(50)
);

INSERT INTO KEI_t (column1, column2) VALUES (1, 'Первая запись');
INSERT INTO KEI_t (column1, column2) VALUES (2, 'Вторая запись');
INSERT INTO KEI_t (column1, column2) VALUES (3, 'Третья запись');

COMMIT;

UPDATE KEI_t 
SET column2 = 'Обновленная первая запись'
WHERE column1 = 1;

UPDATE KEI_t 
SET column2 = 'Обновленная вторая запись'
WHERE column1 = 2;

COMMIT;

SELECT MAX(column1) AS max_value
FROM KEI_t;

DELETE FROM KEI_t 
WHERE column1 = 3;

SELECT * FROM KEI_t;

ROLLBACK;

SELECT * FROM KEI_t;

CREATE TABLE KEI_t_child (
    child_id NUMBER(3) PRIMARY KEY,
    parent_id NUMBER(3),
    child_data VARCHAR2(50),
    CONSTRAINT fk_parent FOREIGN KEY (parent_id) REFERENCES KEI_t(column1)
);

INSERT INTO KEI_t_child (child_id, parent_id, child_data) VALUES (1, 1, 'Новая запись 1');
INSERT INTO KEI_t_child (child_id, parent_id, child_data) VALUES (2, 2, 'Новая запись 2');
INSERT INTO KEI_t_child (child_id, parent_id, child_data) VALUES (3, 1, 'Новая запись 3');

COMMIT;
SELECT * FROM KEI_t_child;

SELECT p.column1 AS parent_id, p.column2 AS parent_data, c.child_id, c.child_data
FROM KEI_t p
JOIN KEI_t_child c ON p.column1 = c.parent_id;

SELECT p.column1 AS parent_id, p.column2 AS parent_data, c.child_id, c.child_data
FROM KEI_t p
LEFT JOIN KEI_t_child c ON p.column1 = c.parent_id;

SELECT * FROM KEI_t;
SELECT * FROM KEI_t_child;

DROP TABLE KEI_t_child;
DROP TABLE KEI_t;
