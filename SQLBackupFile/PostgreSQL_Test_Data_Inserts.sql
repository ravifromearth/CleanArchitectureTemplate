-- PostgreSQL Test Data INSERT Statements
-- Generated for DbcomplexScriptPostgreSQL schema
-- Execute these statements AFTER running the DDL schema creation script

-- ============================================================================
-- 1. INSERT INTO users (base table - no dependencies)
-- ============================================================================

INSERT INTO public.users (username, email, bio, last_login_at, birth_date, preferred_login_time, metadata, tags, favorite_numbers, credit_score, balance, status, role, created_at, updated_at) VALUES
('john_doe', 'john.doe@email.com', 'Software developer with 10 years experience', '2025-10-26 14:30:00', '1990-05-15', '09:00:00', '{"theme": "dark", "notifications": true}', 'developer, tech, software', '7, 13, 42', 750.500000, 5234.7500, 'Active', 'Admin', '2024-01-15 10:00:00', '2025-10-20 08:00:00'),
('jane_smith', 'jane.smith@email.com', 'Marketing specialist and content creator', '2025-10-25 18:45:00', '1988-08-22', '08:30:00', '{"language": "en", "timezone": "PST"}', 'marketing, content, social', '3, 9, 21', 820.750000, 12500.2500, 'Active', 'SuperAdmin', '2024-02-10 11:30:00', '2025-10-18 12:00:00'),
('bob_wilson', 'bob.wilson@email.com', 'Graphic designer and UI/UX enthusiast', '2025-10-24 10:15:00', '1995-03-10', '10:00:00', '{"color_scheme": "light"}', 'design, ui, ux', '2, 5, 8', 680.250000, 3450.0000, 'Active', 'Moderator', '2024-03-20 09:00:00', '2025-10-15 14:00:00'),
('alice_johnson', 'alice.johnson@email.com', 'Data scientist passionate about AI', '2025-10-27 07:20:00', '1992-11-30', '07:00:00', '{"ai_mode": true}', 'data, ai, science', '1, 4, 16', 795.000000, 8920.5000, 'Active', 'User', '2024-04-05 13:00:00', '2025-10-25 16:00:00'),
('charlie_brown', 'charlie.brown@email.com', 'Project manager with agile expertise', '2025-10-20 16:00:00', '1985-07-18', '08:00:00', '{"methodology": "agile"}', 'project, management, agile', '10, 20, 30', 710.500000, 6780.3300, 'Active', 'Admin', '2024-05-12 10:30:00', '2025-10-10 11:00:00'),
('diana_prince', 'diana.prince@email.com', 'Cybersecurity expert and ethical hacker', '2025-10-26 20:30:00', '1991-02-14', '22:00:00', '{"security_level": "high"}', 'security, cyber, hacking', '7, 77, 777', 805.250000, 15600.7500, 'Active', 'SuperAdmin', '2024-06-18 15:00:00', '2025-10-22 10:00:00'),
('edward_stark', 'edward.stark@email.com', 'Backend developer specializing in databases', '2025-10-15 12:00:00', '1993-09-05', '09:30:00', '{"db_preference": "postgresql"}', 'backend, database, sql', '11, 22, 33', 690.000000, 4200.0000, 'Inactive', 'User', '2024-07-22 08:00:00', '2025-09-30 09:00:00'),
('fiona_green', 'fiona.green@email.com', 'Frontend developer loving React', '2025-10-26 11:45:00', '1994-12-25', '10:30:00', '{"framework": "react"}', 'frontend, react, javascript', '6, 12, 18', 745.500000, 7890.2500, 'Active', 'Moderator', '2024-08-30 12:00:00', '2025-10-20 13:00:00'),
('george_martin', 'george.martin@email.com', 'Technical writer and documentation guru', '2025-10-23 09:30:00', '1987-04-12', '08:00:00', '{"writing_style": "technical"}', 'writing, docs, technical', '4, 8, 15', 720.000000, 5670.5000, 'Active', 'User', '2024-09-14 14:30:00', '2025-10-12 15:00:00'),
('hannah_baker', 'hannah.baker@email.com', 'QA engineer ensuring quality products', '2025-10-27 06:00:00', '1996-06-08', '07:30:00', '{"testing_approach": "automated"}', 'qa, testing, quality', '5, 10, 15', 765.250000, 9340.7500, 'Active', 'User', '2024-10-01 10:00:00', '2025-10-24 11:00:00');

-- ============================================================================
-- 2. INSERT INTO products (base table - no dependencies)
-- ============================================================================

