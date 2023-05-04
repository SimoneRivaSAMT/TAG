<?php

class scoreManager
{
    public function index() : void{
        echo "redirect error";
    }

    public function add() : void{
        require_once "application/models/DatabaseConnection.php";
        $conn = DatabaseConnection::getConnection();
        $query = "INSERT INTO plays (user_id, match_id) VALUES (?,?)";
        $stmt = $conn->prepare($query);
        $stmt->bind_param("dd", $_POST['uid'], $_POST['mid']);
        $stmt->execute();
    }

    public function update() : void{
        require_once "application/models/DatabaseConnection.php";
        $conn = DatabaseConnection::getConnection();
        $query = "UPDATE plays SET score = ? WHERE match_id = ? AND user_id = ?";
        $stmt = $conn->prepare($query);
        $stmt->bind_param("ddd", $_POST['score'], $_POST['match_id'], $_POST['user_id']);
        $stmt->execute();
        echo $stmt->error;
    }
    public function get() : void{
        require_once "application/models/DatabaseConnection.php";
        $conn = DatabaseConnection::getConnection();
        $query = "SELECT score FROM plays WHERE user_id = ?";
        $stmt = $conn->prepare($query);
        $stmt->bind_param("s", $_POST['user_id']);
        $stmt->execute();

        $stmt->bind_result($score);
        $stmt->fetch();
        echo $score;
    }
}