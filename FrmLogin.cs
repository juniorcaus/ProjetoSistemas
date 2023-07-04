using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoSistemas
{
    public partial class FrmLogin : Form
    {
        Conexao con = new Conexao(); // "con" chama todos os metados da classe Conexao.cs, Lembrando q o "con" daqui é diferente do "con" dentro do Conexao.cs
        

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            //login
            if (txtLogin.Text.ToString().Trim() == "" || txtTextoSenha.Text.ToString().Trim() =="")
            {
                MessageBox.Show("Digite seus dados");
                txtLogin.Text = "";
                txtLogin.Focus();
                return;
            }

            try
            {
                con.AbrirConexao();
                MySqlCommand cmdVerificar;
                MySqlDataReader reader; //ele vai extrair os dados da tabela
                cmdVerificar = new MySqlCommand("SELECT * FROM login WHERE nome=@nome AND senha=@senha", con.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmdVerificar;
                cmdVerificar.Parameters.AddWithValue("@nome", txtNome.Text);
                cmdVerificar.Parameters.AddWithValue("@senha", txtSenha.Text);
                reader = cmdVerificar.ExecuteReader();
                if(reader.HasRows)
                {
                    FrmMenu frm = new FrmMenu();
                    frm.ShowDialog();

                    this.Hide();
                }
                    
            }

            catch (Exception)
            {
                
            }


        }
    }
}
