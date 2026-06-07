/* ============================================================================
   EP101_* schema mock — lets you run the production query locally.
   Creates the 5 tables referenced by the production query, seeds a few rows.
   Safe to re-run.
   ============================================================================ */

USE loan_dashboard;
GO

-- ───────────── 1. Drop existing (for clean re-runs) ─────────────
IF OBJECT_ID('dbo.EP101_ProcessStage',   'U') IS NOT NULL DROP TABLE dbo.EP101_ProcessStage;
IF OBJECT_ID('dbo.EP101_Process',        'U') IS NOT NULL DROP TABLE dbo.EP101_Process;
IF OBJECT_ID('dbo.EP101_StageEffective', 'U') IS NOT NULL DROP TABLE dbo.EP101_StageEffective;
IF OBJECT_ID('dbo.EP101_StageStatus',    'U') IS NOT NULL DROP TABLE dbo.EP101_StageStatus;
IF OBJECT_ID('dbo.EP101_ActionMaster',   'U') IS NOT NULL DROP TABLE dbo.EP101_ActionMaster;
IF OBJECT_ID('dbo.EP101_Partner',        'U') IS NOT NULL DROP TABLE dbo.EP101_Partner;
GO

-- ───────────── 2. Tables ─────────────
CREATE TABLE dbo.EP101_Partner (
    PartnerId   INT IDENTITY(1,1) PRIMARY KEY,
    PartnerName NVARCHAR(150) NOT NULL,
    IsActive    BIT NOT NULL DEFAULT 1
);

CREATE TABLE dbo.EP101_Process (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    ProcessNo   NVARCHAR(50) NOT NULL,
    Workflow_Id INT NOT NULL,
    Partner_Id  INT NULL
);
CREATE INDEX IX_Process_Partner ON dbo.EP101_Process(Partner_Id);

CREATE TABLE dbo.EP101_ProcessStage (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Process_Id      INT NOT NULL,
    ProcessNo       NVARCHAR(50) NOT NULL,
    StageNo         INT NOT NULL,
    IsApplicable    BIT NOT NULL DEFAULT 1,
    StageStatus_Id  INT NULL,
    CreatedOn       DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
    ReceivedOn      DATETIME2(3) NULL,
    CompletedOn     DATETIME2(3) NULL
);
CREATE INDEX IX_ProcessStage_CreatedOn ON dbo.EP101_ProcessStage(CreatedOn DESC);

CREATE TABLE dbo.EP101_StageEffective (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    WFEffective_Id  INT NOT NULL,
    StageNo         INT NOT NULL,
    StageName_E     NVARCHAR(100) NOT NULL,
    StageName_A     NVARCHAR(100) NULL
);

CREATE TABLE dbo.EP101_StageStatus (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    StatusName_E  NVARCHAR(100) NOT NULL,
    StatusName_A  NVARCHAR(100) NULL
);

CREATE TABLE dbo.EP101_ActionMaster (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    ActionName_E NVARCHAR(100) NOT NULL
);
GO

-- ───────────── 3. Lookup data ─────────────
INSERT INTO dbo.EP101_StageEffective (WFEffective_Id, StageNo, StageName_E, StageName_A) VALUES
    (1,  1, N'Financial Data',               N'البيانات المالية'),
    (1,  2, N'Salary Certificate Check',     N'فحص شهادة الراتب'),
    (1,  3, N'Offer Selection',              N'اختيار العرض'),
    (1,  4, N'Addition of Asset',            N'إضافة الأصل'),
    (1,  5, N'Payment Collection',           N'تحصيل الدفعة'),
    (1,  6, N'Card Verification',            N'التحقق من البطاقة'),
    (1,  7, N'Call Confirmation',            N'تأكيد المكالمة'),
    (1,  8, N'E-Contract Signing',           N'توقيع العقد الإلكتروني'),
    (1,  9, N'E-Promissory Note Acceptance', N'قبول السند الإلكتروني'),
    (1, 10, N'Delivery Status',              N'حالة التسليم'),
    (1, 11, N'Delivery Acknowledgement',     N'إقرار التسليم'),
    (1, 12, N'Commodity Purchase',           N'شراء السلعة');

