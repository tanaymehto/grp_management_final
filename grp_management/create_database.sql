-- Create the database
CREATE DATABASE GrpManagementDb;
GO

-- Use the database
USE GrpManagementDb;
GO

-- Create Groups table
CREATE TABLE Groups (
    GrpId INT IDENTITY(1,1) PRIMARY KEY,
    GrpName NVARCHAR(100) NOT NULL,
    GrpDescription NVARCHAR(MAX),
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO

-- Create SentMsgs table
CREATE TABLE SentMsgs (
    MsgId INT IDENTITY(1,1) PRIMARY KEY,
    GroupId INT,
    SentDate DATETIME DEFAULT GETDATE(),
    SentVia NVARCHAR(50),
    MessageContent NVARCHAR(MAX),
    Status NVARCHAR(50),
    FOREIGN KEY (GroupId) REFERENCES Groups(GrpId)
);
GO

-- Create Templates table
CREATE TABLE Templates (
    TemplateId INT IDENTITY(1,1) PRIMARY KEY,
    TemplateName NVARCHAR(100) NOT NULL,
    TemplateContent NVARCHAR(MAX) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO 