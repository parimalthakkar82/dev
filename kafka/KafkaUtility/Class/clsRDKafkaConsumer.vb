
Option Strict Off
Option Explicit On
Imports System.Text
Imports RdKafka
Imports System.Threading
Imports System.Threading.Tasks
Public Class clsRDKafkaConsumer

#Region "Internal variable"
    Private Const Const_kafkaGroupID As String = "LG_Consumer"
    Private rdconsumer As EventConsumer
    Private rdconfig As Config
    Private _rdConsuerStarted As Boolean = False
    Private _BrokerList As String = ""
    Private _Topics As String
    Private _EnableAutoCommit As Boolean = True
    Private _Commit_BlukMsg_Count As Int32 = 10
    Private _KafkaGroupID As String
#End Region


#Region "Events"
    Public Event KafkaMessage(ByVal msg As String, ByVal Topic As String, ByVal Partition As Integer, ByVal offset As Long)
    Public Event KafkaConsumerStarted()
    Public Event KafkaConsumerStopped()
    Public Event ErrorRaised(ByVal ErrorCode As String, ErrorReason As String)
#End Region


#Region "Properties"
    Public Property BrokerList As String
        Get
            Return _BrokerList
        End Get
        Set(value As String)
            _BrokerList = value
        End Set
    End Property

    Public Property TopicList As String
        Get
            Return _Topics
        End Get
        Set(value As String)
            _Topics = value
        End Set
    End Property
    Public Property EnableAutoCommit As Boolean
        Get
            Return _EnableAutoCommit
        End Get
        Set(value As Boolean)
            _EnableAutoCommit = True
        End Set
    End Property


    Public Property Commit_BlukMsg_Count As Int32
        Get
            Return _Commit_BlukMsg_Count
        End Get
        Set(value As Int32)
            If value < 2 Then
                value = 2
            End If
            _Commit_BlukMsg_Count = value

        End Set
    End Property

    Public ReadOnly Property ConsuerStarted As Boolean
        Get
            Return _rdConsuerStarted
        End Get
    End Property

    Public Property KafkaGroupID As String
        Get
            If _KafkaGroupID = "" Then
                Return Const_kafkaGroupID
            Else
                Return _KafkaGroupID
            End If
        End Get
        Set(value As String)
            If value.Trim = "" Then
                value = Const_kafkaGroupID
            End If
            _KafkaGroupID = value
        End Set
    End Property


#End Region

    Public Sub ConnectConsumer(ByVal Topics As String, ByVal Brokers As String)
        ' KafkaGroupID = Const_kafkaGroupID
        BrokerList = Brokers
        TopicList = Topics
        ConnectConsumer()
    End Sub

    Public Sub ConnectConsumer()
        Try
            'Dim cconfig As New TopicConfig
            'cconfig("max.poll.interval.ms") = 50000
            rdconfig = New Config With {.GroupId = KafkaGroupID, .EnableAutoCommit = EnableAutoCommit}
            rdconsumer = New EventConsumer(rdconfig, BrokerList)

            rdconsumer.Subscribe(New List(Of String) From {TopicList})
            rdconsumer.Start()
            _rdConsuerStarted = True
            RaiseEvent KafkaConsumerStarted()

            AddHandler rdconsumer.OnMessage, Function(obj, Message) GetMessages(obj, Message)
            AddHandler rdconsumer.OnError, Function(obj, e) KafkaError(obj, e)
        Catch ex As Exception
            RaiseEvent ErrorRaised(ex.Source, ex.Message)
        End Try

    End Sub

    Public Sub Dispose()
        Try
            RemoveHandler rdconsumer.OnMessage, Function(obj, Message) GetMessages(obj, Message)
            RemoveHandler rdconsumer.OnError, Function(obj, e) KafkaError(obj, e)
            Thread.Sleep(TimeSpan.FromMilliseconds(500))
        Catch ex As Exception
        End Try

        Try
            rdconsumer.Stop()
            Thread.Sleep(TimeSpan.FromMilliseconds(500))
        Catch ex As Exception
        End Try

        Try
            rdconsumer.Unsubscribe()
            Thread.Sleep(TimeSpan.FromMilliseconds(500))
        Catch ex As Exception
        End Try

        Try
            'RaiseEvent ErrorRaised("rdconsumer.Dispose", "Before rdconsumer.Dispose")
            'rdconsumer.Dispose()
            'RaiseEvent ErrorRaised("rdconsumer.Dispose", "After rdconsumer.Dispose")
            'Thread.Sleep(TimeSpan.FromMilliseconds(500))
        Catch ex As Exception
        End Try
        Try
            rdconsumer = Nothing
        Catch ex As Exception
        End Try
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub



    Public Sub StopKafkaConsumer()
        If _rdConsuerStarted = True Then
            rdconsumer.Stop()
            RaiseEvent KafkaConsumerStopped()
            _rdConsuerStarted = False
        End If
    End Sub

    Public Sub StartKafkaConsumer()
        If _rdConsuerStarted = False Then
            rdconsumer.Start()
            RaiseEvent KafkaConsumerStarted()
            _rdConsuerStarted = True
        End If

    End Sub

    Public Function GetMessages(obj As Object, msg As Message) As Boolean
        Try
            Dim text = Encoding.UTF8.GetString(msg.Payload, 0, msg.Payload.Length)
            RaiseEvent KafkaMessage(Encoding.UTF8.GetString(msg.Payload, 0, msg.Payload.Length), msg.Topic, msg.Partition, msg.Offset)
            If (Not EnableAutoCommit) And (msg.Offset Mod Commit_BlukMsg_Count = 0) Then
                rdconsumer.Commit(msg).Wait()
            End If
        Catch ex As Exception
            RaiseEvent ErrorRaised(ex.Source, ex.Message)
        End Try
        Return True
    End Function

    Public Function KafkaError(obj As Object, e As Handle.ErrorArgs) As Boolean
        DebugLog("KafkaError Found. ErrorCode:" & e.ErrorCode & "; Reason:" & e.Reason.ToString)
        RaiseEvent ErrorRaised(e.ToString(), e.Reason.ToString())
        Return True
    End Function

End Class


'consumer.OnPartitionsAssigned += (obj, partitions) >= {
'    Console.WriteLine($"Assigned partitions: [{String.Join(", ", partitions)}], member id: {consumer.MemberId}");
'    var fromBeginning = partitions.Select(p >= New TopicPartitionOffset(p.Topic, p.Partition, Offset.Beginning)).ToList();
'    Console.WriteLine($"Updated assignment: [{String.Join(", ", fromBeginning)}]");
'    consumer.Assign(fromBeginning);
'};

'AddHandler consumer.OnEndReached, Function(obj, e) KafkaOnEndReached(obj, e)
'AddHandler consumer.OnOffsetCommit, Function(obj, e) KafkaOnOffsetCommit(obj, e)
' AddHandler consumer.OnPartitionsAssigned, Function(obj, e) KafkaOnPartitionsAssigned(obj, e)
'AddHandler consumer.OnPartitionsRevoked, Function(obj, e) KafkaOnPartitionsRevoked(obj, e)
'AddHandler consumer.OnStatistics, Function(obj, e) KafkaOnStatistics(obj, e)
'AddHandler consumer.OnConsumerError, Function(obj, e) KafkaConsumerError(obj, e)
'consumer.Subscribe(New List(Of String) From {strTopic})