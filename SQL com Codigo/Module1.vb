Imports System.Data.SqlClient
Module Module1
    Public Function GetconnectionSql() As SqlConnection
        Dim sql As String = "Data Source=LAPTOP-OT96ST84;Initial Catalog=DBCliente;User ID=sa;Password=Paradoxo22"
        Return New SqlConnection(sql)
    End Function
End Module
