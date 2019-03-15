<?php
if (!empty($_POST['id']) && !empty($_POST['lastName']) && !empty($_POST['login']) && !empty($_POST['passwordHash'])) {
        include('db_connection.php');
        $id = $_POST['id'];
        $firstName = isset($_POST['firstName']) ? $_POST['firstName'] : null;
        $middleName = isset($_POST['middleName']) ? $_POST['middleName'] : null;
        $lastName = $_POST['lastName'];
        $email = isset($_POST['email']) ? $_POST['email'] : null;
        $phone = isset($_POST['phone']) ? $_POST['phone'] : null;
        $login = $_POST['login'];
        $passwordHash = $_POST['passwordHash'];
        $query = $pdo->prepare("CALL `Users.AddUser`(?, ?, ?, ?, ?, ?, ?, ?)");
        $query->execute(array($id, $firstName, $middleName, $lastName, $email, $phone, $login, $passwordHash));

        if ($query->errorCode() === "00000")
            echo "True";
        else
            print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 