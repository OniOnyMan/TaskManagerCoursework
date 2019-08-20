<?php
if (!empty($_POST['id']) && !empty($_POST['lastName'])) {
        include('../db_connection.php');
        $id = $_POST['id'];
        $lastName = $_POST['lastName'];
        $firstName = isset($_POST['firstName']) ? $_POST['firstName'] : null;
        $middleName = isset($_POST['middleName']) ? $_POST['middleName'] : null;
        $query = $pdo->prepare("CALL `Users.UpdateUserName`(?, ?, ?, ?)");
        $query->execute(array($id, $firstName, $middleName, $lastName));

        if ($query->errorCode() === "00000")
            echo "True";
        else
            print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 