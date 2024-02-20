-- tblRecordDeletionLog keeps a copy of every record that is deleted from tblRecords

-- TODO:
-- - consider leaving deleted records in tblRecords with an IsDeleted flag instead of copying them here

CREATE TABLE `tblRecordDeletionLog` (
	`ID` int NOT NULL AUTO_INCREMENT,
	`Moderator` varchar(100) NOT NULL,
	`DeletionDate` datetime(6),
	`CategoryId` int NOT NULL,
	`Player` varchar(100) NOT NULL,
	`RealTimeString` varchar(100) NOT NULL,
	`GameTimeString` varchar(100),
	`RealTimeSeconds` int NOT NULL,
	`GameTimeSeconds` int,
	`Comment` varchar(100),
	`VideoURL` varchar(100),
	`CeresTime` double,
	`DateSubmitted` datetime(6),
	`SubmittedByUserId` int,
	`IPAddress` varchar(100) NOT NULL,
	PRIMARY KEY (`ID`)
) ENGINE InnoDB,
  CHARSET utf8mb4,
  COLLATE utf8mb4_0900_ai_ci;
