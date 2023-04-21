using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OwnedEntitiesTest;

[TestClass]
public class DbContextTests
{
    [AssemblyInitialize]
    public static async Task AssemblyInitialize(TestContext testContext)
    {
        var context = new MyDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    [AssemblyCleanup]
    public static async Task AssemblyCleanup()
    {
        var context = new MyDbContext();
        await context.Database.EnsureDeletedAsync();
    }

    [TestMethod]
    public async Task UpdateParentEntityShouldSucceed()
    {
        var context = new MyDbContext();

        var parentEntity = new ParentEntity
        {
            Name = "Parent Entity",
            OwnedEntities = new[]
            {
                new OwnedEntity
                {
                    Name = "Owned Entity 1",
                },
                new OwnedEntity
                {
                    Name = "Owned Entity 2"
                }
            }
        };

        await context.AddAsync(parentEntity);
        await context.SaveChangesAsync();

        // Reset context
        context = new MyDbContext();

        var parentEntityToModify = await context.ParentEntities.AsNoTracking().FirstAsync(e => e.Name == "Parent Entity");

        parentEntityToModify.Name = "Parent Entity Modified";
        parentEntityToModify.OwnedEntities.Add(new OwnedEntity
        {
            Name = "Owned Entity 3"
        });

        context.Update(parentEntityToModify);
        await context.SaveChangesAsync();
    }
}