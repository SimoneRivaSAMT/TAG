<?php

class userManager
{
    public function index() : void{

    }

    public function login() : void{
        require_once "application/models/DatabaseConnection.php";
        $conn = DatabaseConnection::getConnection();
        if(isset($_POST['email']) && isset($_POST['password'])){
            $uname = $_POST['email'];
            $pw = $_POST['password'];
            $query = "SELECT id, nickname, email FROM user WHERE email = ? AND password = ?";
            $stmt = $conn->prepare($query);
            $stmt->bind_param("ss", $uname, $pw);
            $stmt->execute();
            $stmt->bind_result($uid, $unickname, $uemail);
            $json = "{}";
            while ($stmt->fetch()){
                $json = "{\"Id\": \"$uid\", \"Nickname\": \"$unickname\", \"Email\": \"$uemail\"}";
            }
            echo $json;
        }
    }

    public function signup() : void{
        require_once "application/models/DatabaseConnection.php";
        $conn = DatabaseConnection::getConnection();
        if(strlen($_POST['user_email']) > 0 && strlen($_POST['user_password']) > 0 && strlen($_POST['user_nickname']) > 0){
            $uname = $_POST['user_email'];
            $upass = $_POST['user_password'];
            $unick = $_POST['user_nickname'];
            $query = "INSERT INTO user (`email`, `password`, `nickname`) VALUES (?, ?, ?)";
            $stmt = $conn->prepare($query);
            $stmt->bind_param("sss", $uname, $upass, $unick);
            $stmt->execute();
            echo "Done";
        }else{
            echo "Fill all fields please";
        }
    }
}