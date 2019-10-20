using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    [Serializable]
    public class TipoAnalisis
    {
        [Key]
        public int TipoAnalisisID { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaRegistro { get; set; }
        public TipoAnalisis()
        {
            TipoAnalisisID = 0;
            Descripcion = string.Empty;
            Monto = 0;
            FechaRegistro = DateTime.Now;
        }
        public TipoAnalisis(int tipoAnalisisID, string descripcion, decimal monto, DateTime Fecha)
        {
            TipoAnalisisID = tipoAnalisisID;
            Monto = monto;
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            FechaRegistro = Fecha;
        }
    }
}
