<?php
if (isset($_GET['code'])) {
        include('db_connection.php');
        $code = $_GET['code'];
        $query = $pdo->prepare("CALL `Roles.GetRole`(?)");
        $query->bindParam(1, $code);
        $query->execute();
        echo json_encode($query->fetchAll(PDO::FETCH_ASSOC));
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 