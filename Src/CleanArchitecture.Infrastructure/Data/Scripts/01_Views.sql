-- =============================================
-- Views for Complex Data Queries
-- Migrated to PostgreSQL from SQL Server
-- =============================================

-- View: User Profile Summary
DROP VIEW IF EXISTS vw_user_profile_summary CASCADE;

CREATE VIEW vw_user_profile_summary AS
SELECT 
    u.id AS user_id,
    u.username,
    u.email,
    u.status,
    u.role,
    u.balance,
    u.credit_score,
    u.created_at,
    u.last_login_at,
    up.first_name,
    up.last_name,
    up.phone_number,
    up.home_address_city AS city,
    up.home_address_state AS state,
    up.home_address_country AS country,
    (SELECT CAST(COUNT(*) AS INTEGER) FROM orders WHERE user_id = u.id) AS total_orders,
    (SELECT SUM(total) FROM orders WHERE user_id = u.id) AS total_spent
FROM users u
LEFT JOIN user_profiles up ON u.id = up.user_id;

-- View: Product Inventory Status
DROP VIEW IF EXISTS vw_product_inventory_status CASCADE;

CREATE VIEW vw_product_inventory_status AS
SELECT 
    p.id AS product_id,
    p.name AS product_name,
    p.sku,
    p.price,
    p.sale_price,
    p.status AS product_status,
    p.type AS product_type,
    pi.warehouse_code,
    pi.quantity,
    pi.available_quantity,
    pi.reserved_quantity,
    pi.status AS inventory_status,
    pi.last_updated,
    CASE 
        WHEN pi.available_quantity > 100 THEN 'High Stock'
        WHEN pi.available_quantity BETWEEN 20 AND 100 THEN 'Medium Stock'
        WHEN pi.available_quantity BETWEEN 1 AND 19 THEN 'Low Stock'
        ELSE 'Out of Stock'
    END AS stock_level
FROM products p
LEFT JOIN product_inventories pi ON p.id = pi.product_id;

-- View: Order Details Summary
DROP VIEW IF EXISTS vw_order_details_summary CASCADE;

CREATE VIEW vw_order_details_summary AS
SELECT 
    o.id AS order_id,
    o.order_number,
    o.user_id,
    u.username,
    u.email,
    o.created_at AS order_date,
    o.status AS order_status,
    o.payment_method,
    o.sub_total,
    o.tax_amount,
    o.shipping_cost,
    o.total,
    o.shipping_address_city AS ship_to_city,
    o.shipping_address_state AS ship_to_state,
    o.shipping_address_country AS ship_to_country,
    (SELECT COUNT(*) FROM order_items WHERE order_id = o.id) AS total_items,
    (SELECT SUM(quantity) FROM order_items WHERE order_id = o.id) AS total_quantity
FROM orders o
INNER JOIN users u ON o.user_id = u.id;

-- View: Product Reviews Summary
DROP VIEW IF EXISTS vw_product_reviews_summary CASCADE;

CREATE VIEW vw_product_reviews_summary AS
SELECT 
    p.id AS product_id,
    p.name AS product_name,
    p.sku,
    COUNT(pr.id) AS total_reviews,
    AVG(pr.rating::NUMERIC) AS average_rating,
    SUM(CASE WHEN pr.rating = 5 THEN 1 ELSE 0 END) AS five_star_count,
    SUM(CASE WHEN pr.rating = 4 THEN 1 ELSE 0 END) AS four_star_count,
    SUM(CASE WHEN pr.rating = 3 THEN 1 ELSE 0 END) AS three_star_count,
    SUM(CASE WHEN pr.rating = 2 THEN 1 ELSE 0 END) AS two_star_count,
    SUM(CASE WHEN pr.rating = 1 THEN 1 ELSE 0 END) AS one_star_count
FROM products p
LEFT JOIN product_reviews pr ON p.id = pr.product_id
GROUP BY p.id, p.name, p.sku;

-- View: Active User Sessions
DROP VIEW IF EXISTS vw_active_user_sessions CASCADE;

CREATE VIEW vw_active_user_sessions AS
SELECT 
    us.id AS session_id,
    us.user_id,
    u.username,
    u.email,
    us.session_token,
    us.ip_address,
    us.type AS session_type,
    us.status AS session_status,
    us.created_at,
    us.last_activity_at,
    us.expires_at,
    EXTRACT(EPOCH FROM (now() - us.last_activity_at))::INTEGER / 60 AS minutes_since_last_activity
FROM user_sessions us
INNER JOIN users u ON us.user_id = u.id
WHERE us.status = 'Active' AND us.expires_at > now();

-- Verification message
DO $$
BEGIN
    RAISE NOTICE 'Views created successfully!';
END $$;

