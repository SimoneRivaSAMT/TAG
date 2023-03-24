<?php
include "../config/config.php";

if(isset($_POST['action'])){
    $action = $_POST['action'];
    if($action == "add-lobby" && isset($_POST['username']) && isset($_POST['ip_address']) ){
        $username = $_POST['username'];
        $ip = $_POST['ip_address'];
        $query = "INSERT INTO `vacant_match`(`username`, `ip_address`) VALUES (?, ?)";
        $stmt = $conn->prepare($query);
        $stmt->bind_param("ss", $username, $ip);
        $stmt->execute();
    }else if($action == "get-lobbies"){
        $query = "SELECT `id`, `username`, `ip_address` FROM `vacant_match`";
        $stmt = $conn->prepare($query);
        $stmt->execute();
        $res = $stmt->bind_result($id, $usr, $ipAddr);
        $json = "";
        while ($stmt->fetch()){
            $json .= "{\"Id\": \"$id\", \"Nickname\": \"$usr\", \"IpAddress\": \"$ipAddr\"};";
        }
        $json = substr($json, 0, -1);
        echo $json;
    }
}else{
    echo "Vars not defined";
}