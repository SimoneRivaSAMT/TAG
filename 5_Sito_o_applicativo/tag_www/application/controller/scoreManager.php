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

    public function getMatchIds(){
        require_once "application/models/DatabaseConnection.php";
        $conn = DatabaseConnection::getConnection();
        $query = "SELECT id FROM `match`";
        $stmt = $conn->prepare($query);
        $stmt->execute();
        $stmt->bind_result($mid);
        $res = "";
        while ($stmt->fetch()){
            $res .= "$mid;";
        }
        echo $res;
    }

    public function getLeaderboard(){
        require_once "application/models/DatabaseConnection.php";
        require_once "application/controller/userManager.php";
        $mid = $_POST['match_id'];
        $conn = DatabaseConnection::getConnection();
        $query = "select user_id from plays where match_id = ?"; //ottengo gli users che hanno giocato al match
        $stmt = $conn->prepare($query);
        $stmt->bind_param("d", $mid);
        $stmt->execute();
        $stmt->bind_result($uid);
        $players = array();
        $json = "";
        while ($stmt->fetch()){
            $players[] = $uid;
        }
        $p1 = $players[0] ?? -1;
        $p2 = $players[1] ?? -1;
        $p3 = $players[2] ?? -1;
        $p4 = $players[3] ?? -1;

        $p1Nick = userManager::getUserById($p1);
        $p2Nick = userManager::getUserById($p2);
        $p3Nick = userManager::getUserById($p3);
        $p4Nick = userManager::getUserById($p4);

        $query = "select date_played, (select score from plays where `match_id` = ? and `user_id` = ?) as 'p1Score',
                (select score from plays where `match_id` = ? and `user_id` = ?) as 'p2Score',
                (select score from plays where `match_id` = ? and `user_id` = ?) as 'p3Score',
                (select score from plays where `match_id` = ? and `user_id` = ?) as 'p4Score' from `match` where id = ?";
        $stmt = $conn->prepare($query);
        $stmt->bind_param("ddddddddd", $mid, $p1, $mid, $p2, $mid, $p3, $mid, $p4, $mid);
        $stmt->execute();
        $stmt->bind_result($date_played, $p1Score, $p2Score, $p3Score, $p4Score);
        $stmt->fetch();
        $json = "{\"DatePlayed\":\"$date_played\",\"P1Score\":\"$p1Score\",\"P2Score\":\"$p2Score\",\"P3Score\":\"$p3Score\",\"P4Score\":\"$p4Score\",";
        $json .= "\"P1Nick\":\"$p1Nick\",\"P2Nick\":\"$p2Nick\",\"P3Nick\":\"$p3Nick\",\"P4Nick\":\"$p4Nick\"}";
        echo $json;
    }
}