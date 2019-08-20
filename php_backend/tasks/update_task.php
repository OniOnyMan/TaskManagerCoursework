<?php
if (!empty($_POST['id']) && !empty($_POST['worker']) && !empty($_POST['description']) && !empty($_POST['deadline'])) {
        include('../db_connection.php');
        $id = $_POST['id'];
        $worker = $_POST['worker'];
        $description = $_POST['description'];
        $deadline = $_POST['deadline'];
        $query = $pdo->prepare("CALL `Tasks.UpdateTask`(?, ?, ?, ?)");
        $query->execute(array($id, $worker, $description, $deadline));

        if ($query->errorCode() === "00000")
            echo "True";
        else
            print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 