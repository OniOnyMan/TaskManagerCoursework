<?php
include('../db_connection.php');
$query = $pdo->prepare("CALL `Roles.GetAllRoles`()");
$query->execute();
echo json_encode($query->fetchAll(PDO::FETCH_ASSOC));
$pdo = null;
 