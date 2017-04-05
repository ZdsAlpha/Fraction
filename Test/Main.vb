Imports Zds
Module Main
    Sub Main()
        Console.Write("Enter a number:")
        Dim N1 As MixedFraction = Console.ReadLine
        Console.Write("Enter second number:")
        Dim N2 As MixedFraction = Console.ReadLine
        If N1 Like N2 Then
            Console.WriteLine("Numbers are similar")
        Else
            Console.WriteLine("Numbers are different")
        End If
        Console.ReadKey()
    End Sub
End Module