INSERT INTO public.products (name, description, sku, barcode, price, sale_price, cost, specifications, tags, categories, images, dimensions_length, dimensions_width, dimensions_height, dimensions_unit, weight_value, weight_unit, discontinued_at, status, type, created_at, updated_at) VALUES
('Wireless Bluetooth Headphones', 'Premium noise-cancelling headphones with 30-hour battery life', 'AUDIO-WBH-001', '123456789012', 199.9900, 179.9900, 120.0000, '{"battery": "30hrs", "bluetooth": "5.0", "noise_cancelling": true}', 'audio, bluetooth, headphones', 'Electronics, Audio', '/images/headphones1.jpg', 20.0000, 18.0000, 8.0000, 'cm', 250.0000, 'g', NULL, 'Active', 'Physical', '2024-01-10 10:00:00', '2025-09-15 12:00:00'),
('Smart Watch Pro', '4K fitness tracker with heart rate monitor and GPS', 'WATCH-SWP-002', '234567890123', 299.9900, 249.9900, 180.0000, '{"display": "AMOLED", "water_resistant": "50m", "gps": true}', 'watch, fitness, smart', 'Electronics, Wearables', '/images/smartwatch1.jpg', 4.5000, 4.0000, 1.2000, 'cm', 45.0000, 'g', NULL, 'Active', 'Physical', '2024-02-05 11:00:00', '2025-10-01 10:00:00'),
('Mechanical Gaming Keyboard', 'RGB backlit mechanical keyboard with Cherry MX switches', 'KB-MGK-003', '345678901234', 149.9900, 129.9900, 85.0000, '{"switches": "Cherry MX Blue", "rgb": true, "programmable": true}', 'keyboard, gaming, mechanical', 'Electronics, Gaming', '/images/keyboard1.jpg', 44.0000, 13.5000, 3.5000, 'cm', 1200.0000, 'g', NULL, 'Active', 'Physical', '2024-03-12 09:30:00', '2025-09-20 14:00:00'),
('4K Ultra HD Monitor', '27-inch 4K monitor with HDR support and 144Hz refresh rate', 'MON-4K-004', '456789012345', 449.9900, 399.9900, 280.0000, '{"resolution": "3840x2160", "refresh_rate": "144Hz", "hdr": true}', 'monitor, 4k, display', 'Electronics, Monitors', '/images/monitor1.jpg', 61.0000, 18.0000, 41.0000, 'cm', 5500.0000, 'g', NULL, 'Active', 'Physical', '2024-04-20 13:00:00', '2025-10-10 09:00:00'),
('Ergonomic Office Chair', 'Adjustable lumbar support office chair with breathable mesh', 'FURN-EOC-005', '567890123456', 349.9900, NULL, 200.0000, '{"material": "mesh", "lumbar_support": true, "adjustable": true}', 'furniture, office, chair', 'Furniture, Office', '/images/chair1.jpg', 65.0000, 65.0000, 120.0000, 'cm', 18000.0000, 'g', NULL, 'Active', 'Physical', '2024-05-15 10:30:00', '2025-08-22 11:00:00'),
('Stainless Steel Water Bottle', '1L insulated water bottle keeps drinks cold for 24 hours', 'BOTTLE-SS-006', '678901234567', 29.9900, 24.9900, 12.0000, '{"capacity": "1L", "insulated": true, "material": "stainless_steel"}', 'bottle, water, insulated', 'Kitchen, Drinkware', '/images/bottle1.jpg', 28.0000, 8.0000, 8.0000, 'cm', 350.0000, 'g', NULL, 'Active', 'Physical', '2024-06-08 12:00:00', '2025-09-18 15:00:00'),
('Professional DSLR Camera', '24MP full-frame camera with 4K video recording', 'CAM-DSLR-007', '789012345678', 1899.9900, 1699.9900, 1200.0000, '{"megapixels": 24, "video": "4K60", "sensor": "full_frame"}', 'camera, dslr, photography', 'Electronics, Photography', '/images/camera1.jpg', 14.0000, 11.0000, 8.0000, 'cm', 800.0000, 'g', NULL, 'Active', 'Physical', '2024-07-14 14:30:00', '2025-10-05 10:00:00'),
('Laptop Backpack', 'Water-resistant backpack with padded laptop compartment up to 17 inch', 'BAG-LBP-008', '890123456789', 79.9900, 69.9900, 35.0000, '{"laptop_size": "17inch", "water_resistant": true, "usb_port": true}', 'backpack, laptop, bag', 'Accessories, Bags', '/images/backpack1.jpg', 48.0000, 32.0000, 18.0000, 'cm', 900.0000, 'g', NULL, 'Active', 'Physical', '2024-08-22 09:00:00', '2025-09-25 13:00:00'),
('Yoga Mat Premium', 'Non-slip yoga mat with alignment lines and carrying strap', 'FITNESS-YM-009', '901234567890', 49.9900, 39.9900, 20.0000, '{"thickness": "6mm", "material": "TPE", "non_slip": true}', 'yoga, fitness, mat', 'Sports, Fitness', '/images/yogamat1.jpg', 183.0000, 61.0000, 0.6000, 'cm', 1100.0000, 'g', NULL, 'Active', 'Physical', '2024-09-10 11:30:00', '2025-10-15 12:00:00'),
('Bluetooth Speaker Portable', 'Waterproof portable speaker with 360-degree sound', 'AUDIO-BSP-010', '012345678901', 89.9900, 79.9900, 45.0000, '{"waterproof": "IPX7", "battery": "12hrs", "bass_boost": true}', 'speaker, bluetooth, portable', 'Electronics, Audio', '/images/speaker1.jpg', 18.0000, 18.0000, 7.0000, 'cm', 550.0000, 'g', NULL, 'Active', 'Physical', '2024-10-18 10:00:00', '2025-10-20 14:00:00'),
('USB-C Hub 7-in-1', 'Multi-port USB-C hub with HDMI, USB 3.0, and SD card reader', 'ACC-USBC-011', '112233445566', 39.9900, 34.9900, 18.0000, '{"ports": 7, "hdmi": "4K", "usb_version": "3.0"}', 'usb, hub, adapter', 'Electronics, Accessories', '/images/usbhub1.jpg', 11.0000, 4.5000, 1.2000, 'cm', 85.0000, 'g', NULL, 'Active', 'Physical', '2024-01-25 13:00:00', '2025-09-30 11:00:00'),
('Electric Standing Desk', 'Height-adjustable standing desk with memory presets', 'FURN-ESD-012', '223344556677', 599.9900, 549.9900, 350.0000, '{"motor": "dual", "height_range": "71-121cm", "presets": 4}', 'desk, standing, electric', 'Furniture, Office', '/images/desk1.jpg', 160.0000, 80.0000, 75.0000, 'cm', 35000.0000, 'g', NULL, 'Active', 'Physical', '2024-02-28 15:00:00', '2025-10-12 09:00:00'),
('Wireless Mouse Ergonomic', 'Vertical ergonomic mouse with adjustable DPI', 'MOUSE-WME-013', '334455667788', 44.9900, 39.9900, 22.0000, '{"dpi": "800-3200", "buttons": 6, "battery": "rechargeable"}', 'mouse, wireless, ergonomic', 'Electronics, Accessories', '/images/mouse1.jpg', 12.0000, 7.5000, 7.0000, 'cm', 120.0000, 'g', NULL, 'Active', 'Physical', '2024-03-15 10:30:00', '2025-10-08 14:00:00'),
('LED Desk Lamp', 'Adjustable LED desk lamp with touch control and USB charging', 'LAMP-LED-014', '445566778899', 59.9900, 49.9900, 28.0000, '{"brightness_levels": 5, "color_temp": "adjustable", "usb_port": true}', 'lamp, led, desk', 'Electronics, Lighting', '/images/lamp1.jpg', 40.0000, 18.0000, 12.0000, 'cm', 850.0000, 'g', NULL, 'Active', 'Physical', '2024-04-10 12:00:00', '2025-09-28 10:00:00'),
('Phone Case Premium', 'Shockproof phone case with magnetic car mount', 'ACC-PCP-015', '556677889900', 24.9900, 19.9900, 8.0000, '{"protection": "military_grade", "magnetic": true, "material": "TPU"}', 'phone, case, accessory', 'Electronics, Accessories', '/images/phonecase1.jpg', 16.0000, 8.0000, 1.0000, 'cm', 45.0000, 'g', '2025-11-01 00:00:00', 'Discontinued', 'Physical', '2024-05-20 09:00:00', '2025-10-01 16:00:00');

