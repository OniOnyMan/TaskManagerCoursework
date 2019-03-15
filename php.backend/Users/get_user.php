<?php
if (isset($_GET['login'])) {
		include('db_connection.php');
		//echo urlencode('+79998887766');
		$login = $_GET[urldecode('login')];
		$query = $pdo->query("SELECT COUNT(*) FROM `Users` WHERE `Login` = '" . $login . "'");
		if ($query->fetchColumn() > 0) {
				$query = $pdo->prepare("CALL `Users.GetUserByLogin`(?)");
				$query->bindParam(1, $login);
				$query->execute();
				echo json_encode($query->fetchAll(PDO::FETCH_ASSOC));
			} else {
				$query = $pdo->query("SELECT COUNT(*) FROM `Users` WHERE `Email` = '" . $login . "'");
				if ($query->fetchColumn() > 0) {
						$query = $pdo->prepare("CALL `Users.GetUserByEmail`(?)");
						$query->bindParam(1, $login);
						$query->execute();
						echo json_encode($query->fetchAll(PDO::FETCH_ASSOC));
					} else {
						$query = $pdo->prepare("CALL `Users.GetUserByPhone`(?)");
						$query->bindParam(1, $login);
						$query->execute();
						echo json_encode($query->fetchAll(PDO::FETCH_ASSOC));
					}
			}
		$pdo = null;
	} else {
		echo "Ошибка введеных данных";
	}
 