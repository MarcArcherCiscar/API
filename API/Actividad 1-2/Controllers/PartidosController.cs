using Actividad_1_2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Actividad_1_2.Controllers
{
    public class PartidosController : ApiController
    {
        // GET: api/Partidos
        public IEnumerable<Partido> Get()
        {
            var repo = new PartidosRepository();
            List<Partido> partidos = repo.Retrieve();
            return partidos;
        }

        // GET: api/Partidos/5
        public IEnumerable<Apuestas> Get(int id)
        {
            var repo = new PartidosRepository();
            List<Apuestas> apuestas = repo.RetrieveID(id);
            return apuestas;
        }

        // POST: api/Partidos
        public void Post([FromBody]string value)
        {
        }

        //PUT api/Partidos?id_partido={id_partido}&cuota_under_over={1:1.5,2:2.5,3:3.5}&over_under={1:over,2:under}&dinero={dinero a apostar}
        public void Put(int id_partido, int cuota_under_over, int over_under, double dinero,[FromBody]string value)
        {
            Debug.WriteLine("Entro a put");
            var repo = new PartidosRepository();
            repo.putDinero(id_partido, cuota_under_over, over_under,dinero);
        }

        // DELETE: api/Partidos/5
        public void Delete(int id)
        {
        }
    }
}
