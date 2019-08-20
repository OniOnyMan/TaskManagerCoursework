<?php
include('../db_connection.php');
$query = $pdo->prepare("CALL `Users.GetAllUsers`()");
$query->execute();
/*if($query->errorCode() === "00000")
        print_r($query->errorInfo());*/
echo json_encode($query->fetchAll(PDO::FETCH_ASSOC));
$pdo = null;
 