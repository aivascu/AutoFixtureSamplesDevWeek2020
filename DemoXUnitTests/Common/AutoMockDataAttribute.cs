using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace DemoXUnitTests.Common
{
    public class AutoMockDataAttribute : AutoDataAttribute
    {
        public AutoMockDataAttribute()
            : base(() => new Fixture()
                 .Customize(new AutoMoqCustomization()))
        {
        }
    }
}
