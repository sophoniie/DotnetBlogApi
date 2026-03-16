using BlogApi.DTOs.Users;
using BlogApi.Exceptions;
using BlogApi.Models;
using BlogApi.Models.Enums;
using BlogApi.Repositories.Users;

namespace BlogApi.Services.Users;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserResponse> CreateAsync(RegisterUserRequest request)
    {
        if (await userRepository.EmailExistsAsync(request.Email))
        {
            throw new AlreadyExistsException("User", "Email");
        }
        
        var user = new User(
            request.Name,
            request.Email,
            request.Password,
            request.Role ?? RoleEnum.Reader
        );
        
        var createdUser = await userRepository.CreateAsync(user);
        
        return MapToResponse(createdUser);
    }
    
    public async Task<UserResponse> FindByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        
        return user == null ? throw new NotFoundException("User", id) : MapToResponse(user);
    }
    
    public async Task<IEnumerable<UserResponse>> FindManyAsync(int pageNumber, int pageSize)
    {
        var users = await userRepository.GetAllAsync(pageNumber, pageSize);
        
        return users.Select(MapToResponse);
    }
    
    public async Task<UserResponse?> FindByEmailAsync(string email)
    {
        var user = await userRepository.GetByEmailAsync(email);
        
        return user == null ? null : MapToResponse(user);
    }
    
    public async Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = await userRepository.GetByIdAsync(id);
        
        if (user == null)
        {
            throw new NotFoundException("User", id);
        }
        
        if (await userRepository.EmailExistsAsync(request.Email, id))
        {
            throw new AlreadyExistsException("User", "Email");
        }
        
        user.UpdateInfo(request.Name, request.Email);
        
        var updatedUser = await userRepository.UpdateAsync(user);
        
        return MapToResponse(updatedUser);
    }
    
    public async Task DeleteAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        
        if (user == null)
        {
            throw new NotFoundException("User", id);
        }
        
        await userRepository.RemoveAsync(id);
    }
    
    public async Task RestoreAsync(int id)
    {
        await userRepository.RestoreAsync(id);
    }
    
    public async Task ChangePasswordAsync(int id, UpdatePasswordRequest request)
    {
        var user = await userRepository.GetByIdAsync(id);
        
        if (user == null)
        {
            throw new NotFoundException("User", id);
        }
        
        // TODO: Verify current password when password hashing is implemented
        user.UpdatePassword(request.NewPassword);
        
        await userRepository.UpdateAsync(user);
    }
    
    public async Task ChangeRoleAsync(int id, RoleEnum role)
    {
        var user = await userRepository.GetByIdAsync(id);
        
        if (user == null)
        {
            throw new NotFoundException("User", id);
        }
        
        user.ChangeRole(role);
        
        await userRepository.UpdateAsync(user);
    }
    
    private static UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
