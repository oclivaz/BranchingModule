-- Temporäre Tabelle erstellen
CREATE TABLE #sp_who(
    spid        smallint NULL,
    ecid        smallint NULL,
    status      nchar(30) NULL,
    loginname   nchar(128) NULL,
    hostname    nchar(128) NULL,
    blk         char(5) NULL,
    dbname      nchar(128) NULL,
    cmd         nchar(16) NULL,
    request_id	smallint NULL
)

-- Temporäre Tabelle abfüllen
INSERT INTO #sp_who
EXECUTE('sp_who')

-- Alle Prozesse auf ASK killen
DECLARE @spid smallint,
        @szKill varchar(255)

DECLARE WHO_Cursor CURSOR STATIC FOR
SELECT  spid
FROM    #sp_who
WHERE	dbname = '{Database}'

OPEN WHO_Cursor

WHILE(1=1) BEGIN

        FETCH NEXT FROM WHO_Cursor INTO @spid

        IF(@@FETCH_STATUS <> 0) 
            BREAK

        SELECT  'spid ' + CONVERT(varchar(10), @spid) + ' will be killed.'
        SELECT  @szKill = 'KILL ' + CONVERT(varchar(10), @spid)
        EXECUTE(@szKill)
END

CLOSE WHO_Cursor
DEALLOCATE WHO_Cursor

-- Temporäre Tabelle löschen
DROP TABLE #sp_who