using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.TestSample
{
    public class DateProvider : IDateProvider
    {
        public DateTime Today => DateTime.Today;
    }
}
