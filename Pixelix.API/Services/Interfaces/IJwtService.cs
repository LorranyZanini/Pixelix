using Pixelix.API.DTOs;

namespace Pixelix.API.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(UserDto user);
}