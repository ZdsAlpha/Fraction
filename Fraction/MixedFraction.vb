Imports System.Numerics
'Underconstruction
Public Structure MixedFraction
    Private _Integer As BigInteger
    Private _Numerator As BigInteger
    Private _Denominator As BigInteger
    Public ReadOnly Property [Integer] As BigInteger
        Get
            Return _Integer
        End Get
    End Property
    Public ReadOnly Property ProperNumerator As BigInteger
        Get
            Return _Numerator
        End Get
    End Property
    Public ReadOnly Property Denominator As BigInteger
        Get
            Return _Denominator
        End Get
    End Property
    Public ReadOnly Property Numerator As BigInteger
        Get
            Return _Integer * _Denominator + _Numerator
        End Get
    End Property
    Public ReadOnly Property IsUndefined As Boolean
        Get
            Return _Denominator = 0
        End Get
    End Property
    Public ReadOnly Property IsNull As Boolean
        Get
            Return _Integer = 0 And _Numerator = 0 And _Denominator = 1
        End Get
    End Property
    Public ReadOnly Property IsUnit As Boolean
        Get
            Return _Integer = 1 And _Numerator = 0 And _Denominator = 1
        End Get
    End Property
    Public ReadOnly Property IsInfinity As Boolean
        Get
            Return _Denominator = 0 And Not _Numerator = 0
        End Get
    End Property
    Public ReadOnly Property IsPositiveInfinity As Boolean
        Get
            Return IsInfinity And _Numerator > 0
        End Get
    End Property
    Public ReadOnly Property IsNegativeInfinity As Boolean
        Get
            Return IsInfinity And _Numerator < 0
        End Get
    End Property
    Public Sub SetValue(Numerator As BigInteger, Denominator As BigInteger)
        SetValue(0, Numerator, Denominator)
    End Sub
    Public Sub SetValue([Integer] As BigInteger, Numerator As BigInteger, Denominator As BigInteger)
        _Integer = [Integer]
        _Numerator = Numerator
        _Denominator = Denominator
        Resolve()
    End Sub
    Private Sub Resolve()
        If _Denominator = 0 Then
            _Integer = 0
            If _Numerator > 0 Then
                _Numerator = 1
            ElseIf _Numerator < 0 Then
                _Numerator = -1
            End If
        Else
            Dim GCD As BigInteger = MixedFraction.GCD(_Numerator, _Denominator)
            _Numerator = _Numerator / GCD
            _Denominator = _Denominator / GCD
            If _Numerator < 0 Or _Denominator < 0 And Not (_Numerator < 0 And _Denominator < 0) Then
                Dim Remainder As BigInteger = 0
                Dim WholePart As BigInteger = BigInteger.DivRem(BigInteger.Abs(_Numerator), BigInteger.Abs(_Denominator), Remainder)
                _Integer -= WholePart + 1
                _Numerator = _Denominator - Remainder
            Else
                Dim Remainder As BigInteger = 0
                Dim WholePart As BigInteger = BigInteger.DivRem(BigInteger.Abs(_Numerator), BigInteger.Abs(_Denominator), Remainder)
                _Integer += WholePart
                _Numerator = Remainder
            End If
        End If
    End Sub
    Private Shared Function GCD(First As BigInteger, Second As BigInteger) As BigInteger
        First = BigInteger.Abs(First)
        Second = BigInteger.Abs(Second)
        Dim Reminder As BigInteger
        While Second > 0
            Reminder = First Mod Second
            First = Second
            Second = Reminder
        End While
        Return First
    End Function
End Structure
'Special Fraction:
'   Null:               I=0     N=0     D=1
'   Unit:               I=1     N=0     D=1
'   Undefined:          I=0     N=0     D=0
'   PositiveInfinity:   I=0     N=1     D=0
'   NegativeInfinity:   I=0     N=-1    D=0