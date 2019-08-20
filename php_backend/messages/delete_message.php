<?php
if (isset($_GET['message'])) {
        include('../db_connection.php');
        $id = $_GET['message'];
        $query = $pdo->prepare("CALL `Messages.DeleteMessage`(?)");
        $query->execute(array($id));
        if ($query->errorCode() === "00000") {
            echo "True";
        } else {
            print_r($query->errorInfo());
        }
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 