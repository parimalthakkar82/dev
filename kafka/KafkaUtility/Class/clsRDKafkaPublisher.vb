
Option Strict Off
Option Explicit On
Imports System.Text
Imports RdKafka
Imports System.Threading
Imports System.Threading.Tasks
Public Class clsRDKafkaPublisher

    Dim producer As Producer
    Dim config As Config
    Dim _KafkaBroker As String
    Public Event MessageSent(ByVal Topic As String, msg As String, partition As Integer, offset As Long)
    Public Event ErrorRaised(ByVal ErrorCode As String, ErrorReason As String)
    Public Event OnStatistics(ByVal statics As String)
    Dim col_KafkaTopic As Collection


    Public Sub New()
        col_KafkaTopic = New Collection
    End Sub

    Public Sub Dispose()
        Try
            RemoveHandler producer.OnError, Function(obj, e) KafkaError(obj, e)
            RemoveHandler producer.OnStatistics, Function(obj, e) KafkaStatistics(obj, e)
            Thread.Sleep(TimeSpan.FromMilliseconds(500))
        Catch ex As Exception
        End Try

        Try
            For Each itm As Topic In col_KafkaTopic
                Try
                    itm.Dispose()
                    itm = Nothing
                    Thread.Sleep(TimeSpan.FromMilliseconds(100))
                Catch ex As Exception
                End Try
            Next
        Catch ex As Exception
        End Try

        Try
            producer.Dispose()
            Thread.Sleep(TimeSpan.FromMilliseconds(100))
        Catch ex As Exception
        End Try

        Try
            producer = Nothing
        Catch ex As Exception
        End Try
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Public Function ConnectProducer(kafkatopic As String, ByVal strURL As String) As Boolean
        Try
            If col_KafkaTopic.Contains(Key:=kafkatopic) = False Then
                Dim tp1 As Topic
                _KafkaBroker = strURL
                producer = New Producer(_KafkaBroker)
                tp1 = producer.Topic(kafkatopic)
                col_KafkaTopic.Add(Key:=kafkatopic, Item:=tp1)
                AddHandler producer.OnError, Function(obj, e) KafkaError(obj, e)
                AddHandler producer.OnStatistics, Function(obj, e) KafkaStatistics(obj, e)
            End If
        Catch ex As Exception
            RaiseEvent ErrorRaised(ex.Source, ex.Message)
        End Try
        Return True
    End Function

    Public Function SendMessage(kafkatopic As String, msg As String) As Boolean
        Dim tp1 As Topic
        Try
            If col_KafkaTopic.Contains(Key:=kafkatopic) = True Then
                tp1 = col_KafkaTopic.Item(Key:=kafkatopic)
                Dim data As Byte() = Encoding.UTF8.GetBytes(msg)
                Dim deliveryReport As Task(Of DeliveryReport) = tp1.Produce(data)
                deliveryReport.Wait()
                RaiseEvent MessageSent(kafkatopic, msg, deliveryReport.Result.Partition, deliveryReport.Result.Offset)
            Else
                tp1 = producer.Topic(kafkatopic)
                col_KafkaTopic.Add(Key:=kafkatopic, Item:=tp1)
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

    Public Function KafkaStatistics(obj As Object, statics As String) As Boolean
        Try
            RaiseEvent OnStatistics(statics)
        Catch ex As Exception
            RaiseEvent ErrorRaised(ex.Source, ex.Message)
        End Try
        Return True
    End Function

End Class
'https://github.com/ah-/rdkafka-dotnet/blob/master/examples/Misc/Program.cs

'Imports System
'Imports System.Text
'Imports System.Threading.Tasks
'Imports RdKafka

'Namespace SimpleProducer
'    Public Class Program
'        Public Shared Sub Main(ByVal args As String())
'            Dim brokerList As String = args(0)
'            Dim topicName As String = args(1)

'            Using producer As Producer = New Producer(brokerList)

'                Using topic As Topic = producer.Topic(topicName)
'                    Console.WriteLine($"{producer.Name} producing on {topic.Name}. q to exit.")
'                    Dim text As String

'                    While (CSharpImpl.__Assign(text, Console.ReadLine())) <> "q"
'                        Dim data As Byte() = Encoding.UTF8.GetBytes(text)
'                        Dim deliveryReport As Task(Of DeliveryReport) = topic.Produce(data)
'                        Dim unused = deliveryReport.ContinueWith(Sub(task)
'                                                                     Console.WriteLine($"Partition: {task.Result.Partition}, Offset: {task.Result.Offset}")
'                                                                 End Sub)
'                    End While
'                End Using
'            End Using
'        End Sub

'        Private Class CSharpImpl
'            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
'            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
'                target = value
'                Return value
'            End Function
'        End Class
'    End Class
'End Namespace