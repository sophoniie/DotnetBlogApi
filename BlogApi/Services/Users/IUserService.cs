using BlogApi.DTOs.Users;
using BlogApi.Models;
using BlogApi.Models.Enums;

namespace BlogApi.Services.Users;

public interface IUserService
{
    Task<UserResponse> CreateAsync(RegisterUserRequest request);
    Task<UserResponse> FindByIdAsync(int id);
    Task<IEnumerable<UserResponse>> FindManyAsync(int pageNumber, int pageSize);
    Task<UserResponse?> FindByEmailAsync(string email);
    Task<User?> FindByEmailWithPasswordAsync(string email);
    Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);
    Task ChangePasswordAsync(int id, UpdatePasswordRequest request);
    Task ChangeRoleAsync(int id, RoleEnum role);
}
