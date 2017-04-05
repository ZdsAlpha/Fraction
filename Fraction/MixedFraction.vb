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
    Public ReadOnly Property Direction As BigInteger
        Get
            If IsPositiveInfinity Then Return 1
            If IsNegativeInfinity Then Return -1
            If IsUndefined Then Return 0
            If [Integer] > 0 Then
                Return 1
            ElseIf [Integer] < 0 Then
                Return -1
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property Negative As MixedFraction
        Get
            Return New MixedFraction(-[Integer], -ProperNumerator, Denominator)
        End Get
    End Property
    Public ReadOnly Property Inverse As MixedFraction
        Get
            Return New MixedFraction(Denominator, Numerator)
        End Get
    End Property
    Public ReadOnly Property Half As MixedFraction
        Get
            Return New MixedFraction(Numerator, Denominator * 2)
        End Get
    End Property
    Public ReadOnly Property Square As MixedFraction
        Get
            Return Multiply(Me, Me)
        End Get
    End Property
    Public ReadOnly Property IsPositive As Boolean
        Get
            Return Direction = 1
        End Get
    End Property
    Public ReadOnly Property IsNegative As Boolean
        Get
            Return Direction = -1
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
    Public ReadOnly Property IsUnity As Boolean
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
    Public Overrides Function ToString() As String
        If IsPositiveInfinity Then Return "+Infinity"
        If IsNegativeInfinity Then Return "-Infinity"
        If IsUndefined Then Return "Undefined"
        Return Numerator.ToString + "/"c + Denominator.ToString
    End Function

    Private Sub Resolve()
        If _Denominator < 0 Then
            _Numerator = -_Numerator
            _Denominator = BigInteger.Abs(_Denominator)
        End If
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
            If _Numerator < 0 Then
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

    Sub New(Numerator As BigInteger, Denominator As BigInteger)
        SetValue(Numerator, Denominator)
    End Sub
    Sub New([Integer] As BigInteger, Numerator As BigInteger, Denominator As BigInteger)
        SetValue([Integer], Numerator, Denominator)
    End Sub

    Public Shared ReadOnly Property Null As MixedFraction
        Get
            Return New MixedFraction(0, 1)
        End Get
    End Property
    Public Shared ReadOnly Property Unity As MixedFraction
        Get
            Return New MixedFraction(1, 1)
        End Get
    End Property
    Public Shared ReadOnly Property Undefined As MixedFraction
        Get
            Return New MixedFraction(0, 0)
        End Get
    End Property
    Public Shared ReadOnly Property PositiveInfinity As MixedFraction
        Get
            Return New MixedFraction(1, 0)
        End Get
    End Property
    Public Shared ReadOnly Property NegativeInfinity As MixedFraction
        Get
            Return New MixedFraction(-1, 0)
        End Get
    End Property

    Public Shared Operator -(Fraction As MixedFraction)
        Return Negate(Fraction)
    End Operator
    Public Shared Operator +(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As MixedFraction
        Return Add(Fraction1, Fraction2)
    End Operator
    Public Shared Operator -(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As MixedFraction
        Return Subtract(Fraction1, Fraction2)
    End Operator
    Public Shared Operator *(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As MixedFraction
        Return Multiply(Fraction1, Fraction2)
    End Operator
    Public Shared Operator /(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As MixedFraction
        Return Divide(Fraction1, Fraction2)
    End Operator

    Public Shared Operator =(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As Boolean
        If Fraction1.IsPositiveInfinity And Fraction2.IsPositiveInfinity Then Return True
        If Fraction1.IsNegativeInfinity And Fraction2.IsNegativeInfinity Then Return True
        If Fraction1.IsUndefined Or Fraction2.IsUndefined Then Return False
        Return Fraction1._Integer = Fraction2._Integer And Fraction1._Numerator = Fraction2._Numerator And Fraction1._Denominator = Fraction2._Denominator
    End Operator
    Public Shared Operator <>(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As Boolean
        Return Not Fraction1 = Fraction2
    End Operator
    Public Shared Operator >(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As Boolean
        If (Fraction1._Numerator = 0 And Fraction1._Denominator = 0) Or (Fraction2._Numerator = 0 And Fraction2._Denominator = 0) Then Return False
        If (Fraction1.IsPositiveInfinity And Not Fraction2.IsPositiveInfinity) Or (Fraction2.IsNegativeInfinity And Not Fraction1.IsNegativeInfinity) Then Return True
        If (Fraction1.IsNegativeInfinity And Not Fraction2.IsNegativeInfinity) Or (Fraction2.IsPositiveInfinity And Not Fraction1.IsPositiveInfinity) Then Return False
        If Fraction1.Integer > Fraction2.Integer Then
            Return True
        ElseIf Fraction1.Integer < Fraction2.Integer Then
            Return False
        Else
            Return Fraction1.ProperNumerator * Fraction2.Denominator > Fraction2.ProperNumerator * Fraction1.Denominator
        End If
    End Operator
    Public Shared Operator <(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As Boolean
        If (Fraction1._Numerator = 0 And Fraction1._Denominator = 0) Or (Fraction2._Numerator = 0 And Fraction2._Denominator = 0) Then Return False
        If (Fraction1.IsNegativeInfinity And Not Fraction2.IsNegativeInfinity) Or (Fraction2.IsPositiveInfinity And Not Fraction1.IsPositiveInfinity) Then Return True
        If (Fraction1.IsPositiveInfinity And Not Fraction2.IsPositiveInfinity) Or (Fraction2.IsNegativeInfinity And Not Fraction1.IsNegativeInfinity) Then Return False
        If Fraction1.Integer < Fraction2.Integer Then
            Return True
        ElseIf Fraction1.Integer > Fraction2.Integer Then
            Return False
        Else
            Return Fraction1.ProperNumerator * Fraction2.Denominator < Fraction2.ProperNumerator * Fraction1.Denominator
        End If
    End Operator

    Public Shared Widening Operator CType(Number As Byte) As MixedFraction
        Return New MixedFraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As SByte) As MixedFraction
        Return New MixedFraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As Short) As MixedFraction
        Return New MixedFraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As UShort) As MixedFraction
        Return New MixedFraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As Integer) As MixedFraction
        Return New MixedFraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As UInteger) As MixedFraction
        Return New MixedFraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As Long) As MixedFraction
        Return New MixedFraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As ULong) As MixedFraction
        Return New MixedFraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As BigInteger) As MixedFraction
        Return New MixedFraction(Number, 1)
    End Operator

    Public Shared Narrowing Operator CType(Fraction As MixedFraction) As Byte
        Return CByte(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As MixedFraction) As SByte
        Return CSByte(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As MixedFraction) As Short
        Return CShort(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As MixedFraction) As UShort
        Return CUShort(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As MixedFraction) As Integer
        Return CInt(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As MixedFraction) As UInteger
        Return CUInt(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As MixedFraction) As Long
        Return CLng(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As MixedFraction) As ULong
        Return CULng(Fraction.Integer)
    End Operator

    Public Shared Function Negate(Fraction As MixedFraction) As MixedFraction
        If Fraction.IsPositiveInfinity Then Return NegativeInfinity
        If Fraction.IsNegativeInfinity Then Return PositiveInfinity
        If Fraction.IsUndefined Then Return Undefined
        Return New MixedFraction(-Fraction.Integer, -Fraction.ProperNumerator, Fraction.Denominator)
    End Function
    Public Shared Function Add(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As MixedFraction
        If Fraction1.IsPositiveInfinity And Fraction2.IsPositiveInfinity Then Return PositiveInfinity
        If Fraction1.IsNegativeInfinity And Fraction2.IsNegativeInfinity Then Return NegativeInfinity
        If Fraction1.IsUndefined Or Fraction2.IsUndefined Then Return Undefined
        Return New MixedFraction(Fraction1.Integer + Fraction2.Integer, Fraction1.ProperNumerator * Fraction2.Denominator + Fraction2.ProperNumerator * Fraction1.Denominator, Fraction1.Denominator * Fraction2.Denominator)
    End Function
    Public Shared Function Subtract(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As MixedFraction
        If Fraction1.IsPositiveInfinity And Fraction2.IsNegativeInfinity Then Return PositiveInfinity
        If Fraction1.IsNegativeInfinity And Fraction2.IsPositiveInfinity Then Return NegativeInfinity
        If Fraction1.IsUndefined Or Fraction2.IsUndefined Then Return Undefined
        Return New MixedFraction(Fraction1.Integer - Fraction2.Integer, Fraction1.ProperNumerator * Fraction2.Denominator - Fraction2.ProperNumerator * Fraction1.Denominator, Fraction1.Denominator * Fraction2.Denominator)
    End Function
    Public Shared Function Multiply(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As MixedFraction
        If (Fraction1.IsInfinity Or Fraction2.IsInfinity) And ((Fraction1.IsPositive And Fraction2.IsPositive) Or (Fraction1.IsNegative And Fraction2.IsNegative)) Then Return PositiveInfinity
        If (Fraction1.IsInfinity Or Fraction2.IsInfinity) And ((Fraction1.IsPositive And Fraction2.IsNegative) Or (Fraction1.IsNegative And Fraction2.IsPositive)) Then Return NegativeInfinity
        If Fraction1.IsUndefined Or Fraction2.IsUndefined Then Return Undefined
        Return New MixedFraction(Fraction1.Integer * Fraction2.Integer, Fraction1.Integer * Fraction1.Denominator * Fraction2.ProperNumerator + Fraction2.Integer * Fraction2.Denominator * Fraction1.ProperNumerator + Fraction1.ProperNumerator * Fraction2.ProperNumerator, Fraction1.Denominator * Fraction2.Denominator)
    End Function
    Public Shared Function Divide(Fraction1 As MixedFraction, Fraction2 As MixedFraction) As MixedFraction
        If Fraction1.IsInfinity And Not Fraction2.IsInfinity Then
            If Fraction2.IsPositive Then
                Return Fraction1
            ElseIf Fraction2.IsNegative Then
                Return Negate(Fraction1)
            Else
                Return Undefined
            End If
        End If
        If Fraction2.IsInfinity And Not Fraction1.IsInfinity Then Return Null
        If Fraction1.IsUndefined Or Fraction2.IsUndefined Then Return Undefined
        Return New MixedFraction(Fraction1.Numerator * Fraction2.Denominator, Fraction1.Denominator * Fraction2.Numerator)
    End Function

    Private Shared Function GCD(First As BigInteger, Second As BigInteger) As BigInteger
        Return BigInteger.GreatestCommonDivisor(First, Second)
    End Function
    Private Shared Function Factorial(n As BigInteger) As BigInteger
        If n < 0 Then Throw New ArithmeticException("Factorial of negative number is undefined.")
        If n = 0 Or n = 1 Then Return 1
        Dim Result As BigInteger = 1
        For i = 2 To n
            Result *= i
        Next
        Return Result
    End Function
    Private Shared Function nCr(n As BigInteger, r As BigInteger)
        Return Factorial(n) / (Factorial(r) * Factorial(n - r))
    End Function
End Structure
'Special Fraction:
'   Null:               I=0     N=0     D=1
'   Unity:              I=1     N=0     D=1
'   Undefined:          I=0     N=0     D=0
'   PositiveInfinity:   I=0     N=1     D=0
'   NegativeInfinity:   I=0     N=-1    D=0