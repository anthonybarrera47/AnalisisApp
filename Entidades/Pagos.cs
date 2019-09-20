using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{   [Serializable]
    public class Pagos
    {
        [Key]
        public int PagosID { get; set; }
        public int PacienteID { get; set; }
        public virtual List<DetallesPagos> DetallesPagos { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Pagos()
        {
            PagosID = 0;
            DetallesPagos = new List<DetallesPagos>();
            FechaRegistro = DateTime.Now;
        }
        public Pagos(int pagosID, List<DetallesPagos> detallesPagos)
        {
            PagosID = pagosID;
            DetallesPagos = detallesPagos ?? throw new ArgumentNullException(nameof(detallesPagos));
            FechaRegistro = DateTime.Now;
        }
        public void AgregarDetalle(int DetallesPagoID,int PagosID,int AnalisisID,decimal Monto,string estado)
        {
            DetallesPagos.Add(new DetallesPagos(DetallesPagoID, PagosID, AnalisisID, Monto, estado));
        }
        public void RemoverDetalle(int Index)
        {
            this.DetallesPagos.RemoveAt(Index);
        }
    }
}
