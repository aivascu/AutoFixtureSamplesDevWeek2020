using System;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Xunit;

namespace DemoXUnitTests.Demo10
{
    public class Demo10_IdiomaticTests
    {
        [Theory]
        [AutoData]
        public void ConstructorsShouldCheckForNull(
            GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Employee));
        }

        [Theory]
        [AutoData]
        public void AllWritableAssertionsShouldReturnAssignedValue(
            WritablePropertyAssertion assertion)
        {
            assertion.Verify(typeof(Employee));
        }
    }

    public class Employee
    {
        public Employee(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public string Name { get; }

        public decimal Salary { get; set; }
    }
}
