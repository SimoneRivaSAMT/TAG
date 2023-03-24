#--- CHECK IF SCHEMA ALREADY EXISTS ---
DROP SCHEMA IF EXISTS `tag`;

#--- CREATE SCHEMA ---
CREATE SCHEMA tag;
USE tag;

#--- CREATE TABLES ---
CREATE TABLE `user`(
	id INT PRIMARY KEY AUTO_INCREMENT,
    nickname VARCHAR(50) NOT NULL,
    email VARCHAR(320) NOT NULL, #this is the max mail langth
    password VARCHAR(300) NOT NULL
);

CREATE TABLE `match`(
	id INT PRIMARY KEY AUTO_INCREMENT,
    date_played TIMESTAMP
);

CREATE TABLE `plays`(
	user_id INT,
    match_id INT,
    score INT DEFAULT -1,
    times_tagged INT DEFAULT -1,
    times_tagged_someone INT DEFAULT -1,
    FOREIGN KEY (user_id) REFERENCES `user`(id) ON DELETE SET NULL,
    FOREIGN KEY (match_id) REFERENCES `match`(id) ON DELETE SET NULL
);

CREATE TABLE vacant_match(
	id INT PRIMARY KEY AUTO_INCREMENT,
    ip_address VARCHAR(15) UNIQUE,
    username VARCHAR(50)
);