-- =============================================
-- Views for Complex Data Queries
-- =============================================

-- View: User Profile Summary
IF OBJECT_ID('vw_UserProfileSummary', 'V') IS NOT NULL
    DROP VIEW vw_UserProfileSummary;
GO

CREATE VIEW vw_UserProfileSummary
AS
SELECT 
    u.Id AS UserId,
    u.Username,
    u.Email,
    u.Status,
    u.Role,
    u.Balance,
    u.CreditScore,
    u.CreatedAt,
    u.LastLoginAt,
    up.FirstName,
    up.LastName,
    up.PhoneNumber,
    up.HomeAddress_City AS City,
    up.HomeAddress_State AS State,
    up.HomeAddress_Country AS Country,
    (SELECT CAST(COUNT(*) AS INT) FROM Orders WHERE UserId = u.Id) AS TotalOrders,
    (SELECT SUM(Total) FROM Orders WHERE UserId = u.Id) AS TotalSpent
FROM Users u
LEFT JOIN UserProfiles up ON u.Id = up.UserId;
GO

-- View: Product Inventory Status
IF OBJECT_ID('vw_ProductInventoryStatus', 'V') IS NOT NULL
    DROP VIEW vw_ProductInventoryStatus;
GO

CREATE VIEW vw_ProductInventoryStatus
AS
SELECT 
    p.Id AS ProductId,
    p.Name AS ProductName,
    p.SKU,
    p.Price,
    p.SalePrice,
    p.Status AS ProductStatus,
    p.Type AS ProductType,
    pi.WarehouseCode,
    pi.Quantity,
    pi.AvailableQuantity,
    pi.ReservedQuantity,
    pi.Status AS InventoryStatus,
    pi.LastUpdated,
    CASE 
        WHEN pi.AvailableQuantity > 100 THEN 'High Stock'
        WHEN pi.AvailableQuantity BETWEEN 20 AND 100 THEN 'Medium Stock'
        WHEN pi.AvailableQuantity BETWEEN 1 AND 19 THEN 'Low Stock'
        ELSE 'Out of Stock'
    END AS StockLevel
FROM Products p
LEFT JOIN ProductInventories pi ON p.Id = pi.ProductId;
GO

-- View: Order Details Summary
IF OBJECT_ID('vw_OrderDetailsSummary', 'V') IS NOT NULL
    DROP VIEW vw_OrderDetailsSummary;
GO

CREATE VIEW vw_OrderDetailsSummary
AS
SELECT 
    o.Id AS OrderId,
    o.OrderNumber,
    o.UserId,
    u.Username,
    u.Email,
    o.CreatedAt AS OrderDate,
    o.Status AS OrderStatus,
    o.PaymentMethod,
    o.SubTotal,
    o.TaxAmount,
    o.ShippingCost,
    o.Total,
    o.ShippingAddress_City AS ShipToCity,
    o.ShippingAddress_State AS ShipToState,
    o.ShippingAddress_Country AS ShipToCountry,
    (SELECT COUNT(*) FROM OrderItems WHERE OrderId = o.Id) AS TotalItems,
    (SELECT SUM(Quantity) FROM OrderItems WHERE OrderId = o.Id) AS TotalQuantity
FROM Orders o
INNER JOIN Users u ON o.UserId = u.Id;
GO

-- View: Product Reviews Summary
IF OBJECT_ID('vw_ProductReviewsSummary', 'V') IS NOT NULL
    DROP VIEW vw_ProductReviewsSummary;
GO

CREATE VIEW vw_ProductReviewsSummary
AS
SELECT 
    p.Id AS ProductId,
    p.Name AS ProductName,
    p.SKU,
    COUNT(pr.Id) AS TotalReviews,
    AVG(CAST(pr.Rating AS FLOAT)) AS AverageRating,
    SUM(CASE WHEN pr.Rating = 5 THEN 1 ELSE 0 END) AS FiveStarCount,
    SUM(CASE WHEN pr.Rating = 4 THEN 1 ELSE 0 END) AS FourStarCount,
    SUM(CASE WHEN pr.Rating = 3 THEN 1 ELSE 0 END) AS ThreeStarCount,
    SUM(CASE WHEN pr.Rating = 2 THEN 1 ELSE 0 END) AS TwoStarCount,
    SUM(CASE WHEN pr.Rating = 1 THEN 1 ELSE 0 END) AS OneStarCount
FROM Products p
LEFT JOIN ProductReviews pr ON p.Id = pr.ProductId
GROUP BY p.Id, p.Name, p.SKU;
GO

-- View: Active User Sessions
IF OBJECT_ID('vw_ActiveUserSessions', 'V') IS NOT NULL
    DROP VIEW vw_ActiveUserSessions;
GO

CREATE VIEW vw_ActiveUserSessions
AS
SELECT 
    us.Id AS SessionId,
    us.UserId,
    u.Username,
    u.Email,
    us.SessionToken,
    us.IpAddress,
    us.Type AS SessionType,
    us.Status AS SessionStatus,
    us.CreatedAt,
    us.LastActivityAt,
    us.ExpiresAt,
    DATEDIFF(MINUTE, us.LastActivityAt, GETDATE()) AS MinutesSinceLastActivity
FROM UserSessions us
INNER JOIN Users u ON us.UserId = u.Id
WHERE us.Status = 'Active' AND us.ExpiresAt > GETDATE();
GO

PRINT 'Views created successfully!';

