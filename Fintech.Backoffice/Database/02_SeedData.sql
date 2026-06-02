-- ============================================================================
-- Seed Data for FinTech Backoffice Dashboard
-- Database: FintechBackofficeDb
-- Purpose: Insert sample data for testing and demonstration
-- ============================================================================

USE FintechBackofficeDb;
GO

-- ============================================================================
-- Insert Sample Customers
-- ============================================================================

INSERT INTO dbo.Customers (CustomerName, MobileNumber, EmailAddress, CreatedDate, ModifiedDate)
VALUES
    ('Ahmed Hassan', '+971501234567', 'ahmed.hassan@email.com', GETUTCDATE(), GETUTCDATE()),
    ('Fatima Al-Mansoori', '+971502345678', 'fatima.mansoori@email.com', GETUTCDATE(), GETUTCDATE()),
    ('Mohammed Al-Mazrouei', '+971503456789', 'mohammed.mazrouei@email.com', GETUTCDATE(), GETUTCDATE()),
    ('Layla Al-Naqbi', '+971504567890', 'layla.naqbi@email.com', GETUTCDATE(), GETUTCDATE()),
    ('Ali Al-Marri', '+971505678901', 'ali.marri@email.com', GETUTCDATE(), GETUTCDATE()),
    ('Noor Al-Hosani', '+971506789012', 'noor.hosani@email.com', GETUTCDATE(), GETUTCDATE()),
    ('Salim Al-Kaabi', '+971507890123', 'salim.kaabi@email.com', GETUTCDATE(), GETUTCDATE()),
    ('Hana Al-Khayeli', '+971508901234', 'hana.khayeli@email.com', GETUTCDATE(), GETUTCDATE()),
    ('Rashid Al-Suwaidi', '+971509012345', 'rashid.suwaidi@email.com', GETUTCDATE(), GETUTCDATE()),
    ('Sara Al-Falahi', '+971510123456', 'sara.falahi@email.com', GETUTCDATE(), GETUTCDATE());

PRINT 'Inserted 10 sample customers';
GO

-- ============================================================================
-- Insert Sample Partners
-- ============================================================================

INSERT INTO dbo.Partners (PartnerName, PartnerCode, ContactEmail, ContactPhone, CreatedDate, ModifiedDate)
VALUES
    ('Emirates Bank Corporation', 'EMB001', 'loans@emiratesbank.ae', '+971401234567', GETUTCDATE(), GETUTCDATE()),
    ('First National Bank', 'FNB001', 'api@fnbank.ae', '+971402345678', GETUTCDATE(), GETUTCDATE()),
    ('FinTech Solutions UAE', 'FTS001', 'partnerships@fintech.ae', '+971403456789', GETUTCDATE(), GETUTCDATE()),
    ('Gulf Credit Corporation', 'GCC001', 'integrate@gulfcredit.ae', '+971404567890', GETUTCDATE(), GETUTCDATE()),
    ('Digital Loans Provider', 'DLP001', 'api@digitalloan.ae', '+971405678901', GETUTCDATE(), GETUTCDATE()),
    ('Smart Finance Group', 'SFG001', 'partners@smartfinance.ae', '+971406789012', GETUTCDATE(), GETUTCDATE()),
    ('Express Credit Services', 'ECS001', 'support@expresscredit.ae', '+971407890123', GETUTCDATE(), GETUTCDATE()),
    ('Premium Financial Partners', 'PFP001', 'connect@premiumfinance.ae', '+971408901234', GETUTCDATE(), GETUTCDATE()),
    ('Secure Lending Ltd', 'SLL001', 'api@securelending.ae', '+971409012345', GETUTCDATE(), GETUTCDATE()),
    ('Global Finance Hub', 'GFH001', 'integration@globalfinance.ae', '+971410123456', GETUTCDATE(), GETUTCDATE());

PRINT 'Inserted 10 sample partners';
GO

-- ============================================================================
-- Insert Sample Applications
-- Creating realistic data with mix of statuses and dates
-- ============================================================================

