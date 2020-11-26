using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Xunit;

namespace DemoXUnitTests.Demo09
{
    public class Demo09_Integrations
    {
        [Theory]
        [AutoData]
        public void EmployeeShouldHaveDefaultData(Employee employee)
        {
            employee.Should().NotBeNull();
            employee.Name.Should().NotBeNullOrWhiteSpace();
            employee.Salary.Should().BePositive();
        }

        [Theory]
        [AutoDomainData]
        public void AllInstancesShouldBeCustomized(
            [Frozen] Mock<IEmployeeRepository> repositoryMock,
            [Frozen] Mock<IMailingService> mailingServiceMock,
            MyDemoService service,
            IEnumerable<Employee> employees)
        {
            repositoryMock.Setup(x => x.GetAll()).Returns(employees);

            service.SendPayslips();

            mailingServiceMock.Verify(x => x.SendMail(employees.First().Email), Times.Once);
            employees.Should()
                .AllBeEquivalentTo(
                    new Employee { Salary = 12_357 },
                    x => x.Excluding(e => e.Name).Excluding(e => e.Email));
        }
    }

    public class MyDemoService
    {
        private readonly IEmployeeRepository repository;
        private readonly IMailingService mailingService;

        public MyDemoService(IEmployeeRepository repository, IMailingService mailingService)
        {
            this.repository = repository;
            this.mailingService = mailingService;
        }

        public void SendPayslips()
        {
            this.repository.GetAll()
                .ToList()
                .ForEach(x => mailingService.SendMail(x.Email));
        }
    }

    public interface IMailingService
    {
        public void SendMail(string email);
    }

    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
    }

    public class Employee
    {
        public string Name { get; set; }
        public string Email { get; set; }
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

    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(() => new Fixture().Customize(new DomainDataCustomization()))
        {
        }
    }

    public class DomainDataCustomization : CompositeCustomization
    {
        public DomainDataCustomization()
            : base(
                 new AutoMoqCustomization(),
                 new EmployeeCustomization())
        {
        }
    }
}
