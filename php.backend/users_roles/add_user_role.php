<?php
if (!empty($_POST['userId']) && !empty($_POST['roleCode'])) {
        include('../db_connection.php');
        $userId = $_POST['userId'];
        $roleCode = $_POST['roleCode'];
        $query = $pdo->prepare("CALL `UsersRoles.AddUserRole`(?, ?)");
        $query->execute(array($userId, $roleCode));

        if ($query->errorCode() === "00000")
            echo "True";
        else print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 