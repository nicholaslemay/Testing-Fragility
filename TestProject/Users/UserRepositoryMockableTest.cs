using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BFF.Support.Database;
using BFF.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace TestProject.Users;

public class UserRepositoryMockableTest
{
    private readonly UserRepositoryMockable _sut;
    private readonly IUserRecordMapper _userMapper;
    private readonly BffDbMockable _mockDb;
    private DbSet<UserRecord> _userRecords;

    public UserRepositoryMockableTest()
    {
        _mockDb = Substitute.For<BffDbMockable>();
        _userMapper = Substitute.For<IUserRecordMapper>();
        _userRecords = UserRecordsContaining(new List<UserRecord>());
        _mockDb.Users.Returns(_userRecords);
        _sut = new UserRepositoryMockable(_mockDb, _userMapper);
    }

    [Fact]
    public async Task CanStoreAUserInDb()
    {
        var user = new User("bobby","bobby@hotmail.com", GenderType.Male);
        var expectedRecord = new UserRecord{Id = 33};
        _userMapper.AsRecord(user).Returns(expectedRecord);
        
        var returnedId = await _sut.AddUserAsync(user);

        returnedId.Should().Be(expectedRecord.Id);
        Received.InOrder(() =>
        {
            _userRecords.Add(expectedRecord);
            _mockDb.SaveChangesAsync();
        });
    }
    
    [Fact]
    public void CanReadAllMales()
    {
        _userRecords = UserRecordsContaining(new List<UserRecord>
        {
            new() { Gender = "M"},
            new() { Gender = "M"},
            new() { Gender = "F"},
        });
        _mockDb.Users.Returns(_userRecords);

         var males = _sut.AllMen();
         males.Should().HaveCount(2);
    }

    private DbSet<UserRecord> UserRecordsContaining(List<UserRecord> data)
    {
        var dbSet = Substitute.For<DbSet<UserRecord>, IQueryable<UserRecord>>();

        var queryable = data.AsQueryable();
         ((IQueryable<UserRecord>)dbSet).Provider.Returns(queryable.Provider);
         ((IQueryable<UserRecord>)dbSet).Expression.Returns(queryable.Expression);
        // ((IQueryable<UserRecord>)dbSet).ElementType.Returns(queryable.ElementType);
        ((IQueryable<UserRecord>)dbSet).GetEnumerator().Returns(queryable.GetEnumerator());
       // dbSet.AsNoTracking().Returns(queryable);
       return dbSet;
    }
}