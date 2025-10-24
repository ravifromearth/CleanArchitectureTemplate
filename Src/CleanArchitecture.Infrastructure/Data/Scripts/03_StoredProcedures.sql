-- =============================================
-- Simple Stored Procedures for Testing
-- =============================================

-- Stored Procedure: Get Sales Report
IF OBJECT_ID('sp_GetSalesReport', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetSalesReport;
GO

CREATE PROCEDURE sp_GetSalesReport
    @FromDate DATETIME = NULL,
    @ToDate DATETIME = NULL,
    @Status NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Set default date range if not provided
    IF @FromDate IS NULL
        SET @FromDate = DATEADD(DAY, -30, GETDATE());
    IF @ToDate IS NULL
        SET @ToDate = GETDATE();
    
    SELECT 
        CAST(o.CreatedAt AS DATE) AS OrderDate,
        COUNT(o.Id) AS TotalOrders,
        SUM(o.Total) AS TotalRevenue,
        AVG(o.Total) AS AverageOrderValue,
        SUM(o.ShippingCost) AS TotalShipping,
        SUM(o.TaxAmount) AS TotalTax,
        COUNT(DISTINCT o.UserId) AS UniqueCustomers,
        SUM(oi.Quantity) AS TotalItemsSold
    FROM Orders o
    LEFT JOIN OrderItems oi ON o.Id = oi.OrderId
    WHERE o.CreatedAt >= @FromDate 
        AND o.CreatedAt <= @ToDate
        AND (@Status IS NULL OR o.Status = @Status)
    GROUP BY CAST(o.CreatedAt AS DATE)
    ORDER BY OrderDate DESC;
END;
GO

-- Stored Procedure: Create Order with Items
IF OBJECT_ID('sp_CreateOrderWithItems', 'P') IS NOT NULL
    DROP PROCEDURE sp_CreateOrderWithItems;
GO

CREATE PROCEDURE sp_CreateOrderWithItems
    @UserId UNIQUEIDENTIFIER,
    @OrderNumber NVARCHAR(50),
    @SubTotal DECIMAL(18,2),
    @TaxAmount DECIMAL(18,2),
    @ShippingCost DECIMAL(18,2),
    @OrderId UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Create new order
        SET @OrderId = NEWID();
        
        INSERT INTO Orders (Id, UserId, OrderNumber, Status, SubTotal, TaxAmount, ShippingCost, Total, CreatedAt)
        VALUES (
            @OrderId,
            @UserId,
            @OrderNumber,
            'Pending',
            @SubTotal,
            @TaxAmount,
            @ShippingCost,
            @SubTotal + @TaxAmount + @ShippingCost,
            GETUTCDATE()
        );
        
        COMMIT TRANSACTION;
        
        SELECT @OrderId AS OrderId, 'Order created successfully' AS Message;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

PRINT 'Simple Stored Procedures created successfully!';
