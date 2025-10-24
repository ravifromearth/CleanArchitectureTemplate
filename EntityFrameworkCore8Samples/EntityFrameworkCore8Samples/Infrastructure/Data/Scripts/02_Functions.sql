-- =============================================
-- Scalar and Table-Valued Functions
-- =============================================

-- Scalar Function: Calculate User Lifetime Value
IF OBJECT_ID('fn_CalculateUserLifetimeValue', 'FN') IS NOT NULL
    DROP FUNCTION fn_CalculateUserLifetimeValue;
GO

CREATE FUNCTION fn_CalculateUserLifetimeValue
(
    @UserId UNIQUEIDENTIFIER
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @LifetimeValue DECIMAL(18,2);
    
    SELECT @LifetimeValue = ISNULL(SUM(Total), 0)
    FROM Orders
    WHERE UserId = @UserId AND Status IN ('Delivered', 'Processing', 'Shipped');
    
    RETURN @LifetimeValue;
END;
GO

-- Scalar Function: Get Product Average Rating
IF OBJECT_ID('fn_GetProductAverageRating', 'FN') IS NOT NULL
    DROP FUNCTION fn_GetProductAverageRating;
GO

CREATE FUNCTION fn_GetProductAverageRating
(
    @ProductId UNIQUEIDENTIFIER
)
RETURNS DECIMAL(3,2)
AS
BEGIN
    DECLARE @AverageRating DECIMAL(3,2);
    
    SELECT @AverageRating = ISNULL(AVG(CAST(Rating AS DECIMAL(3,2))), 0)
    FROM ProductReviews
    WHERE ProductId = @ProductId AND Status = 'Approved';
    
    RETURN @AverageRating;
END;
GO

-- Scalar Function: Calculate Order Discount
IF OBJECT_ID('fn_CalculateOrderDiscount', 'FN') IS NOT NULL
    DROP FUNCTION fn_CalculateOrderDiscount;
GO

CREATE FUNCTION fn_CalculateOrderDiscount
(
    @SubTotal DECIMAL(18,2),
    @UserRole NVARCHAR(MAX)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Discount DECIMAL(18,2) = 0;
    
    -- Apply role-based discounts
    IF @UserRole = 'SuperAdmin'
        SET @Discount = @SubTotal * 0.20; -- 20% discount
    ELSE IF @UserRole = 'Admin'
        SET @Discount = @SubTotal * 0.15; -- 15% discount
    ELSE IF @UserRole = 'Moderator'
        SET @Discount = @SubTotal * 0.10; -- 10% discount
    ELSE IF @SubTotal > 1000
        SET @Discount = @SubTotal * 0.05; -- 5% for orders over $1000
    
    RETURN @Discount;
END;
GO

-- Table-Valued Function: Get User Order History
IF OBJECT_ID('fn_GetUserOrderHistory', 'TF') IS NOT NULL
    DROP FUNCTION fn_GetUserOrderHistory;
GO

CREATE FUNCTION fn_GetUserOrderHistory
(
    @UserId UNIQUEIDENTIFIER,
    @FromDate DATETIME = NULL,
    @ToDate DATETIME = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        o.Id,
        o.OrderNumber,
        o.CreatedAt,
        o.Status,
        o.PaymentMethod,
        o.Total,
        COUNT(oi.Id) AS ItemCount,
        SUM(oi.Quantity) AS TotalQuantity
    FROM Orders o
    LEFT JOIN OrderItems oi ON o.Id = oi.OrderId
    WHERE o.UserId = @UserId
        AND (@FromDate IS NULL OR o.CreatedAt >= @FromDate)
        AND (@ToDate IS NULL OR o.CreatedAt <= @ToDate)
    GROUP BY o.Id, o.OrderNumber, o.CreatedAt, o.Status, o.PaymentMethod, o.Total
);
GO

-- Table-Valued Function: Get Top Selling Products
IF OBJECT_ID('fn_GetTopSellingProducts', 'TF') IS NOT NULL
    DROP FUNCTION fn_GetTopSellingProducts;
GO

CREATE FUNCTION fn_GetTopSellingProducts
(
    @TopN INT = 10,
    @FromDate DATETIME = NULL,
    @ToDate DATETIME = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT TOP (@TopN)
        p.Id AS ProductId,
        p.Name AS ProductName,
        p.SKU,
        p.Price,
        COUNT(oi.Id) AS TimesSold,
        SUM(oi.Quantity) AS TotalQuantitySold,
        SUM(oi.TotalPrice) AS TotalRevenue,
        AVG(oi.UnitPrice) AS AverageSellingPrice
    FROM Products p
    INNER JOIN OrderItems oi ON p.Id = oi.ProductId
    INNER JOIN Orders o ON oi.OrderId = o.Id
    WHERE (@FromDate IS NULL OR o.CreatedAt >= @FromDate)
        AND (@ToDate IS NULL OR o.CreatedAt <= @ToDate)
        AND o.Status IN ('Processing', 'Shipped', 'Delivered')
    GROUP BY p.Id, p.Name, p.SKU, p.Price
    ORDER BY TotalQuantitySold DESC
);
GO

-- Table-Valued Function: Get Products Low on Stock
IF OBJECT_ID('fn_GetProductsLowOnStock', 'TF') IS NOT NULL
    DROP FUNCTION fn_GetProductsLowOnStock;
GO

CREATE FUNCTION fn_GetProductsLowOnStock
(
    @Threshold INT = 20
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        p.Id AS ProductId,
        p.Name AS ProductName,
        p.SKU,
        p.Status AS ProductStatus,
        pi.WarehouseCode,
        pi.Quantity,
        pi.AvailableQuantity,
        pi.ReservedQuantity,
        pi.LastRestocked
    FROM Products p
    INNER JOIN ProductInventories pi ON p.Id = pi.ProductId
    WHERE pi.AvailableQuantity <= @Threshold
        AND p.Status = 'Active'
        AND pi.Status = 'InStock'
);
GO

-- Table-Valued Function: Get User Activity Summary
IF OBJECT_ID('fn_GetUserActivitySummary', 'TF') IS NOT NULL
    DROP FUNCTION fn_GetUserActivitySummary;
GO

CREATE FUNCTION fn_GetUserActivitySummary
(
    @UserId UNIQUEIDENTIFIER
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        @UserId AS UserId,
        (SELECT COUNT(*) FROM Orders WHERE UserId = @UserId) AS TotalOrders,
        (SELECT SUM(Total) FROM Orders WHERE UserId = @UserId AND Status = 'Delivered') AS TotalSpent,
        (SELECT COUNT(*) FROM ProductReviews WHERE UserId = @UserId) AS TotalReviews,
        (SELECT COUNT(*) FROM UserSessions WHERE UserId = @UserId AND Status = 'Active') AS ActiveSessions,
        (SELECT MAX(LastActivityAt) FROM UserSessions WHERE UserId = @UserId) AS LastActivity,
        (SELECT AVG(CAST(Rating AS FLOAT)) FROM ProductReviews WHERE UserId = @UserId) AS AverageRating
);
GO

PRINT 'Functions created successfully!';





