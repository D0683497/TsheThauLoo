namespace TsheThauLoo.Models.Account;

public record TokenDto
{
    public string AccessToken { get; set; } = null!;

    public TokenDto(string token)
    {
        AccessToken = token;
    }
}