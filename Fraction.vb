Imports System.Numerics
Public Structure Fraction
    Implements IComparable, IComparable(Of Fraction), IEquatable(Of Fraction)
    Private _Numerator As BigInteger
    Private _Denominator As BigInteger
    Public Property Numerator As BigInteger
        Get
            Return _Numerator
        End Get
        Set(value As BigInteger)
            _Numerator = value
            Simplify()
        End Set
    End Property
    Public Property Denominator As BigInteger
        Get
            Return _Denominator
        End Get
        Set(value As BigInteger)
            _Denominator = value
            Simplify()
        End Set
    End Property
    Public Property WholeNumber As BigInteger
        Get
            If Denominator = 0 Then Return 0
            Return BigInteger.Divide(Numerator, Denominator)
        End Get
        Set(value As BigInteger)
            Dim ProperFraction = Me.ProperFraction
            Dim Fraction = ProperFraction + value
            _Numerator = Fraction.Numerator
            _Denominator = Fraction.Denominator
            Simplify()
        End Set
    End Property
    Public Property ProperFraction As Fraction
        Get
            If Denominator = 0 Then Return New Fraction(0, 0)
            Return New Fraction(Numerator Mod Denominator, Denominator)
        End Get
        Set(value As Fraction)
            Dim WholeNumber = Me.WholeNumber
            Dim Fraction = WholeNumber + value
            _Numerator = Fraction.Numerator
            _Denominator = Fraction.Denominator
            Simplify()
        End Set
    End Property
    Public ReadOnly Property IsProper As Boolean
        Get
            If BigInteger.Abs(Numerator) < Denominator Then Return True
            Return False
        End Get
    End Property
    Public ReadOnly Property IsImproper As Boolean
        Get
            Return Not IsProper
        End Get
    End Property
    Public ReadOnly Property IsNull As Boolean
        Get
            If Numerator = 0 Then Return True
            Return False
        End Get
    End Property
    Public ReadOnly Property IsUnit As Boolean
        Get
            If Numerator = 1 And Denominator = 1 Then Return True
            Return False
        End Get
    End Property
    Public ReadOnly Property IsNegitive As Boolean
        Get
            If Numerator < 0 Then Return True
            Return False
        End Get
    End Property
    Public ReadOnly Property IsUndefined As Boolean
        Get
            If Denominator = 0 Then Return True
            Return False
        End Get
    End Property
    Public ReadOnly Property Absolute As Fraction
        Get
            Return New Fraction(BigInteger.Abs(Numerator), BigInteger.Abs(Denominator))
        End Get
    End Property

    Public Shadows Function ToString() As String
        Return _Numerator.ToString + "/" + _Denominator.ToString
    End Function
    Public Shadows Function ToString(DigitsAfterPoint As ULong) As String
        Dim Whole As BigInteger = BigInteger.Abs(WholeNumber)
        Dim Proper As Fraction = ProperFraction.Absolute
        Dim Value As String = Whole.ToString + "."
        Dim Numerator As BigInteger = Proper.Numerator
        Dim Denominator As BigInteger = Proper.Denominator
        For i = 0 To DigitsAfterPoint - 1
            Numerator = Numerator * 10
            Value += BigInteger.Divide(Numerator, Denominator).ToString
            Numerator = Numerator Mod Denominator
        Next
        If Me.IsNegitive Then
            Return "-" + Value
        Else
            Return Value
        End If
    End Function

    Public Sub SetValue(Numerator As BigInteger, Denominator As BigInteger)
        Me._Numerator = Numerator
        Me._Denominator = Denominator
        Me.Simplify()
    End Sub
    Public Sub Simplify()
        If Denominator = 0 Then
            _Numerator = 0
            _Denominator = 0
        ElseIf Denominator < 0 Then
            _Denominator = BigInteger.Abs(Denominator)
            _Numerator = -Numerator
        ElseIf Numerator = 0 And Denominator <> 0 Then
            _Denominator = 1
        ElseIf Numerator = Denominator And Denominator <> 0 Then
            _Numerator = 1
            _Denominator = 1
        Else
            Dim GDC = Fraction.GCD(Numerator, Denominator)
            Me._Numerator = Numerator / GDC
            Me._Denominator = Denominator / GDC
        End If
    End Sub
    Public Sub Reduce(Optional Change As Long = 1)
        Me.Reduce(Change, Change)
    End Sub
    Public Sub Reduce(ChangeInNumerator As Long, ChangeInDenominator As Long)
        Dim IsNegitive As Boolean = False
        Dim Proper As Fraction = Me.ProperFraction
        If Proper < 0 Then
            IsNegitive = True
            Proper = Proper.Absolute
        End If
        Dim Reduced As Fraction = Proper
        For CIN As Long = -ChangeInNumerator To ChangeInNumerator
            For CID As Long = -ChangeInDenominator To ChangeInDenominator
                Dim Fraction = (New Fraction(Proper.Numerator + CIN, Proper.Denominator + CID)).Absolute
                If _
                    Fraction.Numerator < Reduced.Numerator And
                    Fraction.Denominator < Reduced.Denominator And
                    Not Fraction.IsUndefined Then Reduced = Fraction
            Next
        Next
        If IsNegitive = True Then
            Me.ProperFraction = -Reduced
        Else
            Me.ProperFraction = Reduced
        End If
    End Sub

    Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
    Public Function CompareTo(other As Fraction) As Integer Implements IComparable(Of Fraction).CompareTo
        Return Me.WholeNumber - other.WholeNumber
    End Function
    Public Overloads Function Equals(other As Fraction) As Boolean Implements IEquatable(Of Fraction).Equals
        If Me = other Then Return True
        Return False
    End Function

    Sub New(Numerator As BigInteger, Denominator As BigInteger)
        Me._Numerator = Numerator
        Me._Denominator = Denominator
        Simplify()
    End Sub
    Sub New(Number As BigInteger)
        Me._Numerator = Number
        Me._Denominator = 1
    End Sub
    Sub New(WholeNumber As BigInteger, ProperFraction As Fraction)
        Me._Numerator = WholeNumber * ProperFraction.Denominator + ProperFraction.Numerator
        Me._Denominator = ProperFraction.Denominator
    End Sub
    Sub New(Value As String)
        Dim Fraction As Fraction = Fraction.Parse(Value)
        Me._Numerator = Fraction.Numerator
        Me._Denominator = Fraction.Denominator
        Simplify()
    End Sub
    Sub New(Number As Decimal)
        Me.New(Number.ToString)
    End Sub
    Sub New(Number As Double)
        Me.New(Number.ToString)
    End Sub
    Sub New(Number As Single)
        Me.New(Number.ToString)
    End Sub

    Public Shared Operator +(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
        Return New Fraction(BigInteger.Add(BigInteger.Multiply(Fraction1.Numerator, Fraction2.Denominator), BigInteger.Multiply(Fraction2.Numerator, Fraction1.Denominator)), BigInteger.Multiply(Fraction1.Denominator, Fraction2.Denominator))
    End Operator
    Public Shared Operator -(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
        Return New Fraction(BigInteger.Subtract(BigInteger.Multiply(Fraction1.Numerator, Fraction2.Denominator), BigInteger.Multiply(Fraction2.Numerator, Fraction1.Denominator)), BigInteger.Multiply(Fraction1.Denominator, Fraction2.Denominator))
    End Operator
    Public Shared Operator *(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
        Return New Fraction(BigInteger.Multiply(Fraction1.Numerator, Fraction2.Numerator), BigInteger.Multiply(Fraction1.Denominator, Fraction2.Denominator))
    End Operator
    Public Shared Operator /(Fraction1 As Fraction, Fraction2 As Fraction) As Fraction
        Return New Fraction(BigInteger.Multiply(Fraction1.Numerator, Fraction2.Denominator), BigInteger.Multiply(Fraction1.Denominator, Fraction2.Numerator))
    End Operator
    Public Shared Operator ^(Fraction As Fraction, Power As Long) As Fraction
        If Power > 0 Then
            Return New Fraction(BigInteger.Pow(Fraction.Numerator, Power), BigInteger.Pow(Fraction.Denominator, Power))
        ElseIf Power < 0 Then
            Return New Fraction(BigInteger.Pow(Fraction.Denominator, Power), BigInteger.Pow(Fraction.Numerator, Power))
        End If
        Return New Fraction(1, 1)
    End Operator
    Public Shared Operator -(Fraction As Fraction) As Fraction
        Return 0 - Fraction
    End Operator

    Public Shared Operator =(Fraction1 As Fraction, Fraction2 As Fraction) As Boolean
        If Fraction1.IsNull And Fraction2.IsNull Then Return True
        If (Fraction1 / Fraction2).IsUnit Then Return True
        Return False
    End Operator
    Public Shared Operator <>(Fraction1 As Fraction, Fraction2 As Fraction) As Boolean
        If Fraction1 = Fraction2 Then Return False
        Return True
    End Operator
    Public Shared Operator >(Fraction1 As Fraction, Fraction2 As Fraction) As Boolean
        If Fraction2.IsNull Then
            If Fraction1.IsNegitive Then
                Return False
            Else
                Return True
            End If
        End If
        Dim Division = Fraction1 / Fraction2
        If Division.Numerator > Division.Denominator Then Return True
        Return False
    End Operator
    Public Shared Operator <(Fraction1 As Fraction, Fraction2 As Fraction) As Boolean
        If Fraction2.IsNull Then
            If Fraction1.IsNegitive Then
                Return True
            Else
                Return False
            End If
        End If
        Dim Division = Fraction1 / Fraction2
        If Division.Numerator < Division.Denominator Then Return True
        Return False
    End Operator
    Public Shared Operator >=(Fraction1 As Fraction, Fraction2 As Fraction) As Boolean
        If Fraction1 > Fraction2 Or Fraction1 = Fraction2 Then Return True
        Return False
    End Operator
    Public Shared Operator <=(Fraction1 As Fraction, Fraction2 As Fraction) As Boolean
        If Fraction1 < Fraction2 Or Fraction1 = Fraction2 Then Return True
        Return False
    End Operator

    Public Shared Widening Operator CType(Number As Byte) As Fraction
        Return New Fraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As SByte) As Fraction
        Return New Fraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As Short) As Fraction
        Return New Fraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As UShort) As Fraction
        Return New Fraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As Integer) As Fraction
        Return New Fraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As UInteger) As Fraction
        Return New Fraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As Long) As Fraction
        Return New Fraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As ULong) As Fraction
        Return New Fraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As BigInteger) As Fraction
        Return New Fraction(Number, 1)
    End Operator
    Public Shared Widening Operator CType(Number As Single) As Fraction
        Return Fraction.Parse(Number.ToString)
    End Operator
    Public Shared Widening Operator CType(Number As Double) As Fraction
        Return Fraction.Parse(Number.ToString)
    End Operator
    Public Shared Widening Operator CType(Number As Decimal) As Fraction
        Return Fraction.Parse(Number.ToString)
    End Operator

    Public Shared Widening Operator CType(Fraction As Fraction) As Byte
        Return Fraction.WholeNumber
    End Operator
    Public Shared Widening Operator CType(Fraction As Fraction) As SByte
        Return Fraction.WholeNumber
    End Operator
    Public Shared Widening Operator CType(Fraction As Fraction) As Short
        Return Fraction.WholeNumber
    End Operator
    Public Shared Widening Operator CType(Fraction As Fraction) As UShort
        Return Fraction.WholeNumber
    End Operator
    Public Shared Widening Operator CType(Fraction As Fraction) As Integer
        Return Fraction.WholeNumber
    End Operator
    Public Shared Widening Operator CType(Fraction As Fraction) As UInteger
        Return Fraction.WholeNumber
    End Operator
    Public Shared Widening Operator CType(Fraction As Fraction) As Long
        Return Fraction.WholeNumber
    End Operator
    Public Shared Widening Operator CType(Fraction As Fraction) As ULong
        Return Fraction.WholeNumber
    End Operator
    Public Shared Widening Operator CType(Fraction As Fraction) As Single
        Return Single.Parse(Fraction.ToString(10))
    End Operator
    Public Shared Widening Operator CType(Fraction As Fraction) As Double
        Return Single.Parse(Fraction.ToString(20))
    End Operator

    Public Shared ReadOnly Property Unit As Fraction
        Get
            Return New Fraction(1, 1)
        End Get
    End Property
    Public Shared ReadOnly Property Null As Fraction
        Get
            Return New Fraction(0, 1)
        End Get
    End Property
    Public Shared ReadOnly Property Undefined As Fraction
        Get
            Return New Fraction(0, 0)
        End Get
    End Property
    Public Shared Function GCD(First As BigInteger, Second As BigInteger) As BigInteger
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
    Public Shared Function TryParse(Value As String, ByRef Result As Fraction) As Boolean
        Value = Value.Trim(" ")
        If Value.Contains("/") Then
            Dim Array() = Value.Split("/")
            If Array.Length <> 2 Then Return False
            Dim IsConverted As Boolean
            Dim Numerator As BigInteger = 0
            Dim Denominator As BigInteger = 0
            IsConverted = BigInteger.TryParse(Array(0), Numerator)
            If IsConverted = False Then Return False
            IsConverted = BigInteger.TryParse(Array(1), Denominator)
            If IsConverted = False Then Return False
            Result = New Fraction(Numerator, Denominator)
        ElseIf Value.Contains(".") Then
            Dim IsNegitive As Boolean = False
            If Value(0) = "-" Then IsNegitive = True
            Dim Array() = Value.Split(".")
            If Array.Length <> 2 Then Return False
            If Array(1).Contains("-") Then Return False
            Dim IsConverted As Boolean
            Dim WholeNumber As BigInteger = 0
            If Array(0) <> "" Then
                IsConverted = BigInteger.TryParse(Array(0), WholeNumber)
                If IsConverted = False Then Return False
                WholeNumber = BigInteger.Abs(WholeNumber)
            End If
            Dim ProperNumerator As BigInteger = 0
            IsConverted = BigInteger.TryParse(Array(1), ProperNumerator)
            If IsConverted = False Then Return False
            Dim ProperDenominator(Array(1).Length) As Char
            ProperDenominator(0) = "1"c
            For i = 1 To Array(1).Length
                ProperDenominator(i) = "0"c
            Next
            Dim Proper As New Fraction(ProperNumerator, BigInteger.Parse(ProperDenominator))
            If IsNegitive Then
                Result = -New Fraction(WholeNumber, Proper)
            Else
                Result = New Fraction(WholeNumber, Proper)
            End If
        Else
            Dim IsConverted As Boolean
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
            Throw New Exception("Invalid format. Unable to convert " + Value.ToString + " to fraction.")
        Else
            Return Fraction
        End If
    End Function
End Structure