-- ============================================================================
-- 3. INSERT INTO user_profiles (depends on users)
-- ============================================================================

INSERT INTO public.user_profiles (user_id, first_name, last_name, phone_number, home_address_street, home_address_city, home_address_state, home_address_postal_code, home_address_country, home_address_latitude, home_address_longitude, work_address_street, work_address_city, work_address_state, work_address_postal_code, work_address_country, work_address_latitude, work_address_longitude, preferences, skills, languages, anniversary, created_at, updated_at) 
SELECT 
  u.id,
  CASE u.username
    WHEN 'john_doe' THEN 'John'
    WHEN 'jane_smith' THEN 'Jane'
    WHEN 'bob_wilson' THEN 'Bob'
    WHEN 'alice_johnson' THEN 'Alice'
    WHEN 'charlie_brown' THEN 'Charlie'
    WHEN 'diana_prince' THEN 'Diana'
    WHEN 'edward_stark' THEN 'Edward'
    WHEN 'fiona_green' THEN 'Fiona'
    WHEN 'george_martin' THEN 'George'
    WHEN 'hannah_baker' THEN 'Hannah'
  END,
  CASE u.username
    WHEN 'john_doe' THEN 'Doe'
    WHEN 'jane_smith' THEN 'Smith'
    WHEN 'bob_wilson' THEN 'Wilson'
    WHEN 'alice_johnson' THEN 'Johnson'
    WHEN 'charlie_brown' THEN 'Brown'
    WHEN 'diana_prince' THEN 'Prince'
    WHEN 'edward_stark' THEN 'Stark'
    WHEN 'fiona_green' THEN 'Green'
    WHEN 'george_martin' THEN 'Martin'
    WHEN 'hannah_baker' THEN 'Baker'
  END,
  CASE u.username
    WHEN 'john_doe' THEN '+1-555-0101'
    WHEN 'jane_smith' THEN '+1-555-0102'
    WHEN 'bob_wilson' THEN '+1-555-0103'
    WHEN 'alice_johnson' THEN '+1-555-0104'
    WHEN 'charlie_brown' THEN '+1-555-0105'
    WHEN 'diana_prince' THEN '+1-555-0106'
    WHEN 'edward_stark' THEN '+1-555-0107'
    WHEN 'fiona_green' THEN '+1-555-0108'
    WHEN 'george_martin' THEN '+1-555-0109'
    WHEN 'hannah_baker' THEN '+1-555-0110'
  END,
  CASE u.username
    WHEN 'john_doe' THEN '123 Main St'
    WHEN 'jane_smith' THEN '456 Oak Avenue'
    WHEN 'bob_wilson' THEN '789 Pine Road'
    WHEN 'alice_johnson' THEN '321 Elm Street'
    WHEN 'charlie_brown' THEN '654 Maple Drive'
    WHEN 'diana_prince' THEN '987 Cedar Lane'
    WHEN 'edward_stark' THEN '147 Birch Way'
    WHEN 'fiona_green' THEN '258 Willow Court'
    WHEN 'george_martin' THEN '369 Spruce Path'
    WHEN 'hannah_baker' THEN '741 Ash Boulevard'
  END,
  CASE u.username
    WHEN 'john_doe' THEN 'New York'
    WHEN 'jane_smith' THEN 'Los Angeles'
    WHEN 'bob_wilson' THEN 'Chicago'
    WHEN 'alice_johnson' THEN 'Houston'
    WHEN 'charlie_brown' THEN 'Phoenix'
    WHEN 'diana_prince' THEN 'Philadelphia'
    WHEN 'edward_stark' THEN 'San Antonio'
    WHEN 'fiona_green' THEN 'San Diego'
    WHEN 'george_martin' THEN 'Dallas'
    WHEN 'hannah_baker' THEN 'Seattle'
  END,
  CASE u.username
    WHEN 'john_doe' THEN 'NY'
    WHEN 'jane_smith' THEN 'CA'
    WHEN 'bob_wilson' THEN 'IL'
    WHEN 'alice_johnson' THEN 'TX'
    WHEN 'charlie_brown' THEN 'AZ'
    WHEN 'diana_prince' THEN 'PA'
    WHEN 'edward_stark' THEN 'TX'
    WHEN 'fiona_green' THEN 'CA'
    WHEN 'george_martin' THEN 'TX'
    WHEN 'hannah_baker' THEN 'WA'
  END,
  CASE u.username
    WHEN 'john_doe' THEN '10001'
    WHEN 'jane_smith' THEN '90001'
    WHEN 'bob_wilson' THEN '60601'
    WHEN 'alice_johnson' THEN '77001'
    WHEN 'charlie_brown' THEN '85001'
    WHEN 'diana_prince' THEN '19101'
    WHEN 'edward_stark' THEN '78201'
    WHEN 'fiona_green' THEN '92101'
    WHEN 'george_martin' THEN '75201'
    WHEN 'hannah_baker' THEN '98101'
  END,
  'USA',
  CASE u.username
    WHEN 'john_doe' THEN 40.7128
    WHEN 'jane_smith' THEN 34.0522
    WHEN 'bob_wilson' THEN 41.8781
    WHEN 'alice_johnson' THEN 29.7604
    WHEN 'charlie_brown' THEN 33.4484
    WHEN 'diana_prince' THEN 39.9526
    WHEN 'edward_stark' THEN 29.4241
    WHEN 'fiona_green' THEN 32.7157
    WHEN 'george_martin' THEN 32.7767
    WHEN 'hannah_baker' THEN 47.6062
  END,
  CASE u.username
    WHEN 'john_doe' THEN -74.0060
    WHEN 'jane_smith' THEN -118.2437
    WHEN 'bob_wilson' THEN -87.6298
    WHEN 'alice_johnson' THEN -95.3698
    WHEN 'charlie_brown' THEN -112.0740
    WHEN 'diana_prince' THEN -75.1652
    WHEN 'edward_stark' THEN -98.4936
    WHEN 'fiona_green' THEN -117.1611
    WHEN 'george_martin' THEN -96.7970
    WHEN 'hannah_baker' THEN -122.3321
  END,
  CASE u.username
    WHEN 'john_doe' THEN '100 Tech Plaza'
    WHEN 'jane_smith' THEN '200 Marketing Hub'
    WHEN 'bob_wilson' THEN '300 Design Studio'
    WHEN 'alice_johnson' THEN '400 Data Center'
    WHEN 'charlie_brown' THEN '500 Project Tower'
    WHEN 'diana_prince' THEN '600 Security Building'
    WHEN 'edward_stark' THEN '700 Dev Center'
    WHEN 'fiona_green' THEN '800 Frontend HQ'
    WHEN 'george_martin' THEN '900 Documentation Office'
    WHEN 'hannah_baker' THEN '1000 QA Labs'
  END,
  CASE u.username
    WHEN 'john_doe' THEN 'Manhattan'
    WHEN 'jane_smith' THEN 'Santa Monica'
    WHEN 'bob_wilson' THEN 'Downtown Chicago'
    WHEN 'alice_johnson' THEN 'Downtown Houston'
    WHEN 'charlie_brown' THEN 'Scottsdale'
    WHEN 'diana_prince' THEN 'Center City'
    WHEN 'edward_stark' THEN 'Alamo Heights'
    WHEN 'fiona_green' THEN 'La Jolla'
    WHEN 'george_martin' THEN 'Uptown Dallas'
    WHEN 'hannah_baker' THEN 'Bellevue'
  END,
  CASE u.username
    WHEN 'john_doe' THEN 'NY'
    WHEN 'jane_smith' THEN 'CA'
    WHEN 'bob_wilson' THEN 'IL'
    WHEN 'alice_johnson' THEN 'TX'
    WHEN 'charlie_brown' THEN 'AZ'
    WHEN 'diana_prince' THEN 'PA'
    WHEN 'edward_stark' THEN 'TX'
    WHEN 'fiona_green' THEN 'CA'
    WHEN 'george_martin' THEN 'TX'
    WHEN 'hannah_baker' THEN 'WA'
  END,
  CASE u.username
    WHEN 'john_doe' THEN '10004'
    WHEN 'jane_smith' THEN '90401'
    WHEN 'bob_wilson' THEN '60602'
    WHEN 'alice_johnson' THEN '77002'
    WHEN 'charlie_brown' THEN '85251'
    WHEN 'diana_prince' THEN '19102'
    WHEN 'edward_stark' THEN '78209'
    WHEN 'fiona_green' THEN '92037'
    WHEN 'george_martin' THEN '75204'
    WHEN 'hannah_baker' THEN '98004'
  END,
  'USA',
  CASE u.username
    WHEN 'john_doe' THEN 40.7074
    WHEN 'jane_smith' THEN 34.0195
    WHEN 'bob_wilson' THEN 41.8819
    WHEN 'alice_johnson' THEN 29.7589
    WHEN 'charlie_brown' THEN 33.4942
    WHEN 'diana_prince' THEN 39.9496
    WHEN 'edward_stark' THEN 29.5074
    WHEN 'fiona_green' THEN 32.8329
    WHEN 'george_martin' THEN 32.8029
    WHEN 'hannah_baker' THEN 47.6101
  END,
  CASE u.username
    WHEN 'john_doe' THEN -74.0131
    WHEN 'jane_smith' THEN -118.4912
    WHEN 'bob_wilson' THEN -87.6278
    WHEN 'alice_johnson' THEN -95.3676
    WHEN 'charlie_brown' THEN -111.9261
    WHEN 'diana_prince' THEN -75.1638
    WHEN 'edward_stark' THEN -98.4855
    WHEN 'fiona_green' THEN -117.2713
    WHEN 'george_martin' THEN -96.7853
    WHEN 'hannah_baker' THEN -122.2015
  END,
  '{"newsletter": true, "email_notifications": true}',
  CASE u.username
    WHEN 'john_doe' THEN 'C#, Python, SQL'
    WHEN 'jane_smith' THEN 'Marketing, SEO, Content Creation'
    WHEN 'bob_wilson' THEN 'Photoshop, Illustrator, Figma'
    WHEN 'alice_johnson' THEN 'Python, R, Machine Learning'
    WHEN 'charlie_brown' THEN 'Agile, Scrum, Leadership'
    WHEN 'diana_prince' THEN 'Penetration Testing, Network Security'
    WHEN 'edward_stark' THEN 'PostgreSQL, MongoDB, Redis'
    WHEN 'fiona_green' THEN 'React, TypeScript, CSS'
    WHEN 'george_martin' THEN 'Technical Writing, Documentation'
    WHEN 'hannah_baker' THEN 'Selenium, Jest, Test Automation'
  END,
  CASE u.username
    WHEN 'john_doe' THEN 'English, Spanish'
    WHEN 'jane_smith' THEN 'English, French'
    WHEN 'bob_wilson' THEN 'English'
    WHEN 'alice_johnson' THEN 'English, Mandarin'
    WHEN 'charlie_brown' THEN 'English, German'
    WHEN 'diana_prince' THEN 'English, Russian'
    WHEN 'edward_stark' THEN 'English'
    WHEN 'fiona_green' THEN 'English, Italian'
    WHEN 'george_martin' THEN 'English, Portuguese'
    WHEN 'hannah_baker' THEN 'English, Japanese'
  END,
  CASE u.username
    WHEN 'john_doe' THEN '2015-06-20'::date
    WHEN 'jane_smith' THEN '2016-09-14'::date
    WHEN 'bob_wilson' THEN '2018-03-22'::date
    WHEN 'alice_johnson' THEN '2019-11-08'::date
    WHEN 'charlie_brown' THEN '2010-07-30'::date
    WHEN 'diana_prince' THEN '2017-02-14'::date
    WHEN 'edward_stark' THEN '2020-12-25'::date
    WHEN 'fiona_green' THEN '2021-05-17'::date
    WHEN 'george_martin' THEN '2014-10-31'::date
    WHEN 'hannah_baker' THEN '2022-04-01'::date
  END,
  u.created_at + interval '1 hour',
  u.updated_at
