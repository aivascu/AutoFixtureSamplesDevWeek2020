using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Xunit;

namespace DemoXUnitTests.Demo08
{
    public class Demo08_AutoMocking
    {
        [Fact]
        public void ShouldResolveExpectedMock()
        {
            var fixture = new Fixture();
            var repositoryMock = new Mock<IEmployeeRepository>();
            var mailingServiceMock = new Mock<IMailingService>();
            fixture.Inject(repositoryMock.Object);
            fixture.Inject(mailingServiceMock.Object);

            var service = fixture.Create<MyDemoService>();

            service.SendPayslips();

            mailingServiceMock.Verify(x => x.SendMail(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void ShouldResolveAutoResolveMocks()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var mailingServiceMock = new Mock<IMailingService>();
            fixture.Inject(mailingServiceMock.Object);

            var service = fixture.Create<MyDemoService>();

            service.SendPayslips();

            mailingServiceMock.Verify(x => x.SendMail(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void ShouldResolveAutoResolveFrozenMocks()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var mailingServiceMock = fixture.Freeze<Mock<IMailingService>>();

            var service = fixture.Create<MyDemoService>();

            service.SendPayslips();

            mailingServiceMock.Verify(x => x.SendMail(It.IsAny<string>()), Times.Never);
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
}
