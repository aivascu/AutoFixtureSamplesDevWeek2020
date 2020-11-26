using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoFixture;
using Xunit;

namespace DemoXUnitTests.Demo05
{
    public class Demo05_PrimitiveTypesTests
    {
        #region Primitive Types

        [Fact]
        public void CreatesInstancesForPrimitiveTypes()
        {
            var fixture = new Fixture();

            // Basic types
            var boolValue = fixture.Create<bool>();
            var charValue = fixture.Create<char>();
            var stringValue = fixture.Create<string>();

            // Numeric types
            var byteValue = fixture.Create<byte>();
            var intValue = fixture.Create<int>();
            var doubleValue = fixture.Create<double>();
            var decimalValue = fixture.Create<decimal>();

            Assert.True(true);
        }

        #endregion Primitive Types

        #region BCL Types

        [Fact]
        public void CreatesInstancesForBCLTypes()
        {
            var fixture = new Fixture();

            var guidValue = fixture.Create<Guid>();
            var dateTimeValue = fixture.Create<DateTime>();
            var enumValue = fixture.Create<CustomEnum>();
            var mailAddressValue = fixture.Create<MailAddress>();
            var uriValue = fixture.Create<Uri>();
            var nullableValue = fixture.Create<int?>(); // All types are supported as nullable.
            var taskValue = fixture.Create<Task>();
            var genericTaskValue = fixture.Create<Task<int>>();
            var delegateValue = fixture.Create<Func<int, int>>();

            Assert.True(true);
        }

        private enum CustomEnum
        {
            NoValue = 0,
            SomeValue,
            SomeOtherValue,
            YetAnotherValue
        }

        #endregion BCL Types

        #region Collections

        [Fact]
        public void CreatesInstancesForCollections()
        {
            var fixture = new Fixture();

            var enumerable = fixture.Create<IEnumerable<int>>();
            var array = fixture.Create<double[]>();
            var list = fixture.Create<List<int>>();
            var dictionary = fixture.Create<Dictionary<byte, string>>();
            var hashSet = fixture.Create<HashSet<int>>();
            var observableCollection = fixture.Create<ObservableCollection<int>>();
            var iList = fixture.Create<IList<string>>();
            var readOnlyCollection = fixture.Create<IReadOnlyCollection<string>>();
            var readOnlyDictionary = fixture.Create<IReadOnlyDictionary<string, long>>();

            Assert.True(true);
        }

        [Fact]
        public void CreatesInstancesForCollectionsOfCustomSize()
        {
            var fixture = new Fixture();
            //{
            //    RepeatCount = 3
            //};

            var defaultSizeCollection = fixture.CreateMany<int>();
            var customSizeCollection = fixture.CreateMany<int>(count: 5);
        }

        #endregion Collections
    }
}
