using System.ComponentModel.DataAnnotations;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace DemoXUnitTests.Demo06
{
    public class Demo06_POCOSupport
    {
        [Fact]
        public void EmployeeShouldHaveName()
        {
            var fixture = new Fixture();

            var actual = fixture.Create<Employee>();

            actual.Name.Should().NotBeNullOrEmpty();
            actual.Salary.Should().BeGreaterThan(0);
        }

        [Fact]
        public void EmployeeDtoShouldRespectAnnotations()
        {
            var fixture = new Fixture();

            var actual = fixture.Create<EmployeeDto>();

            actual.Name.Should().MatchRegex("^[A-Z][a-z]{1,20}$");
            actual.Salary.Should().BeInRange(200, 1_500);
        }

        [Fact]
        public void EmployeeShouldContainExpectedData()
        {
            var fixture = new Fixture();

            var employee = fixture.Build<Employee>()
                .FromFactory((string name) => new Employee(name))
                .With(e => e.Salary, 2_000)
                .Create();

            employee.Should()
                .BeEquivalentTo(
                     new Employee(string.Empty) { Salary = 2_000 },
                     config => config.Excluding(x => x.Name));
        }

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

    public class EmployeeDto
    {
        [RegularExpression("^[A-Z][a-z]{1,20}$")]
        public string Name { get; set; }

        [Range(200, 1500)]
        public decimal Salary { get; set; }
    }
}
