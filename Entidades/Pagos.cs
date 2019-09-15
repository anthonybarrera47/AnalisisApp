using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{   [Serializable]
    public class Pagos
    {
        public int PagosID { get; set; }
        public int AnalisisID { get; set; }
        public virtual List<DetallesPagos> DetallesPagos { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Pagos()
        {
            PagosID = 0;
            AnalisisID = 0;
            DetallesPagos = new List<DetallesPagos>();
            FechaRegistro = DateTime.Now;
        }
        public Pagos(int pagosID, int analisisID, List<DetallesPagos> detallesPagos)
        {
            PagosID = pagosID;
            AnalisisID = analisisID;
            DetallesPagos = detallesPagos ?? throw new ArgumentNullException(nameof(detallesPagos));
            FechaRegistro = DateTime.Now;
        }
        public void AgregarDetalle(int DetallesPagoID,int PagosID,decimal Monto)
        {
            DetallesPagos.Add(new DetallesPagos(DetallesPagoID, PagosID, Monto));
        }
    }
}
