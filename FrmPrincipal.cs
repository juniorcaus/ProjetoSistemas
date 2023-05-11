using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace ProjetoSistemas
{
    public partial class FrmPrincipal : Form
    {
        Conexao con  = new Conexao(); // "con" chama todos os metados da classe Conexao.cs, Lembrando q o "con" daqui é diferente do "con" dentro do Conexao.cs

        string sql;  //Varial para pode declarar em outros lugares usando apenas "sql = " e pega os valores dentro da string "sql"
        MySqlCommand cmd;

        
        string id; //Variavel que pega o id do registro


        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FormatarGD()
        {
            grid.Columns[0].HeaderText = "Código"; // "código" é o mesmo que "id"
            grid.Columns[1].HeaderText = "Nome";
            grid.Columns[2].HeaderText = "Endereço";
            grid.Columns[3].HeaderText = "CPF";
            grid.Columns[4].HeaderText = "Celular";
            grid.Columns[5].HeaderText = "Tel..";

            grid.Columns[0].Visible = false; //ocultando a coluna  0 que é o "código" ao exibir o banco de dados no grid
            grid.Columns[4].Visible = false;
        }


        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            // Carrega tudo ao abrir
            ListarGrid();
        }

        private void ListarGrid()
        {
            con.AbrirConexao();
            sql = "SELECT * FROM cliente ORDER BY NOME ASC"; // SELECIONAR TODOS ORDENADO POR NOME ACRESCENTE(A - Z) 
            cmd = new MySqlCommand(sql, con.con);
            MySqlDataAdapter da = new MySqlDataAdapter(); // instanciando tudo de "MySqlDataAdapter" para a variavel " da "
            da.SelectCommand = cmd; // recebendo oq esta dentro da variavel " cmd "
            DataTable dt = new DataTable();  // tudo que vem de "DataTable"  vai ser jogado em " dt "
            da.Fill(dt); //preencher um DataTable com os dados do banco de dados MySql
            grid.DataSource = dt; // defeinir a fonte de dados para qual o grid vai ser exibido q no caso é o " dt "
            con.FecharConexao();

            FormatarGD();
            
        }


        private void btnSalvar_Click(object sender, EventArgs e)
        {
          
            //se
            if(txtNome.Text.ToString().Trim() == "") // Se o texto que digitei no formulario "nome" estiver vazio vai dar error e abrir uma mensagem
            {
                MessageBox.Show("O campo Nome é obrigatório.");
                txtNome.Text = "";
                txtNome.Focus();
                return;
            }

            if (txtCPF.Text == "   .   .   -" || txtCPF.Text.Length <14) // Se o texto que digitei no formulario "nome" estiver vazio vai dar error e abrir uma mensagem
            {
                MessageBox.Show("O campo CPF é obrigatório.");
                txtCPF.Focus();
                return;
            }

            con.AbrirConexao(); // Essa função abre o metado de Conexão dentro da classe Conexao.cs
            //CRUD

            sql =  "INSERT INTO cliente (nome, endereco, cpf, telefone) VALUES (@nome, @endereco, @cpf, @telefone)";
            cmd = new MySqlCommand(sql, con.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text); //função para adicionar parametros com os valores ou seja vai buscar o valor q for digitado no campo "nome" do projeto
            cmd.Parameters.AddWithValue("@endereco", txtEnd.Text);
            cmd.Parameters.AddWithValue("cpf", txtCPF.Text);
            cmd.Parameters.AddWithValue("telefone", txtTel.Text);

            cmd.ExecuteNonQuery();
            con.FecharConexao();

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
            btnAlterar.Enabled = false;
            btnCancelar.Enabled = false;
            btnExcluir.Enabled = false;
            btnSalvar.Enabled = false;
        }

        private void HabilitarBotoes() //METADO PARA HABILITAR BOTOES
        {
            btnNovo.Enabled = true;
            btnSalvar.Enabled = true;
            btnAlterar.Enabled = true;
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

        private void btnAlterar_Click(object sender, EventArgs e)
        {

            //se
            if (txtNome.Text.ToString().Trim() == "") // Se o texto que digitei no formulario "nome" estiver vazio vai dar error e abrir uma mensagem
            {
                MessageBox.Show("O campo Nome é obrigatório.");
                txtNome.Text = "";
                txtNome.Focus();
                return;
            }

            if (txtCPF.Text == "   .   .   -" || txtCPF.Text.Length < 14) // Se o texto que digitei no formulario "nome" estiver vazio vai dar error e abrir uma mensagem
            {
                MessageBox.Show("O campo CPF é obrigatório.");
                txtCPF.Focus();
                return;
            }


            con.AbrirConexao(); // Essa função abre o metado de Conexão dentro da classe Conexao.cs
            //CRUD

            sql = "UPDATE cliente SET nome = @nome, endereco = @endereco, cpf = @cpf, telefone = @telefone WHERE id=@id";  //o código "WHERE id=@id" é para buscar a propriedade expecifica como 'nome, cpf, telefone etc..
            cmd = new MySqlCommand(sql, con.con);
            cmd.Parameters.AddWithValue("@id", id ); // com isso quando for alterar um determinado registro, só vai alterar o selecionado com "id"

            cmd.Parameters.AddWithValue("@nome", txtNome.Text); //função para adicionar parametros com os valores ou seja vai buscar o valor q for digitado no campo "nome" do projeto
            cmd.Parameters.AddWithValue("@endereco", txtEnd.Text);
            cmd.Parameters.AddWithValue("cpf", txtCPF.Text);
            cmd.Parameters.AddWithValue("telefone", txtTel.Text);

            cmd.ExecuteNonQuery();
            con.FecharConexao();

            //quando tiver dados
            LimparCampos();
            DesabalitarCampos();
            DesabilitarBotoes();
            btnNovo.Enabled = true; // PARA DEIXAR APENAS O BOTÃO "NOVO" ATIVADO
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            HabilitarBotoes();
            btnNovo.Enabled = false;
            btnSalvar.Enabled = false;
            HabilitarCampos();

            id= grid.CurrentRow.Cells[0].Value.ToString(); //vai pegar oque ta na posição "0" e jogar na variavel "id"

            txtNome.Text = grid.CurrentRow.Cells[1].Value.ToString(); //converte para texto tudo que vem da celula do grid e jogo para o "txtNome"
            txtEnd.Text = grid.CurrentRow.Cells[2].Value.ToString();
            txtCPF.Text = grid.CurrentRow.Cells[3].Value.ToString();
            txtTel.Text = grid.CurrentRow.Cells[5].Value.ToString();

        }
    }//FIM
}
 