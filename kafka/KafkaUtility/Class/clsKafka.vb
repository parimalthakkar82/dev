Option Strict Off

Imports System.Text
Imports System.Data
Imports System
Imports KafkaNet.Model
Imports KafkaNet.Protocol
Imports KafkaNet
Imports System.Threading
Imports System.Threading.Tasks

Public Class clsKafka

    Dim KafkaConsumer As KafkaNet.Consumer
    Dim options As KafkaOptions
    Dim router As BrokerRouter
    Dim KafkaProducer As Producer
    Dim kafkaMSG As KafkaNet.Protocol.Message

    Dim taskReceive As Task
    Dim taskReceiveCancel As New CancellationTokenSource()
    Dim blnPublisherConnected As Boolean = False
    Dim blnConsumerConnected As Boolean = False

    Public Enum KafkaOffsetPosition
        FromFirst = 0
        LastRead = 1
        Latest = 2
    End Enum

    Dim _KafkaURL As String
    Dim _KafkaTopic As String
    Dim _KafkaThreadWaitTime As Int32
    Dim _OffsetPosition As KafkaOffsetPosition = KafkaOffsetPosition.FromFirst

#Region "Events"
    Public Event KafkaPubliserConnected(ByVal URL As String, ByVal Topic As String)
    Public Event KafkaConsumerConnected(ByVal URL As String, ByVal Topic As String)
    Public Event KafkaMessage(ByVal Message As String)
    Public Event ErrorRaised(ByVal ErrorMsg As String)
    Public Event MSGSent(ByVal URL As String, ByVal Topic As String, ByVal msg As String)
#End Region

#Region "Properties"

    Public ReadOnly Property KafkaURL() As String
        Get
            Return _KafkaURL
        End Get
    End Property
    Public ReadOnly Property KafkaTopic() As String
        Get
            Return _KafkaTopic
        End Get
    End Property

    Public Property KafkaThreadWaitTime() As Int32
        Get
            Return _KafkaThreadWaitTime
        End Get
        Set(value As Int32)
            _KafkaThreadWaitTime = value
        End Set
    End Property
    Public ReadOnly Property GetOffsetPosition As KafkaOffsetPosition
        Get
            Return _OffsetPosition
        End Get
    End Property
    Public WriteOnly Property SetOffsetPosition As KafkaOffsetPosition
        Set(value As KafkaOffsetPosition)
            _OffsetPosition = value
        End Set
    End Property

