using F.Fireworks.Application.Contracts.Services;
using IPTools.Core;

namespace F.Fireworks.Infrastructure.Services;

public class GeoIpService : IGeoIpService
{
    public string GetLocation(string ipAddress)
    {
        try
        {
            var ipInfo = IpTool.Search(ipAddress);
            return $"{ipInfo.Country}-{ipInfo.Province}-{ipInfo.City}";
        }
        catch (Exception)
        {
            return "未知位置";
        }
    }
}