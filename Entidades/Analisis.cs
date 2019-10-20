using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades
{
    [Serializable]
    public class Analisis
    {
        [Key]
        public int AnalisisID { get; set; }
        public int PacienteID { get; set; }
        [ForeignKey("PacienteID")]
        public virtual Pacientes Pacientes { get; set; }
        public decimal Balance { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaRegistro { get; set; }
        public virtual List<DetalleAnalisis> DetalleAnalisis { get; set; }
        public Analisis()
        {
            AnalisisID = 0;
            PacienteID = 0;
            Monto = 0;
            Balance = 0;
            DetalleAnalisis = new List<DetalleAnalisis>();
            FechaRegistro = DateTime.Now;
        }
        public Analisis(int analisisID, int pacienteID, Pacientes pacientes, decimal balance, decimal monto, DateTime fechaRegistro, List<DetalleAnalisis> detalleAnalisis)
        {
            AnalisisID = analisisID;
            PacienteID = pacienteID;
            Pacientes = pacientes ?? throw new ArgumentNullException(nameof(pacientes));
            Balance = balance;
            Monto = monto;
            FechaRegistro = fechaRegistro;
            DetalleAnalisis = detalleAnalisis ?? throw new ArgumentNullException(nameof(detalleAnalisis));
        }

        public void AgregarDetalle(int detalleAnalisisID, int analisisID, int tipoAnalisisID, string descripcion, string resultado)
        {
            this.DetalleAnalisis.Add(new DetalleAnalisis(detalleAnalisisID, analisisID, tipoAnalisisID, descripcion, resultado));
        }
        public void RemoverDetalle(int Index)
        {
            this.DetalleAnalisis.RemoveAt(Index);
        }
    }
}
