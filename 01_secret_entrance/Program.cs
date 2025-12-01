using _01_secret_entrance;

List<Instruction> Instructions = [];
var baseAmount = 50;
const int Modulo = 100;

using var reader = new StreamReader("input.txt");
string? content;
while (!reader.EndOfStream)
{
    content = reader.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(content))
    {
        continue;
    }

    var first = content[0];
    var direction = first == 'L' ? -1 : first == 'R' ? 1 : 0;
    if (direction == 0)
    {
        continue;
    }

    var amountString = content.Length > 1 ? content[1..] : string.Empty;
    if (!int.TryParse(amountString, out var amount))
    {
        continue;
    }

    var instruction = new Instruction
    {
        Direction = direction,
        Amount = amount
    };

    Instructions.Add(instruction);
}

int Normalize(int value, int modulo) => ((value % modulo) + modulo) % modulo;

var passByZero = 0;
foreach (var instruction in Instructions)
{
    var valueBefore = baseAmount;
    baseAmount = Normalize(baseAmount + instruction.Amount * instruction.Direction, Modulo);

    if (baseAmount == 0)
    {
        passByZero++;
    }

    Console.WriteLine($"before was {valueBefore} then add {instruction.Amount * instruction.Direction} to go to {baseAmount}");
}

Console.WriteLine($"number {baseAmount} with {passByZero}");