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

using System.IO;

namespace ProjetoSistemas
{
    public partial class FrmPrincipal : Form
    {
        Conexao con  = new Conexao(); // "con" chama todos os metados da classe Conexao.cs, Lembrando q o "con" daqui é diferente do "con" dentro do Conexao.cs

        string sql;  //Varial para pode declarar em outros lugares usando apenas "sql = " e pega os valores dentro da string "sql"
        MySqlCommand cmd;

        
        string id; //Variavel que pega o id do registro

        string foto;  // Variavel que vai receber a imagem (criado globalmente, para poder ser usado em qualquer lugar do código).

        string alterouFoto = "nao";

        string cpfAntigo;


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
            grid.Columns[6].HeaderText = "foto";

            grid.Columns[0].Visible = false; //ocultando a coluna  0 que é o "código" ao exibir o banco de dados no grid
            grid.Columns[4].Visible = false;
            grid.Columns[6].Visible = false;
        }


        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            // Carrega tudo ao abrir
            LimparFoto();
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

            sql =  "INSERT INTO cliente (nome, endereco, cpf, telefone, imagem) VALUES (@nome, @endereco, @cpf, @telefone, @imagem)";
            cmd = new MySqlCommand(sql, con.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text); //função para adicionar parametros com os valores ou seja vai buscar o valor q for digitado no campo "nome" do projeto
            cmd.Parameters.AddWithValue("@endereco", txtEnd.Text);
            cmd.Parameters.AddWithValue("cpf", txtCPF.Text);
            cmd.Parameters.AddWithValue("telefone", txtTel.Text);
            cmd.Parameters.AddWithValue("imagem", img()); //METADO DE img()

           
            MySqlCommand cmdVerificar; //verificar se cpf já existe
            cmdVerificar = new MySqlCommand("SELECT * FROM cliente WHERE cpf=@cpf", con.con); // os "con" servem para instanciar e abrir conexao
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmdVerificar;
            cmdVerificar.Parameters.AddWithValue("@cpf", txtCPF.Text);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count > 0 ) // se o "dt" encontrar algum registro, vai retornar um error, dizendo q já existe esse cpf
            {
                MessageBox.Show("CPF JÁ CADASTRADO !!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtCPF.Text = "";
                txtCPF.Focus();
                return; // com esse return ele para aqui, e n executa os códigos abaixo, como o " con.FecharConexao();" por exeemplo
            }


            cmd.ExecuteNonQuery();
            con.FecharConexao();

            //quando tiver dados
            LimparCampos();
            DesabalitarCampos();
            DesabilitarBotoes();
          
            btnNovo.Enabled = true; // PARA DEIXAR APENAS O BOTÃO "NOVO" ATIVADO

            LimparFoto();
            ListarGrid(); // ESSE METADO ATUALIZA A GRID 


            MessageBox.Show("Registro Salvo com Sucesso !!!", "Salvar",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {

           var resposta = MessageBox.Show("Deseja realmente excluir esse registro?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question); //transformou essa "MessageBox" na variavel "resposta"

            if (resposta == DialogResult.Yes) //fazer a pergunta de SIM ou NAO, E SE FOR "SIM" VAI TER O RESULTADO DENTRO DO "IF"
            {
                con.AbrirConexao(); // Essa função abre o metado de Conexão dentro da classe Conexao.cs
                                    
                //CRUD: ↓
                // CREAT = INSERT, READ = SELECT, UPDATE = UPDATE, DELETE = DELETE

                sql = "DELETE FROM cliente WHERE id=@id"; // O "id" é uma variavel criado mais acima no código e o " @id " é o id vindo do banco de dados
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", id);  // o "id" recebe um valor quando clica na Grid
               

                cmd.ExecuteNonQuery();
                con.FecharConexao();


                LimparCampos();
                DesabalitarCampos();
                DesabilitarBotoes();
                btnNovo.Enabled = true; // PARA DEIXAR APENAS O BOTÃO "NOVO" ATIVADO

                ListarGrid(); //Para atualizar
                MessageBox.Show("Registro Apagado com Sucesso !!!", "Excluir", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }   

           
            
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            DesabilitarBotoes();
            DesabalitarCampos(); 
            LimparCampos();
            btnNovo.Enabled = true; // FUNÇÃO PARA DEIXAR APENAS O BOTÃO "NOVO" ATIVO, DEPOIS DE CLICAR EM "CANCELAR"

            ListarGrid();
            alterouFoto = "nao";

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            HabilitarCampos();
            LimparCampos();
            LimparFoto();
            txtNome.Focus();
            HabilitarBotoes();

            btnNovo.Enabled = false; //FUNÇÃO PARA DEIXAR O BOTÃO "NOVO" DESATIVATO, DEPOIS DE CLICAR NELE MESMO
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            
         

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

            if(alterouFoto == "sim") // se a string "alterouFoto for "sim" ou seja se a foto vai ser alterada, vai executar as funções abaixo: 
            {
                sql = "UPDATE cliente SET nome = @nome, endereco = @endereco, cpf = @cpf, telefone = @telefone, imagem = @imagem  WHERE id=@id";  //o código "WHERE id=@id" é para buscar a propriedade expecifica como 'nome, cpf, telefone etc..

                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", id); // com isso quando for alterar um determinado registro, só vai alterar o selecionado com "id"

                cmd.Parameters.AddWithValue("@nome", txtNome.Text); //função para adicionar parametros com os valores ou seja vai buscar o valor q for digitado no campo "nome" do projeto
                cmd.Parameters.AddWithValue("@endereco", txtEnd.Text);
                cmd.Parameters.AddWithValue("cpf", txtCPF.Text);
                cmd.Parameters.AddWithValue("telefone", txtTel.Text);
                cmd.Parameters.AddWithValue("imagem", img());
            } 
            
            else if(alterouFoto == "nao")  //se for "não" vai executar as função abaixo sem o parametro "imagem"
            {
                sql = "UPDATE cliente SET nome = @nome, endereco = @endereco, cpf = @cpf, telefone = @telefone  WHERE id=@id";  //o código "WHERE id=@id" é para buscar a propriedade expecifica como 'nome, cpf, telefone etc..

                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", id); // com isso quando for alterar um determinado registro, só vai alterar o selecionado com "id"

                cmd.Parameters.AddWithValue("@nome", txtNome.Text); //função para adicionar parametros com os valores ou seja vai buscar o valor q for digitado no campo "nome" do projeto
                cmd.Parameters.AddWithValue("@endereco", txtEnd.Text);
                cmd.Parameters.AddWithValue("cpf", txtCPF.Text);
                cmd.Parameters.AddWithValue("telefone", txtTel.Text);
            }

            if(txtCPF.Text != cpfAntigo)
            {

                MySqlCommand cmdVerificar; //verificar se cpf já existe
                cmdVerificar = new MySqlCommand("SELECT * FROM cliente WHERE cpf=@cpf", con.con); // os "con" servem para instanciar e abrir conexao
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmdVerificar;
                cmdVerificar.Parameters.AddWithValue("@cpf", txtCPF.Text);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0) // se o "dt" encontrar algum registro, vai retornar um error, dizendo q já existe esse cpf
                {
                    MessageBox.Show("CPF JÁ CADASTRADO !!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtCPF.Text = "";
                    txtCPF.Focus();
                    return; // com esse return ele para aqui, e n executa os códigos abaixo, como o " con.FecharConexao();" por exeemplo
                 }

            }

            cmd.ExecuteNonQuery();
            con.FecharConexao();

            //quando tiver dados
            LimparCampos();
            DesabalitarCampos();
            DesabilitarBotoes();
            btnNovo.Enabled = true; // PARA DEIXAR APENAS O BOTÃO "NOVO" ATIVADO



            ListarGrid(); // ESSE METADO ATUALIZA A GRID 
            LimparFoto();

            MessageBox.Show("Registro Alterado com Sucesso !!!", "Alterar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex > -1) 
            {
                LimparFoto();

                HabilitarBotoes();
                btnNovo.Enabled = false;
                btnSalvar.Enabled = false;
                HabilitarCampos();

                alterouFoto = "nao";

                id = grid.CurrentRow.Cells[0].Value.ToString(); //vai pegar oque ta na posição "0" e jogar na variavel "id"

                txtNome.Text = grid.CurrentRow.Cells[1].Value.ToString(); //converte para texto tudo que vem da celula do grid e jogo para o "txtNome"
                txtEnd.Text = grid.CurrentRow.Cells[2].Value.ToString();
                txtCPF.Text = grid.CurrentRow.Cells[3].Value.ToString();
                txtTel.Text = grid.CurrentRow.Cells[5].Value.ToString();

                cpfAntigo = txtCPF.Text = grid.CurrentRow.Cells[3].Value.ToString();


                // PEGAR A FOTO
                if (grid.CurrentRow.Cells[6].Value != DBNull.Value) // for diferente de nulo(ou seja CONTEM Imagem) vai lá e pega a imagem, se não vai colocar a imagem padrão
                {
                    byte[] imagem = (byte[])grid.Rows[e.RowIndex].Cells[6].Value; //essa função cria uma variavel byte imagem para já receber convertido em byte oque vem da grid 
                    MemoryStream ms = new MemoryStream(imagem); // vai receber a variavel byte que já tem o valor da grid convertido

                    image.Image = System.Drawing.Image.FromStream(ms);
                }
                else
                {
                    image.Image = Properties.Resources.photo; //se não escolher a imagem, vai selecionar a imagem "photo" padrão
                }

            }
            else //CASO NÃO TENHA DADOS NA GRID COM NOME, CPF ETC.. VAI DA RETURN PARA N DAR ERROR

            {
                return;
            }

            


        }

        
        private void BuscarNome() //Metado buscar pelo nome
        {
            con.AbrirConexao(); // abrir conexão
            sql = "SELECT * FROM cliente WHERE nome LIKE @nome ORDER BY nome ASC"; //LIKE , BUSCA O NOME POR APROXIMAÇÃO 
            cmd = new MySqlCommand(sql, con.con);
            cmd.Parameters.AddWithValue("@nome", txtBuscar.Text + "%"); // OPERADOR LIKE, BUSCA POR APROXIMAÇÃO;

            MySqlDataAdapter da = new MySqlDataAdapter(); // instanciando tudo de "MySqlDataAdapter" para a variavel " da "
            da.SelectCommand = cmd; // recebendo oq esta dentro da variavel " cmd "
            DataTable dt = new DataTable();  // tudo que vem de "DataTable"  vai ser jogado em " dt "
            da.Fill(dt); //preencher um DataTable com os dados do banco de dados MySql
            grid.DataSource = dt; // defeinir a fonte de dados para qual o grid vai ser exibido q no caso é o " dt "
            con.FecharConexao();

            FormatarGD();

        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarNome();
        }

        private void btnImg_Click(object sender, EventArgs e)
        {
            alterouFoto = "sim";
            OpenFileDialog dialog = new OpenFileDialog(); // jogando todos os recursos do OpenFileDialog para a varivael " dialog "
            dialog.Filter = "Imagens(*.jpg; *.png) | *,jpg; *.png"; // com isso vai apenas mostrar arquivos com formatos jpg e png
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foto = dialog.FileName.ToString();  //Pega o caminho da img selecionada 
                image.ImageLocation = foto;  // JOGAR O CAMINHO DA IMAGEM, PARA A PICTUREBOX

                alterouFoto = "sim"; // quando abrir e escolher uma foto
            }

            else
            {
                alterouFoto = "nao"; //quando não escolher uma foto ou abrir para adicionar uma foto e clicar em "cancelar"
            }
        }

        private byte[] img() //Metado para enviar imagem ao banco de dados
        {
            byte[] imagem_byte = null;

            if(foto == "") // se o "foto" que é o caminho, for vazio, retorna nulo
            {
                return null;
            }

            FileStream fs = new FileStream(foto, FileMode.Open, FileAccess.Read); // Essas são funções Padrão 'É criada uma instância de FileStream para abrir o arquivo de imagem especificado pela variável'

            BinaryReader br = new BinaryReader(fs); //Adicionando o "fs" dentro do new "binaryReader" é o mesmo que adicionar todo a variavel "FileStream fs" da linha 326

            imagem_byte = br.ReadBytes((int)fs.Length); //pegar o comprimento do FileStream dentro de um tipo Imagem_Byte | e o " ((int))fs.lenght)" força ser um inteiro 

            return imagem_byte;

        }

        private void LimparFoto() //METADO LIMPAR FOTO
        {
            image.Image = Properties.Resources.photo; // componente "image". com a propriedade "Image" vai buscar a foto de perfil "photo"
            foto = "ft/photo.png";      // "foto" é uma variavel tipo string que tem o caminho da imagem 
        }







    }//FIM
}
 