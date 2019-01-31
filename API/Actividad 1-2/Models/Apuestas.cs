using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Actividad_1_2.Models
{
    public class Apuestas
    {
        public int id_apuesta { get; set; }
        public int id_partido { get; set; }
        public double over_under { get; set; }
        public double cuota_over { get; set; }
        public double cuota_under { get; set; }
        public double dinero_over { get; set; }
        public double dinero_under { get; set; }

        public Apuestas(int id_apuesta, int id_partido, double over_under, double cuota_over, double cuota_under, double dinero_over, double dinero_under)
        {
            this.id_apuesta = id_apuesta;
            this.id_partido = id_partido;
            this.over_under = over_under;
            this.cuota_over = cuota_over;
            this.cuota_under = cuota_under;
            this.dinero_over = dinero_over;
            this.dinero_under = dinero_under;
        }
    } 
}