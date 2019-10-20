using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades
{
    [Serializable]
    public class Pagos
    {
        [Key]
        public int PagosID { get; set; }
        public int PacienteID { get; set; }
        [NotMapped]
        public string NombrePaciente { get; set; }
        public virtual List<DetallesPagos> DetallesPagos { get; set; }
        [NotMapped]
        public decimal TotalPagado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Pagos()
        {
            PagosID = 0;
            PacienteID = 0;
            DetallesPagos = new List<DetallesPagos>();
            FechaRegistro = DateTime.Now;
            NombrePaciente = string.Empty;
            TotalPagado = 0;
        }
        public Pagos(int pagosID, int pacienteId, decimal totalPagado, List<DetallesPagos> detallesPagos)
        {
            PagosID = pagosID;
            PacienteID = pacienteId;
            NombrePaciente = string.Empty;
            DetallesPagos = detallesPagos ?? throw new ArgumentNullException(nameof(detallesPagos));
            FechaRegistro = DateTime.Now;
            TotalPagado = totalPagado;
        }
        public void AgregarDetalle(int DetallesPagoID, int PagosID, int AnalisisID, decimal Monto, string estado)
        {
            DetallesPagos.Add(new DetallesPagos(DetallesPagoID, PagosID, AnalisisID, Monto, estado));
        }
        public void RemoverDetalle(int Index)
        {
            this.DetallesPagos.RemoveAt(Index);
        }

    }
}
