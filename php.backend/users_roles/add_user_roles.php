<?php
if (!empty($_POST['userId']) && !empty($_POST['roleCodes'])) {
        include('../db_connection.php');
        $userId = $_POST['userId'];
        $roleCodes = $_POST['roleCodes'];
        //$query = $pdo->prepare("CALL `UsersRoles.AddUserRole`(?, ?)");
        //$query->execute(array($userId, $roleCode));

        //if($query->errorCode() === "00000")
        //    echo "True";
        //else print_r($query->errorInfo());
        echo "TODO: Доделать по необходимости";
        $pdo = null;
    } else {
        echo "Ошибка введеных данных, но всё равно не работает)))";
    }
 