using BFF.Support.Database;
using Microsoft.EntityFrameworkCore;
using static BFF.Users.GenderType;

namespace BFF.Users;

public interface IUserRepository
{
    Task<int> AddUserAsync(User user);
}

public class UserRepository : IUserRepository
{
    private readonly BffDb _db;

    public UserRepository(BffDb db) => _db = db;

    public async Task<int> AddUserAsync(User u)
    {
        var newRecord = u.AsRecord();
        _db.Users.Add(newRecord);
        await _db.SaveChangesAsync();

        return newRecord.Id;
    }

    public List<User> AllMen()
    {
        return _db.Users.Where(u=> u.Gender == "M")
            .Select(u=> u.AsUser()).ToList();
    }
    
    public List<User> AllMenOtherWay()
    {
        return _db.Users.
            FromSqlRaw("SELECT * FROM Users WHERE Gender == 'M'")
            .Select(u=> u.AsUser()).ToList();
    }
}


public class BffDbMockable : DbContext
{
    public BffDbMockable()
    {
    }

    public BffDbMockable(DbContextOptions<BffDb> options) : base(options) { }
    
    public virtual DbSet<UserRecord> Users => Set<UserRecord>(); 
}

public interface IUserRecordMapper
{
    public UserRecord AsRecord(User u);
    public User AsUser(UserRecord u);
}

public class UserRepositoryMockable : IUserRepository
{
    private readonly BffDbMockable _db;
    private readonly IUserRecordMapper _userMapper;

    public UserRepositoryMockable(BffDbMockable db, IUserRecordMapper userMapper)
    {
        _db = db;
        _userMapper = userMapper;
    }

    public async Task<int> AddUserAsync(User u)
    {
        var newRecord = _userMapper.AsRecord(u);
        _db.Users.Add(newRecord);
        await _db.SaveChangesAsync();

        return newRecord.Id;
    }

    public List<User> AllMen()
    {
        return _db.Users.Where(u=> u.Gender == "M")
            .Select(u=> u.AsUser()).ToList();
    }
    
    // public List<User> AllMenOtherWay()
    // {
    //     return _db.Users.
    //         FromSqlRaw("SELECT * FROM Users WHERE Gender == 'M'")
    //         .Select(u=> u.AsUser()).ToList();
    // }
}


public static class UserRecordMapper
{
    public static UserRecord AsRecord(this User u) =>
        new()
        {
            Email = u.Email,
            Name = u.Name,
            Gender = u.Gender switch
            {
                Female => "F",
                Male => "M",
                Other => "O",
                _ => throw new UnknownGenderException()
            }
        };

    public static User AsUser(this UserRecord u) => new User(u.Name,
        u.Email,
        u.Gender switch
        {
            "F" => Female,
            "M" => Male,
            "O" => Other
        });
}

public class UnknownGenderException : Exception
{
}