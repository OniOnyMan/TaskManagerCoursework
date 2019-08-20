<?php
if (isset($_GET['code'])) {
        include('../db_connection.php');
        $code = $_GET['code'];
        $query = $pdo->prepare("CALL `Roles.DeleteRole`(?)");
        $query->execute(array($code));

        if ($query->errorCode() === "00000")
            echo "True";
        else print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 