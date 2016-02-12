-- Zivilstand bei diversen Subjekten bereinigen
-- Keller Hans ist verwitwet
UPDATE SUB SET ZivilStandCd = 5 WHERE SubNr = 100069
-- Reinhard Otto ist verwitwet
UPDATE SUB SET ZivilStandCd = 5 WHERE SubNr = 100404
-- Wyss Bruno ist ebenfalls verwitwet
UPDATE SUB SET ZivilStandCd = 5 WHERE SubNr = 100395
-- Scherrer Marlies ist ledig
UPDATE SUB SET ZivilStandCd = 1 WHERE SubNr = 134
-- Beljean Marcel ist ebenfalls ledig
UPDATE SUB SET ZivilStandCd = 1 WHERE SubNr = 7507
-- AbgDat setzen
-- Gerber Franz Xaver
UPDATE SUB SET AbgDat = NULL WHERE SubNr = 100344

-- INI anpassen
UPDATE INI SET Db = '{Database}' WHERE Db = 'ASK'

-- adeFirma anpassen
UPDATE adeFirma SET FirmaId='{Database}',DBName='{Database}',Name='{Database}'

-- Pfad wird auf C:\Temp\Ablage gesetzt
UPDATE DAP SET Pfad='{AblagePath}'
UPDATE DAPHst SET Pfad='{AblagePath}'

-- TempAblagePfad wird auf C:\Temp\Ablage gesetzt
UPDATE INI SET Wert = '{AblagePath}' WHERE Schluessel = 'TempAblagePfad' AND SSCd = 'DV'

-- Checklisten Prüfpunkte auf nicht zwingend setzten
UPDATE PPT SET Zwingend = 0

-- Starten der GK Engine
DELETE FROM PJQ
INSERT INTO PJQ([PjqId], [AppId], [Aktiv], [Startzeit], [Stoppzeit], [Gestartet], [PruefDat], [CrtDat], [CrtUsr])
VALUES (newid(), N'{Host}/LM/W3SVC/1/ROOT/{ApplicationName}', 1, '19000101', '19000101', getdate(), getdate(), getdate(), 'M-S')
