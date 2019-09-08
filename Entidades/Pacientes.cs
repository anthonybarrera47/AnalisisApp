using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Pacientes
    {
        public int PacienteID { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }

        public Pacientes()
        {
            PacienteID = 0;
            Nombre = string.Empty;
            Fecha = DateTime.Now;
        }
        public Pacientes(int pacienteID, string nombre, DateTime fecha)
        {
            PacienteID = pacienteID;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Fecha = fecha;
        }
    }
}
