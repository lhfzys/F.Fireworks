namespace F.Fireworks.Application.Contracts.Services;

public record DeviceInfo(string DeviceFamily, string OsFamily, string BrowserFamily);

public interface IUserAgentParserService
{
    DeviceInfo Parse(string uaString);
}