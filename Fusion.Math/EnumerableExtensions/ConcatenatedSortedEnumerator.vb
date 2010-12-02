''' <summary>
''' Concatenates ascending sorted enumerators to one ascending sorted enumerator.
''' </summary>
''' <typeparam name="T"></typeparam>
''' <remarks></remarks>
Public Class ConcatenatedSortedEnumerator(Of T)
    Implements IEnumerator(Of T)

    Private Class ActivatableEnumerator
        Public Sub New(ByVal enumerator As IEnumerator(Of T))
            Me.Enumerator = enumerator
            Me.Activated = True
        End Sub

        Public Property Enumerator As IEnumerator(Of T)
        Public Property Activated As Boolean
    End Class

    Private _sourceEnumerators As IEnumerable(Of ActivatableEnumerator)

    Private _compareValueFunction As Func(Of T, Double)

    Public Sub New(ByVal sourceEnumerators As IEnumerable(Of IEnumerator(Of T)), ByVal compareValueFunction As Func(Of T, Double))
        _sourceEnumerators = From enumerator In sourceEnumerators Select New ActivatableEnumerator(enumerator)
    End Sub

    Private _current As T
    Public ReadOnly Property Current As T Implements System.Collections.Generic.IEnumerator(Of T).Current
        Get
            Return _current
        End Get
    End Property

    Public ReadOnly Property CurrentObj As Object Implements System.Collections.IEnumerator.Current
        Get
            Return Me.Current
        End Get
    End Property

    Private _alreadyMovedNext As Boolean = False
    Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
        If Not _alreadyMovedNext Then
            Me.FirstMoveNextInitialize()

            _alreadyMovedNext = True
        End If

        Do While Not Me.MoveNextNotFirst
            If _sourceEnumerators.All(Function(sourceEnumerator) Not sourceEnumerator.Activated) Then Return False
        Loop

        Return True
    End Function

    Public Sub Reset() Implements System.Collections.IEnumerator.Reset
        _alreadyMovedNext = False

        For Each sourceEnumerator In _sourceEnumerators
            sourceEnumerator.Activated = True
            sourceEnumerator.Enumerator.Reset()
        Next
    End Sub

    Private Sub FirstMoveNextInitialize()
        For Each activatableEnumerator In _sourceEnumerators
            If Not activatableEnumerator.Enumerator.MoveNext Then
                activatableEnumerator.Activated = False
            End If
        Next
    End Sub

    Private Function MoveNextNotFirst() As Boolean
        '!!!!!!!!!!!!!!!!!!!!!!!!!!! CURRENT not defined!
        Dim minActivatableEnumerator = (From activatableEnumerator In _sourceEnumerators Where activatableEnumerator.Activated).
            MinItem(Function(activatableEnumerator) _compareValueFunction.Invoke(activatableEnumerator.Enumerator.Current))

        Dim movedNext = minActivatableEnumerator.Enumerator.MoveNext()

        If movedNext Then
            _current = minActivatableEnumerator.Enumerator.Current
        Else
            minActivatableEnumerator.Activated = False
        End If

        Return movedNext
    End Function

    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            For Each sourceEnumerator In _sourceEnumerators
                sourceEnumerator.Enumerator.Dispose()
            Next

            Me.disposedValue = True
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

End Class
