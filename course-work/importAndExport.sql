CREATE OR REPLACE PROCEDURE export_to_file IS
  l_json CLOB;
  l_file UTL_FILE.FILE_TYPE;
BEGIN
  SELECT JSON_ARRAYAGG(
           JSON_OBJECT(
             'bookingId' VALUE b.booking_id,
             'userId' VALUE b.user_id,
             'roomId' VALUE b.room_id,
             'checkInDate' VALUE TO_CHAR(b.check_in_date, 'YYYY-MM-DD'),
             'checkOutDate' VALUE TO_CHAR(b.check_out_date, 'YYYY-MM-DD'),
             'status' VALUE b.status,
             'totalPrice' VALUE b.total_price,
             'createdAt' VALUE TO_CHAR(b.created_at, 'YYYY-MM-DD"T"HH24:MI:SS')
           ) RETURNING CLOB
         )
  INTO l_json
  FROM (SELECT * FROM Bookings ORDER BY created_at DESC FETCH FIRST 5 ROWS ONLY) b;
  
  l_file := UTL_FILE.FOPEN('JSON_DIR', 'bookings.json', 'w', 32767);
  
  FOR i IN 0 .. CEIL(DBMS_LOB.GETLENGTH(l_json) / 32767) - 1 LOOP
    UTL_FILE.PUT(l_file, DBMS_LOB.SUBSTR(l_json, 32767, i * 32767 + 1));
  END LOOP;
  
  UTL_FILE.FCLOSE(l_file);
EXCEPTION
  WHEN OTHERS THEN
    IF UTL_FILE.IS_OPEN(l_file) THEN
      UTL_FILE.FCLOSE(l_file);
    END IF;
    RAISE;
END;
/

EXEC export_to_file;

--

CREATE OR REPLACE PROCEDURE import_from_file IS
  l_file       UTL_FILE.FILE_TYPE;
  l_json       CLOB := EMPTY_CLOB();
  l_buffer     VARCHAR2(32767);
  l_dest_offset NUMBER := 1;
  l_src_offset  NUMBER := 1;
  l_ctx         NUMBER := DBMS_LOB.DEFAULT_CSID;
BEGIN
  l_file := UTL_FILE.FOPEN('JSON_DIR', 'bookings.json', 'r', 32767);

  DBMS_LOB.CREATETEMPORARY(l_json, TRUE);

  LOOP
    BEGIN
      UTL_FILE.GET_LINE(l_file, l_buffer);
      DBMS_LOB.WRITEAPPEND(l_json, LENGTH(l_buffer) + 1, l_buffer || CHR(10));
    EXCEPTION
      WHEN NO_DATA_FOUND THEN
        EXIT;
    END;
  END LOOP;

  UTL_FILE.FCLOSE(l_file);

INSERT INTO Bookings (
  user_id, room_id, check_in_date, check_out_date,
  status, total_price, created_at
)
SELECT
  jt.user_id,
  jt.room_id,
  TO_DATE(jt.check_in_date, 'YYYY-MM-DD'),
  TO_DATE(jt.check_out_date, 'YYYY-MM-DD'),
  jt.status,
  jt.total_price,
  TO_TIMESTAMP(jt.created_at, 'YYYY-MM-DD"T"HH24:MI:SS')
FROM JSON_TABLE(
  l_json,
  '$[*]'
  COLUMNS (
    user_id        NUMBER PATH '$.userId',
    room_id        NUMBER PATH '$.roomId',
    check_in_date  VARCHAR2(20) PATH '$.checkInDate',
    check_out_date VARCHAR2(20) PATH '$.checkOutDate',
    status         VARCHAR2(20) PATH '$.status',
    total_price    NUMBER PATH '$.totalPrice',
    created_at     VARCHAR2(30) PATH '$.createdAt'
  )
) jt;

  COMMIT;
EXCEPTION
  WHEN OTHERS THEN
    IF UTL_FILE.IS_OPEN(l_file) THEN
      UTL_FILE.FCLOSE(l_file);
    END IF;
    RAISE;
END;
/

EXEC import_from_file;

select * from Bookings;