FROM public.users u;

-- ============================================================================
-- 4. INSERT INTO user_sessions (depends on users)
-- ============================================================================

INSERT INTO public.user_sessions (user_id, session_token, ip_address, user_agent, expires_at, last_activity_at, session_data, permissions, status, type, created_at, updated_at)
SELECT 
  u.id,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 1 THEN 'sess_tok_a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 2 THEN 'sess_tok_b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 3 THEN 'sess_tok_c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 4 THEN 'sess_tok_d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 5 THEN 'sess_tok_e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 6 THEN 'sess_tok_f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0u1'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 7 THEN 'sess_tok_g7h8i9j0k1l2m3n4o5p6q7r8s9t0u1v2'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 8 THEN 'sess_tok_h8i9j0k1l2m3n4o5p6q7r8s9t0u1v2w3'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 9 THEN 'sess_tok_i9j0k1l2m3n4o5p6q7r8s9t0u1v2w3x4'
    ELSE 'sess_tok_j0k1l2m3n4o5p6q7r8s9t0u1v2w3x4y5'
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) % 3 = 0 THEN '192.168.1.' || (10 + ROW_NUMBER() OVER (ORDER BY u.username))
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) % 3 = 1 THEN '10.0.0.' || (20 + ROW_NUMBER() OVER (ORDER BY u.username))
    ELSE '172.16.0.' || (30 + ROW_NUMBER() OVER (ORDER BY u.username))
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) % 4 = 0 THEN 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) % 4 = 1 THEN 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) % 4 = 2 THEN 'Mozilla/5.0 (iPhone; CPU iPhone OS 14_6 like Mac OS X)'
    ELSE 'Mozilla/5.0 (Linux; Android 11; SM-G991B) AppleWebKit/537.36'
  END,
  NOW() + interval '7 days',
  NOW() - interval '2 hours',
  '{"preferences": {"theme": "dark"}, "cart_items": []}',
  CASE 
    WHEN u.role = 'SuperAdmin' THEN 'read, write, delete, admin'
    WHEN u.role = 'Admin' THEN 'read, write, delete'
    WHEN u.role = 'Moderator' THEN 'read, write'
    ELSE 'read'
  END,
  CASE 
    WHEN u.status = 'Active' THEN 'Active'
    ELSE 'Expired'
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) % 2 = 0 THEN 'Web'
    ELSE 'Mobile'
  END,
  u.last_login_at - interval '5 hours',
  NOW() - interval '1 hour'
