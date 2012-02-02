using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{
    public static class Conversao
    {
        public static DateTime? ToDateTime(Object date)
        {
            if (date == null || date == DBNull.Value)
                return null;

            return Convert.ToDateTime(date);
        }
    }
}
