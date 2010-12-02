'Public Class FileTest
'    Public Shared Sub Test()
'        Dim writer = New IO.StringWriter()
'        writer.Write("Bla ")
'        writer.WriteLine("1 2 3")
'        writer.WriteLine("4 5 6")


'        Dim path = "C:\Users\jgw\Desktop\SchüA Visual Studio Projekte\Abc.txt"
'        IO.File.WriteAllText(path, writer.ToString)

'        Dim readFileString As String = IO.File.ReadAllText(path)


'        Dim reader = New IO.StringReader(readFileString)

'        If reader.ReadLine() <> "Bla 1 2 3" Then
'            Stop
'        End If
'        If reader.ReadLine() <> "4 5 6" Then
'            Stop
'        End If

'        If Not String.IsNullOrWhiteSpace(reader.ReadToEnd) Then
'            Stop
'        End If

'    End Sub
'End Class
