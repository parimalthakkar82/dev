Option Strict Off
Imports System.Data.SqlClient
Imports System.Windows
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.FileSystem
Imports System.Configuration


Public Class DatabaseLayerSQL
#Region " Variable Declaration "
    Dim StrCn As String
    Private MGCON As New SqlConnection
    Private gMGCON As New SqlConnection

    Dim _Transaction As SqlTransaction
    Dim x As Boolean

    Public Const APSFJS = "100010101!P@#$&^%z01111100010101"
    Public gstrConnectionString As String = getfinalString()

    ''Dim _SychLock1 As New Object
    ''Dim _SychLock2 As New Object
    ''Dim _SychLock3 As New Object
    ''Dim _SychLock4 As New Object
    ''Dim _SychLock5 As New Object
#End Region

#Region " Functions "

    'Public Function DecryptString(ByVal strValue As String) As String
    '    Dim objPassword As New CIMSEC.clsSOXEnc
    '    Dim strError As String = ""
    '    Dim strReturn As String = ""
    '    Try
    '        strReturn = objPassword.Method4(strValue, strError, APSFJS)
    '        Return strReturn
    '    Catch ex As Exception
    '        'MsgBox(ex.Message, MsgBoxStyle.Information)
    '        ErrorLog("DecryptString", ex)
    '        Return ""
    '    End Try
    'End Function

    'Public Function EncryptString(ByVal strValue As String) As String
    '    Dim objPassword As New CIMSEC.clsSOXEnc
    '    Dim strError As String = ""
    '    Dim strReturn As String = ""
    '    Try
    '        strReturn = objPassword.Method3(strValue, strError, APSFJS)
    '        Return strReturn
    '    Catch ex As Exception
    '        'MsgBox(ex.Message, MsgBoxStyle.Information)
    '        ErrorLog("EncryptString", ex)
    '        Return ""
    '    End Try
    'End Function

    'To get the Connectionstring of Database
    Public Function getfinalString() As String


        Return getConfigValuefromAPP("connectionString")

        'Dim finalString As String = ""

        ''--------------------------------------------------------------------------
        ''(start) Added/Modified by Kamlesh Lalwani on 21-Dec-2010, Changed from ConfigurationSettings to ConfigurationManager 

        'Dim userId As String = ConfigurationManager.AppSettings("UserID")
        'Dim Password As String = ConfigurationManager.AppSettings("PWD")
        'Dim DataSource As String = ConfigurationManager.AppSettings("DataSource")

        ''(End) Added/Modified by Kamlesh Lalwani on 21-Dec-2010, Changed from ConfigurationSettings to ConfigurationManager 
        ''--------------------------------------------------------------------------

        ''--------------------------------------------------------------------------
        ''(start) Added/Modified by Kamlesh Lalwani on 21-Dec-2010 for SQLSERVER Database Connection	

        'If (Convert.ToString(ConfigurationManager.AppSettings("DBType")).ToUpper = "SQLSERVER") Then
        '    'Declared Variable on 21_Dec_2010 by Kamlesh Lalwani to make the SQLSERVER Database Connectionstring 
        '    Dim InitialCatalog As String = Convert.ToString(ConfigurationManager.AppSettings("InitialCatalog"))
        '    '[Start] Added/Modified by Keyur Parekh on 27-Sep-2014 for #TN5454 to implements in LG311SP2
        '    If userId = "" AndAlso Password = "" Then
        '        finalString = "Server=" & DataSource & ";Integrated Security=SSPI;Initial Catalog=" & InitialCatalog & ""
        '    Else
        '        finalString = "Server=" & DataSource & ";UId=" & DecryptString(userId) & ";password=" & DecryptString(Password) & ";Initial Catalog=" & InitialCatalog & ""
        '    End If
        '    '[End] Added/Modified by Keyur Parekh on 27-Sep-2014 for #TN5454 to implements in LG311SP2
        'Else
        '    finalString = "User ID=" & DecryptString(userId) & ";"
        '    finalString += "Password=" & DecryptString(Password) & ";"
        '    finalString += "Data Source=" & DataSource & ";Persist Security Info=True;"
        'End If
        'DebugLog("InitialCatalog: " + Convert.ToString(ConfigurationManager.AppSettings("InitialCatalog")))
        ''(End) Added/Modified by Kamlesh Lalwani on 21-Dec-2010 for SQLSERVER Database Connection		  
        ''--------------------------------------------------------------------------
        'Return finalString
    End Function

    Public Sub OpenConnection(ByVal DataSource As String, ByVal InitialCatalog As String, ByVal UserID As String, ByVal Password As String, ByVal pErrorlogPath As String, ByRef pErrNo As Long, ByRef pErrMsg As String)
        Try
            Dim strUserName As String
            Dim strPassword As String
            Dim StrCn As String
            Dim strDatabase As String
            Dim strServerName As String

            'strUserName = Decrypt(UserID)
            'strPassword = Decrypt(Password)
            'strServerName = Decrypt(DataSource)
            'strDatabase = Decrypt(InitialCatalog)

            strUserName = UserID
            strPassword = Password
            strServerName = DataSource
            strDatabase = InitialCatalog

            StrCn = "Data Source=" & strServerName & ";Initial Catalog=" & strDatabase & ";User ID=" & strUserName & ";Password=" & strPassword & ""
            If Not MGCON Is Nothing Then
                Try
                    MGCON.Close()
                Catch ex As Exception
                    ErrorLog("DatabaseLayer.OpenConnection(During Close connection)", ex)
                End Try
                MGCON = Nothing
                MGCON = New SqlConnection(StrCn)
            Else
                MGCON = Nothing
                MGCON = New SqlConnection(StrCn)
            End If

            MGCON.Open()


        Catch ex As Exception
            pErrMsg = Err.Description
            pErrNo = Err.Number
            ErrorLog("DatabaseLayer.OpenConnection", ex)
        End Try
    End Sub

    Public Sub OpenConnection(ByRef pErrNo As Long, ByRef pErrMsg As String)
        Try

            StrCn = gstrConnectionString
            If Not MGCON Is Nothing Then
                Try
                    MGCON.Close()
                Catch ex As Exception
                    ErrorLog("DatabaseLayer.OpenConnection(During Close connection)", ex)
                End Try
                MGCON = Nothing
                MGCON = New SqlConnection(StrCn)
            Else
                MGCON = Nothing
                MGCON = New SqlConnection(StrCn)
            End If

            MGCON.Open()
        Catch ex As Exception
            pErrMsg = Err.Description
            pErrNo = Err.Number
            ErrorLog("DatabaseLayer.OpenConnection", ex)
        End Try
    End Sub

    Private Function CheckConnectionActive() As Boolean
        CheckConnectionActive = False
        Dim lngErrNo As Long
        Dim strErrDesc As String
        strErrDesc = ""
        lngErrNo = 0
        Try
            If MGCON.State = ConnectionState.Broken Or MGCON.State = ConnectionState.Closed Then
                ErrorLog("DatabaseLayer.CheckConnectionActive", 0, "Database Connection is broken or Closed. Now Retry for Connection.")
                'OpenConnection(lngErrNo, strErrDesc, True)
                OpenConnection(lngErrNo, strErrDesc)
                'MGCON = gMGCON
                If lngErrNo <> 0 Then
                    ErrorLog("DatabaseLayer.CheckConnectionActive", lngErrNo, strErrDesc)
                    CheckConnectionActive = False
                Else
                    ErrorLog("DatabaseLayer.CheckConnectionActive", 0, "Connect to database successfully")
                    CheckConnectionActive = True
                End If
            Else
                CheckConnectionActive = True
            End If
        Catch ex As Exception
            ErrorLog("DatabaseLayer.isConnectionActive", ex)
        End Try
    End Function

    Private Function CloseActiveConnection() As Boolean
        CloseActiveConnection = False
        Try
            If (MGCON.State = ConnectionState.Connecting Or MGCON.State = ConnectionState.Open) And MGCON.State <> ConnectionState.Executing And MGCON.State <> ConnectionState.Fetching Then
                MGCON.Close()
                'LogError("CCA.DatabaseDLL", "CloseActiveConnection", 1005, "Database Connection is closed.")
                CloseActiveConnection = True
            ElseIf MGCON.State <> ConnectionState.Closed Then
                'LogError("CCA.DatabaseDLL", "CloseActiveConnection", 1005, "Error to close Database Connection.")
                CloseActiveConnection = False
            End If
        Catch ex As Exception
            ErrorLog("DatabaseLayer.CloseActiveConnection", ex)
        End Try
    End Function
