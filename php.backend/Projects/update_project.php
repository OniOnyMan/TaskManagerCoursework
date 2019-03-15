<?php
if (!empty($_POST['id']) && !empty($_POST['header'])) {
        include('db_connection.php');
        $id = $_POST['id'];
        $header = $_POST['header'];
        $query = $pdo->prepare("CALL `Projects.UpdateProjectHeader`(?, ?)");
        $query->execute(array($id, $header));

        if ($query->errorCode() === "00000")
            echo "True";
        else
            print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 