<?php
if (!empty($_POST['id']) && !empty($_POST['text'])) {
        include('db_connection.php');
        $id = $_POST['id'];
        $text = $_POST['text'];
        $query = $pdo->prepare("CALL `Messages.EditTextMessage`(?, ?)");
        $query->execute(array($id, $text));
        if ($query->errorCode() === "00000") {
            echo "True";
        } else {
            print_r($query->errorInfo());
        }
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 