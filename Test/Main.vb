Imports Zds
Module Main
    Sub Main()
        Dim Fraction1 As New MixedFraction(-1, 2, 5)
        Dim Fraction2 As New MixedFraction(-1, 5, 2)
        Dim Result As MixedFraction = MixedFraction.Divide(Fraction1, Fraction2)
        Dim Inverse As MixedFraction = Result.Inverse
        Result = MixedFraction.Add(Inverse, 1)
        Stop
    End Sub
End Module
