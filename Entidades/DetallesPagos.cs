using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    [Serializable]
    public class DetallesPagos
    {
        [Key]
        public int DetallePagoID { get; set; }
        public int PagosID { get; set; }
        public int AnalisisID { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        [ForeignKey("PagosID")]
        public virtual Pagos Pagos { get; set; }

        [ForeignKey("AnalisisID")]
        public virtual Analisis Analisis { get; set; }
        public DetallesPagos()
        {
            DetallePagoID = 0;
            PagosID = 0;
            AnalisisID = 0;
            Monto = 0;
            Estado = string.Empty;
        }
        public DetallesPagos(int detallePagoID, int pagosID, int analisisID, decimal monto,string estado)
        {
            DetallePagoID = detallePagoID;
            PagosID = pagosID;
            AnalisisID = analisisID;
            Monto = monto;
            Estado = estado;
        }
    }

}