INSERT INTO dbo.EP101_StageStatus (StatusName_E, StatusName_A) VALUES
    (N'Active',    N'نشط'),
    (N'Completed', N'مكتمل'),
    (N'Cancelled', N'ملغى');

INSERT INTO dbo.EP101_ActionMaster (ActionName_E) VALUES
    (N'Approve'), (N'Reject'), (N'Forward'), (N'Return');

INSERT INTO dbo.EP101_Partner (PartnerName, IsActive) VALUES
    (N'Al Rajhi Bank',        1),
    (N'Saudi National Bank',  1),
    (N'Riyad Bank',           1),
    (N'Bank Albilad',         1),
    (N'Alinma Bank',          1);
GO

-- ───────────── 4. Sample processes (randomly assigned to partners) ─────────────
;WITH n AS (
    SELECT TOP (50) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS r
    FROM sys.all_objects
)
INSERT INTO dbo.EP101_Process (ProcessNo, Workflow_Id, Partner_Id)
SELECT
    N'PROC-' + RIGHT(N'00000' + CAST(r AS NVARCHAR(10)), 5),
    1,
    ((ABS(CHECKSUM(NEWID())) % 5) + 1)   -- PartnerId 1..5
FROM n;
GO

-- ───────────── 5. ProcessStage rows (every process completes a few stages today) ─────────────
INSERT INTO dbo.EP101_ProcessStage
    (Process_Id, ProcessNo, StageNo, IsApplicable, StageStatus_Id, CreatedOn, ReceivedOn, CompletedOn)
SELECT
    p.Id,
    p.ProcessNo,
    s.StageNo,
    1,
    2,                                                                              -- Completed
    DATEADD(SECOND, -ABS(CHECKSUM(NEWID()) % 28800), SYSUTCDATETIME()),             -- created within last 8h
    DATEADD(SECOND, -ABS(CHECKSUM(NEWID()) % 14400), SYSUTCDATETIME()),
    DATEADD(SECOND, -ABS(CHECKSUM(NEWID()) %  7200), SYSUTCDATETIME())              -- completed within last 2h
FROM      dbo.EP101_Process p
CROSS APPLY (
    -- each process completes a random number of stages (1..stage_no)
    SELECT StageNo
    FROM   dbo.EP101_StageEffective
    WHERE  WFEffective_Id = 1
      AND  StageNo <= (ABS(CHECKSUM(NEWID())) % 12) + 1
) s;
GO

-- ───────────── 6. Sanity check — run the production query ─────────────
;WITH x AS (
    SELECT
        ps.Id, ps.ProcessNo, ps.StageNo, se.StageName_E,
        ps.IsApplicable, ps.StageStatus_Id, ss.StatusName_E, ss.StatusName_A,
        am.ActionName_E, ps.CreatedOn, ps.ReceivedOn, ps.CompletedOn
    FROM      dbo.EP101_Process              p
    LEFT JOIN dbo.EP101_ProcessStage         ps ON ps.Process_Id      = p.Id
    LEFT JOIN dbo.EP101_StageEffective       se ON se.StageNo         = ps.StageNo
                                               AND se.WFEffective_Id  = p.Workflow_Id
    LEFT JOIN dbo.EP101_StageStatus          ss ON ps.StageStatus_Id  = ss.Id
    LEFT JOIN dbo.EP101_ActionMaster         am ON am.Id              = ps.StageStatus_Id
    WHERE     ps.IsApplicable = 1
      AND     ps.CompletedOn IS NOT NULL
      AND     CAST(ps.CreatedOn AS DATE) = CAST(GETDATE() AS DATE)
)
SELECT x.StageNo, x.StageName_E, COUNT(x.ProcessNo) AS Stage_Count
FROM   x
GROUP BY x.StageNo, x.StageName_E
ORDER BY x.StageNo;
GO
