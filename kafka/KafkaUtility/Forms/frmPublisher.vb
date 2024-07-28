Public Class frmPublisher
    Private WithEvents objKafka As New clsRDKafkaPublisher

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
            objKafka.ConnectProducer(txtTopic.Text, txtURL.Text)
            btnConnect.Enabled = False
            'If objKafka.ConnectPublisher(txtURL.Text.Trim, txtTopic.Text.Trim) = False Then
            '    MessageBox.Show(text:="Unable to connect Kafka server '" & txtURL.Text & "'", caption:="Kafka Connect", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Exclamation)
            'End If

        Catch ex As Exception
            MessageBox.Show(text:="Error in btnConnect_Click " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub objKafka_KafkaPubliserConnected(URL As String, Topic As String) Handles objKafka.KafkaPubliserConnected
    '    Try
    '        txtTopic.Enabled = False
    '        txtURL.Enabled = False

    '        btnConnect.Enabled = False
    '        lblMessage.Text = "Publiser Connected successfully"
    '    Catch ex As Exception
    '        MessageBox.Show(text:="Error in objKafka_KafkaConnected " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
    '    End Try
    'End Sub

    'Private Sub objKafka_ErrorRaised(ErrorMsg As String) Handles objKafka.ErrorRaised
    '    Try
    '        lblMessage.Text = "Error Raised in Kafka, Error:" & ErrorMsg
    '    Catch ex As Exception
    '        MessageBox.Show(text:="Error in objKafka_ErrorRaised " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
    '    End Try
    'End Sub

    'Private Sub objKafka_MSGSent(URL As String, Topic As String, msg As String) Handles objKafka.MSGSent
    '    Try
    '        lblMessage.Text = "Message '" & Microsoft.VisualBasic.Left(msg, 25) & "...' Sent on Topic:" & Topic
    '    Catch ex As Exception
    '        MessageBox.Show(text:="Error in objKafka_MSGSent " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
    '    End Try
    'End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try

            objKafka.SendMessage(txtTopic.Text, txtMessage.Text)
            'If objKafka.isKafkaConnected() = True Then
            '    objKafka.SendMessage(txtMessage.Text)

            'End If
            txtMessage.Text = ""
        Catch ex As Exception
            MessageBox.Show(text:="Error in btnSend_Click " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub objKafka_MessageSent(Topic As String, msg As String, partition As Integer, offset As Long) Handles objKafka.MessageSent
        Try
            lblMessage.Text = "Message '" & Microsoft.VisualBasic.Left(msg, 25) & "...' Sent on Topic:" & Topic & "; Partition:" & partition & "; Offset:" & offset
        Catch ex As Exception
            MessageBox.Show(text:="Error in objKafka_MSGSent " & vbCrLf & "Error : " & ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
        End Try
    End Sub
End Class