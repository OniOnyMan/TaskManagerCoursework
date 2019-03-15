<?php
if (!empty($_POST['worker']) && !empty($_POST['project']) && !empty($_POST['description']) && !empty($_POST['deadline'])) {
        include('db_connection.php');
        $worker = $_POST['worker'];
        $project = $_POST['project'];
        $description = $_POST['description'];
        $deadline = $_POST['deadline'];
        $query = $pdo->prepare("CALL `Tasks.AddTask`(@id, ?, ?, ?, ?)");
        $query->execute(array($worker, $project, $description, $deadline));
        if ($query->errorCode() === "00000")
            echo "True";
        else print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 