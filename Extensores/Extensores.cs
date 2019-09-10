using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        public static void Alerta(System.Web.UI.Page page, TipoAlerta tipoAlerta)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", $"{ tipoAlerta.ToString().ToLower()}()", true);
        }
        static readonly string FECHA_FORMAT = "yyyy-MM-dd";
        public static string ToFormatDate(this DateTime dateTime)
        {
            return dateTime.ToString(FECHA_FORMAT);
        }
    }
}
