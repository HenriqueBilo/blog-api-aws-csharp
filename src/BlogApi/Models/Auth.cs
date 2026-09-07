namespace BlogApi.Models;

public record SignUpRequest(string Email, string Password);
public record ConfirmRequest(string Email, string Code);
public record LoginRequest(string Email, string Password);