#End Region




#Region " Database Related Function "

    Public Function GetDataSet(ByVal pstrQuery As String, ByVal pstrTableName As String) As DataSet
        '---------------------------------------------------------------------------------------'
        ' Name          :   GetDataSet
        ' Description   :   It provides a dataset based on the query.
        ' Author        :   Parimal Thakkar
        ' Created Date  :    01-Mar-2008
        ' Arguments     
        '   Input       :   1.  pstrQuery   String  Query on the basis of which DataSet to be generated.
        '                   2.  pstrTableName  String  Tablename
        '   Output      :   DataSet
        '
        '   Revision History
        '--------------------------------------------------------
        '   Date    Author      Comments    
        '--------------------------------------------------------
        '
        '---------------------------------------------------------------------------------------'
        GetDataSet = Common(pstrQuery, "Dataset", pstrTableName)
        Exit Function

    End Function

    Public Function GetDataTable(ByVal pstrQuery As String) As DataTable
        '---------------------------------------------------------------------------------------'
        ' Name          :   GetDataTable
        ' Description   :   It provides a data table based on the query.
        ' Author        :   Parimal Thakkar
        ' Created Date  :    01-Mar-2008
        ' Arguments     
        '   Input       :   1.  pstrQuery   String  Query on the basis of which Data table to be generated.
        '   Output      :   DataTable
        '
        '   Revision History
        '--------------------------------------------------------
        '   Date    Author      Comments    
        '--------------------------------------------------------
        '
        '---------------------------------------------------------------------------------------'
        If CheckConnectionActive() = False Then Return Nothing
        Dim objCommand As SqlCommand = Nothing
        Dim objAdapter As New SqlDataAdapter
        Dim dtDataTable As New DataTable

        Try
            If _Transaction Is Nothing Then
                objCommand = New SqlCommand(pstrQuery, MGCON)
            Else
                objCommand = New SqlCommand(pstrQuery, MGCON, _Transaction)
            End If
            objAdapter = New SqlDataAdapter(objCommand)
            objAdapter.Fill(dtDataTable)
            Return dtDataTable
        Catch ex As Exception
            ErrorLog("DatabaseLayer.GetDataTable", ex)
            'Throw ex
            Return Nothing
        Finally
            CloseObject(objCommand)
            CloseObject(objAdapter)
            CloseActiveConnection()
        End Try

    End Function

    Public Function GetDataView(ByVal pstrQuery As String) As DataView
        '---------------------------------------------------------------------------------------'
        ' Name          :   GetDataView
        ' Description   :   It provides a data view based on the query.
        ' Author        :   Parimal Thakkar
        ' Created Date  :   01-Mar-2008
        ' Arguments     
        '   Input       :   1.  pstrQuery   String  Qon the basis of which Data view to be generated.
        '   Output      :   DataTable
        '
        '   Revision History
        '--------------------------------------------------------
        '   Date    Author      Comments    
        '--------------------------------------------------------
        '
        '---------------------------------------------------------------------------------------'
        If CheckConnectionActive() = False Then Return Nothing
        Dim objCommand As SqlCommand = Nothing
        Dim objAdapter As New SqlDataAdapter
        Dim dvDataView As DataView = Nothing
        Dim dsDataSet As DataSet = Nothing

        Try
            If _Transaction Is Nothing Then
                objCommand = New SqlCommand(pstrQuery, MGCON)
            Else
                objCommand = New SqlCommand(pstrQuery, MGCON, _Transaction)
            End If

            objAdapter = New SqlDataAdapter(objCommand)
            objAdapter.Fill(dsDataSet)
            Return dsDataSet.Tables(0).DefaultView
        Catch ex As Exception
            ErrorLog("DatabaseLayer.GetDataView", ex)
            Return Nothing
            'Throw ex
        Finally
            CloseObject(objCommand)
            CloseObject(objAdapter)
            CloseActiveConnection()
        End Try

    End Function

    Public Function ExecuteNonQuery(ByVal pstrQuery As String) As Integer
        '---------------------------------------------------------------------------------------'
        ' Name          :   ExecuteNonQuery
        ' Description   :   It executes a query against a connection and returns the number of records
        '                   affected.
        ' Author        :   Parimal Thakkar
        ' Created Date  :   01-Mar-2008
        ' Arguments     
        '   Input       :   1.  pstrQuery   String  Query to be fired against a databsae connection.
        '   Output      :   Integer
        '
        '   Revision History
        '--------------------------------------------------------
        '   Date    Author      Comments    
        '--------------------------------------------------------
        '
        '---------------------------------------------------------------------------------------'

        ExecuteNonQuery = Common(pstrQuery, "NonQry")
        Exit Function
    End Function

    Public Function ExecuteNonQuery(ByVal pobjcmd As SqlCommand) As Integer
        '---------------------------------------------------------------------------------------'
        ' Name          :   ExecuteNonQuery
        ' Description   :   It executes Stored Procedure
        ' Author        :   Parimal Thakkar
        ' Created Date  :   01-Mar-2008
        ' Arguments     
        '   Input       :   pobjCmd
        '   Output      :   Integer
        '
        '   Revision History
        '--------------------------------------------------------
        '   Date    Author      Comments    
        '--------------------------------------------------------
        '
        '---------------------------------------------------------------------------------------'
        If CheckConnectionActive() = False Then Return 0
        'SyncLock _SychLock2
        Dim objCommand As SqlCommand = Nothing
        Dim objconnection As SqlConnection
        Try
            objconnection = MGCON
            objCommand = pobjcmd
            objCommand.Connection = objconnection
            If Not _Transaction Is Nothing Then objCommand.Transaction = _Transaction
            Return objCommand.ExecuteNonQuery()

        Catch ex As Exception
            ErrorLog("DatabaseLayer.ExecuteNonQuery", ex)
            'Throw ex
            Return 0
        Finally
            CloseObject(objCommand)
            'CloseActiveConnection()
        End Try
        'End SyncLock

    End Function