-- Insert past applications (last 30 days)
-- Status: 1=InProgress, 2=Rejected, 3=Completed, 4=Cancelled

INSERT INTO dbo.Applications (ProcessNumber, CustomerId, PartnerId, ApprovedAmount, [Status], ApplicationDate, CompletedDate, Remarks, CreatedDate, ModifiedDate)
VALUES
    -- Completed applications (Status = 3)
    ('APP-2024-001001', 1, 1, 50000.00, 3, DATEADD(DAY, -25, GETUTCDATE()), DATEADD(DAY, -20, GETUTCDATE()), 'Approved successfully', DATEADD(DAY, -25, GETUTCDATE()), DATEADD(DAY, -20, GETUTCDATE())),
    ('APP-2024-001002', 2, 2, 75000.00, 3, DATEADD(DAY, -24, GETUTCDATE()), DATEADD(DAY, -18, GETUTCDATE()), 'Approved successfully', DATEADD(DAY, -24, GETUTCDATE()), DATEADD(DAY, -18, GETUTCDATE())),
    ('APP-2024-001003', 3, 3, 45000.00, 3, DATEADD(DAY, -22, GETUTCDATE()), DATEADD(DAY, -15, GETUTCDATE()), 'Approved successfully', DATEADD(DAY, -22, GETUTCDATE()), DATEADD(DAY, -15, GETUTCDATE())),
    ('APP-2024-001004', 4, 4, 120000.00, 3, DATEADD(DAY, -20, GETUTCDATE()), DATEADD(DAY, -12, GETUTCDATE()), 'Approved successfully', DATEADD(DAY, -20, GETUTCDATE()), DATEADD(DAY, -12, GETUTCDATE())),
    ('APP-2024-001005', 5, 5, 65000.00, 3, DATEADD(DAY, -18, GETUTCDATE()), DATEADD(DAY, -10, GETUTCDATE()), 'Approved successfully', DATEADD(DAY, -18, GETUTCDATE()), DATEADD(DAY, -10, GETUTCDATE())),
    ('APP-2024-001006', 6, 6, 95000.00, 3, DATEADD(DAY, -15, GETUTCDATE()), DATEADD(DAY, -5, GETUTCDATE()), 'Approved successfully', DATEADD(DAY, -15, GETUTCDATE()), DATEADD(DAY, -5, GETUTCDATE())),
    ('APP-2024-001007', 7, 7, 55000.00, 3, DATEADD(DAY, -12, GETUTCDATE()), DATEADD(DAY, -2, GETUTCDATE()), 'Approved successfully', DATEADD(DAY, -12, GETUTCDATE()), DATEADD(DAY, -2, GETUTCDATE())),
    ('APP-2024-001008', 8, 8, 80000.00, 3, DATEADD(DAY, -10, GETUTCDATE()), DATEADD(DAY, -1, GETUTCDATE()), 'Approved successfully', DATEADD(DAY, -10, GETUTCDATE()), DATEADD(DAY, -1, GETUTCDATE())),
    ('APP-2024-001009', 9, 9, 70000.00, 3, DATEADD(DAY, -8, GETUTCDATE()), DATEADD(DAY, 0, GETUTCDATE()), 'Approved successfully', DATEADD(DAY, -8, GETUTCDATE()), DATEADD(DAY, 0, GETUTCDATE())),
    ('APP-2024-001010', 10, 10, 110000.00, 3, DATEADD(DAY, -6, GETUTCDATE()), DATEADD(DAY, 1, GETUTCDATE()), 'Approved successfully', DATEADD(DAY, -6, GETUTCDATE()), DATEADD(DAY, 1, GETUTCDATE())),

    -- Rejected applications (Status = 2)
    ('APP-2024-002001', 1, 2, 0.00, 2, DATEADD(DAY, -19, GETUTCDATE()), DATEADD(DAY, -17, GETUTCDATE()), 'Insufficient income documentation', DATEADD(DAY, -19, GETUTCDATE()), DATEADD(DAY, -17, GETUTCDATE())),
    ('APP-2024-002002', 3, 4, 0.00, 2, DATEADD(DAY, -16, GETUTCDATE()), DATEADD(DAY, -14, GETUTCDATE()), 'Failed credit check', DATEADD(DAY, -16, GETUTCDATE()), DATEADD(DAY, -14, GETUTCDATE())),
    ('APP-2024-002003', 5, 1, 0.00, 2, DATEADD(DAY, -13, GETUTCDATE()), DATEADD(DAY, -11, GETUTCDATE()), 'Unable to verify employment', DATEADD(DAY, -13, GETUTCDATE()), DATEADD(DAY, -11, GETUTCDATE())),
    ('APP-2024-002004', 7, 3, 0.00, 2, DATEADD(DAY, -11, GETUTCDATE()), DATEADD(DAY, -9, GETUTCDATE()), 'Duplicate application within 30 days', DATEADD(DAY, -11, GETUTCDATE()), DATEADD(DAY, -9, GETUTCDATE())),
    ('APP-2024-002005', 9, 5, 0.00, 2, DATEADD(DAY, -7, GETUTCDATE()), DATEADD(DAY, -3, GETUTCDATE()), 'Incomplete documentation', DATEADD(DAY, -7, GETUTCDATE()), DATEADD(DAY, -3, GETUTCDATE())),

    -- In-Progress applications (Status = 1)
    ('APP-2024-003001', 2, 1, 0.00, 1, DATEADD(DAY, -5, GETUTCDATE()), NULL, 'Under review by operations team', DATEADD(DAY, -5, GETUTCDATE()), GETUTCDATE()),
    ('APP-2024-003002', 4, 6, 0.00, 1, DATEADD(DAY, -3, GETUTCDATE()), NULL, 'Awaiting document verification', DATEADD(DAY, -3, GETUTCDATE()), GETUTCDATE()),
    ('APP-2024-003003', 6, 2, 0.00, 1, DATEADD(DAY, -2, GETUTCDATE()), NULL, 'In credit assessment stage', DATEADD(DAY, -2, GETUTCDATE()), GETUTCDATE()),
    ('APP-2024-003004', 8, 8, 0.00, 1, DATEADD(DAY, -1, GETUTCDATE()), NULL, 'Pending final approval', DATEADD(DAY, -1, GETUTCDATE()), GETUTCDATE()),
    ('APP-2024-003005', 10, 4, 0.00, 1, DATEADD(DAY, 0, GETUTCDATE()), NULL, 'Recently submitted - initial review', DATEADD(DAY, 0, GETUTCDATE()), GETUTCDATE()),
    ('APP-2024-003006', 2, 3, 0.00, 1, DATEADD(DAY, 0, GETUTCDATE()), NULL, 'Recently submitted - initial review', DATEADD(DAY, 0, GETUTCDATE()), GETUTCDATE()),

    -- Cancelled applications (Status = 4)
    ('APP-2024-004001', 3, 7, 0.00, 4, DATEADD(DAY, -21, GETUTCDATE()), DATEADD(DAY, -17, GETUTCDATE()), 'Customer requested cancellation', DATEADD(DAY, -21, GETUTCDATE()), DATEADD(DAY, -17, GETUTCDATE())),
    ('APP-2024-004002', 5, 9, 0.00, 4, DATEADD(DAY, -17, GETUTCDATE()), DATEADD(DAY, -15, GETUTCDATE()), 'Application expired - no response from customer', DATEADD(DAY, -17, GETUTCDATE()), DATEADD(DAY, -15, GETUTCDATE())),
    ('APP-2024-004003', 7, 10, 0.00, 4, DATEADD(DAY, -14, GETUTCDATE()), DATEADD(DAY, -12, GETUTCDATE()), 'Customer applied elsewhere', DATEADD(DAY, -14, GETUTCDATE()), DATEADD(DAY, -12, GETUTCDATE()));

