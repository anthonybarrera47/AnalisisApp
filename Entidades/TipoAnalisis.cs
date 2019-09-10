using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    [Serializable]
    public class TipoAnalisis
    {
        [Key]
        public int TipoAnalisisID { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }

        public TipoAnalisis()
        {
            TipoAnalisisID = 0;
            Descripcion = string.Empty;
            FechaRegistro = DateTime.Now;
        }
        public TipoAnalisis(int tipoAnalisisID, string descripcion,DateTime Fecha)
        {
            TipoAnalisisID = tipoAnalisisID;
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            FechaRegistro = Fecha;
        }
    }
}