#End Region

#Region " General Data Access Functions "

    Public Function ExecuteScalar(ByVal pstrQuery As String) As Object
        '---------------------------------------------------------------------------------------'
        ' Name          :   ExecuteScalar
        ' Description   :   It executes a query against a connection and returns first column of the 
        '                   returned resultset.
        ' Author        :   Parimal Thakkar
        ' Created Date  :   01-Mar-2008
        ' Arguments     
        '   Input       :   1.  pstrQuery   String  Query to be fired against a databsae connection.
        '   Output      :   Object
        '
        '   Revision History
        '--------------------------------------------------------
        '   Date    Author      Comments    
        '--------------------------------------------------------
        '
        '---------------------------------------------------------------------------------------'

        ExecuteScalar = Common(pstrQuery, "Scalar")
        Exit Function
    End Function

    Public Function ExecuteReader(ByVal pstrQuery As String, Optional ByVal pblnNewData As Boolean = False) As IDataReader
        '---------------------------------------------------------------------------------------'
        ' Name          :   ExecuteScalar
        ' Description   :   It executes a query against a connection and returns first column of the 
        '                   returned resultset.
        ' Author        :   Parimal Thakkar
        ' Created Date  :   01-Mar-2008
        ' Arguments     
        '   Input       :   1.  pstrQuery   String Query to be fired against a databsae connection.
        '   Output      :   Object
        '
        '   Revision History
        '--------------------------------------------------------
        '   Date    Author      Comments    
        '--------------------------------------------------------
        '
        '---------------------------------------------------------------------------------------'
        ExecuteReader = Common(pstrQuery, "Reader", , pblnNewData)
        Exit Function

    End Function

    Public Sub CloseObject(ByRef pobjObject As Object)

        Try
            If Not pobjObject Is Nothing Then
                pobjObject = Nothing
            End If
        Catch
        End Try
    End Sub

