<?php
if (isset($_GET['project'])) {
        include('../db_connection.php');
        $id = $_GET['project'];
        $query = $pdo->prepare("CALL `Messages.GetAllMessagesForProject`(?)");
        $query->bindParam(1, $id);
        $query->execute();
        echo json_encode($query->fetchAll(PDO::FETCH_ASSOC));
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 