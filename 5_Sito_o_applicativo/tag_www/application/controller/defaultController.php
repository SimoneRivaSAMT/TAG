<?php

class defaultController
{
    public function index(){
        require "application/views/mainView.php";
        $conn = DatabaseConnection::getConnection();
    }
}