#End Region
#Region " Class Properties "
    Public Property Transaction() As SqlTransaction
        Get
            Transaction = _Transaction
        End Get
        Set(ByVal Value As SqlTransaction)
            _Transaction = Value
        End Set
    End Property
#End Region

    Protected Overrides Sub Finalize()
        'MGCON.Dispose()
        'MGCONNEW.Dispose()
        MyBase.Finalize()
    End Sub

    Private Function Common(ByVal pstrQuery As String, ByVal Type As String, Optional ByVal pstrTableName As String = "", Optional ByVal pblnNewData As Boolean = False) As Object
        Common = Nothing
        If CheckConnectionActive() = False Then Exit Function
        Dim objCommand As SqlCommand = Nothing
        Dim objAdapter As New SqlDataAdapter
        Dim dsData As DataSet = Nothing
        objCommand = Nothing
        'Try
        Try
            'LogError("DBDLL", "Common", "1005", "Common " & vbCrLf & pstrQuery)
        Catch ex As Exception
        End Try
        While True
            If x = False Then
                x = True
                'SyncLock (_SychLock4)
                'Monitor.TryEnter(_SychLock4, 1500)
                'Monitor.Enter(_SychLock4)
                Select Case Type
                    Case "Reader"
                        Try
                            If _Transaction Is Nothing Then
                                If pblnNewData = False Then
                                    objCommand = New SqlCommand(pstrQuery, MGCON)
                                    objCommand.CommandTimeout = 200 '30
                                    Return objCommand.ExecuteReader(CommandBehavior.Default)
                                End If

                            Else
                                If pblnNewData = False Then
                                    objCommand = New SqlCommand(pstrQuery, MGCON, _Transaction)
                                    objCommand.CommandTimeout = 200 '30
                                    Return objCommand.ExecuteReader(CommandBehavior.Default)
                                End If
                            End If
                        Catch ex As Exception
                            ErrorLog("DatabaseLayer.ExecuteReader", ex)
                            DebugLog("Error to connect database")
                            MsgBox("Error to connect database", MsgBoxStyle.Critical, "Database Connection Error")
                            Throw ex
                        Finally
                            CloseObject(objCommand)
                            x = False
                            'CloseActiveConnection()
                        End Try
                        Exit Function
                    Case "Dataset"
                        Try
                            For I As Integer = 1 To 2
                                dsData = New DataSet
                                'dsData = Nothing
                                objAdapter = New SqlDataAdapter(pstrQuery, MGCON)
                                objAdapter.Fill(dsData, pstrTableName)
                                If dsData.Tables.Count > 0 Then
                                    Exit For
                                Else
                                    Sleep(100)
                                End If
                            Next
                            Return dsData
                        Catch ex As Exception
                            ErrorLog("DatabaseLayer.GetDataSet", ex)
                            DebugLog("Error to connect database")
                            MsgBox("Error to connect database", MsgBoxStyle.Critical, "Database Connection Error")
                            Throw ex
                            'Throw ex
                        Finally
                            CloseObject(objAdapter)
                            x = False
                            'CloseActiveConnection()
                        End Try
                        Exit Function
                    Case "NonQry"
                        Try
                            If _Transaction Is Nothing Then
                                objCommand = New SqlCommand(pstrQuery, MGCON)
                            Else
                                objCommand = New SqlCommand(pstrQuery, MGCON, _Transaction)
                            End If
                            'Added by prakash on 20-jan-2010
                            objCommand.CommandTimeout = 200 '30
                            Return objCommand.ExecuteNonQuery()
                        Catch ex As Exception
                            '(Start Change) prakash 04-march-2010
                            If Err.Number = 5 Or Err.Description = "ExecuteNonQuery requires an open and available Connection. The connection's current state is closed." Then
                                Try
                                    If CheckConnectionActive() = True Then
                                        If _Transaction Is Nothing Then
                                            objCommand = New SqlCommand(pstrQuery, MGCON)
                                        Else
                                            objCommand = New SqlCommand(pstrQuery, MGCON, _Transaction)
                                        End If
                                        'Added by prakash on 20-jan-2010
                                        objCommand.CommandTimeout = 200 '30
                                        Return objCommand.ExecuteNonQuery()
                                    End If
                                Catch exNew As Exception
                                    ErrorLog("DatabaseLayer.ExecuteNonQuery(ReConnect Database)", ex)
                                    DebugLog("Error to connect database")
                                    MsgBox("Error to ExecuteNonQuery with database", MsgBoxStyle.Critical, "Database Connection Error")
                                    Throw ex
                                End Try
                            Else
                                DebugLog("Error to connect database")
                                MsgBox("Error to ExecuteNonQuery with database", MsgBoxStyle.Critical, "Database Connection Error")
                                Throw ex
                            End If
                            '(End Change) by prakash on 04-march-2010
                            ErrorLog("DatabaseLayer.ExecuteNonQuery", ex)
                        Finally
                            CloseObject(objCommand)
                            'CloseActiveConnection()
                            x = False
                        End Try
                        Exit Function
                    Case "Scalar"
                        Try
                            If _Transaction Is Nothing Then
                                objCommand = New SqlCommand(pstrQuery, MGCON)
                            Else
                                objCommand = New SqlCommand(pstrQuery, MGCON, _Transaction)
                            End If
                            'Return objCommand.ExecuteScalar()

                            'Added by prakshon 20-jan-2010
                            objCommand.CommandTimeout = 200 '30
                            If IsDBNull(objCommand.ExecuteScalar()) Then
                                Return 0
                            Else
                                Return objCommand.ExecuteScalar()
                            End If

                        Catch ex As Exception
                            ErrorLog("DatabaseLayer.ExecuteScalar", ex)
                            DebugLog("Error to ExecuteScalar with database")
                            MsgBox("Error to ExecuteScalar with database", MsgBoxStyle.Critical, "Database Connection Error")
                            Throw ex
                        Finally
                            CloseObject(objCommand)
                            x = False
                            'CloseActiveConnection()
                        End Try
                        Exit Function
                End Select
                'End SyncLock
            End If
            Sleep(5)
            'System.Windows.Forms.Application.DoEvents()
        End While
        'Catch ex As Exception
        '    ErrorLog("DatabaseLayer.Common", ex)
        '    'MsgBox("Error to connect database", MsgBoxStyle.Critical, "Database Connection Error")
        '    Throw ex
        'Finally

        '    Try
        '        'Monitor.Exit(_SychLock4)
        '    Catch ex As Exception
        '    End Try
        'End Try
    End Function

End Class
