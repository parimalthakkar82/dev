
Option Explicit On
Imports System.Data
Imports System.Data.OleDb
Imports System.IO

Public Class clsAccess

    Private MDBCN As New OleDbConnection
    Sub New()
        OpenConnection()
    End Sub
    Public Sub ExecuteSQLScript(ByRef SQLText As String)
        Try
            Dim scriptText As String = SQLText
            Dim splitter As String() = New String() {vbCrLf & "GO" & vbCrLf}
            Dim commandTexts As String() = scriptText.Split(splitter, StringSplitOptions.RemoveEmptyEntries)
            Dim cmd As OleDbCommand
            Dim sql As String
            For Each stext As String In commandTexts
                sql = "Insert into SPs (SQLText) values('" & stext.Replace("'", "") & "')"
                cmd = New OleDbCommand(sql, MDBCN)
                cmd.CommandType = CommandType.Text
                cmd.ExecuteNonQuery()

            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function getfinalString() As String

        Dim finalString As String = ""

        Dim DBPath As String = System.Windows.Forms.Application.StartupPath & "\Database1.mdb"                         ' ConfigurationManager.AppSettings("SourcePath")
        finalString = "Provider=Microsoft.Jet.OLEDB.4.0;User Id=admin;Password='';Data Source=" & DBPath & ";"
        Return finalString
    End Function
    Public Function OpenConnection() As Boolean
        Dim blnConnection As Boolean = False
        Dim gstrConnectionString As String = getfinalString()

        Try
            If Not MDBCN Is Nothing Then
                If MDBCN.State = ConnectionState.Broken Or MDBCN.State = ConnectionState.Closed Or MDBCN.State = ConnectionState.Connecting Then
                    Try
                        MDBCN.Close()
                    Catch ex As Exception
                    End Try
                    MDBCN = Nothing
                    MDBCN = New OleDbConnection(gstrConnectionString)
                    MDBCN.Open()
                ElseIf MDBCN.State = ConnectionState.Executing Or MDBCN.State = ConnectionState.Fetching Or MDBCN.State = ConnectionState.Open Then
                    'do nothing
                End If
            Else
                MDBCN = New OleDbConnection(gstrConnectionString)
                MDBCN.Open()
            End If
            blnConnection = True
        Catch ex As Exception

        End Try
        Return blnConnection
    End Function

End Class
