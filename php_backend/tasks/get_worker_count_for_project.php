<?php
if (isset($_GET['id'])) {
        include('../db_connection.php');
        $id = $_GET['id'];
        $query = $pdo->prepare("CALL `Tasks.GetWorkersCountForProject`(?)");
        $query->bindParam(1, $id);
        $query->execute();
        echo $query->fetchColumn();
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 