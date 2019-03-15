<?php
if (isset($_GET['id'])) {
        include('db_connection.php');
        $id = $_GET['id'];
        $query = $pdo->prepare("CALL `UsersRoles.GetAllRolesMissingUser`(?)");
        $query->bindParam(1, $id);
        $query->execute();
        echo json_encode($query->fetchAll(PDO::FETCH_ASSOC));
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 