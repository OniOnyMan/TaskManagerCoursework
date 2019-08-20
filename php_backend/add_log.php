<?php
if (!empty($_POST['author']) && !empty($_POST['header'])) {
        include('db_connection.php');
        $author = $_POST['author'];
        $header = $_POST['header'];
        $parameters = $_POST['parameters'];
        $query = $pdo->prepare("CALL `Logs.AddLog`(?, ?, ?)");
        $query->execute(array($author, $header, $parameters));

        if ($query->errorCode() === "00000")
            echo "True";
        else print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 