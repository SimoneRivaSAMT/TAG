<?php
require "application/models/DatabaseConnection.php";
//------LINK------//
$actual_link = (isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on'? "https" : "http") . "://{$_SERVER['HTTP_HOST']}";
$documentRoot = $_SERVER['DOCUMENT_ROOT'];
$dir = str_replace('\\', '/', getcwd().'/');
$final = $actual_link . str_replace($documentRoot, '', $dir);
define("URL", $final);
const HOST = "localhost";
const USERNAME = "root";
const PASSWORD = "root";
const DB_NAME = "tag";
const PORT = 3306;
