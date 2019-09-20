using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    [Serializable]
    public class Pacientes
    {
        [Key]
        public int PacienteID { get; set; }
        public string Nombre { get; set; }
        public decimal Balance { get; set; }
        public DateTime Fecha { get; set; }

        public Pacientes()
        {
            PacienteID = 0;
            Nombre = string.Empty;
            Balance = 0;
            Fecha = DateTime.Now;
        }
        public Pacientes(int pacienteID, string nombre,decimal balance,DateTime fecha)
        {
            PacienteID = pacienteID;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Balance = balance;
            Fecha = fecha;
        }
    }
}
