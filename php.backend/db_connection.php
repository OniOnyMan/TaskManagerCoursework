<?php
try {
	$pdo = new PDO("mysql:dbname=*DATABASE*;host=localhost", "*USER*", "*PASSWORD*");
	$pdo->exec('SET NAMES utf8');
} catch (PDOException $e) {
	echo "Ошибка соединения" . $e->getMessage();
	exit;
}
