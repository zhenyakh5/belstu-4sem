-- 1
CREATE TABLE employees (
    emp_id NUMBER PRIMARY KEY,
    emp_name VARCHAR2(100) NOT NULL,
    salary NUMBER(10,2),
    department VARCHAR2(50),
    hire_date DATE DEFAULT SYSDATE
);

-- 2
INSERT INTO employees VALUES (1, 'Иванов Иван', 50000, 'IT', TO_DATE('15-01-2020', 'DD-MM-YYYY'));
INSERT INTO employees VALUES (2, 'Петров Петр', 60000, 'HR', TO_DATE('20-02-2019', 'DD-MM-YYYY'));
INSERT INTO employees VALUES (3, 'Сидорова Мария', 55000, 'IT', TO_DATE('10-03-2021', 'DD-MM-YYYY'));
INSERT INTO employees VALUES (4, 'Кузнецов Алексей', 70000, 'Finance', TO_DATE('05-04-2018', 'DD-MM-YYYY'));
INSERT INTO employees VALUES (5, 'Михайлова Анна', 65000, 'HR', TO_DATE('12-05-2020', 'DD-MM-YYYY'));
INSERT INTO employees VALUES (6, 'Федоров Дмитрий', 75000, 'IT', TO_DATE('25-06-2017', 'DD-MM-YYYY'));
INSERT INTO employees VALUES (7, 'Николаева Елена', 58000, 'Finance', TO_DATE('30-07-2019', 'DD-MM-YYYY'));
INSERT INTO employees VALUES (8, 'Алексеев Сергей', 80000, 'IT', TO_DATE('15-08-2016', 'DD-MM-YYYY'));
INSERT INTO employees VALUES (9, 'Васильева Ольга', 62000, 'HR', TO_DATE('20-09-2020', 'DD-MM-YYYY'));
INSERT INTO employees VALUES (10, 'Павлов Николай', 90000, 'Finance', TO_DATE('10-10-2015', 'DD-MM-YYYY'));

COMMIT;

-- 3
CREATE OR REPLACE TRIGGER before_statement_emp
BEFORE INSERT OR UPDATE OR DELETE ON employees
BEGIN
    DBMS_OUTPUT.PUT_LINE('before_statement_emp: BEFORE statement-level trigger fired');
END;
/

-- 4
CREATE OR REPLACE TRIGGER before_row_emp
BEFORE INSERT OR UPDATE OR DELETE ON employees
FOR EACH ROW
BEGIN
    DBMS_OUTPUT.PUT_LINE('before_row_emp: BEFORE row-level trigger fired');
    
    IF INSERTING THEN
        DBMS_OUTPUT.PUT_LINE('  Inserting new employee with ID: ' || :NEW.emp_id);
    ELSIF UPDATING THEN
        DBMS_OUTPUT.PUT_LINE('  Updating employee from ID: ' || :OLD.emp_id);
    ELSIF DELETING THEN
        DBMS_OUTPUT.PUT_LINE('  Deleting employee with ID: ' || :OLD.emp_id);
    END IF;
END;
/

-- 5
CREATE OR REPLACE TRIGGER before_row_emp_predicates
BEFORE INSERT OR UPDATE OR DELETE ON employees
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        DBMS_OUTPUT.PUT_LINE('before_row_emp_predicates: INSERT operation detected');
        :NEW.hire_date := SYSDATE;
    ELSIF UPDATING('salary') THEN
        DBMS_OUTPUT.PUT_LINE('before_row_emp_predicates: SALARY update detected');
        IF :NEW.salary < :OLD.salary THEN
            RAISE_APPLICATION_ERROR(-20001, 'Salary cannot be decreased');
        END IF;
    ELSIF DELETING THEN
        DBMS_OUTPUT.PUT_LINE('before_row_emp_predicates: DELETE operation detected');
    END IF;
END;
/

-- 6
CREATE OR REPLACE TRIGGER after_statement_emp
AFTER INSERT OR UPDATE OR DELETE ON employees
BEGIN
    DBMS_OUTPUT.PUT_LINE('after_statement_emp: AFTER statement-level trigger fired');
END;
/

-- 7
CREATE OR REPLACE TRIGGER after_row_emp
AFTER INSERT OR UPDATE OR DELETE ON employees
FOR EACH ROW
BEGIN
    DBMS_OUTPUT.PUT_LINE('after_row_emp: AFTER row-level trigger fired');
    
    IF INSERTING THEN
        DBMS_OUTPUT.PUT_LINE('  Inserted employee with ID: ' || :NEW.emp_id);
    ELSIF UPDATING THEN
        DBMS_OUTPUT.PUT_LINE('  Updated employee with ID: ' || :OLD.emp_id);
    ELSIF DELETING THEN
        DBMS_OUTPUT.PUT_LINE('  Deleted employee with ID: ' || :OLD.emp_id);
    END IF;
END;
/

-- 8
CREATE TABLE audit_log (
    log_id NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    operation_date TIMESTAMP DEFAULT SYSTIMESTAMP,
    operation_type VARCHAR2(10),
    trigger_name VARCHAR2(100),
    old_data VARCHAR2(1000),
    new_data VARCHAR2(1000),
    emp_id NUMBER
);