FROM public.users u;

-- ============================================================================
-- 5. INSERT INTO orders (depends on users)
-- ============================================================================

INSERT INTO public.orders (user_id, order_number, shipped_at, delivered_at, sub_total, tax_amount, shipping_cost, total, order_data, tags, status, payment_method, shipping_address_street, shipping_address_city, shipping_address_state, shipping_address_postal_code, shipping_address_country, shipping_address_latitude, shipping_address_longitude, billing_address_street, billing_address_city, billing_address_state, billing_address_postal_code, billing_address_country, billing_address_latitude, billing_address_longitude, created_at, updated_at)
SELECT 
  u.id,
  'ORD-2024-' || LPAD((ROW_NUMBER() OVER (ORDER BY u.username))::text, 6, '0'),
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) <= 5 THEN u.created_at + interval '2 days'
    ELSE NULL
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) <= 3 THEN u.created_at + interval '7 days'
    ELSE NULL
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 1 THEN 379.98
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 2 THEN 849.97
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 3 THEN 279.98
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 4 THEN 1049.99
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 5 THEN 599.98
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 6 THEN 459.96
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 7 THEN 1899.99
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 8 THEN 169.98
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 9 THEN 129.98
    ELSE 649.96
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 1 THEN 30.40
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 2 THEN 68.00
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 3 THEN 22.40
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 4 THEN 84.00
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 5 THEN 48.00
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 6 THEN 36.80
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 7 THEN 152.00
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 8 THEN 13.60
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 9 THEN 10.40
    ELSE 52.00
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) <= 3 THEN 0.00
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) <= 7 THEN 9.99
    ELSE 14.99
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 1 THEN 410.38
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 2 THEN 917.97
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 3 THEN 302.38
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 4 THEN 1143.98
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 5 THEN 657.97
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 6 THEN 506.75
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 7 THEN 2061.98
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 8 THEN 198.57
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) = 9 THEN 155.37
    ELSE 716.95
  END,
  '{"order_notes": "Please deliver to front door", "gift_wrap": false}',
  'electronics, online',
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) <= 3 THEN 'Delivered'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) <= 5 THEN 'Shipped'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) <= 7 THEN 'Processing'
    ELSE 'Pending'
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) % 3 = 0 THEN 'CreditCard'
    WHEN ROW_NUMBER() OVER (ORDER BY u.username) % 3 = 1 THEN 'PayPal'
    ELSE 'DebitCard'
  END,
  up.home_address_street,
  up.home_address_city,
  up.home_address_state,
  up.home_address_postal_code,
  up.home_address_country,
  up.home_address_latitude,
  up.home_address_longitude,
  up.home_address_street,
  up.home_address_city,
  up.home_address_state,
  up.home_address_postal_code,
  up.home_address_country,
  up.home_address_latitude,
  up.home_address_longitude,
  u.created_at + interval '15 days',
  u.updated_at
