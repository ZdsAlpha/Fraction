Imports Zds
Module Main
    Sub Main()
        Console.Write("Enter a number:")
        Dim N1 As Fraction = Console.ReadLine
        Console.Write("Enter second number:")
        Dim N2 As Fraction = Console.ReadLine
        If N1 Like N2 Then
            Console.WriteLine("Numbers are similar")
        Else
            Console.WriteLine("Numbers are different")
        End If
        Console.ReadKey()
    End Sub
End Module
