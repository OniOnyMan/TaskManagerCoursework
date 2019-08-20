# Task Manager for office workers
  Android client for work with DB.

For run this application you have to:

0. Run some server, "OpenServer", e.g.: https://ospanel.io/download/;
1. Import to MySQL server scripts from "mySQL_scripts" folder in the following sequence:
  
  а) Import "0. CreateDatabase.sql";
  
  b) Import "1. LoadDefaultValues.sql";
2. Copy everything from "php_backend" folder to your domain folder;
3. In copied file "db_connection.php" set params for connecting to created database;
4. In Unity project in "Responder.cs" script set the property "DomaintURL".
5. Enjoy!
