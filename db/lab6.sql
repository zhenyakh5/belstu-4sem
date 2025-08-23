CREATE PFILE='D:\Okakl\database\KEI_PFILE.ORA' FROM SPFILE;

CREATE SPFILE FROM PFILE='D:\Okakl\database\KEI_PFILE.ORA';

SHOW PARAMETER control_files;

SHOW PARAMETER background_dump_dest;
SHOW PARAMETER user_dump_dest;
SHOW PARAMETER diagnostic_dest;
