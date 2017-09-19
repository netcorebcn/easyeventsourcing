using System;
using Xunit;

namespace EasyEventSourcing.Tests
{
    public class EventStoreOptionsTest
    {
        [Fact]
        public void Given_EmptyEventStoreOptions_When_Create_Then_DefaultValues()
        {
            var sut = EventStoreOptions.Create();
            AssertDefaultEventStoreOptions(sut);
        }

        [Fact]
        public void Given_Null_EventStoreOptions_When_Create_Then_DefaultValues()
        {
            var sut = EventStoreOptions.Create(null, null);
            AssertDefaultEventStoreOptions(sut);
        }

        [Fact]
        public void Given_MissingCredentials_For_ManagerHost_When_Create_Then_Exception()
        {
            Action action = () => EventStoreOptions.Create(null, "localhost:2113");
            Assert.ThrowsAny<ArgumentException>(action);
        }

        [Fact]
        public void Given_Missing_ManagerHost_When_Create_Then_Exception()
        {
            Action action = () => EventStoreOptions.Create(null, "user:password");
            Assert.ThrowsAny<ArgumentException>(action);
        }

        [Fact]
        public void Given_Valid_ManagerHost_When_Create_Then_Exception()
        {
            var sut = EventStoreOptions.Create(null, "admin:changeit@localhost:2113");
            AssertDefaultEventStoreOptions(sut);
        }

        private static void AssertDefaultEventStoreOptions(EventStoreOptions sut)
        {
            Assert.Equal(sut.ConnectionString, "tcp://admin:changeit@localhost:1113");
            Assert.Equal(sut.ManagerHost, "localhost:2113");
            Assert.Equal(sut.Credentials.Password, "changeit");
            Assert.Equal(sut.Credentials.Username, "admin");
        }
    }
}
