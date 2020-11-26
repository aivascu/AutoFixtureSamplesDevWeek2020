using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Community.AutoEF.Sqlite;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using TestClasses;
using TestClasses.Entities;
using Xunit;

namespace DemoXUnitTests.Bonus
{
    public class Bonus_DbContextTesting
    {
        [Theory]
        [AutoDomainData]
        public void ShouldCreateDbContextInstance(
            [Frozen] SqliteConnection connection,
            [Greedy] TestCustomDbContext context,
            Customer customer)
        {
            connection.Open();
            context.Database.EnsureCreated();
            context.Customers.Add(customer);
            context.SaveChanges();

            context.Customers.Should().HaveCount(1)
                .And.Subject.Should().Contain(customer);
        }
    }

    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(() => new Fixture()
                .Customize(
                new CompositeCustomization(
                    new SqliteContextCustomization(),
                    new AutoMoqCustomization())))
        {
        }
    }
}
