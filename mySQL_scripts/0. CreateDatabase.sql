--
-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Aug 19, 2019 at 09:26 PM
-- Server version: 5.7.24-27
-- PHP Version: 7.2.7

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `taskmanager`
-- Note: If you want to change default database name, check code for function `UsersColumnsExceptPasswordHash
--

DROP DATABASE IF EXISTS `taskmanager`;
CREATE DATABASE IF NOT EXISTS `taskmanager` DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci;
USE `taskmanager`;

--
-- Procedures for table `Logs` 
--

DELIMITER $$

DROP PROCEDURE IF EXISTS `Logs.AddLog`$$
CREATE PROCEDURE `Logs.AddLog` (IN `NewAuthorId` CHAR(36), IN `NewHeader` VARCHAR(50), IN `NewParams` TEXT)  BEGIN
    INSERT INTO `Logs` (`DateTime`, `AuthorId`, `Header`, `Params`)
        VALUES (NOW(), `NewAuthorId`, `NewHeader`, `NewParams`);
END$$

--
-- Procedures for table `Messages` 
--

DROP PROCEDURE IF EXISTS `Messages.AddMessage`$$
CREATE PROCEDURE `Messages.AddMessage` (INOUT `MessageId` CHAR(36), `NewAuthorId` CHAR(36), `NewProjectId` CHAR(36), `NewText` TINYTEXT)  BEGIN
    SET `MessageId` = IFNULL(`MessageId`,  UUID());
    INSERT INTO `Messages` (`Id`, `AuthorId`, `ProjectId`, `Text`, `Date`)
        VALUES (`MessageId`, `NewAuthorId`, `NewProjectId`, `NewText`, NOW());
END$$

DROP PROCEDURE IF EXISTS `Messages.DeleteMessage`$$
CREATE PROCEDURE `Messages.DeleteMessage` (`MessageId` CHAR(36))  BEGIN
    DELETE FROM `Messages` WHERE `Id` = `MessageId`;
END$$

DROP PROCEDURE IF EXISTS `Messages.EditTextMessage`$$
CREATE PROCEDURE `Messages.EditTextMessage` (IN `MessageId` CHAR(36), IN `NewText` TINYTEXT)  BEGIN
    UPDATE `Messages` SET `Text` = `NewText`
        WHERE `Id` = `MessageId`;
END$$

DROP PROCEDURE IF EXISTS `Messages.GetAllMessagesForProject`$$
CREATE PROCEDURE `Messages.GetAllMessagesForProject` (IN `NewProjectId` CHAR(36))  BEGIN
    SELECT * FROM `Messages` WHERE `ProjectId` = `NewProjectId`
    	ORDER BY `Date` DESC;
END$$

--
-- Procedures for table `Projects` 
--

DROP PROCEDURE IF EXISTS `Projects.AddProject`$$
CREATE PROCEDURE `Projects.AddProject` (INOUT `ProjectId` CHAR(36), IN `NewCouratorId` CHAR(36), IN `NewHeader` VARCHAR(100))  BEGIN
    SET `ProjectId` = IFNULL(`ProjectId`,  UUID());
    INSERT INTO `Projects` (`Id`, `CouratorId`, `Header`, `CreationDate`, `StatusCode`) 
        VALUES (`ProjectId`, `NewCouratorId`, `NewHeader`, NOW(), 1);
END$$

DROP PROCEDURE IF EXISTS `Projects.DeleteProject`$$
CREATE PROCEDURE `Projects.DeleteProject` (IN `TargetId` CHAR(36))  BEGIN
    UPDATE `Projects` SET `StatusCode` = 3
        WHERE `Id` = `TargetId`;
    UPDATE `Tasks` SET `StatusCode` = 3
    	WHERE `ProjectId` = `TargetId`;
END$$

DROP PROCEDURE IF EXISTS `Projects.GetAllProjects`$$
CREATE PROCEDURE `Projects.GetAllProjects` ()  BEGIN
    SELECT * FROM `Projects` WHERE `StatusCode` != 3 ORDER BY `CreationDate`;
END$$

DROP PROCEDURE IF EXISTS `Projects.GetAllProjectsByCourator`$$
CREATE PROCEDURE `Projects.GetAllProjectsByCourator` (IN `Id` CHAR(36))  BEGIN
    SELECT * FROM `Projects` 
    	WHERE `CouratorId` = `Id` AND `StatusCode` != 3 
    	ORDER BY `CreationDate`;
END$$

DROP PROCEDURE IF EXISTS `Projects.GetAllProjectsDone`$$
CREATE PROCEDURE `Projects.GetAllProjectsDone` ()  BEGIN
    SELECT * FROM `Projects` WHERE `StatusCode` = 2 ORDER BY `CreationDate`;
END$$

DROP PROCEDURE IF EXISTS `Projects.GetAllProjectsInWork`$$
CREATE PROCEDURE `Projects.GetAllProjectsInWork` ()  BEGIN
    SELECT * FROM `Projects` WHERE `StatusCode` = 1 ORDER BY `CreationDate`;
END$$

DROP PROCEDURE IF EXISTS `Projects.GetProject`$$
CREATE PROCEDURE `Projects.GetProject` (`ProjectId` CHAR(36))  BEGIN
    SELECT * FROM `Projects` WHERE `Id` = `ProjectId`;
END$$

DROP PROCEDURE IF EXISTS `Projects.SetProjectConfirmed`$$
CREATE PROCEDURE `Projects.SetProjectConfirmed` (`ProjectId` CHAR(36))  BEGIN
    UPDATE `Projects` SET `ConfirmationDate` = NOW(), `StatusCode` = 2 
        WHERE `Id` = `ProjectId`;
END$$

DROP PROCEDURE IF EXISTS `Projects.SetProjectInWork`$$
CREATE PROCEDURE `Projects.SetProjectInWork` (`ProjectId` CHAR(36))  BEGIN
    UPDATE `Projects` SET `ConfirmationDate` = NULL, `StatusCode` = 1 
        WHERE `Id` = `ProjectId`;
END$$

DROP PROCEDURE IF EXISTS `Projects.UpdateProjectHeader`$$
CREATE PROCEDURE `Projects.UpdateProjectHeader` (`ProjectId` CHAR(36), `NewHeader` VARCHAR(100))  BEGIN
    UPDATE `Projects` SET `Header` = `NewHeader` 
        WHERE `Id` = `ProjectId`;
END$$

--
-- Procedures for table `Roles` 
--

DROP PROCEDURE IF EXISTS `Roles.AddRole`$$
CREATE PROCEDURE `Roles.AddRole` (OUT `RoleCode` TINYINT UNSIGNED, `RoleName` VARCHAR(50))  BEGIN
    ALTER TABLE `Roles` AUTO_INCREMENT = 1;
    INSERT INTO `Roles`(`Name`) VALUES (`RoleName`);
    SET `RoleCode` = LAST_INSERT_ID();
END$$

DROP PROCEDURE IF EXISTS `Roles.DeleteRole`$$
CREATE PROCEDURE `Roles.DeleteRole` (`RoleCode` TINYINT UNSIGNED)  BEGIN
    DELETE FROM `Roles` WHERE `Code` = `RoleCode`;
END$$

DROP PROCEDURE IF EXISTS `Roles.GetAllRoles`$$
CREATE PROCEDURE `Roles.GetAllRoles` ()  BEGIN
    SELECT * FROM `Roles` WHERE `Code` != 2 ORDER BY `Code`;
END$$

DROP PROCEDURE IF EXISTS `Roles.GetRole`$$
CREATE PROCEDURE `Roles.GetRole` (`RoleCode` TINYINT UNSIGNED)  BEGIN
    SELECT * FROM `Roles` WHERE `Code` = `RoleCode`;
END$$

DROP PROCEDURE IF EXISTS `Roles.UpdateRole`$$
CREATE PROCEDURE `Roles.UpdateRole` (`RoleCode` TINYINT UNSIGNED, `NewName` VARCHAR(50))  BEGIN
    UPDATE `Roles` SET `Name` = `NewName` WHERE `Code` = `RoleCode`;
END$$

--
-- Procedures for table `Statuses` 
--

DROP PROCEDURE IF EXISTS `Statuses.GetAllStatuses`$$
CREATE PROCEDURE `Statuses.GetAllStatuses` ()  BEGIN
    SELECT * FROM `Statuses`;
END$$

DROP PROCEDURE IF EXISTS `Statuses.GetStatus`$$
CREATE PROCEDURE `Statuses.GetStatus` (`StatusCode` TINYINT UNSIGNED)  BEGIN
    SELECT * FROM `Statuses` WHERE `Code` = `StatusCode`;
END$$

--
-- Procedures for table `Tasks` 
--

DROP PROCEDURE IF EXISTS `Tasks.AddTask`$$
CREATE PROCEDURE `Tasks.AddTask` (INOUT `TaskId` CHAR(36), IN `NewWorkerId` CHAR(36), IN `NewProjectId` CHAR(36), IN `NewDescription` TEXT, IN `NewDeadline` DATE)  BEGIN
    SET `TaskId` = IFNULL(`TaskId`,  UUID());
    INSERT INTO `Tasks` (`Id`, `WorkerId`, `ProjectId`, `Description`, `IssuanceDate`, `Deadline`, `StatusCode`) 
        VALUES (`TaskId`, `NewWorkerId`, `NewProjectId`, `NewDescription`, NOW(), `NewDeadline`, 1);
END$$

DROP PROCEDURE IF EXISTS `Tasks.DeleteTask`$$
CREATE PROCEDURE `Tasks.DeleteTask` (`TaskId` CHAR(36))  BEGIN
    UPDATE `Tasks` SET `StatusCode` = 3
        WHERE `Id` = `TaskId`;
END$$

DROP PROCEDURE IF EXISTS `Tasks.GetAllTasksForProject`$$
CREATE PROCEDURE `Tasks.GetAllTasksForProject` (IN `Id` CHAR(36))  BEGIN
    SELECT `Projects`.`Header`, `Tasks`.* FROM `Tasks` 
    	LEFT JOIN `Projects` ON `Projects`.`Id` = `Tasks`.`ProjectId` 
        WHERE `ProjectId` = `Id` AND `Tasks`.`StatusCode` != 3 
        ORDER BY `IssuanceDate`;
END$$

DROP PROCEDURE IF EXISTS `Tasks.GetAllTasksForWorker`$$
CREATE PROCEDURE `Tasks.GetAllTasksForWorker` (IN `Id` CHAR(36))  BEGIN
    SELECT `Projects`.`Header`, `Tasks`.* FROM `Tasks` 
    	LEFT JOIN `Projects` ON `Projects`.`Id` = `Tasks`.`ProjectId`
    	WHERE `WorkerId` = `Id` AND `Tasks`.`StatusCode` != 3
        ORDER BY `Deadline`;
END$$

DROP PROCEDURE IF EXISTS `Tasks.GetAllTasksForWorkerInWork`$$
CREATE PROCEDURE `Tasks.GetAllTasksForWorkerInWork` (IN `Id` CHAR(36))  BEGIN
    SELECT `Projects`.`Header` AS `Header`, `Tasks`.* FROM `Tasks` 
    	LEFT JOIN `Projects` ON `Projects`.`Id` = `Tasks`.`ProjectId`
    	WHERE `WorkerId` = `Id` AND `Tasks`.`StatusCode` = 1 
        ORDER BY `Deadline`;
END$$

DROP PROCEDURE IF EXISTS `Tasks.GetTask`$$
CREATE PROCEDURE `Tasks.GetTask` (IN `TaskId` CHAR(36))  BEGIN
	SELECT `Projects`.`Header`, `Tasks`.* FROM `Tasks` 
    	LEFT JOIN `Projects` ON `Projects`.`Id` = `Tasks`.`ProjectId` 
        WHERE `Tasks`.`Id` = `TaskId`;
END$$

DROP PROCEDURE IF EXISTS `Tasks.GetWorkersCountForProject`$$
CREATE PROCEDURE `Tasks.GetWorkersCountForProject` (IN `TargetId` CHAR(36))  NO SQL
BEGIN
	SELECT COUNT(DISTINCT `WorkerId`) 
    FROM `Tasks` WHERE `ProjectId` = `TargetId`;
END$$

DROP PROCEDURE IF EXISTS `Tasks.SetTaskCompleted`$$
CREATE PROCEDURE `Tasks.SetTaskCompleted` (`TaskId` CHAR(36))  BEGIN
    UPDATE `Tasks` SET `CompletionDate` = NOW(), `StatusCode` = 2 
        WHERE `Id` = `TaskId`;
END$$

DROP PROCEDURE IF EXISTS `Tasks.UpdateTask`$$
CREATE PROCEDURE `Tasks.UpdateTask` (IN `TaskId` CHAR(36), IN `NewWorkerId` CHAR(36), IN `NewDescription` TEXT, IN `NewDeadline` DATE)  NO SQL
BEGIN
	CALL `Tasks.UpdateTaskDeadline`(`TaskId`, `NewDeadline`);
    CALL `Tasks.UpdateTaskDescription`(`TaskId`, `NewDescription`);
    UPDATE `Tasks` SET `WorkerId` = `NewWorkerId` 
        WHERE `Id` = `TaskId`; 
END$$

DROP PROCEDURE IF EXISTS `Tasks.UpdateTaskDeadline`$$
CREATE PROCEDURE `Tasks.UpdateTaskDeadline` (`TaskId` CHAR(36), `NewDeadline` DATE)  BEGIN
    UPDATE `Tasks` SET `Deadline` = `NewDeadline` 
        WHERE `Id` = `TaskId`;
END$$

DROP PROCEDURE IF EXISTS `Tasks.UpdateTaskDescription`$$
CREATE PROCEDURE `Tasks.UpdateTaskDescription` (`TaskId` CHAR(36), `NewDescription` TEXT)  BEGIN
    UPDATE `Tasks` SET `Description` = `NewDescription` 
        WHERE `Id` = `TaskId`;
END$$

--
-- Procedures for table `Users` 
--

DROP PROCEDURE IF EXISTS `Users.AddUser`$$
CREATE PROCEDURE `Users.AddUser` (INOUT `UserId` CHAR(36), IN `NewFirstName` VARCHAR(50), IN `NewMiddleName` VARCHAR(50), IN `NewLastName` VARCHAR(100), IN `NewEmail` VARCHAR(50), IN `NewPhone` VARCHAR(50), IN `NewLogin` VARCHAR(50), IN `NewPasswordHash` CHAR(64))  BEGIN
    SET `UserId` = IFNULL(`UserId`,  UUID());
	INSERT INTO `Users` (`Id`, `FirstName`, `MiddleName`, `LastName`, `Email`, `Phone`, `Login`, `PasswordHash`)
        VALUES (`UserId`, `NewFirstName`, `NewMiddleName`, `NewLastName`, `NewEmail`, `NewPhone`, `NewLogin`, UNHEX(`NewPasswordHash`));
END$$

DROP PROCEDURE IF EXISTS `Users.GetAllUsers`$$
CREATE PROCEDURE `Users.GetAllUsers` ()  BEGIN
    SELECT DISTINCT `Id`, `FirstName`, `MiddleName`, 
    	`LastName`, `Email`, `Phone`, `Login` FROM `Users` 
	LEFT JOIN `UsersRoles` ON `UsersRoles`.`UserId` = `Users`.`Id` 
    WHERE `UsersRoles`.`RoleCode` != 2 OR ISNULL(`UsersRoles`.`RoleCode`)
    ORDER BY `Users`.`LastName`;
END$$

DROP PROCEDURE IF EXISTS `Users.GetUserByEmail`$$
CREATE PROCEDURE `Users.GetUserByEmail` (`UserEmail` VARCHAR(50))  BEGIN
    SELECT `Id`, `FirstName`, `MiddleName`, `LastName`, `Email`, `Phone`, `Login`
        FROM `Users` WHERE `Email` = `UserEmail`;
END$$

DROP PROCEDURE IF EXISTS `Users.GetUserById`$$
CREATE PROCEDURE `Users.GetUserById` (IN `UserId` CHAR(36))  BEGIN
    SELECT `Id`, `FirstName`, `MiddleName`, `LastName`, `Email`, `Phone`, `Login`
        FROM `Users` WHERE `Id` = `UserId`;
END$$

DROP PROCEDURE IF EXISTS `Users.GetUserByLogin`$$
CREATE PROCEDURE `Users.GetUserByLogin` (`UserLogin` VARCHAR(50))  BEGIN
    SELECT `Id`, `FirstName`, `MiddleName`, `LastName`, `Email`, `Phone`, `Login`
        FROM `Users` WHERE `Login` = `UserLogin`;
END$$

DROP PROCEDURE IF EXISTS `Users.GetUserByPhone`$$
CREATE PROCEDURE `Users.GetUserByPhone` (`UserPhone` VARCHAR(50))  BEGIN
    SELECT `Id`, `FirstName`, `MiddleName`, `LastName`, `Email`, `Phone`, `Login`
        FROM `Users` WHERE `Phone` = `UserPhone`;
END$$

DROP PROCEDURE IF EXISTS `Users.GetUserPasswordHash`$$
CREATE PROCEDURE `Users.GetUserPasswordHash` (IN `UserId` CHAR(36))  BEGIN
    SELECT HEX(`PasswordHash`) AS `PasswordHash` FROM `Users` WHERE `Id` = `UserId`;
END$$

DROP PROCEDURE IF EXISTS `Users.UpdateUserName`$$
CREATE PROCEDURE `Users.UpdateUserName` (`UserId` CHAR(36), `NewFirstName` VARCHAR(50), `NewMiddleName` VARCHAR(50), `NewLastName` VARCHAR(100))  BEGIN
    UPDATE `Users` SET `FirstName` = `NewFirstName`, `MiddleName` = `NewMiddleName`, `LastName` = `NewLastName`
        WHERE `Id` = `UserId`;
END$$

DROP PROCEDURE IF EXISTS `Users.UpdateUserPassword`$$
CREATE PROCEDURE `Users.UpdateUserPassword` (`UserId` CHAR(36), `NewPasswordHash` CHAR(64))  BEGIN
    UPDATE `Users` SET `PasswordHash` = UNHEX(`NewPasswordHash`)
        WHERE `Id` = `UserId`;
END$$

DROP PROCEDURE IF EXISTS `Users.UpdateUserToken`$$
CREATE PROCEDURE `Users.UpdateUserToken` (`UserId` CHAR(36), `NewEmail` VARCHAR(50), `NewPhone` VARCHAR(50), `NewLogin` VARCHAR(50))  BEGIN
    UPDATE `Users` SET `Email` = `NewEmail`, `Phone` = `NewPhone`, `Login` = `NewLogin`
        WHERE `Id` = `UserId`;
END$$

--
-- Procedures for table `UsersRoles` 
--

DROP PROCEDURE IF EXISTS `UsersRoles.AddUserRole`$$
CREATE PROCEDURE `UsersRoles.AddUserRole` (`Id` CHAR(36), `Code` TINYINT UNSIGNED)  BEGIN
    INSERT INTO `UsersRoles` (`UserId`, `RoleCode`) 
        VALUES (`Id`, `Code`);
END$$

DROP PROCEDURE IF EXISTS `UsersRoles.DeleteUser`$$
CREATE PROCEDURE `UsersRoles.DeleteUser` (IN `Id` CHAR(36))  BEGIN
    DELETE FROM `UsersRoles` WHERE `UserId` = `Id`;
    INSERT INTO `UsersRoles` (`UserId`, `RoleCode`) 
        VALUES (`Id`, 2);
END$$

DROP PROCEDURE IF EXISTS `UsersRoles.DeleteUserRole`$$
CREATE PROCEDURE `UsersRoles.DeleteUserRole` (`Id` CHAR(36), `Code` TINYINT UNSIGNED)  BEGIN
    DELETE FROM `UsersRoles` WHERE `UserId` = `Id` AND `RoleCode` = `Code`;
END$$

DROP PROCEDURE IF EXISTS `UsersRoles.GetAllRolesForUser`$$
CREATE PROCEDURE `UsersRoles.GetAllRolesForUser` (IN `Id` CHAR(36))  BEGIN
    SELECT `Roles`.* FROM `UsersRoles` 
    LEFT JOIN `Roles` ON `Roles`.`Code` = `UsersRoles`.`RoleCode` 
    WHERE `UsersRoles`.`UserId` = `Id` ORDER BY `Code`;
END$$

DROP PROCEDURE IF EXISTS `UsersRoles.GetAllRolesMissingUser`$$
CREATE PROCEDURE `UsersRoles.GetAllRolesMissingUser` (IN `Id` CHAR(36))  BEGIN
    SELECT `Roles`.* FROM `Roles` WHERE `Code` NOT IN(
	    SELECT `Roles`.`Code` FROM `UsersRoles` 
            LEFT JOIN `Roles` ON `Roles`.`Code` = `UsersRoles`.`RoleCode` 
            WHERE `UsersRoles`.`UserId` = `Id`
    ) AND `Code` != 2 ORDER BY `Code`;
END$$

DROP PROCEDURE IF EXISTS `UsersRoles.GetAllUsersByRole`$$
CREATE PROCEDURE `UsersRoles.GetAllUsersByRole` (IN `Code` TINYINT UNSIGNED)  BEGIN
    SELECT DISTINCT `Users`.`Id`, `Users`.`FirstName`, `Users`.`MiddleName`, `Users`.`LastName`, `Users`.`Email`, `Users`.`Phone`, `Users`.`Login` 
    FROM `UsersRoles` 
    LEFT JOIN `Users` ON `Users`.`Id` = `UsersRoles`.`UserId`
    WHERE `RoleCode` = `Code` ORDER BY `LastName`;
END$$

DROP PROCEDURE IF EXISTS `UsersRoles.IsUserAdmin`$$
CREATE PROCEDURE `UsersRoles.IsUserAdmin` (IN `Id` CHAR(36))  BEGIN
	SELECT COUNT(*) FROM `UsersRoles` 
    	WHERE `RoleCode` = 1 AND `UserId` = `Id`; 
END$$

DROP PROCEDURE IF EXISTS `UsersRoles.IsUserDeleted`$$
CREATE PROCEDURE `UsersRoles.IsUserDeleted` (IN `Id` CHAR(36))  NO SQL
BEGIN
	SELECT COUNT(*) FROM `UsersRoles` 
    	WHERE `RoleCode` = 2 AND `UserId` = `Id`; 
END$$

--
-- Functions
--

DROP FUNCTION IF EXISTS `BinaryToGuid`$$
CREATE FUNCTION `BinaryToGuid` (`Data` BINARY(16)) RETURNS CHAR(36) CHARSET utf8 NO SQL
    DETERMINISTIC
BEGIN
    DECLARE `Result` CHAR(36) DEFAULT NULL;
    IF `Data` IS NOT NULL THEN
        SET `Result` =
            CONCAT(
                HEX(SUBSTRING(`Data`, 4, 1)), HEX(SUBSTRING(`Data`, 3, 1)),
                HEX(SUBSTRING(`Data`, 2, 1)), HEX(SUBSTRING(`Data`, 1, 1)), '-', 
                HEX(SUBSTRING(`Data`, 6, 1)), HEX(SUBSTRING(`Data`, 5, 1)), '-',
                HEX(SUBSTRING(`Data`, 8, 1)), HEX(SUBSTRING(`Data`, 7, 1)), '-',
                HEX(SUBSTRING(`Data`, 9, 2)), '-', HEX(SUBSTRING(`Data`, 11, 6)));
    END IF;
    RETURN `Result`;
END$$

DROP FUNCTION IF EXISTS `GuidToBinary`$$
CREATE FUNCTION `GuidToBinary` (`Data` VARCHAR(36) CHARACTER SET utf8) RETURNS BINARY(16) NO SQL
    DETERMINISTIC
BEGIN
    DECLARE `Result` BINARY(16) DEFAULT NULL;
    IF `Data` IS NOT NULL THEN
        SET `Data` = REPLACE(`Data`,'-','');
        SET `Result` =
            CONCAT( UNHEX(SUBSTRING(`Data`, 7, 2)), UNHEX(SUBSTRING(`Data`, 5, 2)),
                    UNHEX(SUBSTRING(`Data`, 3, 2)), UNHEX(SUBSTRING(`Data`, 1, 2)),
                    UNHEX(SUBSTRING(`Data`, 11, 2)),UNHEX(SUBSTRING(`Data`, 9, 2)),
                    UNHEX(SUBSTRING(`Data`, 15, 2)),UNHEX(SUBSTRING(`Data`, 13, 2)),
                    UNHEX(SUBSTRING(`Data`, 17, 16)));
    END IF;
    RETURN `Result`;
END$$

DROP FUNCTION IF EXISTS `UsersColumnsExceptPasswordHash`$$
CREATE FUNCTION `UsersColumnsExceptPasswordHash` () RETURNS VARCHAR(100) CHARSET utf8 BEGIN
    DECLARE `Result` VARCHAR(100) DEFAULT NULL;
    SET `Result` = CONCAT('SELECT ',
        (SELECT GROUP_CONCAT(COLUMN_NAME)
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_SCHEMA='taskmanager' AND -- <- pay attention, that here  
 	            TABLE_NAME='Users' AND			 --    created database name is required
                COLUMN_NAME NOT IN ('PasswordHash')
            ORDER BY ORDINAL_POSITION
        ), ' FROM Users');
    RETURN `Result`;
END$$

DELIMITER ;

--
-- Table structure for table `Logs`
--

DROP TABLE IF EXISTS `Logs`;
CREATE TABLE IF NOT EXISTS `Logs` (
  `DateTime` datetime NOT NULL,
  `AuthorId` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `Header` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `Params` text COLLATE utf8_unicode_ci,
  KEY `AuthorId` (`AuthorId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Table structure for table `Messages`
