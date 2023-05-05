<?php

class DatabaseConnection
{
    private static $conn = null;

    public static function getConnection() : mysqli{
        $HOST = "efof.myd.infomaniak.com";
        $USERNAME = "efof_i20cesste";
        $PASSWORD = "SteCes05_";
        $DB_NAME = "efof_i20cesste";
        $PORT = 3306;
        if(self::$conn == null){
            self::$conn = new mysqli($HOST, $USERNAME, $PASSWORD, $DB_NAME, $PORT);
        }
        return self::$conn;
    }
}