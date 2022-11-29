using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BFF.Tests.Support;
using BFF.Tests.Support.Database;
using BFF.Users;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using static BFF.Users.GenderType;

namespace BFF.Tests.Users;

public class UserRepositoryTest : DatabaseTest
{
    private readonly UserRepository _userRepository;

    public UserRepositoryTest(DatabaseTestFixture fixture) : base(fixture) => _userRepository = new UserRepository(Db);

    [Fact]
    public async Task CanStoreAUserInDb()
    {
        var user = new User("bobby","bobby@hotmail.com", Male);
        var id = await _userRepository.AddUserAsync(user);

        id.Should().BeGreaterThan(0);
       
        using (new AssertionScope())
        {
            var savedUser = Db.Users.First();
            savedUser.Email.Should().Be(user.Email);
            savedUser.Name.Should().Be(user.Name);
            savedUser.Gender.Should().Be("M");
        }
    }
    
    [Fact]
    public void AllUsersHaveAUniqueId()
    {
        var user = new User("bobby","bobby@hotmail.com", Male);  
        
        List<int> ids = new();
        30.Times(async ()=> ids.Add(await _userRepository.AddUserAsync(user)));

        ids.Should().HaveCount(30).And.OnlyHaveUniqueItems();
    }
    
    [Fact]
    public void CanReadAllMales()
    {
        12.Times(async ()=> await _userRepository.AddUserAsync(SomeUser() with{Gender = Male}));
        18.Times(async ()=> await _userRepository.AddUserAsync(SomeUser() with{Gender = Female}));

        var males = _userRepository.AllMen();
        males.Should().HaveCount(12);
    }
    
    [Fact]
    public void CanReadAllMales_RawSQL()
    {
        12.Times(async ()=> await _userRepository.AddUserAsync(SomeUser() with{Gender = Male}));
        18.Times(async ()=> await _userRepository.AddUserAsync(SomeUser() with{Gender = Female}));

        var males = _userRepository.AllMenOtherWay();
        males.Should().HaveCount(12);
    }

    private static User SomeUser() => new("bobby","bobby@hotmail.com", Male);
}