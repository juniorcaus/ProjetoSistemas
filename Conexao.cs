﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace ProjetoSistemas
{
    class Conexao
    {
        // CONEXÃO LOCAL ↓
        public string conec = "SERVER=localhost; DATABASE=aula; UID=root; PWD=; PORT=;"; //string de conexão


        //CONEXÃO REMOTO ↓
       // public string conec = "SERVER=mysql744.umbler.com; DATABASE=dbaula; UID=aula; PWD=j32447188; PORT=41890;"; //string de conexão remoto com a UMBLER.COM


        public MySqlConnection con = null;

        public void AbrirConexao() //abrir conexao
        {
            //testar 
            try
            {
                con = new MySqlConnection(conec);
                con.Open();
            }
            catch(Exception ex)
            {
                //error
                MessageBox.Show("Erro no Servidor! ==> " + ex.Message);
            }
        }

        public void FecharConexao() //fechar conexao
        {
            try
            {
                con = new MySqlConnection(conec);
                con.Close();
            }
            catch(Exception ex)
            {
                //error
                MessageBox.Show("Erro no Servidor! --> " + ex.Message);
            }
        }


    }

}
