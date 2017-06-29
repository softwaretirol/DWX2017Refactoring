using System;

namespace Tetris.TestSample
{
    public class AgeCalculator
    {
        private readonly IDateProvider dateProvider;

        public AgeCalculator(IDateProvider dateProvider)
        {
            this.dateProvider = dateProvider;
        }
        public TimeSpan Calculate(DateTime date)
        {
            return dateProvider.Today - date;
        }
    }
}