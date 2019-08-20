<?php
if (!empty($_POST['name'])) {
        include('../db_connection.php');
        $name = $_POST['name'];
        $query = $pdo->prepare("CALL `Roles.AddRole`(@code, ?)");
        $query->execute(array($name));

        if ($query->errorCode() === "00000")
            echo "True";
        else print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 