#End Region

    Public Function isKafkaConnected(Optional ByVal KafkaConnectionTimeout As Integer = 10) As Boolean
        Dim blnReturn As Boolean = False

        Try
            'Dim xoptions = New KafkaOptions(New Uri(_KafkaURL))      '"http://172.35.5.12:9092"
            'xoptions.ResponseTimeoutMs = TimeSpan.FromSeconds(2)
            'xoptions.MaximumReconnectionTimeout = TimeSpan.FromSeconds(2.5)
            'Dim xrouter = New BrokerRouter(xoptions)
            'Dim x = xrouter.SelectBrokerRoute("LG-Plugin-ssLight-Data", 0).Connection
            Dim x = Nothing
            Dim xtoken As New CancellationTokenSource()
            ' Dim xtsk = New Task()
            Dim reporter As Task
            reporter = New Task(Function()
                                    Dim xoptions = New KafkaOptions(New Uri(_KafkaURL))      '"http://172.35.5.12:9092"
                                    xoptions.ResponseTimeoutMs = TimeSpan.FromSeconds(2)
                                    xoptions.MaximumReconnectionTimeout = TimeSpan.FromSeconds(2.5)
                                    Dim xrouter = New BrokerRouter(xoptions)
                                    x = xrouter.SelectBrokerRoute(_KafkaTopic, 0).Connection

                                End Function, xtoken)

            reporter.Start()
            reporter.Wait(TimeSpan.FromSeconds(KafkaConnectionTimeout))
            xtoken.Cancel(False)

            If x IsNot Nothing Then
                blnReturn = True
            End If


            'Dim xoptions = New KafkaOptions(New Uri("http://172.35.5.12:9092"))
            'Dim xrouter = New BrokerRouter(xoptions)
            'options.MaximumReconnectionTimeout = TimeSpan.FromSeconds(2.5)
            'options.ResponseTimeoutMs = TimeSpan.FromSeconds(15)
            '''xrouter._brokerConnectionIndex.count
            'Dim endpoint = New DefaultKafkaConnectionFactory().Resolve(xoptions.KafkaServerUri.First(), xoptions.Log)
            'Dim s As New KafkaTcpSocket(New DefaultTraceLog(), endpoint)


            'Dim sendpoint = New DefaultKafkaConnectionFactory().Resolve(options.KafkaServerUri.First(), options.Log)
            'Dim ss As New KafkaTcpSocket(New DefaultTraceLog(), sendpoint)


        Catch ex As Exception
            RaiseEvent ErrorRaised(ex.Message)
            ' ErrorLog(ex.TargetSite.DeclaringType.Name, New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, 0, " Error :" & ex.ToString(), ex.StackTrace.ToString())
        End Try
        Return blnReturn
    End Function
    Public Function ConnectPublisher(ByVal URL As String, ByVal Topic As String) As Boolean
        Dim blnReturn As Boolean = False
        Try

            _KafkaURL = URL
            _KafkaTopic = Topic

            If isKafkaConnected(3) = False Then
                Exit Try
            End If
            If options Is Nothing Then
                options = New KafkaOptions(New Uri(_KafkaURL))
                options.MaximumReconnectionTimeout = TimeSpan.FromSeconds(2.5)
                options.ResponseTimeoutMs = TimeSpan.FromSeconds(15)
                router = New BrokerRouter(options)
                KafkaProducer = New Producer(router)
            Else
                options = New KafkaOptions(New Uri(_KafkaURL))
                router = New BrokerRouter(options)
                KafkaProducer = New Producer(router)
            End If


            RaiseEvent KafkaPubliserConnected(_KafkaURL, _KafkaTopic)
            blnPublisherConnected = True
            blnReturn = True
        Catch ex As Exception
            RaiseEvent ErrorRaised(ex.Message)
            ' ErrorLog(ex.TargetSite.DeclaringType.Name, New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, 0, " Error :" & ex.ToString(), ex.StackTrace.ToString())
        End Try
        Return blnReturn
    End Function
    Public Function ConnectConsumer(ByVal URL As String, ByVal Topic As String) As Boolean
        Dim blnReturn As Boolean = False
        Try
            _KafkaURL = URL
            _KafkaTopic = Topic

            If isKafkaConnected(3) = False Then
                Exit Try
            End If
            If options Is Nothing Then
                options = New KafkaOptions(New Uri(_KafkaURL))
                'options.MaximumReconnectionTimeout = TimeSpan.FromSeconds(2.5)
                'options.ResponseTimeoutMs = TimeSpan.FromSeconds(1)
                router = New BrokerRouter(options)

                KafkaConsumer = New Consumer(New ConsumerOptions(_KafkaTopic, router))
                If GetOffsetPosition = KafkaOffsetPosition.Latest Then
                    Dim offset = KafkaConsumer.GetTopicOffsetAsync(_KafkaTopic, 10000000).Result
                    Dim objOffset = From x In offset Select New OffsetPosition(x.PartitionId, x.Offsets.Max())
                    KafkaConsumer.SetOffsetPosition(objOffset.ToArray())
                End If
            Else
                options = New KafkaOptions(New Uri(_KafkaURL))
                router = New BrokerRouter(options)
                KafkaConsumer = New Consumer(New ConsumerOptions(_KafkaTopic, router))
            End If
            RaiseEvent KafkaConsumerConnected(_KafkaURL, _KafkaTopic)
            blnConsumerConnected = True
            blnReturn = True
        Catch ex As Exception
            RaiseEvent ErrorRaised(ex.Message)
            ' ErrorLog(ex.TargetSite.DeclaringType.Name, New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, 0, " Error :" & ex.ToString(), ex.StackTrace.ToString())
        End Try
        Return blnReturn
    End Function

    'Private Sub ConnectKafka()
    '    'Dim offsetPositions As OffsetPosition() = New OffsetPosition() {New OffsetPosition() With {.Offset = 0, .PartitionId = 0}}
    '    'Dim offsetPositions As OffsetPosition() = New OffsetPosition() {New OffsetPosition() With {.Offset = 0}}   'will start message from 0
    '    'Dim offsetPositions As IEnumerable(Of OffsetPosition)

    '    Try
    '        If options Is Nothing Then
    '            options = New KafkaOptions(New Uri(_KafkaURL))
    '            options.MaximumReconnectionTimeout = TimeSpan.FromSeconds(2.5)
    '            options.ResponseTimeoutMs = TimeSpan.FromSeconds(15)
    '            router = New BrokerRouter(options)

    '            KafkaConsumer = New Consumer(New ConsumerOptions(_KafkaTopic, router))
    '            If GetOffsetPosition = KafkaOffsetPosition.Latest Then
    '                Dim offset = KafkaConsumer.GetTopicOffsetAsync(_KafkaTopic, 10000000).Result
    '                Dim objOffset = From x In offset Select New OffsetPosition(x.PartitionId, x.Offsets.Max())
    '                KafkaConsumer.SetOffsetPosition(objOffset.ToArray())
    '            End If
    '            KafkaProducer = New Producer(router)
    '            blnKafkaConnected = True
    '        Else
    '            options = New KafkaOptions(New Uri(_KafkaURL))
    '            router = New BrokerRouter(options)
    '            KafkaConsumer = New Consumer(New ConsumerOptions(_KafkaTopic, router))
    '            KafkaProducer = New Producer(router)
    '        End If

    '        RaiseEvent KafkaConnected(_KafkaURL, _KafkaTopic)
    '        blnKafkaConnected = True
    '    Catch ex As Exception
    '        RaiseEvent ErrorRaised(ex.Message)
    '        ' ErrorLog(ex.TargetSite.DeclaringType.Name, New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, 0, " Error :" & ex.ToString(), ex.StackTrace.ToString())
    '    End Try
    'End Sub
    Public Sub StopMessages()
        Try
            blnConsumerConnected = False
            taskReceiveCancel.Cancel(False)
            Thread.Sleep(100)
        Catch ex As Exception
            RaiseEvent ErrorRaised(ex.Message)
            ' ErrorLog(ex.TargetSite.DeclaringType.Name, New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, 0, " Error :" & ex.ToString(), ex.StackTrace.ToString())
        End Try
    End Sub
    Public Sub StartMessages()
        Try
            If blnConsumerConnected = True Then
                taskReceive = New Task(AddressOf ReceiveDataToken, taskReceiveCancel.Token)
                taskReceive.Start()
            End If
        Catch ex As Exception
            RaiseEvent ErrorRaised(ex.Message)
            ' ErrorLog(ex.TargetSite.DeclaringType.Name, New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, 0, " Error :" & ex.ToString(), ex.StackTrace.ToString())
        End Try
    End Sub

    Private Sub ReceiveDataToken(CancelToken As CancellationToken)
        Dim msg As Protocol.Message = Nothing
        While True
            Try
                If CancelToken.IsCancellationRequested = True Then
                    DebugLog("Abort: ReceiveDataToken Thread")
                    Exit While
                End If
                Try
                    For Each msg In KafkaConsumer.Consume(CancelToken)
                        RaiseEvent KafkaMessage(Encoding.UTF8.GetString(msg.Value))
                        ' Dim obj As New OffsetPosition(msg.Meta.PartitionId, msg.Meta.Offset)
                        ' KafkaConsumer.SetOffsetPosition(obj)
                    Next
                Catch ex As Exception
                    KafkaConsumer.Dispose()
                    Thread.Sleep(100)
                    ConnectConsumer(_KafkaURL, _KafkaTopic)
                End Try

            Catch ex As Exception
                RaiseEvent ErrorRaised(ex.Message)
                '  ErrorLog(ex.TargetSite.DeclaringType.Name, New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, 0, " Error :" & ex.ToString(), ex.StackTrace.ToString())
            End Try
            taskReceive.Wait(_KafkaThreadWaitTime)
        End While
    End Sub

    Public Function SendMessage(ByVal message As String) As Boolean
        Dim blnReturn As Boolean = False
        Try
            If _KafkaTopic Is Nothing Or _KafkaTopic = "" Then
                RaiseEvent ErrorRaised("Kafka Topic not set")
                Return False
            End If
            Dim m As New Concurrent.ConcurrentQueue(Of KafkaNet.Protocol.Message)
            kafkaMSG = New KafkaNet.Protocol.Message(message)
            m.Enqueue(kafkaMSG)
            Dim response = KafkaProducer.SendMessageAsync(_KafkaTopic, m.AsEnumerable) ' CType(msg, IEnumerable(Of Protocol.Message)))
            blnReturn = True
            RaiseEvent MSGSent(_KafkaURL, _KafkaTopic, message)
        Catch ex As Exception
            'Catch ex1 As KafkaNet.Protocol.ResponseTimeoutException
            RaiseEvent ErrorRaised(ex.Message)
            '  ErrorLog(ex.TargetSite.DeclaringType.Name, New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, 0, " Error :" & ex.ToString(), ex.StackTrace.ToString())
        End Try
        Return blnReturn
    End Function
    Public Sub Disconnect()
        Try
            StopMessages()
            Try
                If blnConsumerConnected = True Then
                    KafkaConsumer.Dispose()
                End If
                KafkaConsumer = Nothing
            Catch ex As Exception

            End Try
            Try
                If blnPublisherConnected = True Then
                    KafkaProducer.Stop(True, TimeSpan.FromSeconds(1))
                    KafkaProducer.Dispose()
                End If
                KafkaProducer = Nothing
            Catch ex As Exception

            End Try
            Try
                router.Dispose()
                router = Nothing
            Catch ex As Exception
            End Try

            options = Nothing
            kafkaMSG = Nothing
        Catch ex As Exception
            RaiseEvent ErrorRaised(ex.Message)
            '  ErrorLog(ex.TargetSite.DeclaringType.Name, New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, 0, " Error :" & ex.ToString(), ex.StackTrace.ToString())
        End Try
    End Sub
    'https://csharp.hotexamples.com/examples/KafkaNet.Model/KafkaOptions/-/php-kafkaoptions-class-examples.html
    ' https://csharp.hotexamples.com/examples/KafkaNet/Consumer/GetOffsetPosition/php-consumer-getoffsetposition-method-examples.html
    'https://csharp.hotexamples.com/examples/KafkaNet/Consumer/Consume/php-consumer-consume-method-examples.html
End Class
