using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace DemoXUnitTests.Demo04
{
    public class Demo04_BuilderPattern
    {
        [Fact]
        public void ShouldRemoveExpectedEmployees()
        {
            var repositoryMock = new Mock<IRepository>();
            var employees = new [] {
                new EmployeeBuilder().WithName("Jimmy Kutrapali").WithSalary(2000).Build(),
                new EmployeeBuilder().WithName("Jane Robins").WithSalary(1000).Build()
            };
            var sut = new MyService(repositoryMock.Object);


            sut.LayOff(employees);

            repositoryMock.Verify(x => x.Remove(employees), Times.Once);
        }

        [Fact]
        public void ShouldRemoveExpectedEmployees_WithServiceBuilder()
        {
            var employees = new[] {
                new EmployeeBuilder().WithName("Jimmy Kutrapali").WithSalary(2000).Build(),
                new EmployeeBuilder().WithName("Jane Robins").WithSalary(1000).Build()
            };
            var repositoryMock = new Mock<IRepository>();
            var sut = new MyServiceBuilder()
                .WithRepository(repositoryMock)
                .Build();


            sut.LayOff(employees);

            repositoryMock.Verify(x => x.Remove(employees), Times.Once);
        }
    }

    public class MyService
    {
        private readonly IRepository repository;

        public MyService(IRepository repository)
        {
            this.repository = repository;
        }

        public void LayOff(IEnumerable<Employee> employees)
        {
            this.repository.Remove(employees);
        }

        public void SendMessage(Employee employee, string message)
        {

        }
    }

    public class ISMSService
    {

    }

    public interface IRepository
    {
        void Remove(IEnumerable<Employee> employees);
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

    public class EmployeeBuilder
    {
        private string name = "John Doe";
        private decimal salary = 1250;

        public EmployeeBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public EmployeeBuilder WithSalary(decimal salary)
        {
            this.salary = salary;
            return this;
        }

        public Employee Build()
        {
            return new Employee(this.name)
            {
                Salary = this.salary
            };
        }
    }

    public class MyServiceBuilder
    {
        private Mock<IRepository> repositoryMock = new Mock<IRepository>();

        public MyServiceBuilder WithRepository(Action<Mock<IRepository>> setup)
        {
            setup(repositoryMock);
            return this;
        }

        public MyServiceBuilder WithRepository(Mock<IRepository> repositoryMock)
        {
            this.repositoryMock = repositoryMock;
            return this;
        }

        public MyService Build()
        {
            return new MyService(repositoryMock.Object);
        }
    }
}
