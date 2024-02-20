-- tblCategories is the categories table

CREATE TABLE `tblCategories` (
	`Id` int NOT NULL AUTO_INCREMENT,
	`Name` varchar(200) NOT NULL,
	`UrlName` varchar(200),
	`ParentId` int,
	`SectionId` int,
	`AllowSubmission` tinyint(1) NOT NULL,
	`Visible` tinyint(1) NOT NULL,
	`DisplayOrder` int NOT NULL,
	`GameTime` tinyint(1) NOT NULL,
	`EscapeGameTime` tinyint(1) NOT NULL,
	`RealTime` tinyint(1) NOT NULL,
	`ShortName` varchar(200),
	`Enabled` tinyint(1) NOT NULL,
	`WikiUrl` varchar(200),
	PRIMARY KEY (`Id`)
) ENGINE InnoDB,
  CHARSET utf8mb4,
  COLLATE utf8mb4_0900_ai_ci;
