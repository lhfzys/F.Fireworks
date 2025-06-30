using F.Fireworks.Application.Contracts.Services;
using UAParser;

namespace F.Fireworks.Infrastructure.Services;

public class UserAgentParserService : IUserAgentParserService
{
    public DeviceInfo Parse(string uaString)
    {
        if (string.IsNullOrEmpty(uaString)) return new DeviceInfo("Unknown", "Unknown", "Unknown");
        var uaParser = Parser.GetDefault();
        var c = uaParser.Parse(uaString);

        return new DeviceInfo(c.Device.Family, c.OS.Family, c.UA.Family);
    }
}