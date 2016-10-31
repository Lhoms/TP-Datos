﻿using ClinicaFrba.extras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicaFrba.Listados
{
    public partial class Listados : Form
    {
        Sesion sesion;

        Dictionary<string, int> planes;
        List<string> plan_descrip;

        Dictionary<string, int> tipos_esp_id;
        List<string> tipos_esp;

        Dictionary<string, int> especialidades;
        List<string> esp_descrip;

        List<string> vacia;

        DataTable dt;

        int anio;
        int semestre;
        int mes;

        public Listados(Sesion sesion)
        {
            InitializeComponent();

            this.sesion = sesion;

            plan_descrip = new List<string>();
            planes = new Dictionary<string, int>();

            esp_descrip = new List<string>();
            especialidades = new Dictionary<string, int>();

            tipos_esp_id = new Dictionary<string, int>();
            tipos_esp = new List<string>();

            vacia = new List<string>();

            llenarTipoEsp();
            get_planes();
            llenarEspecialidades();

            this.comboBoxPlan.DataSource = plan_descrip;
            ocultarPlanEspecialidad();

        }

        private void llenarTipoEsp()
        {
            this.tipos_esp_id.Clear();
            this.tipos_esp.Clear();

            get_tipo_esp();
            this.comboBoxTipoEsp.DataSource = tipos_esp;

        }
        private void ocultarPlanEspecialidad()
        {
            this.labelPlan.Visible = false;
            this.comboBoxPlan.Visible = false;
            this.comboBoxTipoEsp.Visible = false;
            this.labelTipo.Visible = false;
            this.labelEspecialidad.Visible = false;
            this.comboBoxEspecialidad.Visible = false;
        }


        private void comboBoxListado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try{

                switch (this.comboBoxListado.Text)
                {

                    case "Top 5 de las especialidades que mas registrarion cancelaciones.":

                        ocultarPlanEspecialidad();
                        break;

                    case "Top 5 de los profesionales mas consultados por plan.":

                        ocultarPlanEspecialidad();
                        this.labelPlan.Visible = true;
                        this.comboBoxPlan.Visible = true;
                        break;

                    case "Top 5 de los profesionales con menos horas trabajadas por plan y especialidad.":

                        this.labelPlan.Visible = true;
                        this.comboBoxPlan.Visible = true;
                        this.comboBoxTipoEsp.Visible = true;
                        this.labelTipo.Visible = true;
                        this.labelEspecialidad.Visible = true;
                        this.comboBoxEspecialidad.Visible = true;
                        break;

                    case "Top 5 de los afiliados con mayor cantidad de bonos comprados.":

                        ocultarPlanEspecialidad();
                        break;

                    case "Top 5 de las especialidades de medicos con mas bonos de consultas utilizados.":

                        ocultarPlanEspecialidad();
                        break;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }

        private void get_top5_esp_cancel()
        {  
            SqlParameter[] dbParams = new SqlParameter[]
            {
                DAL.Classes.DBHelper.MakeParam("@anio", SqlDbType.Decimal, 0, this.anio),
                DAL.Classes.DBHelper.MakeParam("@semestre", SqlDbType.Decimal, 0, this.semestre),
                DAL.Classes.DBHelper.MakeParam("@mes", SqlDbType.Decimal, 0, this.mes),
            };

            this.dt = DAL.Classes.DBHelper.ExecuteDataSet("NUL.sp_get_top5_esp_cancel", dbParams).Tables[0];
        }

        private void get_top5_prof_consultados(int plan)
        {   
            SqlParameter[] dbParams = new SqlParameter[]
            {
                DAL.Classes.DBHelper.MakeParam("@anio", SqlDbType.Decimal, 0, this.anio),
                DAL.Classes.DBHelper.MakeParam("@semestre", SqlDbType.Decimal, 0, this.semestre),
                DAL.Classes.DBHelper.MakeParam("@mes", SqlDbType.Decimal, 0, this.mes),
                DAL.Classes.DBHelper.MakeParam("@plan_id", SqlDbType.Decimal, 0, plan),

            };

            this.dt = DAL.Classes.DBHelper.ExecuteDataSet("NUL.sp_get_top5_prof_consultados", dbParams).Tables[0];
        }

        private void get_top5_prof_horas(int plan, int especialidad)
        {
            SqlParameter[] dbParams = new SqlParameter[]
            {
                DAL.Classes.DBHelper.MakeParam("@anio", SqlDbType.Decimal, 0, this.anio),
                DAL.Classes.DBHelper.MakeParam("@semestre", SqlDbType.Decimal, 0, this.semestre),
                DAL.Classes.DBHelper.MakeParam("@mes", SqlDbType.Decimal, 0, this.mes),
                DAL.Classes.DBHelper.MakeParam("@plan_id", SqlDbType.Decimal, 0, plan),
                DAL.Classes.DBHelper.MakeParam("@esp_id", SqlDbType.Decimal, 0, especialidad),
            };

            this.dt = DAL.Classes.DBHelper.ExecuteDataSet("NUL.sp_get_top5_prof_horas", dbParams).Tables[0];
        }

        private void get_top5_afil_bonos()
        {
            SqlParameter[] dbParams = new SqlParameter[]
            {
                DAL.Classes.DBHelper.MakeParam("@anio", SqlDbType.Decimal, 0, this.anio),
                DAL.Classes.DBHelper.MakeParam("@semestre", SqlDbType.Decimal, 0, this.semestre),
                DAL.Classes.DBHelper.MakeParam("@mes", SqlDbType.Decimal, 0, this.mes),
            };

            this.dt = DAL.Classes.DBHelper.ExecuteDataSet("NUL.sp_get_top5_afil_bonos", dbParams).Tables[0];
        }


        private void get_top5_esp_bonos()
        {
            SqlParameter[] dbParams = new SqlParameter[]
            {
                DAL.Classes.DBHelper.MakeParam("@anio", SqlDbType.Decimal, 0, this.anio),
                DAL.Classes.DBHelper.MakeParam("@semestre", SqlDbType.Decimal, 0, this.semestre),
                DAL.Classes.DBHelper.MakeParam("@mes", SqlDbType.Decimal, 0, this.mes),
            };

            this.dt = DAL.Classes.DBHelper.ExecuteDataSet("NUL.sp_get_top5_esp_bonos", dbParams).Tables[0];
        }

        private void get_planes()
        {
            string expresion = "SELECT plan_id, plan_descrip FROM NUL.Plan_medico";

            SqlDataReader lector = DAL.Classes.DBHelper.ExecuteQuery_DR(expresion);

            if (lector.HasRows)
            {
                planes.Add((string)lector["plan_descrip"].ToString(), int.Parse(lector["plan_id"].ToString()));
                plan_descrip.Add((string)lector["plan_descrip"].ToString());

                while (lector.Read())
                {
                    planes.Add((string)lector["plan_descrip"].ToString(), int.Parse(lector["plan_id"].ToString()));
                    plan_descrip.Add((string)lector["plan_descrip"].ToString());
                }
            }

        }

        private void buttonVolver_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1(this.sesion);
            form.Show();
            this.Close();
        }

        private void buttonMostrar_Click(object sender, EventArgs e)
        {
            this.anio = (int)this.numericAño.Value;
            this.semestre = (int)this.numericSemestre.Value;
            this.mes = (int)this.numericMes.Value;

            switch (this.comboBoxListado.Text)
            {

                case "Top 5 de las especialidades que mas registrarion cancelaciones.":

                    get_top5_esp_cancel();
                    break;

                case "Top 5 de los profesionales mas consultados por plan.":

                    get_top5_prof_consultados(this.planes[this.comboBoxPlan.Text]);
                    break;

                case "Top 5 de los profesionales con menos horas trabajadas por plan y especialidad.":

                    get_top5_prof_horas(this.planes[this.comboBoxPlan.Text], especialidades[this.comboBoxEspecialidad.Text]);
                    break;

                case "Top 5 de los afiliados con mayor cantidad de bonos comprados.":

                    get_top5_afil_bonos();
                    break;

                case "Top 5 de las especialidades de medicos con mas bonos de consultas utilizados.":

                    get_top5_esp_bonos();
                    break;
            }

            this.dataGridTop5.DataSource = this.dt;

        }

        private void comboBoxTipoEsp_SelectionChangeCommitted(object sender, EventArgs e)
        {
            llenarEspecialidades();
        }

        private void llenarEspecialidades()
        {
            this.esp_descrip.Clear();
            this.especialidades.Clear();

            this.comboBoxEspecialidad.DataSource = this.vacia;
            get_especialidades();
            this.comboBoxEspecialidad.DataSource = this.esp_descrip;

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
                especialidades.Add((string)lector["esp_descrip"].ToString(), int.Parse(lector["esp_id"].ToString()));
                esp_descrip.Add((string)lector["esp_descrip"].ToString());

                while (lector.Read())
                {
                    especialidades.Add((string)lector["esp_descrip"].ToString(), int.Parse(lector["esp_id"].ToString()));
                    esp_descrip.Add((string)lector["esp_descrip"].ToString());
                }
            }

        }
    }
}
