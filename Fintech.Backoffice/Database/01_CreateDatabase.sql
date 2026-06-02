-- ============================================================================
-- FinTech Backoffice Dashboard Database Creation Script
-- Database: FintechBackofficeDb
-- Server: localhost\SQLEXPRESS01
-- Authentication: Windows Authentication
-- ============================================================================

-- Drop existing database if it exists (USE WITH CAUTION in production)
-- IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'FintechBackofficeDb')
-- BEGIN
--     ALTER DATABASE FintechBackofficeDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
--     DROP DATABASE FintechBackofficeDb;
-- END

-- Create the database
-- Note: Adjust file paths according to your SQL Server installation
CREATE DATABASE FintechBackofficeDb
    ON PRIMARY (
        NAME = FintechBackofficeDb_data,
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS01\MSSQL\DATA\FintechBackofficeDb.mdf',
        SIZE = 100MB,
        FILEGROWTH = 10MB
    )
    LOG ON (
        NAME = FintechBackofficeDb_log,
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS01\MSSQL\DATA\FintechBackofficeDb_log.ldf',
        SIZE = 50MB,
        FILEGROWTH = 5MB
    );
GO

-- Use the newly created database for all subsequent commands
USE FintechBackofficeDb;
GO

-- ============================================================================
-- Create Tables
-- ============================================================================

-- 1. Customers Table
-- Stores customer master data
-- Indexes on frequently queried columns for performance
CREATE TABLE dbo.Customers (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    CustomerName NVARCHAR(200) NOT NULL,
    MobileNumber NVARCHAR(20) NOT NULL,
    EmailAddress NVARCHAR(255) NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(MAX) NULL,
    ModifiedBy NVARCHAR(MAX) NULL
);

-- Index for searching customers by name
CREATE INDEX IX_Customers_Name ON dbo.Customers(CustomerName);

-- Index for searching customers by phone
CREATE INDEX IX_Customers_Mobile ON dbo.Customers(MobileNumber);

PRINT 'Created Customers table';
GO

-- 2. Partners Table
-- Stores partner/organization master data
CREATE TABLE dbo.Partners (
    PartnerId INT PRIMARY KEY IDENTITY(1,1),
    PartnerName NVARCHAR(200) NOT NULL,
    PartnerCode NVARCHAR(50) NOT NULL UNIQUE,
    ContactEmail NVARCHAR(255) NULL,
    ContactPhone NVARCHAR(20) NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(MAX) NULL,
    ModifiedBy NVARCHAR(MAX) NULL
);

-- Index for searching partners by code (unique identifier)
CREATE INDEX IX_Partners_Code ON dbo.Partners(PartnerCode);

-- Index for searching partners by name
CREATE INDEX IX_Partners_Name ON dbo.Partners(PartnerName);

PRINT 'Created Partners table';
GO

-- 3. Applications Table
-- Core table storing all application data
-- This is the fact table with dimensions from Customers and Partners
CREATE TABLE dbo.Applications (
    ApplicationId INT PRIMARY KEY IDENTITY(1,1),
    ProcessNumber NVARCHAR(50) NOT NULL UNIQUE,
    CustomerId INT NOT NULL,
    PartnerId INT NOT NULL,
    ApprovedAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    [Status] INT NOT NULL DEFAULT 1,  -- 1=InProgress, 2=Rejected, 3=Completed, 4=Cancelled
    ApplicationDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Remarks NVARCHAR(2000) NULL,
    CompletedDate DATETIME2 NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(MAX) NULL,
    ModifiedBy NVARCHAR(MAX) NULL,
    -- Foreign key constraints
    CONSTRAINT FK_Applications_Customer
        FOREIGN KEY (CustomerId)
        REFERENCES dbo.Customers(CustomerId),
    CONSTRAINT FK_Applications_Partner
        FOREIGN KEY (PartnerId)
        REFERENCES dbo.Partners(PartnerId)
);

-- Index on ProcessNumber for fast lookups
CREATE INDEX IX_Applications_ProcessNumber ON dbo.Applications(ProcessNumber);

-- Index on ApplicationDate for date-range queries and reporting
CREATE INDEX IX_Applications_ApplicationDate ON dbo.Applications(ApplicationDate);

-- Index on Status for filtering by status
CREATE INDEX IX_Applications_Status ON dbo.Applications([Status]);

-- Index on PartnerId for partner-wise analysis
CREATE INDEX IX_Applications_PartnerId ON dbo.Applications(PartnerId);

-- Index on CustomerId for customer-wise analysis
CREATE INDEX IX_Applications_CustomerId ON dbo.Applications(CustomerId);

-- Composite index on Status and ApplicationDate for common queries like
-- "Get all completed applications from last month"
CREATE INDEX IX_Applications_Status_Date ON dbo.Applications([Status], ApplicationDate);

PRINT 'Created Applications table with indexes';
GO

