Imports System.Numerics
Public Structure Fraction
    Implements IComparable, IComparable(Of Fraction), IEquatable(Of Fraction)
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
                If Numerator = 0 Then Return 0 Else Return 1
            End If
        End Get
    End Property
    Public ReadOnly Property Negative As Fraction
        Get
            Return New Fraction(-[Integer], -ProperNumerator, Denominator)
        End Get
    End Property
    Public ReadOnly Property Inverse As Fraction
        Get
            Return New Fraction(Denominator, Numerator)
        End Get
    End Property
    Public ReadOnly Property Half As Fraction
        Get
            Return New Fraction(Numerator, Denominator * 2)
        End Get
    End Property
    Public ReadOnly Property Square As Fraction
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

    Public Function Approches(Fraction As Fraction, Optional Depth As Integer = 1) As Boolean
        Return Approaches(Me, Fraction, Depth)
    End Function
    Public Function ApprochesE(Fraction As Fraction, Optional Depth As Integer = 2) As Boolean
        Return ApproachesE(Me, Fraction, Depth)
    End Function

    Public Sub SetValue(Numerator As BigInteger, Denominator As BigInteger)
        SetValue(0, Numerator, Denominator)
    End Sub
    Public Sub SetValue([Integer] As BigInteger, Numerator As BigInteger, Denominator As BigInteger)
        _Integer = [Integer]
        _Numerator = Numerator
        _Denominator = Denominator
        Resolve()
    End Sub
    Public Shadows Function ToString() As String
        If IsPositiveInfinity Then Return "+Infinity"
        If IsNegativeInfinity Then Return "-Infinity"
        If IsUndefined Then Return "Undefined"
        Return Numerator.ToString + "/"c + Denominator.ToString
    End Function
    Public Shadows Function ToString(DigitsAfterPoint As ULong) As String
        If IsPositiveInfinity Then Return "+Infinity"
        If IsNegativeInfinity Then Return "-Infinity"
        If IsUndefined Then Return "Undefined"
        Dim sb As New Text.StringBuilder
        Dim Whole As BigInteger = BigInteger.Abs([Integer])
        sb.Append([Integer].ToString)
        sb.Append("."c)
        Dim Numerator As BigInteger = _Numerator
        Dim Denominator As BigInteger = _Denominator
        For i = 0 To DigitsAfterPoint - 1
            Numerator = Numerator * 10
            sb.Append(BigInteger.DivRem(Numerator, Denominator, Numerator).ToString)
        Next
        Return sb.ToString
    End Function
    Public Shared Function TryParse(Value As String, ByRef Result As Fraction) As Boolean
        If Value Is Nothing OrElse Value = "" Then Return False
        Value = Value.Trim(" "c)
        Value = LCase(Value)
        Dim IsConverted As Boolean
        If Value = "undefined" Then
            Result = Undefined
        ElseIf Value = "infinity" Or Value = "+infinity" Then
            Result = PositiveInfinity
        ElseIf Value = "-infinity" Then
            Result = NegativeInfinity
        ElseIf Value.Contains("/") Then
            Dim tmp() = Value.Split("/"c)
            If tmp.Length <> 2 Then Return False
            Dim Numerator As Fraction = 0
            Dim Denominator As Fraction = 0
            IsConverted = TryParse(tmp(0), Numerator)
            If IsConverted = False Then Return False
            IsConverted = TryParse(tmp(1), Denominator)
            If IsConverted = False Then Return False
            Result = Numerator / Denominator
        ElseIf Value.Contains(".") Then
            Dim tmp() = Value.Split("."c)
            If tmp(1).Contains("-") Then Return False
            Dim [Integer] As BigInteger = 0
            If tmp(0) <> "" Then
                IsConverted = BigInteger.TryParse(tmp(0), [Integer])
                If IsConverted = False Then Return False
            End If
            Dim ProperNumerator As BigInteger = 0
            If tmp(1) <> "" Then
                IsConverted = BigInteger.TryParse(tmp(1), ProperNumerator)
                If IsConverted = False Then Return False
            End If
            Dim ProperDenominator As BigInteger = 10
            ProperDenominator = BigInteger.Pow(ProperDenominator, tmp(1).Length)
            Result = New Fraction([Integer], ProperNumerator, ProperDenominator)
        Else
            Dim Number As BigInteger = 0
            IsConverted = BigInteger.TryParse(Value, Number)
            If IsConverted = False Then Return False
            Result = New Fraction(Number)
        End If
        Return True
    End Function
    Public Shared Function Parse(Value As String) As Fraction
        Dim Fraction As Fraction = 0
        Dim IsConverted As Boolean
        IsConverted = TryParse(Value, Fraction)
        If IsConverted = False Then
            Throw New FormatException("Invalid format. Unable to convert " + Value.ToString + " to fraction.")
        Else
            Return Fraction
        End If
    End Function

    Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        If obj Is Nothing Then Return 1
        If TypeOf obj Is Fraction Then
            Return CompareTo(CType(obj, Fraction))
        Else
            Throw New ArgumentException(String.Format("{0} is not a {1}", NameOf(obj), [GetType].FullName))
        End If
    End Function
    Public Function CompareTo(other As Fraction) As Integer Implements IComparable(Of Fraction).CompareTo
        If Me = other Then Return 0
        If Me > other Then
            Return 1
        Else
            Return -1
        End If
    End Function
    Public Overloads Function Equals(other As Fraction) As Boolean Implements IEquatable(Of Fraction).Equals
        Return Me = other
    End Function

    Private Sub Resolve()
        If _Denominator = 0 Then
            _Integer = 0
            If _Numerator > 0 Then
                _Numerator = 1
            ElseIf _Numerator < 0 Then
                _Numerator = -1
            End If
        Else
            If _Denominator < 0 Then
                _Numerator = -_Numerator
                _Denominator = BigInteger.Abs(_Denominator)
            End If
            Dim GCD As BigInteger = Fraction.GCD(_Numerator, _Denominator)
            _Numerator = _Numerator / GCD
            _Denominator = _Denominator / GCD
            If _Numerator < 0 Then
                Dim Remainder As BigInteger = 0
                Dim WholePart As BigInteger = BigInteger.DivRem(BigInteger.Abs(_Numerator), _Denominator, Remainder)
                _Integer -= WholePart + 1
                _Numerator = _Denominator - Remainder
            Else
                Dim Remainder As BigInteger = 0
                Dim WholePart As BigInteger = BigInteger.DivRem(BigInteger.Abs(_Numerator), _Denominator, Remainder)
                _Integer += WholePart
                _Numerator = Remainder
            End If
        End If
    End Sub

    Sub New(Value As BigInteger)
        SetValue(Value, 0, 1)
    End Sub
    Sub New(Numerator As BigInteger, Denominator As BigInteger)
        SetValue(Numerator, Denominator)
    End Sub
    Sub New([Integer] As BigInteger, Numerator As BigInteger, Denominator As BigInteger)
        SetValue([Integer], Numerator, Denominator)
    End Sub

    Public Shared ReadOnly Property Null As Fraction
        Get
            Return New Fraction(0, 1)
        End Get
    End Property
    Public Shared ReadOnly Property Unity As Fraction
        Get
            Return New Fraction(1, 1)
        End Get
    End Property
    Public Shared ReadOnly Property Undefined As Fraction
        Get
            Return New Fraction(0, 0)
        End Get
    End Property
    Public Shared ReadOnly Property Infinity As Fraction
        Get
            Return PositiveInfinity
        End Get
    End Property
    Public Shared ReadOnly Property PositiveInfinity As Fraction
        Get
            Return New Fraction(1, 0)
        End Get
    End Property
    Public Shared ReadOnly Property NegativeInfinity As Fraction
        Get
            Return New Fraction(-1, 0)
        End Get
    End Property

    Public Shared Operator -(Fraction As Fraction)
        Return Negate(Fraction)
    End Operator
    Public Shared Operator +(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
        Return Add(Fraction1, Fraction2)
    End Operator
    Public Shared Operator -(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
        Return Subtract(Fraction1, Fraction2)
    End Operator
    Public Shared Operator *(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
        Return Multiply(Fraction1, Fraction2)
    End Operator
    Public Shared Operator /(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
        Return Divide(Fraction1, Fraction2)
    End Operator
    Public Shared Operator ^(Fraction As Fraction, Power As Integer) As Fraction
        Return Pow(Fraction, Power)
    End Operator

    Public Shared Operator =(Fraction1 As Fraction, Fraction2 As Fraction) As Boolean
        If Fraction1.IsPositiveInfinity And Fraction2.IsPositiveInfinity Then Return True
        If Fraction1.IsNegativeInfinity And Fraction2.IsNegativeInfinity Then Return True
        If Fraction1.IsUndefined Or Fraction2.IsUndefined Then Return False
        Return Fraction1._Integer = Fraction2._Integer And Fraction1._Numerator = Fraction2._Numerator And Fraction1._Denominator = Fraction2._Denominator
    End Operator
    Public Shared Operator <>(Fraction1 As Fraction, Fraction2 As Fraction) As Boolean
        Return Not Fraction1 = Fraction2
    End Operator
    Public Shared Operator >(Fraction1 As Fraction, Fraction2 As Fraction) As Boolean
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
    Public Shared Operator <(Fraction1 As Fraction, Fraction2 As Fraction) As Boolean
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
    Public Shared Operator Like(Fraction1 As Fraction, Fraction2 As Fraction) As Boolean
        Return ApproachesE(Fraction1, Fraction2)
    End Operator


    Public Shared Widening Operator CType(Number As Byte) As Fraction
        Return New Fraction(Number)
    End Operator
    Public Shared Widening Operator CType(Number As SByte) As Fraction
        Return New Fraction(Number)
    End Operator
    Public Shared Widening Operator CType(Number As Short) As Fraction
        Return New Fraction(Number)
    End Operator
    Public Shared Widening Operator CType(Number As UShort) As Fraction
        Return New Fraction(Number)
    End Operator
    Public Shared Widening Operator CType(Number As Integer) As Fraction
        Return New Fraction(Number)
    End Operator
    Public Shared Widening Operator CType(Number As UInteger) As Fraction
        Return New Fraction(Number)
    End Operator
    Public Shared Widening Operator CType(Number As Long) As Fraction
        Return New Fraction(Number)
    End Operator
    Public Shared Widening Operator CType(Number As ULong) As Fraction
        Return New Fraction(Number)
    End Operator
    Public Shared Widening Operator CType(Number As BigInteger) As Fraction
        Return New Fraction(Number)
    End Operator
    Public Shared Narrowing Operator CType(Number As String) As Fraction
        Return Parse(Number)
    End Operator

    Public Shared Narrowing Operator CType(Fraction As Fraction) As Byte
        Return CByte(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As Fraction) As SByte
        Return CSByte(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As Fraction) As Short
        Return CShort(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As Fraction) As UShort
        Return CUShort(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As Fraction) As Integer
        Return CInt(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As Fraction) As UInteger
        Return CUInt(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As Fraction) As Long
        Return CLng(Fraction.Integer)
    End Operator
    Public Shared Narrowing Operator CType(Fraction As Fraction) As ULong
        Return CULng(Fraction.Integer)
    End Operator
    Public Shared Widening Operator CType(Fraction As Fraction) As String
        Return Fraction.ToString
    End Operator

    Public Shared Function Abs(Fraction As Fraction) As Fraction
        If Fraction.IsPositive Then
            Return Fraction
        Else
            Return Negate(Fraction)
        End If
    End Function
    Public Shared Function Negate(Fraction As Fraction) As Fraction
        If Fraction.IsPositiveInfinity Then Return NegativeInfinity
        If Fraction.IsNegativeInfinity Then Return PositiveInfinity
        If Fraction.IsUndefined Then Return Undefined
        Return New Fraction(-Fraction.Integer, -Fraction.ProperNumerator, Fraction.Denominator)
    End Function
    Public Shared Function Add(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
        If Fraction1.IsPositiveInfinity And Fraction2.IsPositiveInfinity Then Return PositiveInfinity
        If Fraction1.IsNegativeInfinity And Fraction2.IsNegativeInfinity Then Return NegativeInfinity
        If Fraction1.IsUndefined Or Fraction2.IsUndefined Then Return Undefined
        Return New Fraction(Fraction1.Integer + Fraction2.Integer, Fraction1.ProperNumerator * Fraction2.Denominator + Fraction2.ProperNumerator * Fraction1.Denominator, Fraction1.Denominator * Fraction2.Denominator)
    End Function
    Public Shared Function Subtract(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
        If Fraction1.IsPositiveInfinity And Fraction2.IsNegativeInfinity Then Return PositiveInfinity
        If Fraction1.IsNegativeInfinity And Fraction2.IsPositiveInfinity Then Return NegativeInfinity
        If Fraction1.IsUndefined Or Fraction2.IsUndefined Then Return Undefined
        Return New Fraction(Fraction1.Integer - Fraction2.Integer, Fraction1.ProperNumerator * Fraction2.Denominator - Fraction2.ProperNumerator * Fraction1.Denominator, Fraction1.Denominator * Fraction2.Denominator)
    End Function
    Public Shared Function Multiply(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
        If (Fraction1.IsInfinity Or Fraction2.IsInfinity) And ((Fraction1.IsPositive And Fraction2.IsPositive) Or (Fraction1.IsNegative And Fraction2.IsNegative)) Then Return PositiveInfinity
        If (Fraction1.IsInfinity Or Fraction2.IsInfinity) And ((Fraction1.IsPositive And Fraction2.IsNegative) Or (Fraction1.IsNegative And Fraction2.IsPositive)) Then Return NegativeInfinity
        If Fraction1.IsUndefined Or Fraction2.IsUndefined Then Return Undefined
        Return New Fraction(Fraction1.Integer * Fraction2.Integer, Fraction1.Integer * Fraction1.Denominator * Fraction2.ProperNumerator + Fraction2.Integer * Fraction2.Denominator * Fraction1.ProperNumerator + Fraction1.ProperNumerator * Fraction2.ProperNumerator, Fraction1.Denominator * Fraction2.Denominator)
    End Function
    Public Shared Function Divide(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
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
        Return New Fraction(Fraction1.Numerator * Fraction2.Denominator, Fraction1.Denominator * Fraction2.Numerator)
    End Function
    Public Shared Function Pow(Fraction As Fraction, Power As Integer) As Fraction
        If Fraction.IsNull Then
            If Power = 0 Then
                Return Undefined
            Else
                Return Null
            End If
        ElseIf Fraction.IsInfinity Then
            If Power > 0 Then
                Return Fraction
            ElseIf Power < 0 Then
                Return Null
            Else
                Return Undefined
            End If
        ElseIf Fraction.IsUndefined Then
            Return Undefined
        ElseIf Fraction.IsUnity Then
            Return Unity
        ElseIf Fraction.Negative.IsUnity Then
            If Power Mod 2 = 0 Then
                Return Unity
            Else
                Return Unity.Negative
            End If
        End If
        If Power = 0 Then
            Return 1
        ElseIf Power < 0 Then
            Fraction = Fraction.Inverse
            Power = Math.Abs(Power)
        End If
        Dim Result As Fraction = BigInteger.Pow(Fraction.Integer, Power)
        For i = 1 To Power - 1
            Result += nCr(Power, i) * BigInteger.Pow(Fraction.Integer, Power - i) * New Fraction(BigInteger.Pow(Fraction.ProperNumerator, i), BigInteger.Pow(Fraction.Denominator, i))
        Next
        Result += New Fraction(BigInteger.Pow(Fraction.ProperNumerator, Power), BigInteger.Pow(Fraction.Denominator, Power))
        Return Result
    End Function

    Public Shared Function Approaches(Fraction1 As Fraction, Fraction2 As Fraction, Optional Depth As Integer = 1) As Boolean
        If (Fraction1.IsPositiveInfinity And Fraction2.IsPositiveInfinity) Or (Fraction1.IsNegativeInfinity And Fraction2.IsNegativeInfinity) Then Return True
        If Fraction1.IsUndefined Or Fraction2.IsUndefined Then Return False
        Dim Delta As Fraction = Abs(Fraction1 - Fraction2)
        Dim Log10 As Double = BigInteger.Log10(Delta.Numerator) - BigInteger.Log10(Delta.Denominator)
        Return Log10 < -Depth
    End Function
    Public Shared Function ApproachesE(Fraction1 As Fraction, Fraction2 As Fraction, Optional Depth As Integer = 2) As Boolean
        If (Fraction1.IsPositiveInfinity And Fraction2.IsPositiveInfinity) Or (Fraction1.IsNegativeInfinity And Fraction2.IsNegativeInfinity) Then Return True
        If Fraction1.IsUndefined Or Fraction2.IsUndefined Then Return False
        Dim Delta As Fraction = Abs(Fraction1 - Fraction2)
        Dim Log As Double = BigInteger.Log(Delta.Numerator) - BigInteger.Log(Delta.Denominator)
        Return Log < -Depth
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