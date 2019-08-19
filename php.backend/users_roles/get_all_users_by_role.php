<?php
if (isset($_GET['code'])) {
        include('../db_connection.php');
        $id = $_GET['code'];
        $query = $pdo->prepare("CALL `UsersRoles.GetAllUsersByRole`(?)");
        $query->bindParam(1, $id);
        $query->execute();
        echo json_encode($query->fetchAll(PDO::FETCH_ASSOC));
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 