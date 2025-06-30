namespace F.Fireworks.Application.Contracts.Services;

public interface IPermissionService
{
    Task<List<string>> GetPermissionCodesForUserAsync(Guid userId);
}