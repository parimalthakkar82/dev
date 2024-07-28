Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Ipc

Public Class frmMDI
    Dim KafkaPath As String = ""


    Private Sub OptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.Click
        Dim objAccess As New clsAccess
        Dim objReader As StreamReader
        objReader = New StreamReader("D:\Utility\Kafka POC\Subscriber\Subscriber\bin\Debug\SQLStatements.txt")
        Dim strsql As String = objReader.ReadToEnd()
        objAccess.ExecuteSQLScript(strsql)

        'Dim p As Process = New Process()
        'p.StartInfo.UseShellExecute = False
        'p.StartInfo.RedirectStandardInput = True
        'p.StartInfo.RedirectStandardOutput = True
        'p.StartInfo.FileName = "cmd.exe"
        'p.StartInfo.CreateNoWindow = True
        'p.Start()
        'Dim wr As System.IO.StreamWriter = p.StandardInput
        'Dim rr As System.IO.StreamReader = p.StandardOutput
        'wr.Write("dir" & vbLf)
        'Dim rrlen = Private Sub New(ByVal _ As _, ByVal _ As BindingFlags.NonPublic, ByVal _ As BindingFlags.Instance, ByVal _ As BindingFlags.DeclaredOnly)
        'Do While Not rr.EndOfStream
        '    If rr.Then Then


        '    End If

        '    Dim r1 As String = rr.ReadLine
        '    TextBox1.Text = TextBox1.Text & vbCrLf & r1

        'Loop
        'wr.Flush()

        'mnuStartService_Click()
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

    Private Sub mnuKafkaServiceStatus_Click()
        'Dim processlist As Process() = Process.GetProcesses()
        'Dim blnZookeeper As Boolean = False
        'Dim blnKafka As Boolean = False
        'For Each process As Process In processlist
        '    TextBox1.Text = TextBox1.Text & String.Format("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle) & vbCrLf
        '    If process.ProcessName = "cmd" AndAlso (process.MainWindowTitle.Contains("zookeeper.properties")) Then
        '        blnZookeeper = True
        '    End If
        '    If process.ProcessName = "cmd" AndAlso (process.MainWindowTitle.Contains("server.properties")) Then
        '        blnKafka = True
        '    End If
        'Next
        'If blnZookeeper = True AndAlso blnKafka = True Then
        '    MessageBox.Show("Kafka Service running in command mode.")
        'End If
    End Sub

    Private Sub mnuStartService_Click()
        Try
            Dim pid As Integer
            Dim proc As Process
            pid = ExecuteCommandPrompt("", False)
            proc = Process.GetProcessById(pid)
            proc.StartInfo.RedirectStandardInput = True
            proc.StartInfo.RedirectStandardOutput = True
            proc.StandardInput.WriteLine("Dir")
            proc.StandardInput.Close()
            'StartZooKeeperSerivce()
            ' Threading.Thread.Sleep(TimeSpan.FromSeconds(5))
            'StartKafkaService()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub StartZooKeeperSerivce()
        Try
            Dim strPath As String = KafkaPath & "\bin\windows\zookeeper-server-start.bat"
            Dim pr = New Process()
            pr.StartInfo.FileName = strPath
            pr.StartInfo.WorkingDirectory = KafkaPath        ' "D:\Softwares\kafka_2.13-2.6.0"
            pr.StartInfo.UseShellExecute = False
            Threading.Thread.Sleep(2000)
            pr.StartInfo.Arguments = KafkaPath & "\config\zookeeper.properties"
            pr.Start()
            pr.Refresh()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Private Sub StartKafkaService()
        Try
            Dim strPath As String = KafkaPath & "\bin\windows\kafka-server-start.bat"
            Dim pr = New Process()
            pr.StartInfo.FileName = strPath
            pr.StartInfo.WorkingDirectory = KafkaPath
            pr.StartInfo.UseShellExecute = False
            Threading.Thread.Sleep(2000)
            pr.StartInfo.Arguments = KafkaPath & "\config\server.properties"
            pr.Start()

            pr.Refresh()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Function ExecuteCommandPrompt(ByVal cmdtext As String, ByVal showWindow As Boolean) As Integer

        ' Dim ipcCh As New IpcChannel("cmd.exe")
        ' ChannelServices.RegisterChannel(ipcCh, False)

        'Dim obj As SharedInterfaces.ICommunicationService =
        '  DirectCast(Activator.GetObject(GetType(SharedInterfaces.ICommunicationService),
        '  "ipc://IPChannelName/SreeniRemoteObj"), SharedInterfaces.ICommunicationService)
        'obj.SaySomething(txtText.Text)

        '  ChannelServices.UnregisterChannel(ipcCh)


        Dim intCode As Integer = 0
        Try
            Dim p As New Process

            p.StartInfo = New ProcessStartInfo("cmd.exe")
            p.StartInfo.UseShellExecute = False
            p.StartInfo.WorkingDirectory = "d:\"
            p.Start()
            intCode = p.Id
            'p.WaitForExit()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Return intCode
    End Function

    Private Sub MenuStrip_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip.ItemClicked

    End Sub

    Private Sub frmMDI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        KafkaPath = Configuration.ConfigurationManager.AppSettings("KafkaPath")
    End Sub

    Private Sub mnuConsumer_Click(sender As Object, e As EventArgs) Handles mnuConsumer.Click
        'frmMain.Show()
    End Sub

    Private Sub mnuGetTopicList_Click(sender As Object, e As EventArgs) Handles mnuGetTopicList.Click

    End Sub

    Private Sub ConnectServerToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ConnectServerToolStripMenuItem1.Click
        Dim objFrm As frmSubscriber
        Try
            objFrm = New frmSubscriber
            objFrm.MdiParent = Me
            objFrm.Show()
        Catch ex As Exception
            MessageBox.Show(text:="Error in ConnectServerToolStripMenuItem1_Click " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
        Finally
        End Try
    End Sub

    Private Sub ConnectServerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConnectServerToolStripMenuItem.Click
        Dim objFrm As frmPublisher
        Try
            objFrm = New frmPublisher
            objFrm.MdiParent = Me
            objFrm.Show()
        Catch ex As Exception
            MessageBox.Show(text:="Error in ConnectServerToolStripMenuItem_Click " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
        Finally
        End Try
    End Sub

    Private Sub frmMDI_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        End
    End Sub

    Private Sub frmMDI_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        End
    End Sub
End Class
