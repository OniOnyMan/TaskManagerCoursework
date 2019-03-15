<?php
if (isset($_GET['worker'])) {
        include('db_connection.php');
        $id = $_GET['worker'];
        $query = $pdo->prepare("CALL `Tasks.GetAllTasksForWorkerInWork`(?)");
        $query->bindParam(1, $id);
        $query->execute();
        echo json_encode($query->fetchAll(PDO::FETCH_ASSOC));
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 