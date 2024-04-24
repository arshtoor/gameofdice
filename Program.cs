using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Die
{
    protected Random random;

    public Die()
    {
        random = new Random();
    }

    public int Roll()
    {
        return random.Next(1, 7); //It generates random number from 1 to 6
    }
}

public abstract class Game
{
    protected Die[] dice;
    protected int total;

    public Game(int numOfDice)
    {
        dice = new Die[numOfDice];
        for (int i = 0; i < numOfDice; i++)
        {
            dice[i] = new Die();
        }
    }

    public abstract void Play();

    public int RollDice() //For changing access modifier from protected to public
    {
        total = 0;
        foreach (var die in dice)
        {
            total += die.Roll();
        }
        return total;
    }
}

public class SevensOut : Game
{
    public SevensOut() : base(2) // This helps to pass the required argument for the base constructor
    {
    }

    public override void Play()
    {
        Console.WriteLine("Starting Sevens Out game...");

        while (true)
        {
            int result = RollDice();
            Console.WriteLine($"Total: {result}");

            if (result == 7)
            {
                Console.WriteLine("You hit 7! Game Over!");
                break;
            }
            else if (dice[0].Roll() == dice[1].Roll())
            {
                Console.WriteLine("You rolled a double! Doubling your score!");
                result *= 2;
            }

            Console.WriteLine("Press any key to roll again, or 'Q' to quit...");
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Q)
                break;
        }
    }
}

public class ThreeOrMore : Game
{
    public ThreeOrMore() : base(5) //It passes the required argument for the base constructor
    {
    }

    public override void Play()
    {
        Console.WriteLine("Starting Three or More game...");

        while (total < 20)
        {
            int result = RollDice();
            Console.WriteLine($"Total: {result}");

            int[] counts = new int[7];
            foreach (var die in dice)
            {
                counts[die.Roll()]++;
            }

            bool hasThreeOrMoreOfAKind = false;
            bool hasTwoOfAKind = false;
            for (int i = 1; i <= 6; i++)
            {
                if (counts[i] >= 3)
                {
                    Console.WriteLine($"You got {counts[i]} of {i}!");
                    hasThreeOrMoreOfAKind = true;
                    break;
                }
                else if (counts[i] == 2)
                {
                    Console.WriteLine($"You got {counts[i]} of {i}.");
                    hasTwoOfAKind = true;
                }
            }

            if (hasThreeOrMoreOfAKind)
            {
                if (total + 12 >= 20)
                {
                    Console.WriteLine("You reached 20 or more! Game Over!");
                    break;
                }
                else
                {
                    total += 12;
                }
            }
            else if (hasTwoOfAKind)
            {
                Console.WriteLine("Do you want to reroll all the dice (A) or just the non-matching ones (B)?");
                var choice = Console.ReadKey(true).Key;
                if (choice == ConsoleKey.A)
                {
                    continue;
                }
            }

            Console.WriteLine("Press any key to roll again, or 'Q' to quit...");
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Q)
                break;
        }
    }
}

public class Statistics
{
    private Dictionary<string, int> gameStats;

    public Statistics()
    {
        gameStats = new Dictionary<string, int>();
    }

    public void UpdateStats(string gameType)
    {
        if (gameStats.ContainsKey(gameType))
            gameStats[gameType]++;
        else
            gameStats[gameType] = 1;
    }

    public void DisplayStats()
    {
        Console.WriteLine("Game Statistics:");
        foreach (var kvp in gameStats)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value} plays");
        }
    }
}

public class Testing
{
    public void RunTests()
    {
        Console.WriteLine("Running tests...");

        //This section is the testing for Sevens Out
        SevensOut sevensOutTest = new SevensOut();
        Debug.Assert(sevensOutTest.RollDice() is >= 2 and <= 12);

        //This is testing for Three or More
        ThreeOrMore threeOrMoreTest = new ThreeOrMore();
        Debug.Assert(threeOrMoreTest.RollDice() is >= 5 and <= 30);

        Console.WriteLine("Tests passed.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new SevensOut(); //It will helps to initialize game to a non-null value;
        Statistics stats = new Statistics();
        Testing tester = new Testing();

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Play Sevens Out");
            Console.WriteLine("2. Play Three or More");
            Console.WriteLine("3. View Statistics");
            Console.WriteLine("4. Run Tests");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    game = new SevensOut();
                    game.Play();
                    stats.UpdateStats("Sevens Out");
                    break;
                case 2:
                    game = new ThreeOrMore();
                    game.Play();
                    stats.UpdateStats("Three or More");
                    break;
                case 3:
                    stats.DisplayStats();
                    break;
                case 4:
                    tester.RunTests();
                    break;
                case 5:
                    Console.WriteLine("Exiting program...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a number from the menu.");
                    break;
            }
        }
    }
}
