<?php

class DatabaseConnection
{
    private static $conn = null;

    public static function getConnection() : mysqli{
        if(self::$conn == null){
            self::$conn = new mysqli(HOST, USERNAME, PASSWORD, DB_NAME, PORT);
        }
        return self::$conn;
    }
}