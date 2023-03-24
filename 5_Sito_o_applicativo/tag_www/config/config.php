<?php
$db_name = "tag";
$db_ip = "localhost";
$db_username = "root"; //in seguito creare un utente apposito
$db_user_pass = "";
$db_port = 3306;

$conn = new mysqli($db_ip, $db_username, $db_user_pass, $db_name, $db_port);
if($conn->connect_error){
    die("Connection failed: " . $conn->connect_error);
}