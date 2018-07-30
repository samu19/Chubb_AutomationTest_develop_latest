using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumAutomation
{

    public interface IFillable
    {
        void Fill(FullElementSelector fullElementSelector, string testId = null, string testName = null);
    }
}