select name, value from v$parameter where name like 'sga%';

SELECT * FROM v$parameter;

select * from gv$sga_dynamic_components;

CREATE TABLE keep_table (id NUMBER, data VARCHAR2(100))
STORAGE (BUFFER_POOL KEEP);

SELECT segment_name, tablespace_name, buffer_pool
FROM dba_segments
WHERE segment_name = 'KEEP_TABLE';

CREATE TABLE default_table (id NUMBER, data VARCHAR2(100))
STORAGE (BUFFER_POOL DEFAULT);

SELECT segment_name, tablespace_name, buffer_pool
FROM dba_segments
WHERE segment_name = 'DEFAULT_TABLE';

SELECT name, value FROM v$parameter WHERE name = 'log_buffer';

SELECT * FROM v$sgastat
WHERE pool = 'large pool' AND name = 'free memory';

SELECT * FROM v$parameter WHERE name IN ('shared_servers', 'dedicated_servers');

select * from v$session;

select * from v$active_services;

SELECT * FROM v$parameter WHERE name LIKE '%dispatcher%';
