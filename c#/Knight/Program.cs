using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knight
{
    class Program
    {
        static int[] moveSet = {-17, -15, -10, -6, 6, 10, 15, 17};
        static bool[] locked = new bool[64];
        static int steps = 0;


        static void Main(string[] args)
        {
            Console.SetBufferSize(Console.WindowWidth, 150 * 9);
            for (int i = 0; i < 64; i++)
            {
                locked[i] = false;
            }
            Console.Write("Choose a starting position between 0 and 63: ");
            int startPos = Convert.ToInt32(Console.ReadLine());
            while (startPos > 63 || startPos < 0)
            {
                Console.WriteLine("invalid intput");
                Console.Write("Choose a starting position between 0 and 63: ");
                startPos = Convert.ToInt32(Console.ReadLine());
            }

            if (RecursiveTreeSearch(startPos) == 0)
                Console.WriteLine($"success\ncompleted in {steps} moves");
            else
                Console.WriteLine("fail");
            Console.ReadKey();
        }

        static byte RecursiveTreeSearch(int move) // 0=success 1=fail
        {
            steps++;
            locked[move] = true;
            Debug.DisplayMove(move);
            if (UnLockedCount() == 0) return 0;

            int[] possibleMoves = GetValidMoves(move);
            if (possibleMoves.Length == 0) return 1;

            Dictionary<int, int> evalSet = new Dictionary<int, int>();
            for (int i = 0; i < possibleMoves.Length; i++)
                evalSet.Add(possibleMoves[i], GetValidMoves(move + moveSet[possibleMoves[i]]).Length);

            foreach (var m in evalSet.OrderBy(x => x.Value))
                if (RecursiveTreeSearch(move + moveSet[m.Key]) == 0) return 0;

            locked[move] = false;
            return 1;
        }

        static byte UnLockedCount()
        {
            byte count = 0;
            for (int i = 0; i < locked.Length; i++)
                if (!locked[i]) count++;
            return count;
        }

        static int[] GetValidMoves(int parentPos)
        {
            List<int> valMoves = new List<int>();
            for (int i = 0; i < 8; i++)
                if (CheckMoveValidity(parentPos, i)) valMoves.Add(i);

            return valMoves.ToArray();
        }

        static bool CheckMoveValidity(int parentPos, int moveIndex)
        {
            if (parentPos + moveSet[moveIndex] < 0) return false;
            if (parentPos + moveSet[moveIndex] > 63) return false;
            if (locked[parentPos + moveSet[moveIndex]]) return false;
            switch (moveIndex / 4)
            {
                case 0:
                    if (moveIndex % 2 == 0) return parentPos % 8 >= Math.Abs(moveSet[moveIndex] % 8);
                    return parentPos % 8 < Math.Abs(moveSet[moveIndex] % 8);
                default:
                    if (moveIndex % 2 == 0) return parentPos % 8 >= 8 - Math.Abs(moveSet[moveIndex] % 8);
                    return parentPos % 8 < 8 - Math.Abs(moveSet[moveIndex] % 8);
            }
        }

        class Debug
        {
            public static void VisualizeMoveValidity(int parentPos)
            {
                Console.SetCursorPosition((10 + (parentPos % 8)) * 2, 10 + (parentPos / 8));
                Console.WriteLine($"{parentPos:00}");
                for (int i = 0; i < 8; i++)
                {
                    Console.SetCursorPosition((10 + ((parentPos + moveSet[i]) % 8)) * 2, 10 + ((parentPos + moveSet[i]) / 8));
                    if (CheckMoveValidity(parentPos, i))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"{parentPos + moveSet[i]:00}");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"{parentPos + moveSet[i]:00}");
                    }
                    Console.ResetColor();
                }
            }

            public static void DisplayMove(int move)
            {
                Console.WriteLine($"------------------{steps:00}");
                for (int j = 0; j < 8; j++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        switch (j % 2)
                        {
                            case 0:
                                if (i % 2 == 0) {
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    break;  
                                }
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            default:
                                if (i % 2 == 0)
                                {
                                    Console.BackgroundColor = ConsoleColor.White;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    break;
                                }
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                        }
                        if ((j * 8) + i == move)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Blue;
                        }
                        if (locked[(j * 8) + i])
                        {
                            Console.Write($"{(j * 8) + i:00}");
                        }
                        else
                        {
                            Console.Write("  ");
                        }
                    }
                    Console.WriteLine();
                }
                Console.ResetColor();
            }
        }
    }
}
