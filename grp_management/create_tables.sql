-- Create database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'GrpManagementDb')
BEGIN
    CREATE DATABASE GrpManagementDb;
END
GO

USE GrpManagementDb;
GO

-- Drop existing foreign key constraints if they exist
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Contacts_Employees_EmpNO')
    ALTER TABLE Contacts DROP CONSTRAINT FK_Contacts_Employees_EmpNO;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Contacts_Groups_GrpID')
    ALTER TABLE Contacts DROP CONSTRAINT FK_Contacts_Groups_GrpID;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_SentMsgs_Groups_GroupId')
    ALTER TABLE SentMsgs DROP CONSTRAINT FK_SentMsgs_Groups_GroupId;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_SentMsgs_Templates_TemplateID')
    ALTER TABLE SentMsgs DROP CONSTRAINT FK_SentMsgs_Templates_TemplateID;
GO

-- Drop existing tables if they exist (in correct order due to dependencies)
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE name='SentMsgs' AND xtype='U')
    DROP TABLE SentMsgs;
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE name='Contacts' AND xtype='U')
    DROP TABLE Contacts;
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE name='Templates' AND xtype='U')
    DROP TABLE Templates;
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE name='Groups' AND xtype='U')
    DROP TABLE Groups;
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE name='Employees' AND xtype='U')
    DROP TABLE Employees;
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE name='Users' AND xtype='U')
    DROP TABLE Users;
GO

-- Create Users table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Create Employees table
CREATE TABLE Employees (
    EmpNO INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Dept NVARCHAR(100) NOT NULL
);
GO

-- Create Groups table
CREATE TABLE Groups (
    GrpID INT IDENTITY(1,1) PRIMARY KEY,
    GrpName NVARCHAR(100) NOT NULL,
    GrpDescription NVARCHAR(MAX)
);
GO

-- Create Templates table
CREATE TABLE Templates (
    TemplateID INT IDENTITY(1,1) PRIMARY KEY,
    TemplateName NVARCHAR(100) NOT NULL,
    TemplateMsg NVARCHAR(MAX) NOT NULL,
    TemplateType NVARCHAR(50) NOT NULL,
    Placeholders NVARCHAR(MAX)
);
GO

-- Create Contacts table
CREATE TABLE Contacts (
    ContactID INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    EmpNO INT NOT NULL,
    GrpID INT NOT NULL,
    FOREIGN KEY (EmpNO) REFERENCES Employees(EmpNO),
    FOREIGN KEY (GrpID) REFERENCES Groups(GrpID)
);
GO

-- Create SentMsgs table
CREATE TABLE SentMsgs (
    MsgId INT IDENTITY(1,1) PRIMARY KEY,
    GroupId INT,
    SentDate DATETIME DEFAULT GETDATE(),
    SentVia NVARCHAR(50) NOT NULL,
    MessageContent NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    TemplateID INT,
    FOREIGN KEY (GroupId) REFERENCES Groups(GrpID),
    FOREIGN KEY (TemplateID) REFERENCES Templates(TemplateID)
);
GO

-- Insert default groups
SET IDENTITY_INSERT Groups ON;
INSERT INTO Groups (GrpID, GrpName, GrpDescription) 
VALUES (1, 'dmp', 'Default Marketing Group');
INSERT INTO Groups (GrpID, GrpName, GrpDescription) 
VALUES (2, 'all', 'All Users Group');
SET IDENTITY_INSERT Groups OFF;
GO

-- Insert default admin user
INSERT INTO Users (Username, Email, PasswordHash, CreatedAt)
VALUES ('admin', 'admin@example.com', 'admin123', GETDATE());
GO

-- Verify the tables and data
SELECT 'Users' as TableName, COUNT(*) as RecordCount FROM Users
UNION ALL
SELECT 'Employees', COUNT(*) FROM Employees
UNION ALL
SELECT 'Groups', COUNT(*) FROM Groups
UNION ALL
SELECT 'Templates', COUNT(*) FROM Templates
UNION ALL
SELECT 'Contacts', COUNT(*) FROM Contacts
UNION ALL
SELECT 'SentMsgs', COUNT(*) FROM SentMsgs;
GO 