using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCalefornia.Models
{
    public class FormattingServices
    {
        public String AsReadableDate(DateTime dateTime)
        {
            return dateTime.ToString("d");
        }
    }
}
