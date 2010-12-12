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
            _activated = True
        End Sub

        Public Property Enumerator As IEnumerator(Of T)

        Private _activated As Boolean
        Public ReadOnly Property Activated As Boolean
            Get
                Return _activated
            End Get
        End Property

        Public Function MoveNext() As Boolean
            Dim movedNext = Me.Enumerator.MoveNext()
            _activated =  movedNext 
            Return movedNext
        End Function

        Public Sub Reset()
            Me.Enumerator.Reset()
            _activated = True
        End Sub

    End Class

    Private _sourceEnumerators As IEnumerable(Of ActivatableEnumerator)

    Private _compareValueFunction As Func(Of T, Double)

    Public Sub New(ByVal sourceEnumerators As IEnumerable(Of IEnumerator(Of T)), ByVal compareValueFunction As Func(Of T, Double))
        'WHY THAT?
        _sourceEnumerators = sourceEnumerators.Select(Function(enumerator) New ActivatableEnumerator(enumerator)).ToList
        _compareValueFunction = compareValueFunction
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
            Me.FirstMoveNextAll()

            Me.SetMinEnumeratorByAllActivatedCurrent()

            Me.SetCurrentByMinEnumerator()

            Return Me.MoveAnyMinEnumeratorNext()
        End If

        Me.SetCurrentByMinEnumerator()

        Return Me.MoveAnyMinEnumeratorNext()
    End Function

    Private Function MoveAnyMinEnumeratorNext() As Boolean
        Do Until Me.MoveMinEnumeratorNext()
            Me.SetMinEnumeratorByAllActivatedCurrent()

            If Me.EndPassed Then Return False
        Loop

        Return True
    End Function

    Private Sub SetMinEnumeratorByAllActivatedCurrent()
        _minEnumerator = (From activatableEnumerator In _sourceEnumerators Where activatableEnumerator.Activated).
            MinItem(Function(activatableEnumerator) _compareValueFunction.Invoke(activatableEnumerator.Enumerator.Current))
    End Sub

    Private Sub SetCurrentByMinEnumerator()
        _current = _minEnumerator.Enumerator.Current
    End Sub


    Private ReadOnly Property EndPassed As Boolean
        Get
            Return _sourceEnumerators.All(Function(sourceEnumerator) Not sourceEnumerator.Activated)
        End Get
    End Property

    Private Sub FirstMoveNextAll()
        For Each activatableEnumerator In _sourceEnumerators
            activatableEnumerator.MoveNext()
        Next

        _alreadyMovedNext = True
    End Sub

    Private _minEnumerator As ActivatableEnumerator

    Private Function MoveMinEnumeratorNext() As Boolean
        Return _minEnumerator.MoveNext()
    End Function

    Public Sub Reset() Implements System.Collections.IEnumerator.Reset
        _alreadyMovedNext = False

        For Each sourceEnumerator In _sourceEnumerators
            sourceEnumerator.Reset()
        Next
    End Sub

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
