CREATE OR REPLACE PROCEDURE send_gmail_fixed(
  p_to      VARCHAR2,
  p_subject VARCHAR2,
  p_body    VARCHAR2
) AS
  v_conn      UTL_SMTP.connection;
  v_crlf      VARCHAR2(2) := CHR(13) || CHR(10);
  v_username  VARCHAR2(100) := 'zhenya130905@gmail.com';
  v_password  VARCHAR2(100) := '';
  v_from      VARCHAR2(100) := 'zhenya130905@gmail.com';
BEGIN
    v_conn := UTL_SMTP.OPEN_CONNECTION(
      host                          => 'smtp.gmail.com',
      port                          => 587,
      tx_timeout                    => 30,
        wallet_path                 => 'file:D:/oracle/wallets/server_wallet.',
      wallet_password               => 'SecretPwd123',
      secure_connection_before_smtp => FALSE
    );
    UTL_SMTP.EHLO(v_conn, 'smtp.gmail.com');
    UTL_SMTP.STARTTLS(v_conn);
    UTL_SMTP.EHLO(v_conn, 'smtp.gmail.com');
    UTL_SMTP.AUTH(v_conn, v_username, v_password, UTL_SMTP.ALL_SCHEMES);

  UTL_SMTP.MAIL(v_conn, v_from);
  UTL_SMTP.RCPT(v_conn, p_to);
    
    UTL_SMTP.OPEN_DATA(v_conn);
    
    UTL_SMTP.WRITE_DATA(v_conn,
      'From: ' || v_from || v_crlf ||
      'To: ' || p_to || v_crlf ||
      'Subject: =?UTF-8?B?' || UTL_ENCODE.TEXT_ENCODE(p_subject, 'UTF8', UTL_ENCODE.BASE64) || '?=' || v_crlf ||
      'Content-Type: text/plain; charset=UTF-8' || v_crlf ||
      'Content-Transfer-Encoding: base64' || v_crlf || v_crlf
    );
    
    UTL_SMTP.WRITE_RAW_DATA(v_conn,
      UTL_ENCODE.BASE64_ENCODE(UTL_RAW.CAST_TO_RAW(CONVERT(p_body, 'UTF8')))
    );
    
    UTL_SMTP.CLOSE_DATA(v_conn);


  UTL_SMTP.QUIT(v_conn);

  DBMS_OUTPUT.PUT_LINE('Email успешно отправлен на ' || p_to);
EXCEPTION
  WHEN OTHERS THEN
    BEGIN
      UTL_SMTP.QUIT(v_conn);
    EXCEPTION
      WHEN OTHERS THEN NULL;
    END;
    DBMS_OUTPUT.PUT_LINE('Ошибка отправки: ' || SQLERRM);
END;

-- Триггер
CREATE OR REPLACE TRIGGER trg_booking_notify
AFTER INSERT OR UPDATE
   ON Bookings
FOR EACH ROW
DECLARE
   PRAGMA AUTONOMOUS_TRANSACTION;
   v_subject VARCHAR2(200);
   v_body    VARCHAR2(1000);
   v_action  VARCHAR2(10);
BEGIN
   IF INSERTING THEN
      v_action := 'created';
   ELSIF UPDATING THEN
      v_action := 'updated';
   END IF;

   v_subject := 'Booking ' || v_action || ': ID #' || :NEW.booking_id;
   v_body    := 'Dear user,' || CHR(10) ||
                'Your booking (ID ' || :NEW.booking_id || ') has been ' || v_action || '.' || CHR(10) ||
                'Details:' || CHR(10) ||
                '   User ID      : ' || :NEW.user_id    || CHR(10) ||
                '   Room ID      : ' || :NEW.room_id    || CHR(10) ||
                '   Check-in date: ' || TO_CHAR(:NEW.check_in_date,  'YYYY-MM-DD') || CHR(10) ||
                '   Check-out date: ' || TO_CHAR(:NEW.check_out_date, 'YYYY-MM-DD') || CHR(10) ||
                '   Status       : ' || :NEW.status    || CHR(10) ||
                '   Total price  : ' || TO_CHAR(:NEW.total_price, 'FM9999990.00') || CHR(10) || CHR(10) ||
                'Thank you for using our service.' || CHR(10) ||
                'Best regards,' || CHR(10) ||
                'Your Hotel Team';

   send_gmail_fixed(
     p_to      => 'zhkharchenko@gmail.com',
     p_subject => v_subject,
     p_body    => v_body
   );

   COMMIT;
EXCEPTION
   WHEN OTHERS THEN
      NULL;
END trg_booking_notify;
/

-- Тест
BEGIN
  send_gmail_fixed(
    p_to      => 'zhkharchenko@gmail.com',
    p_subject => 'Тест из Oracle',
    p_body    => 'Это тестовое письмо, отправленное через Gmail SMTP из Oracle!'
  );
END;
/
commit;