using System.ComponentModel.DataAnnotations;

namespace F.Fireworks.Infrastructure.Options;

public class InitialSetupSettings
{
    public const string SectionName = "InitialSetupSettings";

    [Required] public string SuperAdminRoleName { get; set; } = null!;

    [Required] public DefaultAdminSettings DefaultAdmin { get; set; } = null!;
}

public class DefaultAdminSettings
{
    [Required] public string UserName { get; set; } = null!;

    [Required] [EmailAddress] public string Email { get; set; } = null!;

    [Required] public string DefaultPassword { get; set; } = null!;
}