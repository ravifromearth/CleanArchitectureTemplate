namespace EntityFrameworkCore8Samples.Domain.Enums;

public enum ProductStatus
{
    Active = 1,
    Inactive = 2,
    Discontinued = 3,
    OutOfStock = 4
}

public enum ProductType
{
    Physical = 1,
    Digital = 2,
    Service = 3,
    Subscription = 4
}

public enum InventoryStatus
{
    InStock = 1,
    LowStock = 2,
    OutOfStock = 3,
    Discontinued = 4
}

public enum InventoryType
{
    Physical = 1,
    Digital = 2,
    Consumable = 3
}

public enum ReviewStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Hidden = 4
}

public enum ReviewType
{
    Product = 1,
    Service = 2,
    Experience = 3
}


