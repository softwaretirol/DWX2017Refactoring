using System;
using Moq;
using NUnit.Framework;
using Tetris.TestSample;

namespace Tetris.Tests
{
    [TestFixture]
    public class AgeCalculatorTests
    {
        [Test]
        public void CalculateValidAge()
        {
            //TypeMock
            var dateProviderMock = new Mock<IDateProvider>();
            dateProviderMock.Setup(x => x.Today).Returns(new DateTime(2017, 06, 28));
            AgeCalculator calculator = new AgeCalculator(dateProviderMock.Object);
            var age = calculator.Calculate(new DateTime(2017, 06, 29));

            dateProviderMock.Verify(x => x.Today, Times.Exactly(1));
            Assert.That(age, Is.EqualTo(TimeSpan.FromDays(1)));

        }
    }
}