--
-- Database: `taskmanager`
-- Note: If you changed default database name, check matches here
--

USE `taskmanager`;
SET GLOBAL FOREIGN_KEY_CHECKS = 0;

-- Name of administrator's role. Will get `RoleCode` = 1
SET @adminsRoleName = 'Administrators';

-- Name of role for deleted users. Will get `RoleCode` = 2
SET @deletedRoleName = 'Deleted';

-- Default admin params that is required
SET @adminLastName = 'Administrator';
SET @adminLogin = 'admin';
SET @adminPassword = '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918'; 
				   -- password is 'admin'. Just crypting by SHA256 algorithm on Unity client

-- Required status names for logic
SET @workStatusName = 'In work';	-- `StatusCode` = 1
SET @successStatusName = 'Success';	-- `StatusCode` = 2
SET @deletedStatusName = 'Deleted';	-- `StatusCode` = 3


-- Add default roles

CALL `Roles.AddRole`(@adminsRoleCode, @adminsRoleName);
CALL `Roles.AddRole`(@deletedRoleCode, @deletedRoleName);

-- Add default admin

CALL `Users.AddUser`(@adminId, NULL, NULL, @adminLastName, NULL, NULL, @adminLogin, @adminPassword);
CALL `UsersRoles.AddUserRole`(@adminId, @adminsRoleCode);

-- Add required statuses

ALTER TABLE `Statuses` AUTO_INCREMENT = 1;
INSERT INTO `Statuses` (`Name`) VALUES (@workStatusName);
INSERT INTO `Statuses` (`Name`) VALUES (@successStatusName);
INSERT INTO `Statuses` (`Name`) VALUES (@deletedStatusName);

-- Loging that database has created

CALL `Logs.AddLog`(@adminId, 'CREATE DATABASE', NULL);

SET GLOBAL FOREIGN_KEY_CHECKS = 1;
