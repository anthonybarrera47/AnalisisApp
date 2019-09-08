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
    public class DetalleAnalisis
    {
        [Key]
        public int DetalleAnalisisID { get; set; }
        public int AnalisisID { get; set; }
        [ForeignKey("AnalisisID")]
        public virtual Analisis Analisis { get; set; }
        public int TipoAnalisisID { get; set; }
        [ForeignKey("TipoAnalisisID")]
        public virtual TipoAnalisis TipoAnalisis { get; set; }
        public string Resultado { get; set; }

        public DetalleAnalisis()
        {
            DetalleAnalisisID = 0;
            AnalisisID = 0;
            TipoAnalisisID = 0;
            Resultado = string.Empty;
        }

        public DetalleAnalisis(int detalleAnalisisID, int analisisID, int tipoAnalisisID, string resultado)
        {
            DetalleAnalisisID = detalleAnalisisID;
            AnalisisID = analisisID;
            TipoAnalisisID = tipoAnalisisID;
            Resultado = resultado ?? throw new ArgumentNullException(nameof(resultado));
        }
    }
}
