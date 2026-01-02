using _01_secret_entrance;
using System;

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

var Exact = (int baseAmount, Instruction instruction) =>
{
    var value = baseAmount;
    value = Normalize(value + instruction.Delta, Modulo);

    if (value == 0)
    {
        passByZero++;
    }

    return value;
};

int Advance(int start, Instruction instr)
{
    var delta = instr.Amount * instr.Direction;
    if (delta == 0) return start;

    var passes = 0;
    if (delta > 0)
    {
        var total = start + delta;
        passes = (total - 1) / Modulo; // exclude landing exactly on 0
    }
    else
    {
        var distance = -delta;
        if (start == 0)
        {
            passes = (distance - 1) / Modulo; // exclude landing exactly on 0
        }
        else if (distance > start)
        {
            var remaining = distance - start;
            passes = 1 + (remaining - 1) / Modulo; // exclude landing exactly on 0
        }
    }

    passByZero += passes;
    return Normalize(start + delta, Modulo);
}

var EachTick = (int start, Instruction instruction) =>
{
    if (instruction.Delta == 0)
    {
        return start;
    }

    var passes = 0;
    if (instruction.Delta > 0)
    {
        var total = start + instruction.Delta;
        passes = (total - 1) / Modulo; // exclude landing exactly on 0
    }
    else
    {
        var distance = -instruction.Delta;
        if (start == 0)
        {
            passes = (distance - 1) / Modulo; // exclude landing exactly on 0
        }
        else if (distance > start)
        {
            var remaining = distance - start;
            passes = 1 + (remaining - 1) / Modulo; // exclude landing exactly on 0
        }
    }

    passByZero += passes;
    return Normalize(start + instruction.Delta, Modulo);
};


foreach (var instruction in Instructions)
{
    baseAmount = EachTick(baseAmount, instruction);
}

Console.WriteLine($"number {baseAmount} with {passByZero}");