using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

namespace DemoXUnitTests.Demo01
{
    public class Demo01_InlineTestData
    {
        private Mock<IEmployeeRepository> employeeRepositoryMock;

        public Demo01_InlineTestData()
        {
            employeeRepositoryMock = new Mock<IEmployeeRepository>();
        }

        [Fact]
        public void CalculateAverageForOneValue()
        {
            employeeRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new[] {
                    new Employee
                    {
                        Name = "Mike",
                        Salary = 120
                    }
                });

            var sut = new MyDemoService(employeeRepositoryMock.Object);

            var actual = sut.GetAverageSalary();

            Assert.Equal(120, actual);
        }

        [Fact]
        public void CalculateAverageForTwoValue()
        {
            employeeRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new[] {
                    new Employee
                    {
                        Name = "Mike",
                        Salary = 120
                    },
                    new Employee
                    {
                        Name = "John",
                        Salary = 258
                    }
                });

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
