using Riok.Mapperly.Abstractions;

namespace AdapterPattern;

// Adapter

[Mapper]
public partial class UserMapper
{
    public partial UserDto Map(User user);

    public partial User Map(UserDto dto);
}
