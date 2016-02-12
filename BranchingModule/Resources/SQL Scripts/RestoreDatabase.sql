RESTORE DATABASE {Database}
   FROM DISK='{Dump}'
   WITH MOVE '{Database}' to 'C:\Database\{Database}\{Database}.mdf', 
		MOVE '{Database}Dat' to 'C:\Database\{Database}\{Database}.ndf', 
		MOVE '{Database}Log' to 'C:\Database\{Database}\{Database}.ldf',
		REPLACE
