<?php
if (isset($_GET['id'])) {
        include('db_connection.php');
        $id = $_GET['id'];
        $query = $pdo->prepare("CALL `UsersRoles.DeleteUser`(?)");
        $query->execute(array($id));

        if ($query->errorCode() === "00000")
            echo "True";
        else print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 