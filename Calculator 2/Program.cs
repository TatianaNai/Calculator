using Calculator_2;

string expression = string.Empty;
do
{
    Console.WriteLine("Введите выражение");
    expression = Console.ReadLine();
}
while (!Validator.IsFirstElementCprrect(expression[0]) || !Validator.IsAmountOfBracketsCprrect(expression) || !Validator.IsNotLetter(expression));

Console.WriteLine(Calculator.OpenBrackets(expression));




