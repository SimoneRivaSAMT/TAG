<?php

class userManager
{
    public function index() : void{

    }

    public function login() : void{
        require_once "application/models/DatabaseConnection.php";
        require_once "application/models/DataCleaner.php";
        $conn = DatabaseConnection::getConnection();
        if(isset($_POST['email']) && isset($_POST['password'])){
            $uname = DataCleaner::cleanEmail($_POST['email']);
            $pw = DataCleaner::cleanHtmlSpecial($_POST['password']);
            $query = "SELECT id, nickname, email, password FROM user WHERE email = ?";
            $stmt = $conn->prepare($query);
            $stmt->bind_param("s", $uname);
            $stmt->execute();
            $stmt->bind_result($uid, $unickname, $uemail, $upass);
            $stmt->fetch();
            $json = "{}";
            if(password_verify($pw, $upass)){
                $json = "{\"Id\": \"$uid\", \"Nickname\": \"$unickname\", \"Email\": \"$uemail\"}";
            }else{
                $json = "{error: 'user_not_found'}";
            }
            echo $json;
        }
    }

    public function signup() : void{
        require_once "application/models/DatabaseConnection.php";
        $conn = DatabaseConnection::getConnection();
        if(strlen($_POST['user_email']) > 0 && strlen($_POST['user_password']) > 0 && strlen($_POST['user_nickname']) > 0){
            $uname = $_POST['user_email'];
            $upass = password_hash($_POST['user_password'], CRYPT_SHA256);
            $unick = $_POST['user_nickname'];
            $users = $this->getAllUsers();
            foreach ($users as $user){
                if($user['email'] == $uname || $user['nick'] == $unick){
                    echo "There is already a user with your email or nickname";
                    return;
                }
            }
            $query = "INSERT INTO user (`email`, `password`, `nickname`) VALUES (?, ?, ?)";
            $stmt = $conn->prepare($query);
            $stmt->bind_param("sss", $uname, $upass, $unick);
            $stmt->execute();
            echo "Done";
        }else{
            echo "Fill all fields please";
        }
    }

    public static function getUserById($uid){
        require_once "application/models/DatabaseConnection.php";
        $conn = DatabaseConnection::getConnection();
        $query = "SELECT nickname FROM user WHERE id = ?";
        $stmt = $conn->prepare($query);
        $stmt->bind_param("d", $uid);
        $stmt->execute();
        $stmt->bind_result($userId);
        $stmt->fetch();
        return $userId;
    }

    private function getAllUsers(){
        require_once "application/models/DatabaseConnection.php";
        $conn = DatabaseConnection::getConnection();
        $query = "SELECT email, nickname FROM user";
        $stmt = $conn->prepare($query);
        $stmt->execute();
        $stmt->bind_result($mail, $nick);
        $userData = array();
        while ($stmt->fetch()){
            $userData[] = array('email' => $mail, 'nick' => $nick);
        }
        return $userData;
    }


}