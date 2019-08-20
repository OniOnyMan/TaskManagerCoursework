<?php
if (isset($_GET['user'])) {
    include('../db_connection.php');
    $id = $_GET['user'];
    $query = $pdo->query("CALL `UsersRoles.IsUserAdmin`('" . $id . "')");
    if ($query->fetchColumn() > 0)
        echo "True";
    else echo "False";
    $pdo = null;
} else {
    echo "Ошибка введеных данных";
}
 