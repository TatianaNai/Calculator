using Calculator_2;

string expression = string.Empty;
do
{
    Console.WriteLine("Введите выражение");
    expression = Console.ReadLine();
}
while (!Validator.IsFirstElementCorrect(expression[0]) || !Validator.IsAmountOfBracketsCorrect(expression) || !Validator.IsNotLetter(expression));

Console.WriteLine(Calculator.FindResult(expression));




