Imports System.ComponentModel

Public Class frmSubscriber

    Private WithEvents objKafka As New clsRDKafkaConsumer

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Try
            If txtURL.Text.Trim = "" Then
                MessageBox.Show("Kafka Broker is mandatory.")
                txtURL.Focus()
                Exit Try
            End If
            If txtTopic.Text.Trim = "" Then
                MessageBox.Show("Kafka Topic is mandatory.")
                txtTopic.Focus()
                Exit Try
            End If
            If txtGroup.Text.Trim = "" Then
                MessageBox.Show("Kafka Group is mandatory.")
                txtGroup.Focus()
                Exit Try
            End If
            objKafka.KafkaGroupID = txtGroup.Text

            objKafka.ConnectConsumer(txtTopic.Text.Trim, txtURL.Text)


        Catch ex As Exception
            MessageBox.Show(text:="Error in btnConnect_Click " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub objKafka_ErrorRaised(ErrorCode As String, ErrorReason As String) Handles objKafka.ErrorRaised
        Try
            ' objKafka.StopKafkaConsumer()
            txtStatus.Text = txtStatus.Text & vbCrLf & "Error Raised in Kafka, Error:" & ErrorCode & "; Error Reason:" & ErrorReason
        Catch ex As Exception
            MessageBox.Show(text:="Error in objKafka_ErrorRaised " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub objKafka_KafkaMessage(msg As String, Topic As String, Partition As Integer, offset As Long) Handles objKafka.KafkaMessage
        Try
            txtStatus.Text = txtStatus.Text & vbCrLf & "Msg:" & msg
        Catch ex As Exception
            MessageBox.Show(text:="Error in objKafka_KafkaMessage " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmSubscriber_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            CheckForIllegalCrossThreadCalls = False
        Catch ex As Exception
            MessageBox.Show(text:="Error in frmSubscriber_Load " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmSubscriber_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            objKafka.Dispose()
            objKafka = Nothing
        Catch ex As Exception
            MessageBox.Show(text:="Error in frmSubscriber_Load " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Stop" Then
            objKafka.StopKafkaConsumer()
            Button1.Text = "Start"
        Else
            objKafka.StartKafkaConsumer()
            Button1.Text = "Stop"
        End If
    End Sub

    Private Sub objKafka_KafkaConsumerStarted() Handles objKafka.KafkaConsumerStarted
        txtStatus.Text = txtStatus.Text & vbCrLf & " Kafka Messages Started"
        btnConnect.Enabled = False
        Button1.Text = "Stop"
    End Sub

    Private Sub objKafka_KafkaConsumerStopped() Handles objKafka.KafkaConsumerStopped
        txtStatus.Text = txtStatus.Text & vbCrLf & " Kafka Messages Stopped"
        Button1.Text = "Start"
    End Sub
End Class

'objKafka.SetOffsetPosition = clsKafka.KafkaOffsetPosition.FromFirst
'If objKafka.ConnectConsumer(txtURL.Text.Trim, txtTopic.Text.Trim) = False Then
'    MessageBox.Show(text:="Unable to connect Kafka server '" & txtURL.Text & "'", caption:="Kafka Connect", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Exclamation)
'    Exit Try
'End If

'objKafka.StartMessages()
'Private Sub frmSubscriber_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
'    Try
'        objKafka.Disconnect()
'    Catch ex As Exception
'        MessageBox.Show(text:="Error in objKafka_KafkaMessage " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
'    End Try
'End Sub
'Private Sub objKafka_ErrorConsumerRaised(ErrorMsg As String, errorcode As String) Handles objKafka.ErrorConsumerRaised
'    Try
'        objKafka.StopKafkaConsumer()
'    Catch ex As Exception
'        MessageBox.Show(text:="Error in objKafka_KafkaMessage " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
'    End Try

'End Sub
'Private Sub objKafka_KafkaConnected(URL As String, Topic As String) Handles objKafka.KafkaConsumerConnected
'    Try
'        txtTopic.Enabled = False
'        txtURL.Enabled = False
'        btnConnect.Enabled = False
'        txtStatus.Text = "Consumer Connected successfully"
'    Catch ex As Exception
'        MessageBox.Show(text:="Error in objKafka_KafkaConnected " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
'    End Try
'End Sub