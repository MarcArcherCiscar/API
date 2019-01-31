using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Actividad_1_2.Models
{
    public class PartidosRepository
    {
        private MySqlConnection Connect()
        {
            string conString = "Server=localhost;Port=3306;Database=Apuestas;Uid=root";
            MySqlConnection con = new MySqlConnection(conString);
            return con;
        }

        internal List<Partido> Retrieve()
        {
            MySqlConnection con = Connect();
            MySqlCommand command = con.CreateCommand();
            command.CommandText = "select * from partidos";

            try
            {
                con.Open();
                MySqlDataReader res = command.ExecuteReader();
                Partido partido = null;
                List<Partido> partidos = new List<Partido>();
                while (res.Read())
                {
                    partido = new Partido(res.GetInt32(0), res.GetString(1),res.GetString(2));
                    partidos.Add(partido);
                }
                con.Close();
                return partidos;
            }
            catch (MySqlException e)
            {
                return null;
            }
        }

        internal List<Apuestas> RetrieveID(int id)
        {
            MySqlConnection con = Connect();
            MySqlCommand command = con.CreateCommand();
            command.CommandText = "select * from apuestas where id_partido="+id;

            try
            {
                con.Open();
                MySqlDataReader res = command.ExecuteReader();
                List<Apuestas> apuestas = new List<Apuestas>();
                Apuestas ap = null;
                while (res.Read())
                {
                    ap = new Apuestas(res.GetInt32(0),res.GetInt32(1),res.GetDouble(2),res.GetDouble(3),res.GetDouble(4),res.GetDouble(5),res.GetDouble(6));
                    apuestas.Add(ap);
                }
                con.Close();
                return apuestas;
            }
            catch (MySqlException e)
            {
                return null;
            }
        }
        internal void putDinero(int id_partido,int cuota_over_under,int over_under, double dinero)
        {
            MySqlConnection con = Connect();
            MySqlCommand command = con.CreateCommand();
            command.CommandText = "select * from apuestas where id_partido=" + id_partido;
            

                con.Open();
                MySqlDataReader res = command.ExecuteReader();
                List<Apuestas> apuestas = new List<Apuestas>();
                Apuestas ap = null;
                while (res.Read())
                {
                    ap = new Apuestas(res.GetInt32(0), res.GetInt32(1), res.GetDouble(2), res.GetDouble(3), res.GetDouble(4), res.GetDouble(5), res.GetDouble(6));
                    apuestas.Add(ap);
                }
                 con.Close();

            bool temp = false;
            int id = 0;
            double dinero_over, dinero_under, probabilidad_under, probabilidad_over, cuota_over, cuota_under, cuota;
            dinero_over = 0;
            dinero_under = 0;
            cuota = 0;
            foreach (var apuesta in apuestas)
            {
                if (cuota_over_under == 1)
                {
                    cuota = 1.5;
                }
                else if (cuota_over_under == 2)
                {
                    cuota = 2.5;
                }
                else if (cuota_over_under == 3)
                {
                    cuota = 3.5;
                }

                if (apuesta.id_partido == id_partido && temp==false)
                {
                    if (apuesta.over_under==cuota)
                    {
                        temp = true;
                        if (over_under == 1)
                        {
                            dinero_over = apuesta.dinero_over + dinero;
                            dinero_under = apuesta.dinero_under;
                        }
                        else if (over_under == 2)
                        {
                            dinero_over = apuesta.dinero_over;
                            dinero_under = apuesta.dinero_under + dinero;
                        }
                    }
                    if (temp==true)
                    {
                        id = apuesta.id_apuesta;
                        probabilidad_over = dinero_over / (dinero_over + dinero_under);
                        probabilidad_under = dinero_under / (dinero_over + dinero_under);

                        cuota_over = (1 / probabilidad_over) * 0.95;
                        cuota_under = (1 / probabilidad_under) * 0.95;

                        apuesta.cuota_over = cuota_over;
                        apuesta.cuota_under = cuota_under;
                        apuesta.dinero_over = dinero_over;
                        apuesta.dinero_under = dinero_under;

                        MySqlConnection con1 = Connect();
                        MySqlCommand command1 = con1.CreateCommand();
                        command1.CommandText = "UPDATE `apuestas`.`apuestas` SET `cuota over`='" + apuesta.cuota_over.ToString().Replace(",", ".") + "', `cuota under`='" + apuesta.cuota_under.ToString().Replace(",", ".") + "', `dinero over`='" + apuesta.dinero_over.ToString().Replace(",", ".") + "', `dinero under`='" + apuesta.dinero_under.ToString().Replace(",", ".") + "' WHERE  `ID_Apuesta`=" + id + ";";
                        con1.Open();
                        command1.ExecuteNonQuery();
                        con1.Close();
                    }
                }
               
            }
           

        }


    }
}