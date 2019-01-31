using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Actividad_1_2.Models
{
    public class Partido
    {

        public int id { get; set; }
        public string equipo_local { get; set; }
        public string equipo_visitante { get; set; }

        public Partido(int id, string equipo_local, string equipo_visitante)
        {
            this.id = id;
            this.equipo_local = equipo_local;
            this.equipo_visitante = equipo_visitante;
        }

    }
}