-- =============================================
-- Scalar and Table-Valued Functions
-- Migrated to PostgreSQL (PL/pgSQL) from SQL Server
-- =============================================

-- Scalar Function: Calculate User Lifetime Value
DROP FUNCTION IF EXISTS fn_calculate_user_lifetime_value(uuid);

CREATE OR REPLACE FUNCTION fn_calculate_user_lifetime_value(
    p_user_id uuid
)
RETURNS NUMERIC(18,2)
LANGUAGE plpgsql
AS $$
DECLARE
    v_lifetime_value NUMERIC(18,2);
BEGIN
    SELECT COALESCE(SUM(total), 0) INTO v_lifetime_value
    FROM orders
    WHERE user_id = p_user_id AND status IN ('Delivered', 'Processing', 'Shipped');
    
    RETURN v_lifetime_value;
END;
$$;

-- Scalar Function: Get Product Average Rating
DROP FUNCTION IF EXISTS fn_get_product_average_rating(uuid);

CREATE OR REPLACE FUNCTION fn_get_product_average_rating(
    p_product_id uuid
)
RETURNS NUMERIC(3,2)
LANGUAGE plpgsql
AS $$
DECLARE
    v_average_rating NUMERIC(3,2);
BEGIN
    SELECT COALESCE(AVG(rating::NUMERIC(3,2)), 0) INTO v_average_rating
    FROM product_reviews
    WHERE product_id = p_product_id AND status = 'Approved';
    
    RETURN v_average_rating;
END;
$$;

-- Scalar Function: Calculate Order Discount
DROP FUNCTION IF EXISTS fn_calculate_order_discount(NUMERIC, text);

CREATE OR REPLACE FUNCTION fn_calculate_order_discount(
    p_sub_total NUMERIC(18,2),
    p_user_role text
)
RETURNS NUMERIC(18,2)
LANGUAGE plpgsql
AS $$
DECLARE
    v_discount NUMERIC(18,2) := 0;
BEGIN
    -- Apply role-based discounts
    IF p_user_role = 'SuperAdmin' THEN
        v_discount := p_sub_total * 0.20; -- 20% discount
    ELSIF p_user_role = 'Admin' THEN
        v_discount := p_sub_total * 0.15; -- 15% discount
    ELSIF p_user_role = 'Moderator' THEN
        v_discount := p_sub_total * 0.10; -- 10% discount
    ELSIF p_sub_total > 1000 THEN
        v_discount := p_sub_total * 0.05; -- 5% for orders over $1000
    END IF;
    
    RETURN v_discount;
END;
$$;

-- Table-Valued Function: Get User Order History
DROP FUNCTION IF EXISTS fn_get_user_order_history(uuid, timestamp, timestamp);

CREATE OR REPLACE FUNCTION fn_get_user_order_history(
    p_user_id uuid,
    p_from_date timestamp DEFAULT NULL,
    p_to_date timestamp DEFAULT NULL
)
RETURNS TABLE (
    id uuid,
    order_number varchar(50),
    created_at timestamp,
    status text,
    payment_method text,
    total NUMERIC(18,2),
    item_count bigint,
    total_quantity bigint
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        o.id,
        o.order_number,
        o.created_at,
        o.status,
        o.payment_method,
        o.total,
        COUNT(oi.id) AS item_count,
        SUM(oi.quantity) AS total_quantity
    FROM orders o
    LEFT JOIN order_items oi ON o.id = oi.order_id
    WHERE o.user_id = p_user_id
        AND (p_from_date IS NULL OR o.created_at >= p_from_date)
        AND (p_to_date IS NULL OR o.created_at <= p_to_date)
    GROUP BY o.id, o.order_number, o.created_at, o.status, o.payment_method, o.total;
END;
$$;

-- Table-Valued Function: Get Top Selling Products
DROP FUNCTION IF EXISTS fn_get_top_selling_products(integer, timestamp, timestamp);

CREATE OR REPLACE FUNCTION fn_get_top_selling_products(
    p_top_n integer DEFAULT 10,
    p_from_date timestamp DEFAULT NULL,
    p_to_date timestamp DEFAULT NULL
)
RETURNS TABLE (
    product_id uuid,
    product_name varchar(200),
    sku varchar(100),
    price NUMERIC(18,4),
    times_sold bigint,
    total_quantity_sold bigint,
    total_revenue NUMERIC(18,4),
    average_selling_price NUMERIC(18,4)
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.id AS product_id,
        p.name AS product_name,
        p.sku,
        p.price,
        COUNT(oi.id) AS times_sold,
        SUM(oi.quantity) AS total_quantity_sold,
        SUM(oi.total_price) AS total_revenue,
        AVG(oi.unit_price) AS average_selling_price
    FROM products p
    INNER JOIN order_items oi ON p.id = oi.product_id
    INNER JOIN orders o ON oi.order_id = o.id
    WHERE (p_from_date IS NULL OR o.created_at >= p_from_date)
        AND (p_to_date IS NULL OR o.created_at <= p_to_date)
        AND o.status IN ('Processing', 'Shipped', 'Delivered')
    GROUP BY p.id, p.name, p.sku, p.price
    ORDER BY total_quantity_sold DESC
    LIMIT p_top_n;
END;
$$;

-- Table-Valued Function: Get Products Low on Stock
DROP FUNCTION IF EXISTS fn_get_products_low_on_stock(integer);

CREATE OR REPLACE FUNCTION fn_get_products_low_on_stock(
    p_threshold integer DEFAULT 20
)
RETURNS TABLE (
    product_id uuid,
    product_name varchar(200),
    sku varchar(100),
    product_status text,
    warehouse_code varchar(50),
    quantity integer,
    available_quantity integer,
    reserved_quantity integer,
    last_restocked timestamp
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.id AS product_id,
        p.name AS product_name,
        p.sku,
        p.status AS product_status,
        pi.warehouse_code,
        pi.quantity,
        pi.available_quantity,
        pi.reserved_quantity,
        pi.last_restocked
    FROM products p
    INNER JOIN product_inventories pi ON p.id = pi.product_id
    WHERE pi.available_quantity <= p_threshold
        AND p.status = 'Active'
        AND pi.status = 'InStock';
END;
$$;

-- Table-Valued Function: Get User Activity Summary
DROP FUNCTION IF EXISTS fn_get_user_activity_summary(uuid);

CREATE OR REPLACE FUNCTION fn_get_user_activity_summary(
    p_user_id uuid
)
RETURNS TABLE (
    user_id uuid,
    total_orders bigint,
    total_spent NUMERIC(18,2),
    total_reviews bigint,
    active_sessions bigint,
    last_activity timestamp,
    average_rating NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p_user_id AS user_id,
        (SELECT COUNT(*) FROM orders WHERE user_id = p_user_id) AS total_orders,
        (SELECT SUM(total) FROM orders WHERE user_id = p_user_id AND status = 'Delivered') AS total_spent,
        (SELECT COUNT(*) FROM product_reviews WHERE user_id = p_user_id) AS total_reviews,
        (SELECT COUNT(*) FROM user_sessions WHERE user_id = p_user_id AND status = 'Active') AS active_sessions,
        (SELECT MAX(last_activity_at) FROM user_sessions WHERE user_id = p_user_id) AS last_activity,
        (SELECT AVG(rating::NUMERIC) FROM product_reviews WHERE user_id = p_user_id) AS average_rating;
END;
$$;

-- Verification message
DO $$
BEGIN
    RAISE NOTICE 'Functions created successfully!';
END $$;

