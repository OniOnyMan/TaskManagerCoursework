<?php
if (!empty($_POST['id']) && !empty($_POST['newPassword'])) {
        include('db_connection.php');
        $id = $_POST['id'];
        $newPassword = $_POST['newPassword'];
        $query = $pdo->prepare("CALL `Users.UpdateUserPassword`(?, ?)");
        $query->execute(array($id, $newPassword));

        if ($query->errorCode() === "00000")
            echo "True";
        else
            print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 