FROM public.users u
LEFT JOIN public.user_profiles up ON u.id = up.user_id;

-- ============================================================================
-- 6. INSERT INTO product_inventories (depends on products)
-- ============================================================================

INSERT INTO public.product_inventories (product_id, warehouse_code, location, quantity, reserved_quantity, available_quantity, unit_cost, unit_price, last_updated, last_restocked, expiry_date, inventory_data, batch_numbers, serial_numbers, status, type, created_at, updated_at)
SELECT 
  p.id,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) % 3 = 0 THEN 'WH-EAST-01'
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) % 3 = 1 THEN 'WH-WEST-02'
    ELSE 'WH-CENTRAL-03'
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) % 3 = 0 THEN 'Aisle 1, Rack A, Shelf 5'
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) % 3 = 1 THEN 'Aisle 2, Rack B, Shelf 3'
    ELSE 'Aisle 3, Rack C, Shelf 7'
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 1 THEN 150
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 2 THEN 85
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 3 THEN 220
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 4 THEN 45
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 5 THEN 110
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 6 THEN 500
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 7 THEN 25
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 8 THEN 180
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 9 THEN 300
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 10 THEN 95
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 11 THEN 400
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 12 THEN 15
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 13 THEN 130
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 14 THEN 75
    ELSE 10
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 1 THEN 30
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 2 THEN 15
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 3 THEN 40
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 4 THEN 5
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 5 THEN 20
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 6 THEN 50
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 7 THEN 5
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 8 THEN 25
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 9 THEN 30
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 10 THEN 15
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 11 THEN 45
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 12 THEN 3
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 13 THEN 20
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 14 THEN 10
    ELSE 2
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 1 THEN 120
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 2 THEN 70
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 3 THEN 180
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 4 THEN 40
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 5 THEN 90
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 6 THEN 450
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 7 THEN 20
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 8 THEN 155
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 9 THEN 270
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 10 THEN 80
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 11 THEN 355
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 12 THEN 12
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 13 THEN 110
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 14 THEN 65
    ELSE 8
  END,
  p.cost,
  p.price,
  NOW() - interval '1 day',
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) % 2 = 0 THEN NOW() - interval '10 days'
    ELSE NOW() - interval '25 days'
  END,
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 6 THEN NOW() + interval '365 days'
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 9 THEN NOW() + interval '730 days'
    ELSE NULL
  END,
  '{"supplier": "Global Tech Supplies", "purchase_order": "PO-2024-' || LPAD(ROW_NUMBER() OVER (ORDER BY p.name)::text, 4, '0') || '"}',
  'BATCH-' || TO_CHAR(NOW(), 'YYYY') || '-' || LPAD(ROW_NUMBER() OVER (ORDER BY p.name)::text, 4, '0'),
  CASE 
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) <= 7 THEN 'SN-' || p.sku || '-' || LPAD(ROW_NUMBER() OVER (ORDER BY p.name)::text, 6, '0')
    ELSE NULL
  END,
  CASE 
    WHEN p.status = 'Discontinued' THEN 'OutOfStock'
    WHEN ROW_NUMBER() OVER (ORDER BY p.name) = 15 THEN 'LowStock'
    ELSE 'InStock'
  END,
  'Standard',
  p.created_at + interval '2 days',
  NOW() - interval '12 hours'
FROM public.products p;

-- ============================================================================
-- 7. INSERT INTO order_items (depends on orders and products)
-- ============================================================================

