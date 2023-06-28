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
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }

        private void MenuCliente_Click(object sender, EventArgs e)
        {
            FrmCadastroCliente frm = new FrmCadastroCliente();
            frm.ShowDialog();
        }
    }
}
