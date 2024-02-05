using Microsoft.EntityFrameworkCore;
using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Repositories;

namespace OrderSolution.Infrastructure.Tests.Repositories;


public class TestEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<TestEntity> TestEntities { get; set; }
}
public class TestRepository : Repository<TestEntity>
{
    public TestRepository(TestDbContext context) : base(context)
    {
    }
}

public class Repository_Tests
{
    private TestRepository _testRepository;
    private TestDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _testRepository = new TestRepository(_context);
    }

    [Test]
    public async Task AddAsync_AddsEntityToDatabase()
    {
        // Arrange
        var testEntity = new TestEntity {Id = Guid.NewGuid()};

        // Act
        var result = await _testRepository.AddAsync(testEntity);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(testEntity.Id));
    }
    
    [Test]
    public async Task AddAsync_AddsEntityToDatabase_FailsWhenEntityWithSameIdExists()
    {
        // Arrange
        var testEntity1 = new TestEntity { Id = Guid.NewGuid() };
        await _context.Set<TestEntity>().AddAsync(testEntity1);
        await _context.SaveChangesAsync();

        var testEntity2 = new TestEntity { Id = testEntity1.Id }; // Same ID as testEntity1

        // Act
        var result = await _testRepository.AddAsync(testEntity2);

        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task GetAsync_ReturnsEntityFromDatabase()
    {
        // Arrange
        var testEntity = new TestEntity { Id = Guid.NewGuid() };
        _context.Set<TestEntity>().Add(testEntity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _testRepository.GetAsync(e => e.Id == testEntity.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(testEntity.Id));
    }
    
    [Test]
    public async Task GetAsync_ReturnsNullWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentEntityId = Guid.NewGuid();

        // Act
        var result = await _testRepository.GetAsync(x => x.Id == nonExistentEntityId);

        // Assert
        Assert.IsNull(result);
    }
    
    [Test]
    public async Task GetAllAsync_ReturnsAllEntitiesFromDatabase()
    {
        // Arrange
        var testEntity1 = new TestEntity { Id = Guid.NewGuid() };
        var testEntity2 = new TestEntity { Id = Guid.NewGuid() };
        _context.Set<TestEntity>().Add(testEntity1);
        _context.Set<TestEntity>().Add(testEntity2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _testRepository.GetAllAsync();
        var testEntities = result.ToList();

        // Assert
        Assert.That(testEntities, Is.Not.Null);
        Assert.That(testEntities, Has.Count.EqualTo(2));
        Assert.That(testEntities, Does.Contain(testEntity1));
        Assert.That(testEntities, Does.Contain(testEntity2));
    }
    
    [Test]
    public async Task GetAllAsync_ReturnsEmptyListWhenDatabaseIsEmpty()
    {
        // Act
        var result = await _testRepository.GetAllAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public async Task ExistsAsync_ReturnsTrueWhenEntityExists()
    {
        // Arrange
        var testEntity = new TestEntity { Id = Guid.NewGuid() };
        _context.Set<TestEntity>().Add(testEntity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _testRepository.ExistsAsync(e => e.Id == testEntity.Id);

        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task ExistsAsync_ReturnsFalseWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentEntityId = Guid.NewGuid();

        // Act
        var result = await _testRepository.ExistsAsync(x => x.Id == nonExistentEntityId);

        // Assert
        Assert.IsFalse(result);
    }
    
    [Test]
    public async Task UpdateAsync_UpdatesEntityInDatabase()
    {
        // Arrange
        var testEntity = new TestEntity { Id = Guid.NewGuid(), Name = "Test"};
        _context.Set<TestEntity>().Add(testEntity);
        await _context.SaveChangesAsync();

        var updatedEntity = new TestEntity { Id = testEntity.Id, Name = "Updated"};

        // Act
        var result = await _testRepository.UpdateAsync(e => e.Id == testEntity.Id, updatedEntity);

        // Assert
        Assert.That(result, Is.True);
        var entityInDb = await _context.Set<TestEntity>().FindAsync(updatedEntity.Id);
        Assert.That(entityInDb?.Name, Is.EqualTo(updatedEntity.Name));
    }
    
    [Test]
    public async Task UpdateAsync_ReturnsFalseWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentEntityId = Guid.NewGuid();
        var updatedEntity = new TestEntity { Id = nonExistentEntityId, Name = "Updated"};

        // Act
        var result = await _testRepository.UpdateAsync(x => x.Id == nonExistentEntityId, updatedEntity);

        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task DeleteAsync_DeletesEntityFromDatabase()
    {
        // Arrange
        var testEntity = new TestEntity { Id = Guid.NewGuid() };
        _context.Set<TestEntity>().Add(testEntity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _testRepository.DeleteAsync(e => e.Id == testEntity.Id);

        // Assert
        Assert.That(result, Is.True);
        var entityInDb = await _context.Set<TestEntity>().FindAsync(testEntity.Id);
        Assert.That(entityInDb, Is.Null);
    }
    
    [Test]
    public async Task DeleteAsync_ReturnsFalseWhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentEntityId = Guid.NewGuid();

        // Act
        var result = await _testRepository.DeleteAsync(x => x.Id == nonExistentEntityId);

        // Assert
        Assert.IsFalse(result);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
    }
}