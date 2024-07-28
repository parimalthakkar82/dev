Imports System
Imports System.IO
Imports System.Xml
Imports System.Windows.Forms
Imports System.Text
Imports System.Threading
Imports System.Text.Encoder
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic
Imports System.Configuration

Public Module modGeneral
    Dim lastCheckTime As DateTime = Now
    Dim IntervalLogFileCheck As Integer = 60
    Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Integer) 'For Sleep
    Public objDB As DatabaseLayerSQL
    Public IMEILen As Integer = 15
    Public SLCIDLen As Integer
    Public WaitTime As Integer = 50
    Public APP_Title As String

    Public Function DebugLog(ByVal pstrMessage As String, Optional strlabel As String = "LabelPrint") As Boolean
        Dim objReader As StreamWriter = Nothing
        Try
            If Directory.Exists(System.Windows.Forms.Application.StartupPath & "\Log") = False Then Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath & "\Log")
            If Math.Abs(DateDiff(DateInterval.Second, lastCheckTime, Now)) >= IntervalLogFileCheck Then

                Try
                    If FileIO.FileSystem.FileExists(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & strlabel & ".log") = True Then
                        If FileLen(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & strlabel & ".log") > 9000000 Then
                            FileIO.FileSystem.RenameFile(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & strlabel & ".log", Format(Today.Date, "dd-MM-yy") & "_" & Now.Hour.ToString.PadLeft(2, CChar("00")) & Now.Minute.ToString.PadLeft(2, CChar("00")) & "_" & strlabel & ".log")
                        End If
                    End If
                Catch ex As Exception
                End Try
                lastCheckTime = Now
            End If

            objReader = New StreamWriter(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & strlabel & ".log", True)
            If pstrMessage <> "" Then
                objReader.WriteLine(Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + ", " + vbTab + pstrMessage)
            End If
            Return True
        Catch ex As Exception
            Return False
        Finally
            Try
                objReader.Close()
            Catch ex As Exception
            End Try
            Try
                objReader.Dispose()
            Catch ex As Exception
            End Try
            Try
                objReader = Nothing
            Catch ex As Exception
            End Try
        End Try
        Return True
    End Function

    Public Function ErrorLog(ByVal ModuleName As String, ByVal ex As Exception, Optional ByVal pstrTypeLog As String = "Error") As Boolean
        Dim objReader As StreamWriter = Nothing
        Try
            If Math.Abs(DateDiff(DateInterval.Second, lastCheckTime, Now)) >= 60 Then
                If Directory.Exists(System.Windows.Forms.Application.StartupPath & "\Log") = False Then Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath & "\Log")
                Try
                    If FileIO.FileSystem.FileExists(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & pstrTypeLog & ".log") = True Then
                        If FileLen(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & pstrTypeLog & ".log") > 9000000 Then
                            FileIO.FileSystem.RenameFile(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & pstrTypeLog & ".log", Format(Today.Date, "dd-MM-yy") & "_" & Now.Hour.ToString.PadLeft(2, CChar("00")) & Now.Minute.ToString.PadLeft(2, CChar("00")) & "_" & pstrTypeLog & ".log")
                        End If
                    End If
                Catch

                End Try
                lastCheckTime = Now
            End If

            objReader = New StreamWriter(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & pstrTypeLog & ".log", True)
            If ModuleName <> "" Then
                objReader.WriteLine(Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + ", ModuleName: " + ModuleName + ", " + vbTab + "Error: " + ex.ToString() + ", Stack: " + ex.StackTrace.ToString())
            End If
            Return True
        Catch
            Return False
        Finally
            Try
                objReader.Close()
            Catch
            End Try
            Try
                objReader.Dispose()
            Catch
            End Try
            Try
                objReader = Nothing
            Catch
            End Try
        End Try
    End Function

    Public Function getConfigValuefromAPP(ByVal pstrKey As String) As String
        '***********************************************************
        '* Name      : getConfigValuefromAPP
        '* Remark    : Gets value from the configuration file of .net.
        '
        '* Arguments : pstrKey
        '* Returns   : String
        '* References:
        '* Author    : parimal thakkar
        '* Date      : 05-Mar-2008 
        '***********************************************************
        '************************History Section***********************************
        'Date           Developer           Comments
        '05-Mar-2008    Parimal Thakkar     Initial Creation
        '**************************************************************************
        getConfigValuefromAPP = ""
        Try
            Dim str As String
            str = System.Configuration.ConfigurationManager.AppSettings.Get(pstrKey)
            Return str
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub setConfigValuefromAPP(key As String, value As String)
        Try
            Dim configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            Dim settings = configFile.AppSettings.Settings
            If IsNothing(settings(key)) Then
                settings.Add(key, value)
            Else
                settings(key).Value = value
            End If
            configFile.Save(ConfigurationSaveMode.Modified)
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
        Catch e As ConfigurationErrorsException
            Throw e
        End Try
    End Sub


    Public Function ReportLog(ByVal pstrMessage As String) As Boolean
        '***********************************************************
        '* Name      : DebugLog
        '* Remark    : Maintain the Debug log
        '* Arguments : ByVal pstrMessage As String
        '* Returns   : Boolean
        '* References:
        '* Author    : parimal thakkar
        '* Date      : 19-Jul-2016
        '***********************************************************
        '************************History Section***********************************
        'Date           Developer           Comments
        '19-Jul-2016    Parimal Thakkar     Initial Creation
        '**************************************************************************
        'If _ConfigSettings._debugEnable = 1 Then '--- If debug log is explicitly 0 then  not to write 
        Dim objReader_Report As StreamWriter = Nothing
        Try

            If Directory.Exists(System.Windows.Forms.Application.StartupPath & "\Report") = False Then
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath & "\Report")
                objReader_Report = New StreamWriter(Application.StartupPath & "\Report\" & "Summary.log", True)
                objReader_Report.WriteLine("Date,Status,EUID,Channel,Test Node Packet Rec,Test Node RSSI,Test Node TX Power,Golden Node Packet Rec,Reason,Power,RSSI High,RSSI Low,TX Power High,TX Power Low,RX Sensitivity,Attenuation,PER (%),No Of Packet,Model No")
            ElseIf File.Exists(Application.StartupPath & "\Report\" & "Summary.log") = False Then
                objReader_Report = New StreamWriter(Application.StartupPath & "\Report\" & "Summary.log", True)
                objReader_Report.WriteLine("Date,Status,EUID,Channel,Test Node Packet Rec,Test Node RSSI,Test Node TX Power,Golden Node Packet Rec,Reason,Power,RSSI High,RSSI Low,TX Power High,TX Power Low,RX Sensitivity,Attenuation,PER (%),No Of Packet,Model No")
            Else
                objReader_Report = New StreamWriter(Application.StartupPath & "\Report\" & "Summary.log", True)
            End If
            If pstrMessage <> "" Then
                objReader_Report.WriteLine(pstrMessage)
            End If
            Return True
        Catch ex As Exception
            Return False
        Finally
            Try
                objReader_Report.Close()
            Catch ex As Exception
            End Try
            Try
                objReader_Report.Dispose()
            Catch ex As Exception
            End Try
            Try
                objReader_Report = Nothing
            Catch ex As Exception
            End Try
            'Application.DoEvents() '--- added by parimal thakkar dated: 14-Mar-2009
        End Try

        'End SyncLock
        'End If
        Return True
    End Function

    Public Function ErrorLog(ByVal ModuleName As String, ByVal ErrNumber As Long, ByVal ErrorMsg As String, Optional ByVal pstrTypeLog As String = "Error") As Boolean
        '***********************************************************
        '* Name      : ErrorLog
        '* Remark    : Maintain the Error log
        '* Arguments : ByVal pstrMessage As String
        '* Returns   : Boolean
        '* References:
        '* Author    : parimal thakkar
        '* Date      : 19-Jul-2016
        '***********************************************************
        '************************History Section***********************************
        'Date           Developer           Comments
        '19-Jul-2016    Parimal Thakkar     Initial Creation
        '**************************************************************************
        Dim objReader As StreamWriter = Nothing
        Try
            'If DateDiff(DateInterval.Second, dtLastTime, Now) >= 60 Then
            If Directory.Exists(System.Windows.Forms.Application.StartupPath & "\Log") = False Then Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath & "\Log")
            Try
                If FileIO.FileSystem.FileExists(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & pstrTypeLog & ".log") = True Then
                    If FileLen(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & pstrTypeLog & ".log") > 10000000 Then
                        FileIO.FileSystem.RenameFile(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & pstrTypeLog & ".log", Format(Today.Date, "dd-MM-yy") & "_" & Now.Hour.ToString.PadLeft(2, CChar("00")) & Now.Minute.ToString.PadLeft(2, CChar("00")) & "_" & pstrTypeLog & ".log")
                    End If
                End If
            Catch

            End Try
            'dtLastTime = Now
            'End If

            objReader = New StreamWriter(Application.StartupPath & "\Log\" & Format(Today.Date, "dd-MM-yy") & "_" & pstrTypeLog & ".log", True)
            If ModuleName <> "" Then
                objReader.WriteLine(Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + ", ModuleName: " + ModuleName + ", " + vbTab + "ErrorNo: " + ErrNumber.ToString() + ", " + ErrorMsg)
            End If
            Return True
        Catch
            'Call LogError("ControlInterface.clsTCPFEP", "DebugLog", Err.Number, ex)
            Return False
        Finally
            Try
                objReader.Close()
            Catch
            End Try
            Try
                objReader.Dispose()
            Catch
            End Try
            Try
                objReader = Nothing
            Catch
            End Try
            'Application.DoEvents() '--- added by parimal thakkar dated: 14-Mar-2009
        End Try
    End Function

    Public Function DebugLog_Barcode(ByVal pstrMessage As String, ByVal pstrTypeLog As String) As Boolean
        '***********************************************************
        '* Name      : DebugLog
        '* Remark    : Maintain the Debug log
        '* Arguments : ByVal pstrMessage As String
        '* Returns   : Boolean
        '* References:
        '* Author    : parimal thakkar
        '* Date      : 23-Jan-2009
        '***********************************************************
        '************************History Section***********************************
        'Date           Developer           Comments
        '23-Jan-2009    Parimal Thakkar     Initial Creation
        '**************************************************************************
        'SyncLock _synchLock
        Dim bAns As Boolean = False
        Dim objReader As StreamWriter = Nothing
        Try

            If Directory.Exists(System.Windows.Forms.Application.StartupPath & "\Barcode") = False Then
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath & "\Barcode")
            End If
            objReader = New StreamWriter(System.Windows.Forms.Application.StartupPath & "\Barcode\" & pstrTypeLog & ".log", True)
            If pstrMessage <> "" Then
                objReader.WriteLine(pstrMessage)
            End If
        Catch ex As Exception
            Call ErrorLog("modGeneral.DebugLog", ex)
        Finally

            Try
                objReader.Close()
            Catch ex As Exception
            End Try
            Try
                objReader.Dispose()
            Catch ex As Exception
            End Try
            Try
                objReader = Nothing
            Catch ex As Exception
            End Try
            ''Application.DoEvents() '--- added by parimal thakkar dated: 14-Mar-2009
        End Try
        'End SyncLock
    End Function

End Module
