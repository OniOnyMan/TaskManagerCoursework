# Task Manager for office workers
  Android client for work with DB.

For run this application you have to:

1. Run some server, "OpenServer", e.g.: https://ospanel.io/download/;
2. Import to MySQL server scripts from "mySQL_scripts" folder in the following sequence:
  * Import "0. CreateDatabase.sql";  
  * Import "1. LoadDefaultValues.sql";  
3. Copy everything from "php_backend" folder to your domain folder;
4. In copied file "db_connection.php" set params for connecting to created database;
5. In Unity project in "Responder.cs" script set the property "DomaintURL".
6. Enjoy!







(i hate markdown now)
