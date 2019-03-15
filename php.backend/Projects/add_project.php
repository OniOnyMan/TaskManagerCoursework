<?php
if (!empty($_POST['courator']) && !empty($_POST['header']) && !empty($_POST['id'])) {
        include('db_connection.php');
        $id = $_POST['id'];
        $courator = $_POST['courator'];
        $header = $_POST['header'];
        $query = $pdo->prepare("CALL `Projects.AddProject`(?, ?, ?);");
        $query->execute(array($id, $courator, $header));

        if ($query->errorCode() === "00000")
            echo "True";
        else
            print_r($query->errorInfo());
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 