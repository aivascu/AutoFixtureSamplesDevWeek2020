using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

namespace DemoXUnitTests.Demo03
{
    public abstract class TestBase
    {
        // Here be many more test data setup members

        protected IEnumerable<Employee> GetEmployeesForSalaries(params int[] salaries)
        {
            foreach (var item in salaries)
            {
                yield return new Employee
                {
                    Name = Guid.NewGuid().ToString(),
                    Salary = item
                };
            }
        }
    }

    public class Demo03_BaseClassForTestData : TestBase
    {
        private Mock<IEmployeeRepository> employeeRepositoryMock;

        public Demo03_BaseClassForTestData()
        {
            employeeRepositoryMock = new Mock<IEmployeeRepository>();
        }

        [Fact]
        public void CalculateAverageForOneValue()
        {
            employeeRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(GetEmployeesForSalaries(120).ToArray());

            var sut = new MyDemoService(employeeRepositoryMock.Object);

            var actual = sut.GetAverageSalary();

            Assert.Equal(120, actual);
        }

        [Fact]
        public void CalculateAverageForTwoValue()
        {
            employeeRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(GetEmployeesForSalaries(120, 258).ToArray());

            var sut = new MyDemoService(employeeRepositoryMock.Object);

            var actual = sut.GetAverageSalary();

            Assert.Equal(189, actual);
        }
    }

    public class MyDemoService
    {
        private readonly IEmployeeRepository repository;

        public MyDemoService(IEmployeeRepository repository)
        {
            this.repository = repository;
        }

        public decimal GetAverageSalary()
        {
            return this.repository.GetAll().Average(x => x.Salary);
        }
    }

    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
    }

    public class Employee
    {
        public string Name { get; set; }
        public decimal Salary { get; set; }
    }
}
