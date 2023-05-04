<?php

class DataCleaner
{
    public static function cleanUrl($url) : string{
        return filter_var($url, FILTER_SANITIZE_URL);
    }

    public static function cleanSpecialChars($text) : string{
        return filter_var($text, FILTER_SANITIZE_FULL_SPECIAL_CHARS);
    }

    public static function cleanHtmlSpecial($text) : string{
        return htmlspecialchars($text);
    }

    public static function cleanEmail($email) : string{
        return filter_var($email, FILTER_SANITIZE_EMAIL);
    }

    public static function clearAll($text) : string{
        $res = htmlspecialchars($text);
        return filter_var($res, FILTER_SANITIZE_FULL_SPECIAL_CHARS);
    }
}