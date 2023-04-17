namespace EServicePortal.Application.Users.DTOs;

public record LoginResponse(string AccessToken, DateTime Expiry);
