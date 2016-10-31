﻿using ClinicaFrba.extras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicaFrba.Cancelar_Atencion
{
    public partial class CancelarAtencion : Form
    {
        public Sesion sesion;

        List<string> tipos_esp;
        Dictionary<string, int> tipos_esp_id;

        List<string> especialidades;
        Dictionary<string, int> especialidades_id;

        List<string> vacia;

        public CancelarAtencion(Sesion sesion)
        {
            InitializeComponent();
            this.sesion = sesion;

            tipos_esp = new List<string>();
            tipos_esp_id = new Dictionary<string, int>();

            especialidades = new List<string>();
            especialidades_id = new Dictionary<string, int>();

            vacia = new List<string>();
            
            llenarTipoEsp();
            llenarEspecialidades();

            this.dateTimePicker1.Value = DateTime.Parse(ConfigurationManager.AppSettings.Get("FechaSistema"));
            this.dateTimePicker2.Value = DateTime.Parse(ConfigurationManager.AppSettings.Get("FechaSistema"));

            comprobarSiEsProfesional();
        }

        private void comprobarSiEsProfesional()
        {
            if (this.sesion.rol_actual_id == 3)
            {
                this.textBoxNroAfiliado.Visible = false;
                this.label3.Visible = false;
            }
        }


        private void llenarTipoEsp()
        {
            try
            {
                this.tipos_esp_id.Clear();
                this.tipos_esp.Clear();

                get_tipo_esp();
                this.comboBoxTipoEsp.DataSource = tipos_esp;


            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Aviso", MessageBoxButtons.OK);
            }

        }

        private void llenarEspecialidades()
        {
            this.especialidades.Clear();
            this.especialidades_id.Clear();

            this.comboBoxEsp.DataSource = this.vacia;
            get_especialidades();
            this.comboBoxEsp.DataSource = this.especialidades;

        }

        private void get_tipo_esp()
        {
            string expresion = "SELECT tipo_esp_id, tipo_esp_descrip FROM NUL.Tipo_esp";

            SqlDataReader lector = DAL.Classes.DBHelper.ExecuteQuery_DR(expresion);

            if (lector != null)
            {
                tipos_esp_id.Add((string)lector["tipo_esp_descrip"].ToString(), int.Parse(lector["tipo_esp_id"].ToString()));
                tipos_esp.Add((string)lector["tipo_esp_descrip"].ToString());

                while (lector.Read())
                {
                    tipos_esp_id.Add((string)lector["tipo_esp_descrip"].ToString(), int.Parse(lector["tipo_esp_id"].ToString()));
                    tipos_esp.Add((string)lector["tipo_esp_descrip"].ToString());
                }
            }

        }

        private void get_especialidades()
        {
            string expresion = "SELECT esp_id, esp_descrip FROM NUL.Especialidad WHERE esp_tipo = " + this.tipos_esp_id[this.comboBoxTipoEsp.Text].ToString();

            SqlDataReader lector = DAL.Classes.DBHelper.ExecuteQuery_DR(expresion);

            if (lector != null)
            {
                especialidades_id.Add((string)lector["esp_descrip"].ToString(), int.Parse(lector["esp_id"].ToString()));
                especialidades.Add((string)lector["esp_descrip"].ToString());

                while (lector.Read())
                {
                    especialidades_id.Add((string)lector["esp_descrip"].ToString(), int.Parse(lector["esp_id"].ToString()));
                    especialidades.Add((string)lector["esp_descrip"].ToString());
                }
            }

        }


        private void comboBoxTipoEsp_SelectionChangeCommitted(object sender, EventArgs e)
        {
            llenarEspecialidades();

        }

        private void buttonVolver_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1(this.sesion);
            form.Show();
            this.Hide();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.sesion.rol_actual_id == 3)
                {
                    //cancelar todos los turnos del medico
                }

                else
                {

                }

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Aviso", MessageBoxButtons.OK);
            }
        }
    }
}
