/* ============================================================================
   Loan Dashboard — SQL Server schema
   Equivalent of the existing PostgreSQL loan_dashboard database
   Run as a user with CREATE DATABASE permission (sysadmin / dbcreator).
   ============================================================================ */

-- ───────────────────────── 1. Database ─────────────────────────
IF DB_ID(N'loan_dashboard') IS NULL
BEGIN
    CREATE DATABASE loan_dashboard;
END
GO

USE loan_dashboard;
GO

-- ───────────────────────── 2. Tables ───────────────────────────
IF OBJECT_ID(N'dbo.loan_stage_tracking', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.loan_stage_tracking
    (
        id              UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_loan_stage_tracking PRIMARY KEY
                                         CONSTRAINT DF_lst_id          DEFAULT NEWSEQUENTIALID(),
        application_id  NVARCHAR(50)     NOT NULL,
        stage_name      NVARCHAR(100)    NOT NULL,
        status          NVARCHAR(50)     NOT NULL CONSTRAINT DF_lst_status      DEFAULT N'Active',
        created_at      DATETIME2(3)     NOT NULL CONSTRAINT DF_lst_created_at  DEFAULT SYSUTCDATETIME()
    );

    CREATE INDEX IX_lst_application_id ON dbo.loan_stage_tracking(application_id);
    CREATE INDEX IX_lst_stage_name     ON dbo.loan_stage_tracking(stage_name);
    CREATE INDEX IX_lst_created_at     ON dbo.loan_stage_tracking(created_at DESC);
END
GO

IF OBJECT_ID(N'dbo.stage_aggregates', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.stage_aggregates
    (
        stage_name   NVARCHAR(100) NOT NULL CONSTRAINT PK_stage_aggregates PRIMARY KEY,
        [count]      INT           NOT NULL CONSTRAINT DF_sa_count        DEFAULT 0,
        last_updated DATETIME2(3)  NOT NULL CONSTRAINT DF_sa_last_updated DEFAULT SYSUTCDATETIME()
    );
END
GO

-- ───────────────────────── 3. Seed stages ──────────────────────
MERGE dbo.stage_aggregates AS tgt
USING (VALUES
    (N'Financial Data'),
    (N'Salary Certificate Check'),
    (N'Offer Selection'),
    (N'Addition of Asset'),
    (N'Payment Collection'),
    (N'Card Verification'),
    (N'Call Confirmation'),
    (N'E-Contract Signing'),
    (N'E-Promissory Note Acceptance'),
    (N'Delivery Status'),
    (N'Delivery Acknowledgement'),
    (N'Commodity Purchase')
) AS src(stage_name)
ON  tgt.stage_name = src.stage_name
WHEN NOT MATCHED THEN
    INSERT (stage_name, [count], last_updated) VALUES (src.stage_name, 0, SYSUTCDATETIME());
GO

-- ───────────────────────── 4. Demo data ────────────────────────
-- 25 Card Verification rows so the alert fires immediately
;WITH numbers AS (
    SELECT TOP (25) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
    FROM sys.all_objects
)
INSERT INTO dbo.loan_stage_tracking (application_id, stage_name, status, created_at)
SELECT
    N'CARD-' + RIGHT(N'00000' + CAST(n AS NVARCHAR(10)), 5),
    N'Card Verification',
    N'Active',
    DATEADD(SECOND, -ABS(CHECKSUM(NEWID()) % 3600), SYSUTCDATETIME())
FROM numbers;
GO

-- ───────────────────────── 5. Sanity check ─────────────────────
SELECT stage_name, COUNT(*) AS [count]
FROM   dbo.loan_stage_tracking
GROUP  BY stage_name
ORDER  BY stage_name;
GO
