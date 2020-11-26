using AutoFixture;
using FluentAssertions;
using Xunit;

namespace DemoXUnitTests.Demo07
{
    public class Demo07_Customization
    {
        [Fact]
        public void CustomizedTypeShouldHaveExpectedValues()
        {
            var fixture = new Fixture();

            fixture.Customize<Employee>(
                x => x.With(x => x.Salary, 2_020));

            var employee1 = fixture.Create<Employee>();
            var employee2 = fixture.Create<Employee>();

            employee1.Salary.Should().Be(employee2.Salary)
                .And.Subject.Should().Be(2_020);
        }

        [Fact]
        public void FixtureResolvesInjectedValueOnEachRequest()
        {
            var fixture = new Fixture();
            var employee = new Employee("Luke Skywalker")
            {
                Salary = 1_000_000
            };

            fixture.Inject(employee);

            var employee1 = fixture.Create<Employee>();
            var employee2 = fixture.Create<Employee>();

            employee.Should().Be(employee1)
                .And.Subject.Should().Be(employee2);
        }

        [Fact]
        public void FixtureResolvesFrozenValueOnEachRequest()
        {
            var fixture = new Fixture();
            fixture.Customize<Employee>(
                x => x
                    .FromFactory(() => new Employee("Darth Vader"))
                    .With(x => x.Salary, 2_020));

            var employee = fixture.Freeze<Employee>();

            var employee1 = fixture.Create<Employee>();
            var employee2 = fixture.Create<Employee>();

            employee.Should().Be(employee1)
                .And.Subject.Should().Be(employee2)
                .And.Subject.Should()
                    .BeEquivalentTo(new Employee("Darth Vader") { Salary = 2_020 });
        }

        [Fact]
        public void FixtureResolvesOnlyCustomizedData()
        {
            var fixture = new Fixture()
                .Customize(new EmployeeCustomization());

            var employees = fixture.CreateMany<Employee>(20);

            employees.Should()
                .AllBeEquivalentTo(
                    new Employee("") { Salary = 12_357 },
                    x => x.Excluding(e => e.Name));

        }

        public class Employee
        {
            public Employee(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public decimal Salary { get; set; }
        }

        public class EmployeeCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<Employee>(
                    x => x.With(x => x.Salary, 12_357));
            }
        }
    }
}
