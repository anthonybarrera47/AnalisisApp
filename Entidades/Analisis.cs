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
    public class Analisis
    {
        [Key]
        public int AnalisisID { get; set; }
        public int PacienteID { get; set; }
        [ForeignKey("PacienteID")]
        public virtual Pacientes Pacientes { get; set; }
        public virtual List<DetalleAnalisis> DetalleAnalisis { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Analisis()
        {
            AnalisisID = 0;
            PacienteID = 0;
            DetalleAnalisis = new List<DetalleAnalisis>();
            FechaRegistro = DateTime.Now;
        }
        public Analisis(int analisisID, int pacienteID, List<DetalleAnalisis> detalleAnalisis, DateTime fechaRegistro)
        {
            AnalisisID = analisisID;
            PacienteID = pacienteID;
            DetalleAnalisis = detalleAnalisis ?? throw new ArgumentNullException(nameof(detalleAnalisis));
            FechaRegistro = fechaRegistro;
        }
        public void AgregarDetalle(int detalleAnalisisID, int analisisID, int tipoAnalisisID, string resultado)
        {
            this.DetalleAnalisis.Add(new DetalleAnalisis(detalleAnalisisID, analisisID, tipoAnalisisID, resultado));
        }
        public void RemoverDetalle(int Index)
        {
            this.DetalleAnalisis.RemoveAt(Index);
        }
    }
}
