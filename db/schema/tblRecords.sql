-- tblRecords is the table that holds records (i.e. times/runs) for all categories

-- TODO: 
-- - replace the Player column with UserId
-- - remove RealTimeString and GameTimeString columns
-- - normalize how times are stored in general

CREATE TABLE `tblRecords` (
	`ID` int NOT NULL AUTO_INCREMENT,
	`CategoryId` int NOT NULL,
	`Player` varchar(100) NOT NULL,
	`RealTimeSeconds` int NOT NULL,
	`RealTimeString` varchar(100) NOT NULL,
	`GameTimeSeconds` int,
	`GameTimeString` varchar(100),
	`Comment` varchar(100),
	`VideoURL` varchar(100),
	`CeresTime` double,
	`DateSubmitted` datetime(6),
	`SubmittedByUserId` int,
	PRIMARY KEY (`ID`)
) ENGINE InnoDB,
  CHARSET utf8mb4,
  COLLATE utf8mb4_0900_ai_ci;
