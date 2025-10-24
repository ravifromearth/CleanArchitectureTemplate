-- =============================================
-- Verification Script - Check Database Objects
-- =============================================

PRINT '=== Verifying Database Objects ===';

-- Check Views
PRINT 'Checking Views...';
SELECT 
    'View' AS ObjectType,
    name AS ObjectName,
    create_date AS CreatedDate
FROM sys.views 
WHERE name IN ('vw_UserProfileSummary', 'vw_ProductInventoryStatus', 'vw_OrderDetailsSummary')
ORDER BY name;

-- Check Functions
PRINT 'Checking Functions...';
SELECT 
    'Function' AS ObjectType,
    name AS ObjectName,
    type_desc AS ObjectTypeDesc,
    create_date AS CreatedDate
FROM sys.objects 
WHERE type IN ('FN', 'TF') 
    AND name IN ('fn_CalculateUserLifetimeValue', 'fn_CalculateOrderDiscount', 'fn_GetUserOrderHistory')
ORDER BY name;

-- Check Stored Procedures
PRINT 'Checking Stored Procedures...';
SELECT 
    'Stored Procedure' AS ObjectType,
    name AS ObjectName,
    create_date AS CreatedDate
FROM sys.procedures 
WHERE name IN ('sp_CreateOrderWithItems', 'sp_UpdateOrderStatus', 'sp_RestockProductInventory', 'sp_GetSalesReport', 'sp_ProcessUserRegistration', 'sp_GetCustomerInsights')
ORDER BY name;

-- Summary
PRINT '=== Summary ===';
SELECT 
    'Total Views' AS ObjectType,
    COUNT(*) AS Count
FROM sys.views 
WHERE name IN ('vw_UserProfileSummary', 'vw_ProductInventoryStatus', 'vw_OrderDetailsSummary')

UNION ALL

SELECT 
    'Total Functions' AS ObjectType,
    COUNT(*) AS Count
FROM sys.objects 
WHERE type IN ('FN', 'TF') 
    AND name IN ('fn_CalculateUserLifetimeValue', 'fn_CalculateOrderDiscount', 'fn_GetUserOrderHistory')

UNION ALL

SELECT 
    'Total Stored Procedures' AS ObjectType,
    COUNT(*) AS Count
FROM sys.procedures 
WHERE name IN ('sp_CreateOrderWithItems', 'sp_UpdateOrderStatus', 'sp_RestockProductInventory', 'sp_GetSalesReport', 'sp_ProcessUserRegistration', 'sp_GetCustomerInsights');

PRINT 'Verification completed!';

