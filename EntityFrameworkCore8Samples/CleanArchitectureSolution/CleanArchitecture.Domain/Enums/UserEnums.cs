namespace EntityFrameworkCore8Samples.Domain.Enums;

public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Suspended = 3,
    Pending = 4
}

public enum UserRole
{
    User = 1,
    Admin = 2,
    Moderator = 3,
    SuperAdmin = 4
}

public enum SessionStatus
{
    Active = 1,
    Expired = 2,
    Revoked = 3,
    Suspended = 4
}

public enum SessionType
{
    Web = 1,
    Mobile = 2,
    API = 3,
    Desktop = 4
}


