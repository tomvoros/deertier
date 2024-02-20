-- tblModerationLog is a log of moderator actions (e.g. deleting or creating records)

CREATE TABLE `tblModerationLog` (
	`Id` int NOT NULL AUTO_INCREMENT,
	`UserId` int NOT NULL,
	`Action` tinyint unsigned NOT NULL,
	`Description` varchar(1000),
	`RelatedId1` int,
	`RelatedId2` int,
	`RelatedId3` int,
	`Date` datetime(6) NOT NULL,
	`IPAddress` varchar(100) NOT NULL,
	`UserAgent` varchar(1000) NOT NULL,
	PRIMARY KEY (`Id`)
) ENGINE InnoDB,
  CHARSET utf8mb4,
  COLLATE utf8mb4_0900_ai_ci;
