using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPattern;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class UserRepository
{
    private readonly IDictionary<int, User> _users = new Dictionary<int, User>();

    public UserRepository()
    {
        _users.Add(1, new User() {  Id = 1, FirstName = "John", LastName = "Smith", Email = "john@irc.pl", PasswordHash = "123" });
    }

    public User GetById(int id)
    {
        return _users[id];
    }

    public void Add(User user)
    {
        _users.Add(user.Id, user);
    }

   
}

public class UserController
{
    private readonly UserRepository repository;
    private readonly UserMapper mapper;

    public UserController(UserRepository repository, UserMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    // GET api/users/{id}
    public UserDto Get(int id)
    {
        var user = repository.GetById(id);

        var dto = mapper.Map(user);

        return dto;
    }

    public void Post(UserDto dto)
    {
        repository.Add(mapper.Map(dto));
    }
}