-- Azure SQL Setup Script for lempiAI.POC
-- This script creates the initial database and user

-- Create database (run on master database)
-- CREATE DATABASE [LempiAI_POC]
-- GO

-- Switch to the new database
-- USE [LempiAI_POC]
-- GO

-- Create application user
-- CREATE USER [lempiaiuser] WITH PASSWORD = 'YourSecurePasswordHere123!'
-- GO

-- Grant permissions
-- EXEC sp_addrolemember 'db_owner', 'lempiaiuser'
-- GO

-- Run the EF Core migration script
-- See: database-migration.sql

-- Verify tables were created
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' 
ORDER BY TABLE_NAME;

-- Verify seed data
SELECT COUNT(*) as EmployeeCount FROM dbo.Employees;
SELECT COUNT(*) as ScheduleCount FROM dbo.Schedules;
SELECT COUNT(*) as FeedbackCount FROM dbo.Feedback;
SELECT COUNT(*) as ProcessImprovementCount FROM dbo.ProcessImprovements;

-- Connection string for appsettings.json:
-- Server=tcp:lempiaiserver.database.windows.net,1433;Initial Catalog=LempiAI_POC;Persist Security Info=False;User ID=lempiaiuser;Password=YourSecurePasswordHere123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
