namespace TeamTrackPro.API.Helpers;

public static class RoleConstants
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string User = "User";
    
    public static readonly Dictionary<string, string> RoleDescriptions = new()
    {
        { Admin, "Administrator with full system access" },
        { Manager, "Project manager with project and ticket management capabilities" },
        { User, "Regular user with basic ticket management capabilities" }
    };
} 