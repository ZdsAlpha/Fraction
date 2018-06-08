## Fraction ##
It is implemented over BigInteger. Minimum .Net Framework version is 4.0.

----------

## Features ##

 1. Automatically simplifies the fraction.
 2. Fraction can be represented in form of Mixed Fraction.
 3. Supports undefined values (Undefined,+Infinity,-Infinity).
 4. Data type BigInteger is used and can store all possible rational numbers.
 5. String-Fraction conversion (ToString() or TryParse()).
 7. Supports comparisons, conversions and operations.
 8. Use `Approces()` function or `Like` operator to check if two numbers are similar. e.g `(3.14 Like 22/7) == true`
 9. Open source.

----------

## Upcoming Features ##

 1. Reducing fraction
 2. Convertion from Float/Double/Decimal to Fraction

----------

## Example ##
```csharp
Fraction f1 = "0.5";			//1/2
Fraction f2 = "1.2/0.6";		//2/1
Fraction f3 = f1 + f2;			//5/2
Fraction f4 = f1 * f2;			//1/1
Fraction f5 = f1.Square;		//1/4
Fraction pi1 = "3.14";			//157/50
Fraction pi2 = "22/7";			//22/7
bool b = pi1.Approches(pi2);	//true
```