PRINT 'Inserted 20 sample applications with mixed statuses';
GO

-- ============================================================================
-- Insert Sample Audit Logs
-- ============================================================================

INSERT INTO dbo.AuditLogs (EntityName, EntityId, [Action], OldValues, NewValues, ChangedBy, IpAddress, Reason, CreatedDate, ModifiedDate)
VALUES
    ('Application', 1, 'Create', NULL, '{"ProcessNumber":"APP-2024-001001","ApprovedAmount":0,"Status":1}', 'system', '192.168.1.100', 'Application submitted', DATEADD(DAY, -25, GETUTCDATE()), DATEADD(DAY, -25, GETUTCDATE())),
    ('Application', 1, 'Update', '{"Status":1}', '{"Status":3,"ApprovedAmount":50000}', 'admin', '192.168.1.101', 'Application approved', DATEADD(DAY, -20, GETUTCDATE()), DATEADD(DAY, -20, GETUTCDATE())),
    ('Application', 2, 'Create', NULL, '{"ProcessNumber":"APP-2024-001002","ApprovedAmount":0,"Status":1}', 'system', '192.168.1.100', 'Application submitted', DATEADD(DAY, -24, GETUTCDATE()), DATEADD(DAY, -24, GETUTCDATE())),
    ('Application', 11, 'Create', NULL, '{"ProcessNumber":"APP-2024-002001","ApprovedAmount":0,"Status":1}', 'system', '192.168.1.100', 'Application submitted', DATEADD(DAY, -19, GETUTCDATE()), DATEADD(DAY, -19, GETUTCDATE())),
    ('Application', 11, 'Update', '{"Status":1}', '{"Status":2}', 'operations', '192.168.1.102', 'Application rejected - insufficient documents', DATEADD(DAY, -17, GETUTCDATE()), DATEADD(DAY, -17, GETUTCDATE())),
    ('Partner', 1, 'Create', NULL, '{"PartnerName":"Emirates Bank Corporation","PartnerCode":"EMB001"}', 'admin', '192.168.1.101', 'Partner onboarded', DATEADD(DAY, -90, GETUTCDATE()), DATEADD(DAY, -90, GETUTCDATE())),
    ('Customer', 1, 'Create', NULL, '{"CustomerName":"Ahmed Hassan","MobileNumber":"+971501234567"}', 'system', '192.168.1.100', 'Customer created', DATEADD(DAY, -45, GETUTCDATE()), DATEADD(DAY, -45, GETUTCDATE()));

