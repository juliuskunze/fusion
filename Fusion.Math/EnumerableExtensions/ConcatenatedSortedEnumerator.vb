''' <summary>
''' Concatenates ascending sorted enumerators to one ascending sorted enumerator.
''' </summary>
''' <typeparam name="T"></typeparam>
''' <remarks></remarks>
Public Class ConcatenatedSortedEnumerator(Of T)
    Implements IEnumerator(Of T)

    Private Class ActivatableEnumerator
        Public Sub New(enumerator As IEnumerator(Of T))
            Me.Enumerator = enumerator
            _Activated = True
        End Sub

        Public Property Enumerator As IEnumerator(Of T)

        Private _Activated As Boolean
        Public ReadOnly Property Activated As Boolean
            Get
                Return _Activated
            End Get
        End Property

        Public Function MoveNext() As Boolean
            Dim movedNext = Me.Enumerator.MoveNext()
            _Activated = movedNext
            Return movedNext
        End Function

        Public Sub Reset()
            Me.Enumerator.Reset()
            _Activated = True
        End Sub

    End Class

    Private _SourceEnumerators As IEnumerable(Of ActivatableEnumerator)

    Private _CompareValueFunction As Func(Of T, Double)

    Public Sub New(sourceEnumerators As IEnumerable(Of IEnumerator(Of T)), compareValueFunction As Func(Of T, Double))
        'Deleting ".ToList" will cause failing tests.
        _SourceEnumerators = sourceEnumerators.Select(Function(enumerator) New ActivatableEnumerator(enumerator)).ToList
        _CompareValueFunction = compareValueFunction
    End Sub

    Private _Current As T
    Public ReadOnly Property Current As T Implements System.Collections.Generic.IEnumerator(Of T).Current
        Get
            Return _Current
        End Get
    End Property

    Public ReadOnly Property CurrentObj As Object Implements System.Collections.IEnumerator.Current
        Get
            Return Me.Current
        End Get
    End Property

    Private _AlreadyMovedNext As Boolean = False
    Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
        If _AlreadyMovedNext Then
            Me.MoveMinEnumeratorNext()
        Else
            Me.FirstMoveNextAll()
        End If

        If Me.EndPassed Then Return False

        Me.SetMinEnumeratorByAllActivatedCurrent()
        Me.SetCurrentByMinEnumerator()

        Return True
    End Function

    Private Sub SetMinEnumeratorByAllActivatedCurrent()
        _MinEnumerator = (From activatableEnumerator In _SourceEnumerators Where activatableEnumerator.Activated).
            MinItem(Function(activatableEnumerator) _CompareValueFunction.Invoke(activatableEnumerator.Enumerator.Current))
    End Sub

    Private Sub SetCurrentByMinEnumerator()
        _Current = _MinEnumerator.Enumerator.Current
    End Sub


    Private ReadOnly Property EndPassed As Boolean
        Get
            Return _SourceEnumerators.All(Function(sourceEnumerator) Not sourceEnumerator.Activated)
        End Get
    End Property

    Private Sub FirstMoveNextAll()
        For Each activatableEnumerator In _SourceEnumerators
            activatableEnumerator.MoveNext()
        Next

        _AlreadyMovedNext = True
    End Sub

    Private _MinEnumerator As ActivatableEnumerator

    Private Function MoveMinEnumeratorNext() As Boolean
        Return _MinEnumerator.MoveNext()
    End Function

    Public Sub Reset() Implements System.Collections.IEnumerator.Reset
        _AlreadyMovedNext = False

        For Each sourceEnumerator In _SourceEnumerators
            sourceEnumerator.Reset()
        Next
    End Sub

    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            For Each sourceEnumerator In _SourceEnumerators
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
