using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Actividad_1_2.Models
{
    public class ApuestasRepository
    {
        private MySqlConnection Connect()
        {//establim la conexió
            string conString = "Server=localhost;Port=3306;Database=Apuestas;Uid=root";
            MySqlConnection con = new MySqlConnection(conString);
            return con;
        }

        internal List<eventosFutbol> Retrieve()
        {//fem un metod que mos retornara tots els partits
            MySqlConnection con = Connect();
            MySqlCommand command = con.CreateCommand();
            command.CommandText = "select * from partidos";

            try
            {
                con.Open();
                MySqlDataReader res = command.ExecuteReader();
                eventosFutbol partido = null;
                List<eventosFutbol> partidos = new List<eventosFutbol>();
                while (res.Read())
                {//mentres que encara hi haguen partits per llegir executara el bucle i añadira el partit
                    partido = new eventosFutbol(res.GetInt32(0), res.GetString(1),res.GetString(2));
                    partidos.Add(partido);
                }
                con.Close();//tanquem la conexió
                return partidos;
            }
            catch (MySqlException e)
            {
                return null;
            }
        }

        internal List<Apuestas> RetrieveID(int id)
        {//traguem en aquest metod totes les apostes de la id de un partit
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
                {//mentres que encara hi haguen partits per llegir executara el bucle i añadira les apostes per ixe partit
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
        internal void añadir(int id_partido,int cuota_over_under,int over_under, double dinero)
        {//agafem totes kes apostes de un partit per la id
            MySqlConnection con = Connect();
            MySqlCommand command = con.CreateCommand();
            command.CommandText = "select * from apuestas where id_partido=" + id_partido;
            

                con.Open();//obrim la conexio i llegim la base de dades
                MySqlDataReader res = command.ExecuteReader();
                List<Apuestas> apuestas = new List<Apuestas>();
                Apuestas ap = null;
                while (res.Read())
            {//mentres que encara hi haguen apostes per llegir executara el bucle i añadira les apostes per a ixe partit
                ap = new Apuestas(res.GetInt32(0), res.GetInt32(1), res.GetDouble(2), res.GetDouble(3), res.GetDouble(4), res.GetDouble(5), res.GetDouble(6));
                    apuestas.Add(ap);
                }
                 con.Close();//tanquem la conexió


            //declarem variables
            bool iguals = false;
            int id = 0;
            double dinero_over, dinero_under, probabilidad_under, probabilidad_over, cuota_over, cuota_under, cuota;
            dinero_over = 0;
            dinero_under = 0;
            cuota = 0;


            //fem un foreach
            foreach (var apuesta in apuestas)
            {//asi averiguem a quina cuota li hem clavat
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

                if (apuesta.id_partido == id_partido && iguals == false)
                {//comprobamos que la id del partido es la correcta
                    if (apuesta.over_under==cuota)
                    {//cogemos la apuesta de over o under
                        iguals = true;//ponemos el iguals a true pa que no torne a entrar
                        if (over_under == 1)
                        {//si es over le sumamos el dinero
                            dinero_over = apuesta.dinero_over + dinero;
                            dinero_under = apuesta.dinero_under;
                        }
                        else if (over_under == 2)
                        {//si es under ¡se lo sumamos a under
                            dinero_over = apuesta.dinero_over;
                            dinero_under = apuesta.dinero_under + dinero;
                        }
                    }
                    if (iguals == true)
                    {
                        id = apuesta.id_apuesta;

                        //hacemos los calculos
                        probabilidad_over = dinero_over / (dinero_over + dinero_under);
                        probabilidad_under = dinero_under / (dinero_over + dinero_under);

                        cuota_over = (1 / probabilidad_over) * 0.95;
                        cuota_under = (1 / probabilidad_under) * 0.95;

                        //asignamos los valores a las variables 
                        apuesta.cuota_over = cuota_over;
                        apuesta.cuota_under = cuota_under;
                        apuesta.dinero_over = dinero_over;
                        apuesta.dinero_under = dinero_under;

                        MySqlConnection con1 = Connect();//iniciamos la conexion
                        MySqlCommand command1 = con1.CreateCommand();
                        //ponemos la sentencia Sql para hacer el UPDATE
                        command1.CommandText = "UPDATE `apuestas`.`apuestas` SET `cuota_over`='" + apuesta.cuota_over.ToString().Replace(",", ".") + "', `cuota_under`='" + apuesta.cuota_under.ToString().Replace(",", ".") + "', `dinero_over`='" + apuesta.dinero_over.ToString().Replace(",", ".") + "', `dinero_under`='" + apuesta.dinero_under.ToString().Replace(",", ".") + "' WHERE  `ID_Apuesta`=" + id + ";";
                        con1.Open();
                        command1.ExecuteNonQuery();
                        con1.Close();//cerramos la conexion

                    }
                }
               
            }
           

        }


    }
}