-- Order 1 (john_doe): 2 items
INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, modified_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  2,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * 2,
  '{"sku": "' || p.sku || '"}',
  '{"color": "black", "warranty": "1 year"}',
  p.categories,
  o.created_at,
  NULL,
  o.created_at,
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
JOIN public.products p ON p.sku = 'AUDIO-WBH-001'
WHERE u.username = 'john_doe';

INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, modified_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  1,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * 1,
  '{"sku": "' || p.sku || '"}',
  '{"size": "one size", "warranty": "2 years"}',
  p.categories,
  o.created_at + interval '5 minutes',
  NULL,
  o.created_at + interval '5 minutes',
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
JOIN public.products p ON p.sku = 'BOTTLE-SS-006'
WHERE u.username = 'john_doe';

-- Order 2 (jane_smith): 3 items
INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, modified_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  1,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * 1,
  '{"sku": "' || p.sku || '"}',
  '{"type": "electric", "color": "black"}',
  p.categories,
  o.created_at,
  NULL,
  o.created_at,
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
JOIN public.products p ON p.sku = 'FURN-ESD-012'
WHERE u.username = 'jane_smith';

INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, modified_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  1,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * 1,
  '{"sku": "' || p.sku || '"}',
  '{"color": "silver", "connectivity": "wireless"}',
  p.categories,
  o.created_at + interval '3 minutes',
  NULL,
  o.created_at + interval '3 minutes',
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
JOIN public.products p ON p.sku = 'WATCH-SWP-002'
WHERE u.username = 'jane_smith';

INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, modified_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  1,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * 1,
  '{"sku": "' || p.sku || '"}',
  '{"ports": "7-in-1", "compatibility": "universal"}',
  p.categories,
  o.created_at + interval '8 minutes',
  NULL,
  o.created_at + interval '8 minutes',
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
JOIN public.products p ON p.sku = 'ACC-USBC-011'
WHERE u.username = 'jane_smith';

-- Order 3 (bob_wilson): 2 items
INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, modified_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  1,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * 1,
  '{"sku": "' || p.sku || '"}',
  '{"switch_type": "Cherry MX Blue", "rgb": "enabled"}',
  p.categories,
  o.created_at,
  NULL,
  o.created_at,
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
JOIN public.products p ON p.sku = 'KB-MGK-003'
WHERE u.username = 'bob_wilson';

INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, modified_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  1,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * 1,
  '{"sku": "' || p.sku || '"}',
  '{"size": "large", "color": "black"}',
  p.categories,
  o.created_at + interval '2 minutes',
  NULL,
  o.created_at + interval '2 minutes',
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
JOIN public.products p ON p.sku = 'BAG-LBP-008'
WHERE u.username = 'bob_wilson';

-- Order 4 (alice_johnson): 2 items
INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, modified_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  1,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * 1,
  '{"sku": "' || p.sku || '"}',
  '{"resolution": "4K", "refresh_rate": "144Hz"}',
  p.categories,
  o.created_at,
  NULL,
  o.created_at,
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
JOIN public.products p ON p.sku = 'MON-4K-004'
WHERE u.username = 'alice_johnson';

INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, modified_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  1,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * 1,
  '{"sku": "' || p.sku || '"}',
  '{"color": "black", "material": "mesh"}',
  p.categories,
  o.created_at + interval '10 minutes',
  NULL,
  o.created_at + interval '10 minutes',
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
JOIN public.products p ON p.sku = 'FURN-EOC-005'
WHERE u.username = 'alice_johnson';

-- Order 5 (charlie_brown): 2 items
INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, modified_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  1,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * 1,
  '{"sku": "' || p.sku || '"}',
  '{"material": "TPE", "thickness": "6mm"}',
  p.categories,
  o.created_at,
  NULL,
  o.created_at,
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
JOIN public.products p ON p.sku = 'FITNESS-YM-009'
WHERE u.username = 'charlie_brown';

INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, modified_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  1,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * 1,
  '{"sku": "' || p.sku || '"}',
  '{"ergonomic": "vertical", "dpi": "adjustable"}',
  p.categories,
  o.created_at + interval '4 minutes',
  NULL,
  o.created_at + interval '4 minutes',
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
JOIN public.products p ON p.sku = 'MOUSE-WME-013'
WHERE u.username = 'charlie_brown';

-- Additional order items for remaining orders (simplified for brevity)
INSERT INTO public.order_items (order_id, product_id, product_name, description, quantity, unit_price, total_price, product_data, attributes, categories, added_at, created_at, updated_at)
SELECT 
  o.id,
  p.id,
  p.name,
  p.description,
  CASE WHEN RANDOM() < 0.5 THEN 1 ELSE 2 END,
  COALESCE(p.sale_price, p.price),
  COALESCE(p.sale_price, p.price) * CASE WHEN RANDOM() < 0.5 THEN 1 ELSE 2 END,
  '{"sku": "' || p.sku || '"}',
  '{"auto_generated": true}',
  p.categories,
  o.created_at,
  o.created_at,
  o.updated_at
FROM public.orders o
JOIN public.users u ON o.user_id = u.id
CROSS JOIN LATERAL (
  SELECT * FROM public.products 
  WHERE status = 'Active' 
  ORDER BY RANDOM() 
  LIMIT 2
) p
WHERE u.username IN ('diana_prince', 'edward_stark', 'fiona_green', 'george_martin', 'hannah_baker');

-- ============================================================================
-- 8. INSERT INTO product_reviews (depends on products and users)
-- ============================================================================

