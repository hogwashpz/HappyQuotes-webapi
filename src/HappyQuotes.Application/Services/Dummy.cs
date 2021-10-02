using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyQuotes.Application.Services
{
    public class DummyService : IDummyService
    {
        public DateTime GetDate()
        {
            return DateTime.Now;
        }
    }
}
