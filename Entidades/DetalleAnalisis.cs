using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public string DescripcionTipoAnalisis { get; set; }
        [ForeignKey("TipoAnalisisID")]
        public virtual TipoAnalisis TipoAnalisis { get; set; }
        public string Resultado { get; set; }

        public DetalleAnalisis()
        {
            DetalleAnalisisID = 0;
            AnalisisID = 0;
            TipoAnalisisID = 0;
            DescripcionTipoAnalisis = string.Empty;
            Resultado = string.Empty;
        }

        public DetalleAnalisis(int detalleAnalisisID, int analisisID, int tipoAnalisisID, string descripcionTipoAnalisis, string resultado)
        {
            DetalleAnalisisID = detalleAnalisisID;
            AnalisisID = analisisID;
            TipoAnalisisID = tipoAnalisisID;
            DescripcionTipoAnalisis = descripcionTipoAnalisis ?? throw new ArgumentNullException(nameof(descripcionTipoAnalisis));
            Resultado = resultado ?? throw new ArgumentNullException(nameof(resultado));
        }
    }
}