PRINT 'Inserted 7 sample audit log entries';
GO

-- ============================================================================
-- Verify Data Insertion
-- ============================================================================

PRINT '';
PRINT '========== Data Verification ==========';
PRINT 'Total Customers: ' + CAST((SELECT COUNT(*) FROM dbo.Customers) AS NVARCHAR(10));
PRINT 'Total Partners: ' + CAST((SELECT COUNT(*) FROM dbo.Partners) AS NVARCHAR(10));
PRINT 'Total Applications: ' + CAST((SELECT COUNT(*) FROM dbo.Applications) AS NVARCHAR(10));
PRINT 'Total Audit Logs: ' + CAST((SELECT COUNT(*) FROM dbo.AuditLogs) AS NVARCHAR(10));
PRINT '';

-- Application status breakdown
SELECT
    'InProgress' as Status,
    COUNT(*) as Count
FROM dbo.Applications
WHERE [Status] = 1
UNION ALL
SELECT 'Rejected', COUNT(*) FROM dbo.Applications WHERE [Status] = 2
UNION ALL
SELECT 'Completed', COUNT(*) FROM dbo.Applications WHERE [Status] = 3
UNION ALL
SELECT 'Cancelled', COUNT(*) FROM dbo.Applications WHERE [Status] = 4;

PRINT '';
PRINT 'Seed data insertion completed successfully!';
GO

-- ============================================================================
-- End of Script
-- ============================================================================
