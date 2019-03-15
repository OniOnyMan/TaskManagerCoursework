<?php
include('db_connection.php');
$query = $pdo->prepare("CALL `Projects.GetAllProjectsDone`()");
$query->execute();
echo json_encode($query->fetchAll(PDO::FETCH_ASSOC));
$pdo = null;
 