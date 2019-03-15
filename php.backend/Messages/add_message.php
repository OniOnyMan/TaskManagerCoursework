<?php
if (!empty($_POST['author']) && !empty($_POST['project']) && !empty($_POST['text'])) {
        include('db_connection.php');
        $author = $_POST['author'];
        $project = $_POST['project'];
        $text = $_POST['text'];
        $query = $pdo->prepare("CALL `Messages.AddMessage`(@id, ?, ?, ?)");
        //$query->bindParam(1, $id, PDO::PARAM_STR|PDO::PARAM_INPUT_OUTPUT, 36);
        //$query->bindParam(1, $author);
        //$query->bindParam(2, $project);
        //$query->bindParam(3, $text);
        $query->execute(array($author, $project, $text));

        if ($query->errorCode() === "00000") {
                //$sql = "SELECT @id as Id"; 
                //$stmt = $pdo->prepare($sql); 
                //$stmt->execute(); 
                //echo "MessageId: ".$stmt->fetchColumn();
                echo "True";
            } else {
                print_r($query->errorInfo());
            }

        //print_r("<br />CALL `Messages.AddMessage`(".$id.", '".$author."', '".$project."', '".$text."')");
        //echo $text;
        $pdo = null;
    } else {
        echo "Ошибка введеных данных";
    }
 