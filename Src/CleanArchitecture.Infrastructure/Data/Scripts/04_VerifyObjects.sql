-- =============================================
-- Verification Script - Check Database Objects
-- Migrated to PostgreSQL from SQL Server
-- =============================================

DO $$
BEGIN
    RAISE NOTICE '=== Verifying Database Objects ===';
END $$;

-- Check Views
DO $$
BEGIN
    RAISE NOTICE 'Checking Views...';
END $$;

SELECT 
    'View' AS object_type,
    table_name AS object_name,
    'public' AS schema_name
FROM information_schema.views 
WHERE table_schema = 'public' 
    AND table_name IN ('vw_user_profile_summary', 'vw_product_inventory_status', 'vw_order_details_summary', 'vw_product_reviews_summary', 'vw_active_user_sessions')
ORDER BY table_name;

-- Check Functions
DO $$
BEGIN
    RAISE NOTICE 'Checking Functions...';
END $$;

SELECT 
    'Function' AS object_type,
    routine_name AS object_name,
    routine_type AS object_type_desc,
    'public' AS schema_name
FROM information_schema.routines 
WHERE routine_schema = 'public' 
    AND routine_type = 'FUNCTION'
    AND routine_name IN (
        'fn_calculate_user_lifetime_value', 
        'fn_get_product_average_rating', 
        'fn_calculate_order_discount', 
        'fn_get_user_order_history',
        'fn_get_top_selling_products',
        'fn_get_products_low_on_stock',
        'fn_get_user_activity_summary',
        'fn_create_order_with_items',
        'fn_get_sales_report'
    )
ORDER BY routine_name;

-- Check Stored Procedures
DO $$
BEGIN
    RAISE NOTICE 'Checking Stored Procedures...';
END $$;

SELECT 
    'Stored Procedure' AS object_type,
    routine_name AS object_name,
    'public' AS schema_name
FROM information_schema.routines 
WHERE routine_schema = 'public' 
    AND routine_type = 'PROCEDURE'
    AND routine_name IN (
        'sp_create_order_with_items', 
        'sp_get_sales_report',
        'sp_create_order_with_items_simple',
        'sp_get_sales_report_simple'
    )
ORDER BY routine_name;

-- Summary
DO $$
BEGIN
    RAISE NOTICE '=== Summary ===';
END $$;

SELECT 
    'Total Views' AS object_type,
    COUNT(*) AS count
FROM information_schema.views 
WHERE table_schema = 'public' 
    AND table_name IN ('vw_user_profile_summary', 'vw_product_inventory_status', 'vw_order_details_summary', 'vw_product_reviews_summary', 'vw_active_user_sessions')

UNION ALL

SELECT 
    'Total Functions' AS object_type,
    COUNT(*) AS count
FROM information_schema.routines 
WHERE routine_schema = 'public' 
    AND routine_type = 'FUNCTION'
    AND routine_name IN (
        'fn_calculate_user_lifetime_value', 
        'fn_get_product_average_rating', 
        'fn_calculate_order_discount', 
        'fn_get_user_order_history',
        'fn_get_top_selling_products',
        'fn_get_products_low_on_stack',
        'fn_get_user_activity_summary',
        'fn_create_order_with_items',
        'fn_get_sales_report'
    )

UNION ALL

SELECT 
    'Total Stored Procedures' AS object_type,
    COUNT(*) AS count
FROM information_schema.routines 
WHERE routine_schema = 'public' 
    AND routine_type = 'PROCEDURE'
    AND routine_name IN (
        'sp_create_order_with_items', 
        'sp_get_sales_report',
        'sp_create_order_with_items_simple',
        'sp_get_sales_report_simple'
    );

DO $$
BEGIN
    RAISE NOTICE 'Verification completed!';
END $$;
