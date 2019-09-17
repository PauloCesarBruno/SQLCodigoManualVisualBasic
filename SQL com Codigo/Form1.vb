' Nesse Projeto Vai se criando Privates Subs e carregando no botão principal...
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Printing
Public Class Form1
    Dim cmd As SqlCommand
    Private paginaAtual As Integer = 1
    Private MyConnection As SqlConnection
    Private Leitor As SqlDataReader
    Private RelatorioTitulo As String
    Private Sub PCarregaSQL() ' Sub para carregar os dados da tabela Controlado pelo (PCarregaSQL())
        ' O (PCarregaSQL()) colocado no botão carregar carrega meus dados do DGV.
        Using con As SqlConnection = GetconnectionSql()
            Try
                con.Open()
                Dim sql As String = "SELECT * FROM tblClientes"
                Dim cmd As SqlCommand = New SqlCommand(sql, con)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                DataGridView1.DataSource = dt
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub



    Private Sub PSomaProdutos() ' Sub para somar Valores de colunas de um DGV, no meu caso 
        ' estou somando a coluna (4) do meu DGV, onde consta os valores
        ' O (PSomaProdutos()) colocado no botao carregar soma os valores da cokuna (4) ao
        ' carregar o DGV.
        Dim linha As DataGridViewRow
        Dim valor As Double
        For Each linha In DataGridView1.Rows
            valor = valor + linha.Cells(4).Value
        Next
        Label1.Text = FormatCurrency(valor)
    End Sub

    Public Sub PFormataGridView() ' Sub Para formatar Certos parámetros do DGV, como alinhamento, formatar valor p/moeda etc ...
        Try

            With DataGridView1
                .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' Centraliza Valores
                .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' Centraliza
                .Columns(4).DefaultCellStyle.Format = "c" ' Formata coluna 4 do DataGridView para moeda R$
                .Columns(0).HeaderText = "Código" ' Muda o Texto do Cabeçalho
                .Columns(3).HeaderText = "Nascimento" ' Muda o Texto do Cabeçalho
                .Columns(4).HeaderText = "Limite Cred." ' Muda o Texto do Cabeçalho

            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using con As SqlConnection = GetconnectionSql()
            Try
                Dim sql As String = "insert into tblClientes (Nome,CPF,DataNascimento,LimiteDeCredito) values ('" & NomeTextBox.Text & "','" & CpfMaskedtextbox.Text & "', '" & DataNascimentoTextBox.Text & "', '" & LimiteDeCreditoTextBox.Text.Replace(",", ".") & "')"
                Dim cmd As SqlCommand = New SqlCommand(sql, con)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                DataGridView1.DataSource = dt
            Catch
                MsgBox("ERRO !!! Detalhe: CPF já Cadastrado, favor verificar.", vbCritical)
                IdClienteTextBox.Text = ""
                NomeTextBox.Text = ""
                CpfMaskedtextbox.Text = ""
                DataNascimentoTextBox.Text = ""
                LimiteDeCreditoTextBox.Text = ""
                NomeTextBox.Focus()

            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PCarregaSQL()
        PSomaProdutos()
        PFormataGridView()
        TextBox1.Text = ""
        NomeTextBox.Text = ""
        CpfMaskedtextbox.Text = ""
        DataNascimentoTextBox.Text = ""
        LimiteDeCreditoTextBox.Text = ""
        IdClienteTextBox.Text = ""
        NomeTextBox.Focus()
        Label1.Visible = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Application.Exit()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Using con As SqlConnection = GetconnectionSql()
            Try
                con.Open()
                Dim sql As String = "UPDATE tblClientes SET Nome = '" & NomeTextBox.Text & "' WHERE IdCliente = '" & IdClienteTextBox.Text & "'" + "UPDATE tblClientes SET CPF = '" & CpfMaskedtextbox.Text & "' WHERE IdCliente = '" & IdClienteTextBox.Text & "'" + "UPDATE tblClientes SET DataNascimento = '" & DataNascimentoTextBox.Text & "' WHERE IdCliente = '" & IdClienteTextBox.Text & "'" + "UPDATE tblClientes SET LimiteDeCredito = '" & LimiteDeCreditoTextBox.Text.Replace(",", ".") & "' WHERE IdCliente = '" & IdClienteTextBox.Text & "'"
                Dim cmd As SqlCommand = New SqlCommand(sql, con)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                DataGridView1.DataSource = dt
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub PBusca()
        Using con As SqlConnection = GetconnectionSql()
            Try
                con.Open()
                Dim sql As String = "SELECT IdCliente,Nome,CPF,DataNascimento,LimiteDeCredito FROM tblClientes WHERE Nome LIKE '%" & TextBox1.Text & "%'"
                Dim cmd As SqlCommand = New SqlCommand(sql, con)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                DataGridView1.DataSource = dt

                ' preenche os textbox´s de acordo com a linha do DGV pesquisada ----------------
                IdClienteTextBox.Text = DataGridView1.CurrentRow.Cells(0).Value.ToString()
                NomeTextBox.Text = DataGridView1.CurrentRow.Cells(1).Value.ToString()
                CpfMaskedtextbox.Text = DataGridView1.CurrentRow.Cells(2).Value.ToString()
                DataNascimentoTextBox.Text = DataGridView1.CurrentRow.Cells(3).Value.ToString()

                '  Cast Para Moeda Nacional Brasil.
                txtCast.Text = String.Format("{0:N}", DataGridView1.CurrentRow.Cells(4).Value)
                LimiteDeCreditoTextBox.Text = txtCast.Text
                '---------------------------------------------------------------------------------

            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using

    End Sub
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try

            Dim index As Integer
            index = e.RowIndex
            Dim selectedRow As DataGridViewRow
            selectedRow = DataGridView1.Rows(index)
            IdClienteTextBox.Text = selectedRow.Cells(0).Value.ToString
            NomeTextBox.Text = selectedRow.Cells(1).Value.ToString
            CpfMaskedtextbox.Text = selectedRow.Cells(2).Value.ToString
            DataNascimentoTextBox.Text = selectedRow.Cells(3).Value.ToString

            'Cast Para Moeda Nacional Brasil.
            txtCast.Text = String.Format("{0:N}", selectedRow.Cells(4).Value)
            LimiteDeCreditoTextBox.Text = txtCast.Text
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Label1.Visible = False
        PBusca()
        Button4.Enabled = True
        Button5.Enabled = True
        Button1.Enabled = False
        If TextBox1.Text = "" Then
            Button4.Enabled = False
            Button5.Enabled = False
            IdClienteTextBox.Text = ""
            NomeTextBox.Text = ""
            CpfMaskedtextbox.Text = ""
            DataNascimentoTextBox.Text = ""
            LimiteDeCreditoTextBox.Text = ""
        End If

        ' A Rotina abaixo impede que os textbox´s fiquem preeenchidos e com risco de ser deletado registro 
        ' caso não for encontrado nada na pesquisa.

        Dim p As Boolean
        p = DataGridView1.RowCount
        If p = False Then
            Button4.Enabled = False
            Button5.Enabled = False
            IdClienteTextBox.Text = ""
            NomeTextBox.Text = ""
            CpfMaskedtextbox.Text = ""
            DataNascimentoTextBox.Text = ""
            LimiteDeCreditoTextBox.Text = ""
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Using con As SqlConnection = GetconnectionSql()
            Try
                con.Open()
                Dim sql As String = "SELECT * FROM tblClientes"
                Dim cmd As SqlCommand = New SqlCommand(sql, con)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                DataGridView1.DataSource = dt
                PFormataGridView()
                PSomaProdutos()
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub NomeTextBox_TextChanged(sender As Object, e As EventArgs) Handles NomeTextBox.TextChanged
        Button1.Enabled = True
        If NomeTextBox.Text = "" Then Button1.Enabled = False

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim questao As Integer
        questao = MessageBox.Show("Deseja Realmente Apagar este Registro ???", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If questao = vbNo Then
            GoTo Line1
        ElseIf MessageBox.Show("Registro Apagado com êxito !", "Confirmação", MessageBoxButtons.OK, MessageBoxIcon.Information) Then

        Else

        End If
        Using con As SqlConnection = GetconnectionSql()
            Try
                con.Open()
                Dim sql As String = "DELETE FROM tblClientes WHERE IdCliente='" & IdClienteTextBox.Text & "'"
                Dim cmd As SqlCommand = New SqlCommand(sql, con)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                DataGridView1.DataSource = dt
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
Line1:
        PCarregaSQL()
        TextBox1.Text = ""
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        'obtem a string de conexao
        MyConnection = New SqlConnection("Data Source=LAPTOP-PP6GUSQU;Initial Catalog=DBCliente;User ID=sa;Password=Paradoxo22")

        'define o titulo do relatorio
        RelatorioTitulo = "Lista de Clientes - "

        'define os objetos printdocument e os eventos associados
        Dim pd As Printing.PrintDocument = New Printing.PrintDocument()

        'IMPORTANTE - definimos 3 eventos para tratar a impressão : PringPage, BeginPrint e EndPrint.
        AddHandler pd.PrintPage, New Printing.PrintPageEventHandler(AddressOf Me.pdRelatorios_PrintPage)
        AddHandler pd.BeginPrint, New Printing.PrintEventHandler(AddressOf Me.Begin_Print)
        AddHandler pd.EndPrint, New Printing.PrintEventHandler(AddressOf Me.End_Print)

        'define o objeto para visualizar a impressao
        Dim objPrintPreview As New PrintPreviewDialog

        Try
            'define o formulário como maximizado e com Zoom
            With objPrintPreview
                .Document = pd
                .WindowState = FormWindowState.Maximized
                .PrintPreviewControl.Zoom = 1
                .Text = "Relacao de Clientes"
                .ShowDialog()
            End With
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    'A conexÆo e o DataReader ‚ aberto aqui
    Private Sub Begin_Print(ByVal sender As Object, ByVal e As Printing.PrintEventArgs)

        Dim Sql As String = "SELECT * from tblClientes"
        Dim MyComand As New SqlCommand(Sql, MyConnection)

        MyConnection.Open()
        Leitor = MyComand.ExecuteReader()
        paginaAtual = 1

    End Sub

    'Layout da(s) p gina(s) a imprimir
    Private Sub pdRelatorios_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        'Variaveis das linhas
        Dim LinhasPorPagina As Single = 0
        Dim PosicaoDaLinha As Single = 0
        Dim LinhaAtual As Integer = 0

        'Variaveis das margens
        Dim MargemEsquerda As Single = e.MarginBounds.Left
        Dim MargemSuperior As Single = e.MarginBounds.Top + 100
        Dim MargemDireita As Single = e.MarginBounds.Right
        Dim MargemInferior As Single = e.MarginBounds.Bottom
        Dim CanetaDaImpressora As Pen = New Pen(Color.Black, 1)
        Dim IdCliente As Integer
        Dim Nome As String
        Dim CPF As String
        Dim DataNascimento As String
        Dim LimiteDeCredito As Decimal

        'Variaveis das fontes
        Dim FonteNegrito As Font
        Dim FonteTitulo As Font
        Dim FonteSubTitulo As Font
        Dim FonteRodape As Font
        Dim FonteNormal As Font

        'define efeitos em fontes
        FonteNegrito = New Font("Arial", 9, FontStyle.Bold)
        FonteTitulo = New Font("Arial", 15, FontStyle.Bold)
        FonteSubTitulo = New Font("Arial", 18, FontStyle.Bold)
        FonteRodape = New Font("Arial", 8)
        FonteNormal = New Font("Arial", 9)

        'define valores para linha atual e para linha da impressao
        LinhaAtual = 0
        Dim L As Integer = 0

        'Cabecalho
        e.Graphics.DrawLine(CanetaDaImpressora, MargemEsquerda, 60, MargemDireita, 60)
        e.Graphics.DrawLine(CanetaDaImpressora, MargemEsquerda, 160, MargemDireita, 160)


        'Imagem
        e.Graphics.DrawImage(Image.FromFile("C:\Paulo Bruno\Icones - Imagens e Gif´s\Imagens\" & "Teste.jpg"), 100, 68)
        e.Graphics.DrawString(RelatorioTitulo & System.DateTime.Today, FonteSubTitulo, Brushes.Black, MargemEsquerda + 180, 100, New StringFormat())


        'campos a serem impressos: Codigo e Nome
        e.Graphics.DrawString("IdCliente", FonteNegrito, Brushes.Black, MargemEsquerda + 0, 170, New StringFormat())
        e.Graphics.DrawString("Nome", FonteNegrito, Brushes.Black, MargemEsquerda + 70, 170, New StringFormat())
        e.Graphics.DrawString("CPF", FonteNegrito, Brushes.Black, MargemEsquerda + 300, 170, New StringFormat())
        e.Graphics.DrawString("DataNas.", FonteNegrito, Brushes.Black, MargemEsquerda + 450, 170, New StringFormat())
        e.Graphics.DrawString("Credito", FonteNegrito, Brushes.Black, MargemEsquerda + 550, 170, New StringFormat())
        e.Graphics.DrawLine(CanetaDaImpressora, MargemEsquerda, 190, MargemDireita, 190)

        LinhasPorPagina = CInt(e.MarginBounds.Height / FonteNormal.GetHeight(e.Graphics) - 9)

        'Aqui sao lidos os dados
        While (LinhaAtual < LinhasPorPagina AndAlso Leitor.Read())

            'obtem os valores do datareader
            IdCliente = Leitor.GetInt32(0)
            Nome = Leitor.GetString(1)
            CPF = Leitor.GetString(2)
            DataNascimento = Leitor.GetString(3)
            LimiteDeCredito = Leitor.GetDecimal (4)

            'inicia a impressao
            PosicaoDaLinha = MargemSuperior + (LinhaAtual * FonteNormal.GetHeight(e.Graphics))
            e.Graphics.DrawString(IdCliente.ToString(), FonteNormal, Brushes.Black, MargemEsquerda + 0, PosicaoDaLinha, New StringFormat())
            e.Graphics.DrawString(Nome.ToString, FonteNormal, Brushes.Black, MargemEsquerda + 70, PosicaoDaLinha, New StringFormat())
            e.Graphics.DrawString(CPF.ToString, FonteNormal, Brushes.Black, MargemEsquerda + 300, PosicaoDaLinha, New StringFormat())
            e.Graphics.DrawString(DataNascimento.ToString, FonteNormal, Brushes.Black, MargemEsquerda + 450, PosicaoDaLinha, New StringFormat())
            e.Graphics.DrawString(LimiteDeCredito.ToString, FonteNormal, Brushes.Black, MargemEsquerda + 550, PosicaoDaLinha, New StringFormat())
            LinhaAtual += 1
        End While
        'Rodape
        e.Graphics.DrawLine(CanetaDaImpressora, MargemEsquerda, MargemInferior, MargemDireita, MargemInferior)
        e.Graphics.DrawString(System.DateTime.Now.ToString(), FonteRodape, Brushes.Black, MargemEsquerda, MargemInferior, New StringFormat())
        LinhaAtual += CInt(FonteNormal.GetHeight(e.Graphics))
        LinhaAtual += 1
        e.Graphics.DrawString("P gina : " & paginaAtual, FonteRodape, Brushes.Black, MargemDireita - 50, MargemInferior, New StringFormat())

        'Incrementa o n£mero da pagina
        paginaAtual += 1

        'verifica se continua imprimindo
        If (LinhaAtual > LinhasPorPagina) Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If
    End Sub

    'Encerra a conexÆo e o DataReader
    Private Sub End_Print(ByVal sender As Object, ByVal byvale As Printing.PrintEventArgs)
        Leitor.Close()
        MyConnection.Close()
    End Sub

    Private Sub DataGridView1_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEnter
        Try

            Dim index As Integer
            index = e.RowIndex
            Dim selectedRow As DataGridViewRow
            selectedRow = DataGridView1.Rows(index)
            IdClienteTextBox.Text = selectedRow.Cells(0).Value.ToString
            NomeTextBox.Text = selectedRow.Cells(1).Value.ToString
            CpfMaskedtextbox.Text = selectedRow.Cells(2).Value.ToString
            DataNascimentoTextBox.Text = selectedRow.Cells(3).Value.ToString

            'Cast Para Moeda Nacional Brasil.
            txtCast.Text = String.Format("{0:N}", selectedRow.Cells(4).Value)
            LimiteDeCreditoTextBox.Text = txtCast.Text
        Catch ex As Exception

        End Try
    End Sub
End Class

