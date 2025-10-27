-- =============================================
-- Stored Procedures for Testing
-- Migrated to PostgreSQL (PL/pgSQL) from SQL Server
-- =============================================

-- Stored Procedure: Get Sales Report
DROP PROCEDURE IF EXISTS sp_get_sales_report(timestamp, timestamp, text);

CREATE OR REPLACE PROCEDURE sp_get_sales_report(
    p_from_date timestamp DEFAULT NULL,
    p_to_date timestamp DEFAULT NULL,
    p_status text DEFAULT NULL,
    INOUT result refcursor DEFAULT 'sales_report_cursor'
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_from_date timestamp;
    v_to_date timestamp;
BEGIN
    -- Set default date range if not provided
    IF p_from_date IS NULL THEN
        v_from_date := now() - INTERVAL '30 days';
    ELSE
        v_from_date := p_from_date;
    END IF;
    
    IF p_to_date IS NULL THEN
        v_to_date := now();
    ELSE
        v_to_date := p_to_date;
    END IF;
    
    OPEN result FOR
    SELECT 
        o.created_at::date AS order_date,
        COUNT(o.id)::bigint AS total_orders,
        SUM(o.total) AS total_revenue,
        AVG(o.total) AS average_order_value,
        SUM(o.shipping_cost) AS total_shipping,
        SUM(o.tax_amount) AS total_tax,
        COUNT(DISTINCT o.user_id)::bigint AS unique_customers,
        SUM(oi.quantity)::bigint AS total_items_sold
    FROM orders o
    LEFT JOIN order_items oi ON o.id = oi.order_id
    WHERE o.created_at >= v_from_date 
        AND o.created_at <= v_to_date
        AND (p_status IS NULL OR o.status = p_status)
    GROUP BY o.created_at::date
    ORDER BY order_date DESC;
END;
$$;

-- Stored Procedure: Create Order with Items
DROP PROCEDURE IF EXISTS sp_create_order_with_items(uuid, varchar, NUMERIC, NUMERIC, NUMERIC);

CREATE OR REPLACE PROCEDURE sp_create_order_with_items(
    p_user_id uuid,
    p_order_number varchar(50),
    p_sub_total NUMERIC(18,2),
    p_tax_amount NUMERIC(18,2),
    p_shipping_cost NUMERIC(18,2),
    INOUT p_order_id uuid DEFAULT NULL,
    OUT p_message text DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
BEGIN
    BEGIN
        -- Create new order
        p_order_id := gen_random_uuid();
        
        INSERT INTO orders (id, user_id, order_number, status, sub_total, tax_amount, shipping_cost, total, created_at)
        VALUES (
            p_order_id,
            p_user_id,
            p_order_number,
            'Pending',
            p_sub_total,
            p_tax_amount,
            p_shipping_cost,
            p_sub_total + p_tax_amount + p_shipping_cost,
            timezone('utc', now())
        );
        
        p_message := 'Order created successfully';
        
    EXCEPTION
        WHEN OTHERS THEN
            -- Rollback is automatic in PostgreSQL for procedures
            RAISE EXCEPTION 'Error creating order: %', SQLERRM;
    END;
END;
$$;

-- Alternative function version for easier calling from EF Core
DROP FUNCTION IF EXISTS fn_create_order_with_items(uuid, varchar, NUMERIC, NUMERIC, NUMERIC);

CREATE OR REPLACE FUNCTION fn_create_order_with_items(
    p_user_id uuid,
    p_order_number varchar(50),
    p_sub_total NUMERIC(18,2),
    p_tax_amount NUMERIC(18,2),
    p_shipping_cost NUMERIC(18,2)
)
RETURNS TABLE (
    order_id uuid,
    message text
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_order_id uuid;
BEGIN
    -- Create new order
    v_order_id := gen_random_uuid();
    
    INSERT INTO orders (id, user_id, order_number, status, sub_total, tax_amount, shipping_cost, total, created_at)
    VALUES (
        v_order_id,
        p_user_id,
        p_order_number,
        'Pending',
        p_sub_total,
        p_tax_amount,
        p_shipping_cost,
        p_sub_total + p_tax_amount + p_shipping_cost,
        timezone('utc', now())
    );
    
    RETURN QUERY SELECT v_order_id, 'Order created successfully'::text;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE EXCEPTION 'Error creating order: %', SQLERRM;
END;
$$;

-- Function wrapper for sales report (for easier calling from EF Core)
DROP FUNCTION IF EXISTS fn_get_sales_report(timestamp, timestamp, text);

CREATE OR REPLACE FUNCTION fn_get_sales_report(
    p_from_date timestamp DEFAULT NULL,
    p_to_date timestamp DEFAULT NULL,
    p_status text DEFAULT NULL
)
RETURNS TABLE (
    order_date date,
    total_orders bigint,
    total_revenue NUMERIC(18,2),
    average_order_value NUMERIC(18,2),
    total_shipping NUMERIC(18,2),
    total_tax NUMERIC(18,2),
    unique_customers bigint,
    total_items_sold bigint
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_from_date timestamp;
    v_to_date timestamp;
BEGIN
    -- Set default date range if not provided
    IF p_from_date IS NULL THEN
        v_from_date := now() - INTERVAL '30 days';
    ELSE
        v_from_date := p_from_date;
    END IF;
    
    IF p_to_date IS NULL THEN
        v_to_date := now();
    ELSE
        v_to_date := p_to_date;
    END IF;
    
    RETURN QUERY
    SELECT 
        o.created_at::date AS order_date,
        COUNT(o.id)::bigint AS total_orders,
        SUM(o.total) AS total_revenue,
        AVG(o.total) AS average_order_value,
        SUM(o.shipping_cost) AS total_shipping,
        SUM(o.tax_amount) AS total_tax,
        COUNT(DISTINCT o.user_id)::bigint AS unique_customers,
        SUM(oi.quantity)::bigint AS total_items_sold
    FROM orders o
    LEFT JOIN order_items oi ON o.id = oi.order_id
    WHERE o.created_at >= v_from_date 
        AND o.created_at <= v_to_date
        AND (p_status IS NULL OR o.status = p_status)
    GROUP BY o.created_at::date
    ORDER BY order_date DESC;
END;
$$;

-- Verification message
DO $$
BEGIN
    RAISE NOTICE 'Stored Procedures created successfully!';
END $$;

