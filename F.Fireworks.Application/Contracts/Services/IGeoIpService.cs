namespace F.Fireworks.Application.Contracts.Services;

public interface IGeoIpService
{
    string GetLocation(string ipAddress);
}