-- 4. AuditLogs Table
-- Tracks all changes for compliance and audit trail
CREATE TABLE dbo.AuditLogs (
    AuditLogId INT PRIMARY KEY IDENTITY(1,1),
    EntityName NVARCHAR(100) NOT NULL,
    EntityId INT NOT NULL,
    [Action] NVARCHAR(50) NOT NULL,
    OldValues NVARCHAR(MAX) NULL,  -- JSON format
    NewValues NVARCHAR(MAX) NULL,  -- JSON format
    ChangedBy NVARCHAR(255) NULL,
    IpAddress NVARCHAR(45) NULL,   -- Supports IPv6
    Reason NVARCHAR(500) NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(MAX) NULL,
    ModifiedBy NVARCHAR(MAX) NULL
);

-- Index for quick audit log queries
CREATE INDEX IX_AuditLogs_EntityName_EntityId ON dbo.AuditLogs(EntityName, EntityId);

-- Index on CreatedDate for searching recent changes
CREATE INDEX IX_AuditLogs_CreatedDate ON dbo.AuditLogs(CreatedDate);

PRINT 'Created AuditLogs table';
GO

-- ============================================================================
-- Create Stored Procedures (Optional but useful for complex queries)
-- ============================================================================

-- Procedure to get application statistics
CREATE PROCEDURE sp_GetApplicationStatistics
AS
BEGIN
    SELECT
        COUNT(*) as TotalApplications,
        SUM(CASE WHEN [Status] = 1 THEN 1 ELSE 0 END) as InProgressCount,
        SUM(CASE WHEN [Status] = 2 THEN 1 ELSE 0 END) as RejectedCount,
        SUM(CASE WHEN [Status] = 3 THEN 1 ELSE 0 END) as CompletedCount,
        SUM(CASE WHEN [Status] = 4 THEN 1 ELSE 0 END) as CancelledCount,
        SUM(CASE WHEN CONVERT(DATE, ApplicationDate) = CONVERT(DATE, GETUTCDATE()) AND [Status] = 3
                THEN ApprovedAmount ELSE 0 END) as TodayApprovedAmount,
        SUM(CASE WHEN MONTH(ApplicationDate) = MONTH(GETUTCDATE())
                 AND YEAR(ApplicationDate) = YEAR(GETUTCDATE())
                 AND [Status] = 3
                THEN ApprovedAmount ELSE 0 END) as MonthlyApprovedAmount
    FROM dbo.Applications;
END
GO

PRINT 'Created stored procedure: sp_GetApplicationStatistics';
GO

-- Procedure to get partner-wise application count
CREATE PROCEDURE sp_GetPartnerApplications
AS
BEGIN
    SELECT
        p.PartnerId,
        p.PartnerName,
        p.PartnerCode,
        COUNT(a.ApplicationId) as ApplicationCount,
        SUM(CASE WHEN a.[Status] = 3 THEN a.ApprovedAmount ELSE 0 END) as TotalApprovedAmount,
        SUM(CASE WHEN a.[Status] = 1 THEN 1 ELSE 0 END) as InProgressCount,
        SUM(CASE WHEN a.[Status] = 2 THEN 1 ELSE 0 END) as RejectedCount,
        SUM(CASE WHEN a.[Status] = 3 THEN 1 ELSE 0 END) as CompletedCount,
        SUM(CASE WHEN a.[Status] = 4 THEN 1 ELSE 0 END) as CancelledCount
    FROM dbo.Partners p
    LEFT JOIN dbo.Applications a ON p.PartnerId = a.PartnerId
    GROUP BY p.PartnerId, p.PartnerName, p.PartnerCode
    ORDER BY ApplicationCount DESC;
END
GO

PRINT 'Created stored procedure: sp_GetPartnerApplications';
GO

-- ============================================================================
-- Insert Sample/Seed Data (Optional - for testing)
-- ============================================================================

-- Note: Seed data is added in a separate script
PRINT 'Database schema created successfully!';
GO

-- ============================================================================
-- Summary
-- ============================================================================
-- Tables Created:
-- 1. Customers - Master data for loan applicants (Primary table for Customer Dimension)
-- 2. Partners - Master data for partner organizations (Primary table for Partner Dimension)
-- 3. Applications - Core transaction table with application records
-- 4. AuditLogs - Audit trail for compliance
--
-- Total indexes: 10+
-- Stored Procedures: 2
--
-- Key Design Decisions:
-- 1. Used INT for IDs (identity columns) for performance
-- 2. Used DATETIME2 for timestamps (more accurate than DATETIME)
-- 3. Used DECIMAL for currency amounts (precision and accuracy)
-- 4. Status stored as INT (1,2,3,4) for performance, application maps to enum
-- 5. JSON columns for audit logs (flexible schema)
-- 6. Multiple indexes for dashboard and reporting queries
-- ============================================================================
