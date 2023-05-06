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
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            //quando tiver dados
            LimparCampos();
            DesabalitarCampos();
            DesabilitarBotoes();
            btnNovo.Enabled = true; // PARA DEIXAR APENAS O BOTÃO "NOVO" ATIVADO

        
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            LimparCampos();
            DesabalitarCampos();
            DesabilitarBotoes();

            btnNovo.Enabled = true; // PARA DEIXAR APENAS O BOTÃO "NOVO" ATIVADO
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            DesabilitarBotoes();
            btnNovo.Enabled = true; // FUNÇÃO PARA DEIXAR APENAS O BOTÃO "NOVO" ATIVO, DEPOIS DE CLICAR EM "CANCELAR"
            LimparCampos();
            DesabalitarCampos(); 

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            HabilitarCampos();
            txtNome.Focus();
            LimparCampos();
            HabilitarBotoes();
            btnNovo.Enabled = false; //FUNÇÃO PARA DEIXAR APENAS O BOTÃO "NOVO" DESATIVATO, DEPOIS DE CLICAR NELE MESMO
            
         

        }
        /// <summary>
        /// 
        /// </summary>
        
        private void DesabilitarBotoes() //METADO DESABILITA BOTOES
        {
            btnNovo.Enabled = false;
            btnCancelar.Enabled = false;
            btnExcluir.Enabled = false;
            btnSalvar.Enabled = false;
        }

        private void HabilitarBotoes() //METADO PARA HABILITAR BOTOES
        {
            btnNovo.Enabled = true;
            btnSalvar.Enabled = true;
            btnExcluir.Enabled = true;
            btnCancelar.Enabled = true;
        }

        private void HabilitarCampos() //METADO PARA HABILATAR OS CAMPO DO FORMULÁRIO
        {
            txtNome.Enabled = true; // função para ao clicar em "novo", ativar o campo para ser preenchido.
            txtEnd.Enabled = true;
            txtCPF.Enabled = true;
            txtTel.Enabled = true;
        }

        private void DesabalitarCampos()  //METADO PARA DESABILITAR OS CAMPO DO FORMULÁRIO
        {
            txtNome.Enabled = false;    // função para ao clicar em "cancelar", DESATIVA OS CAMPOS DE SER PREENCHIDO
            txtEnd.Enabled = false;
            txtCPF.Enabled = false;
            txtTel.Enabled = false;
        }

        private void LimparCampos() //METADO PARA LIMPA O CAMPO
        {
            txtNome.Text = "";
            txtEnd.Text = "";
            txtCPF.Text = "";
            txtTel.Text = "";
        }


    }//FIM
}
