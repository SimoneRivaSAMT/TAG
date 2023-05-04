<?php

class matchManager
{
    public function index() : void{
        echo "error in matchManagement!";
    }

    public function manageVacant($action) : void{
        require_once "application/models/DatabaseConnection.php";
        $conn = DatabaseConnection::getConnection();
        if(strlen($action) > 0){
            if($action == "addLobby" && isset($_POST['username']) && isset($_POST['ip_address']) && isset($_POST['user_id'])){
                $username = $_POST['username'];
                $ip = $_POST['ip_address'];
                $uid = $_POST['user_id'];
                $query = "INSERT INTO `vacant_match`(`username`, `ip_address`, `user_id`) VALUES (?, ?, ?)";
                $stmt = $conn->prepare($query);
                $stmt->bind_param("ssd", $username, $ip, $uid);
                $stmt->execute();
            }else if($action == "getLobbies") {
                $query = "SELECT `id`, `username`, `ip_address`, `user_id` FROM `vacant_match`";
                $stmt = $conn->prepare($query);
                $stmt->execute();
                $stmt->bind_result($id, $usr, $ipAddr, $userId);
                $json = "";
                while ($stmt->fetch()) {
                    $json .= "{\"Id\": \"$id\", \"Nickname\": \"$usr\", \"IpAddress\": \"$ipAddr\", \"UserId\": \"$userId\"};";
                }
                $json = substr($json, 0, -1);
                echo $json;
            }
        }
    }
    public function startMatch() : void{
        require_once "application/models/DatabaseConnection.php";
        if(isset($_POST['date_played'])){
            $date = $_POST['date_played'];
            $date = date_create_from_format("YYYYmd", $date);
            $conn = DatabaseConnection::getConnection();
            $query = "INSERT INTO `match` (date_played) VALUES (?)";
            $stmt = $conn->prepare($query);
            $stmt->bind_param("s", $_POST['date_played']);
            $stmt->execute();
            $query = "SELECT id FROM `match` ORDER BY id DESC LIMIT 1";
            $stmt = $conn->prepare($query);
            $stmt->execute();
            $stmt->bind_result($id);
            $stmt->fetch();
            echo $id;
        }else{
            echo "Missing field!";
        }
    }
}
//todo in unity fare che si aggiorna il db quando faccio o perdo punti