INSERT INTO public.product_reviews (product_id, user_id, title, comment, rating, review_data, tags, status, type, created_at, updated_at)
WITH ranked_reviews AS (
  SELECT 
    p.id AS product_id,
    u.id AS user_id,
    p.name AS product_name,
    u.username,
    p.created_at AS product_created_at,
    ROW_NUMBER() OVER (PARTITION BY p.id ORDER BY u.username) AS rn,
    ROW_NUMBER() OVER (ORDER BY p.name, u.username) AS global_rn
  FROM public.products p
  CROSS JOIN public.users u
  WHERE p.status = 'Active' 
    AND u.status = 'Active'
)
SELECT 
  rr.product_id,
  rr.user_id,
  CASE 
    WHEN rr.global_rn % 5 = 0 THEN 'Excellent product!'
    WHEN rr.global_rn % 5 = 1 THEN 'Great value for money'
    WHEN rr.global_rn % 5 = 2 THEN 'Good quality'
    WHEN rr.global_rn % 5 = 3 THEN 'Decent purchase'
    ELSE 'Very satisfied'
  END AS title,
  CASE 
    WHEN rr.global_rn % 5 = 0 THEN 'Absolutely love this product! It exceeded my expectations in every way. Highly recommended.'
    WHEN rr.global_rn % 5 = 1 THEN 'Really impressed with the quality and performance. Worth every penny.'
    WHEN rr.global_rn % 5 = 2 THEN 'Good product overall. Does what it''s supposed to do. No complaints.'
    WHEN rr.global_rn % 5 = 3 THEN 'It''s okay. Could be better in some aspects but generally satisfied.'
    ELSE 'Very happy with my purchase. Works great and looks amazing.'
  END AS comment,
  CASE 
    WHEN rr.global_rn % 5 = 0 THEN 5
    WHEN rr.global_rn % 5 = 1 THEN 5
    WHEN rr.global_rn % 5 = 2 THEN 4
    WHEN rr.global_rn % 5 = 3 THEN 3
    ELSE 5
  END AS rating,
  '{"verified_purchase": true, "helpful_count": ' || (RANDOM() * 50)::int || '}' AS review_data,
  'verified, helpful' AS tags,
  CASE 
    WHEN RANDOM() < 0.9 THEN 'Approved'
    ELSE 'Pending'
  END AS status,
  'Customer' AS type,
  rr.product_created_at + interval '30 days' + (RANDOM() * interval '60 days') AS created_at,
  NULL AS updated_at
FROM ranked_reviews rr
WHERE rr.rn <= 3
ORDER BY rr.product_name, rr.username
LIMIT 30;

-- ============================================================================
-- 9. INSERT INTO order_status_histories (depends on orders)
-- ============================================================================

-- Create status history for all orders
INSERT INTO public.order_status_histories (order_id, changed_by, old_status, new_status, changed_at, reason, notes, change_data, tags, created_at, updated_at)
SELECT 
  o.id,
  'system',
  'Draft',
  'Pending',
  o.created_at,
  'Order placed by customer',
  'Initial order creation',
  '{"automated": true}',
  'automated, initial',
  o.created_at,
  NULL
FROM public.orders o;

-- Add processing status for orders that were shipped
INSERT INTO public.order_status_histories (order_id, changed_by, old_status, new_status, changed_at, reason, notes, change_data, tags, created_at, updated_at)
SELECT 
  o.id,
  'warehouse_team',
  'Pending',
  'Processing',
  o.created_at + interval '1 day',
  'Order picked for processing',
  'Items verified and packaged',
  '{"warehouse": "WH-CENTRAL-03"}',
  'warehouse, processing',
  o.created_at + interval '1 day',
  NULL
FROM public.orders o
WHERE o.status IN ('Processing', 'Shipped', 'Delivered');

-- Add shipped status
INSERT INTO public.order_status_histories (order_id, changed_by, old_status, new_status, changed_at, reason, notes, change_data, tags, created_at, updated_at)
SELECT 
  o.id,
  'shipping_team',
  'Processing',
  'Shipped',
  o.shipped_at,
  'Order dispatched',
  'Shipped via FedEx, tracking number: FDX' || LPAD((RANDOM() * 1000000)::int::text, 12, '0'),
  '{"carrier": "FedEx", "service": "2-day"}',
  'shipped, fedex',
  o.shipped_at,
  NULL
FROM public.orders o
WHERE o.status IN ('Shipped', 'Delivered') AND o.shipped_at IS NOT NULL;

-- Add delivered status
INSERT INTO public.order_status_histories (order_id, changed_by, old_status, new_status, changed_at, reason, notes, change_data, tags, created_at, updated_at)
SELECT 
  o.id,
  'delivery_system',
  'Shipped',
  'Delivered',
  o.delivered_at,
  'Order delivered successfully',
  'Signed by customer',
  '{"signature": "electronic", "delivered_to": "front_door"}',
  'delivered, signed',
  o.delivered_at,
  NULL
FROM public.orders o
WHERE o.status = 'Delivered' AND o.delivered_at IS NOT NULL;

-- ============================================================================
-- Summary
-- ============================================================================

-- To verify the data insertion, you can run these queries:
-- SELECT COUNT(*) FROM public.users;              -- Should return 10
-- SELECT COUNT(*) FROM public.products;           -- Should return 15
-- SELECT COUNT(*) FROM public.user_profiles;      -- Should return 10
-- SELECT COUNT(*) FROM public.user_sessions;      -- Should return 10
-- SELECT COUNT(*) FROM public.orders;             -- Should return 10
-- SELECT COUNT(*) FROM public.product_inventories; -- Should return 15
-- SELECT COUNT(*) FROM public.order_items;        -- Should return ~25-30
-- SELECT COUNT(*) FROM public.product_reviews;    -- Should return ~30
-- SELECT COUNT(*) FROM public.order_status_histories; -- Should return ~30-35

-- End of test data insertion script

