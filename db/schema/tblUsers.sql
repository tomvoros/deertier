-- tblUsers is the users table. Passwords are hashed. 

CREATE TABLE `tblUsers` (
	`ID` int NOT NULL AUTO_INCREMENT,
	`Name` varchar(100) NOT NULL,
	`Password` varchar(100) NOT NULL,
	`PasswordType` tinyint unsigned NOT NULL,
	`IsModerator` tinyint unsigned NOT NULL,
	PRIMARY KEY (`ID`)
) ENGINE InnoDB,
  CHARSET utf8mb4,
  COLLATE utf8mb4_0900_ai_ci;
