using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoSistemas.Relatorio
{
    public partial class FrmCliente : Form
    {
        public FrmCliente()
        {
            InitializeComponent();
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            // TODO: esta linha de código carrega dados na tabela 'aulaDataSet.produtos'. Você pode movê-la ou removê-la conforme necessário.
            this.produtosTableAdapter.Fill(this.aulaDataSet.produtos);
            this.clienteTableAdapter.Fill(this.aulaDataSet.cliente);
            this.loginTableAdapter.Fill(this.aulaDataSet.login);


            this.reportViewer1.RefreshReport();
        }

    }
}
