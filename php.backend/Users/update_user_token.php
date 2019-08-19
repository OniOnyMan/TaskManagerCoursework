<?php
if (!empty($_POST['id']) && !empty($_POST['login'])) {
        include('../db_connection.php');
        $id = $_POST['id'];
        $login = $_POST['login'];
        $email = isset($_POST['email']) ? $_POST['email'] : null;
        $phone = isset($_POST['phone']) ? $_POST['phone'] : null;
        $query = $pdo->prepare("CALL `Users.UpdateUserToken`(?, ?, ?, ?)");
        $query->execute(array($id, $email, $phone, $login));

        if ($query->errorCode() === "00000")
            echo "True";
        else
            print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 