--

DROP TABLE IF EXISTS `Messages`;
CREATE TABLE IF NOT EXISTS `Messages` (
  `Id` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `AuthorId` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `ProjectId` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `Text` tinytext COLLATE utf8_unicode_ci NOT NULL,
  `Date` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `AuthorId` (`AuthorId`),
  KEY `ProjectId` (`ProjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Table structure for table `Projects`
--

DROP TABLE IF EXISTS `Projects`;
CREATE TABLE IF NOT EXISTS `Projects` (
  `Id` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `CouratorId` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `Header` varchar(100) COLLATE utf8_unicode_ci NOT NULL,
  `CreationDate` date NOT NULL,
  `ConfirmationDate` date DEFAULT NULL,
  `StatusCode` tinyint(3) UNSIGNED NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `CouratorId` (`CouratorId`),
  KEY `StatusCode` (`StatusCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Table structure for table `Roles`
--

DROP TABLE IF EXISTS `Roles`;
CREATE TABLE IF NOT EXISTS `Roles` (
  `Code` tinyint(3) UNSIGNED NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`Code`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Table structure for table `Statuses`
--

DROP TABLE IF EXISTS `Statuses`;
CREATE TABLE IF NOT EXISTS `Statuses` (
  `Code` tinyint(3) UNSIGNED NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`Code`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Table structure for table `Tasks`
--

DROP TABLE IF EXISTS `Tasks`;
CREATE TABLE IF NOT EXISTS `Tasks` (
  `Id` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `WorkerId` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `ProjectId` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `Description` text COLLATE utf8_unicode_ci NOT NULL,
  `IssuanceDate` date NOT NULL,
  `Deadline` date NOT NULL,
  `CompletionDate` date DEFAULT NULL,
  `StatusCode` tinyint(3) UNSIGNED NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `WorkerId` (`WorkerId`),
  KEY `ProjectId` (`ProjectId`),
  KEY `StatusCode` (`StatusCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Table structure for table `Users`
--

DROP TABLE IF EXISTS `Users`;
CREATE TABLE IF NOT EXISTS `Users` (
  `Id` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `FirstName` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `MiddleName` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `LastName` varchar(100) COLLATE utf8_unicode_ci NOT NULL,
  `Email` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Phone` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Login` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `PasswordHash` binary(32) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Login` (`Login`),
  UNIQUE KEY `Email` (`Email`),
  UNIQUE KEY `Phone` (`Phone`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Table structure for table `UsersRoles`
--

DROP TABLE IF EXISTS `UsersRoles`;
CREATE TABLE IF NOT EXISTS `UsersRoles` (
  `UserId` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `RoleCode` tinyint(3) UNSIGNED NOT NULL,
  UNIQUE KEY `UserId` (`UserId`,`RoleCode`),
  KEY `RoleCode` (`RoleCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Constraints for table `Logs`
--
ALTER TABLE `Logs`
  ADD CONSTRAINT `Logs_ibfk_1` FOREIGN KEY (`AuthorId`) REFERENCES `Users` (`Id`) ON UPDATE CASCADE;

--
-- Constraints for table `Messages`
--
ALTER TABLE `Messages`
  ADD CONSTRAINT `Messages_ibfk_1` FOREIGN KEY (`AuthorId`) REFERENCES `Users` (`Id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `Messages_ibfk_2` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`Id`) ON UPDATE CASCADE;

--
-- Constraints for table `Projects`
--
ALTER TABLE `Projects`
  ADD CONSTRAINT `Projects_ibfk_1` FOREIGN KEY (`CouratorId`) REFERENCES `Users` (`Id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `Projects_ibfk_2` FOREIGN KEY (`StatusCode`) REFERENCES `Statuses` (`Code`) ON UPDATE CASCADE;

--
-- Constraints for table `Tasks`
--
ALTER TABLE `Tasks`
  ADD CONSTRAINT `Tasks_ibfk_1` FOREIGN KEY (`WorkerId`) REFERENCES `Users` (`Id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `Tasks_ibfk_2` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`Id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `Tasks_ibfk_3` FOREIGN KEY (`StatusCode`) REFERENCES `Statuses` (`Code`) ON UPDATE CASCADE;

--
-- Constraints for table `UsersRoles`
--
ALTER TABLE `UsersRoles`
  ADD CONSTRAINT `UsersRoles_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `UsersRoles_ibfk_2` FOREIGN KEY (`RoleCode`) REFERENCES `Roles` (`Code`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
