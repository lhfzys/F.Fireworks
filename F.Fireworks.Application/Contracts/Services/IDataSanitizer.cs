namespace F.Fireworks.Application.Contracts.Services;

public interface IDataSanitizer
{
    string Sanitize(string json);
}