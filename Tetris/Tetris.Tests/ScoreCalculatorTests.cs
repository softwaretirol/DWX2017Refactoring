using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tetris.Tests
{
    [TestFixture]
    public class ScoreCalculatorTests
    {
        // "Behauptung"
        // AAA - Assign, Act, Assert
        [Test]
        public void CalculateLevelWithValidValue()
        {
            ScoreCalculator calculator = new ScoreCalculator();
            var level = calculator.CalculateLevel(2);

            Assert.That(level, Is.EqualTo(1));
        }

        [Test]
        public void CalculateLevelWithNegativeValue()
        {
            ScoreCalculator calculator = new ScoreCalculator();
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var level = calculator.CalculateLevel(-1);
            });

        }
    }
}
