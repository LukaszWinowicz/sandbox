namespace AdapterPattern;

// Adapter
public class UserMapper
{
    public UserDto Map(User user)
    {
        return new UserDto { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };
    }

    public User Map(UserDto dto)
    {
        return new User { Id = dto.Id, FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email };
    }   
}
