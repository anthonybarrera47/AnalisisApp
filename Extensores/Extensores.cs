using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensores
{
    public static class Extensores
    {
        public static Analisis ToAnalisis(this object obj)
        {
            return (Analisis)obj;
        }
        public static bool EsNulo(this object obj)
        {
            return obj != null;
        }
        public static int ToInt(this object obj)
        {
            int.TryParse(obj.ToString(), out int value);
            return value;
        }
        public static DateTime ToDatetime(this object obj)
        {
            DateTime.TryParse(obj.ToString(), out DateTime value);
            return value;
        }
    }
}