-- 9
CREATE OR REPLACE TRIGGER before_row_emp_audit
BEFORE INSERT OR UPDATE OR DELETE ON employees
FOR EACH ROW
DECLARE
    v_operation VARCHAR2(10);
BEGIN
    IF INSERTING THEN
        v_operation := 'INSERT';
        INSERT INTO audit_log(operation_type, trigger_name, new_data, emp_id)
        VALUES (v_operation, 'before_row_emp_audit', 
               'ID=' || :NEW.emp_id || ',NAME=' || :NEW.emp_name || ',SALARY=' || :NEW.salary, 
               :NEW.emp_id);
    ELSIF UPDATING THEN
        v_operation := 'UPDATE';
        INSERT INTO audit_log(operation_type, trigger_name, old_data, new_data, emp_id)
        VALUES (v_operation, 'before_row_emp_audit', 
               'ID=' || :OLD.emp_id || ',NAME=' || :OLD.emp_name || ',SALARY=' || :OLD.salary,
               'ID=' || :NEW.emp_id || ',NAME=' || :NEW.emp_name || ',SALARY=' || :NEW.salary,
               :NEW.emp_id);
    ELSIF DELETING THEN
        v_operation := 'DELETE';
        INSERT INTO audit_log(operation_type, trigger_name, old_data, emp_id)
        VALUES (v_operation, 'before_row_emp_audit', 
               'ID=' || :OLD.emp_id || ',NAME=' || :OLD.emp_name || ',SALARY=' || :OLD.salary,
               :OLD.emp_id);
    END IF;
END;
/

-- 10
-- Попытка вставить дубликат первичного ключа
BEGIN
    INSERT INTO employees VALUES (1, 'Дубликат', 10000, 'IT', SYSDATE);
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: ' || SQLERRM);
END;
/

-- Проверка аудита
SELECT * FROM audit_log WHERE emp_id = 1;

-- 11
-- Попытка удаления таблицы
DROP TABLE employees;

-- Создание триггера, запрещающего удаление таблицы
CREATE OR REPLACE TRIGGER prevent_emp_table_drop
BEFORE DROP ON SCHEMA
BEGIN
    IF ORA_DICT_OBJ_NAME = 'EMPLOYEES' THEN
        RAISE_APPLICATION_ERROR(-20002, 'Таблица EMPLOYEES не может быть удалена');
    END IF;
END;
/

-- Повторная попытка удаления таблицы
BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE employees';
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: ' || SQLERRM);
END;
/

-- 12
DROP TABLE AUDIT;
SELECT trigger_name, status FROM user_triggers WHERE table_name = 'EMPLOYEES';

-- Попытка выполнения операции, которая активирует триггеры
BEGIN
  UPDATE employees SET salary = salary + 100 WHERE emp_id = 1;
  COMMIT;
EXCEPTION
  WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('Ошибка: ' || SQLERRM);
    ROLLBACK;
END;
/

-- 13
CREATE VIEW employees_view AS SELECT * FROM employees;

CREATE OR REPLACE TRIGGER instead_of_update_emp
INSTEAD OF UPDATE ON employees_view
FOR EACH ROW
BEGIN
    -- Помечаем старую запись как недействительную
    UPDATE employees 
    SET emp_name = emp_name || ' (OLD)'
    WHERE emp_id = :OLD.emp_id;
    
    -- Вставляем новую запись
    INSERT INTO employees (emp_id, emp_name, salary, department, hire_date)
    VALUES (:NEW.emp_id, :NEW.emp_name, :NEW.salary, :NEW.department, SYSDATE);
    
    DBMS_OUTPUT.PUT_LINE('instead_of_update_emp: Старая запись помечена, новая добавлена');
END;
/

UPDATE employees_view SET salary = salary + 1000 WHERE emp_id = 3;
SELECT * FROM employees WHERE emp_id = 3 OR emp_name LIKE '%(OLD)';

-- 14
SET SERVEROUTPUT ON;

UPDATE employees SET salary = salary + 100 WHERE emp_id = 5;
COMMIT;

-- 15
-- Создание дополнительных триггеров
CREATE OR REPLACE TRIGGER before_row_emp_extra1
BEFORE UPDATE ON employees
FOR EACH ROW
FOLLOWS before_row_emp
BEGIN
    DBMS_OUTPUT.PUT_LINE('before_row_emp_extra1: Дополнительный триггер 1 (FOLLOWS)');
END;
/

CREATE OR REPLACE TRIGGER before_row_emp_extra2
BEFORE UPDATE ON employees
FOR EACH ROW
PRECEDES before_row_emp
BEGIN
    DBMS_OUTPUT.PUT_LINE('before_row_emp_extra2: Дополнительный триггер 2 (PRECEDES)');
END;
/

UPDATE employees SET salary = salary + 100 WHERE emp_id = 6;
COMMIT;

ALTER TRIGGER before_row_emp_extra1 PRECEDES before_row_emp_extra2;

UPDATE employees SET salary = salary + 100 WHERE emp_id = 7;
COMMIT;