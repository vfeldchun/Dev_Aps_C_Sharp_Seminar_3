using System.Reflection.Metadata.Ecma335;

namespace Task1
{
    internal class Program
    {

        private static Stack<Tuple<int, int>> _path = new Stack<Tuple<int, int>>();
        enum MoveDirection { Up, Down, Left, Right };

        static void Main(string[] args)
        {
            int[,] labirynth1 = new int[,] {
                {1, 1, 1, 1, 1, 1, 1},
                {1, 0, 0, 0, 0, 0, 1},
                {1, 0, 1, 1, 1, 0, 1},
                {0, 0, 0, 0, 1, 0, 0},
                {1, 1, 0, 0, 1, 1, 1},
                {1, 1, 1, 0, 1, 1, 1},
                {1, 1, 1, 0, 1, 1, 1}
            };

            int exitsCount = FindExit(labirynth1, new Tuple<int, int>(1, 5));

            Console.WriteLine("У лабиринта:\n");
            PrintLabirynth(labirynth1);

            if (exitsCount > 0) Console.WriteLine($"\nНайдено выходов {exitsCount}!");
            else Console.WriteLine("\nВыходов не найденo!");


        }

        static void PrintLabirynth(int[,] labirynth)
        {
            for (int i = 0; i < labirynth.GetLength(0); i++)
            {
                for (int j = 0; j < labirynth.GetLength(1); j++)
                {
                    Console.Write(labirynth[i, j] + " ");
                }

                Console.WriteLine();
            }
        }

        static int FindExit(int[,] labirynth, Tuple<int, int> startPosition)
        {
            if (labirynth[startPosition.Item1, startPosition.Item2] == 1) return -1;
            // if (labirynth[startPosition.Item1, startPosition.Item2] == 2) return true;

            _path.Push(startPosition);

            var currentDirection = MoveDirection.Right;
            bool isWall = false;
            var listOfExits = new List<Tuple<int, int>>();
            int iterations = 0;


            // реализуем алгоритм правой руки
            while (_path.Count > 0 && iterations <= labirynth.Length * 2)
            {

                var current = _path.Pop();
                // В случае отсутствия выходов останавливаем цикл после
                // превышения количества итераций количества элементов списка
                iterations++;

                // Нашли выход вышли из метода
                // if (labirynth[current.Item1, current.Item2] == 2)
                //    return true;

                // Идем вперед направо пока не упремся в стену
                if (currentDirection == MoveDirection.Right && !isWall)
                {
                    if (labirynth[current.Item1, current.Item2 + 1] != 1)
                        _path.Push(new Tuple<int, int>(current.Item1, current.Item2 + 1));
                    else
                    {
                        // Уперлись в стену поворачиваем на лево как бы касаясь правой рукой стены
                        isWall = true;
                        _path.Push(current);
                        currentDirection = MoveDirection.Up;
                    }
                }
                else if (isWall)
                {
                    // Проверяем на обнаружение всех входов
                    if (listOfExits != null)
                        if (IsInListCount(out int count, listOfExits, current))
                            return count;

                    // Проверка на координаты входа
                    if (current.Item1 == 0)
                    {
                        currentDirection = MoveDirection.Down;
                        listOfExits?.Add(current);
                    }
                    else if (current.Item1 == labirynth.GetLength(0) - 1)
                    {
                        currentDirection = MoveDirection.Up;
                        listOfExits?.Add(current);
                    }

                    if (current.Item2 == 0)
                    {                        
                        currentDirection = MoveDirection.Right;
                        listOfExits?.Add(current);
                    }
                    else if (current.Item2 == labirynth.GetLength(1) - 1)
                    {                        
                        currentDirection = MoveDirection.Left;
                        listOfExits?.Add(current);
                    }

                    // Выполняем повороты по правой стороне с сохранением координат
                    if (currentDirection == MoveDirection.Right)
                    {
                        if (labirynth[current.Item1, current.Item2 + 1] != 1 &&
                            labirynth[current.Item1 + 1, current.Item2 + 1] == 1)
                        {
                            _path.Push(new Tuple<int, int>(current.Item1, current.Item2 + 1));
                        }
                        else if (labirynth[current.Item1, current.Item2 + 1] != 1 &&
                            labirynth[current.Item1 + 1, current.Item2 + 1] != 1)
                        {
                            _path.Push(new Tuple<int, int>(current.Item1 + 1, current.Item2 + 1));
                            currentDirection = MoveDirection.Down;
                        }
                        else
                        {
                            currentDirection = MoveDirection.Up;
                            _path.Push(current);
                        }
                    }
                    else if (currentDirection == MoveDirection.Up)
                    {
                        if (labirynth[current.Item1 - 1, current.Item2] != 1 &&
                            labirynth[current.Item1 - 1, current.Item2 + 1] == 1)
                        {
                            _path.Push(new Tuple<int, int>(current.Item1 - 1, current.Item2));
                        }
                        else if (labirynth[current.Item1 - 1, current.Item2] != 1 &&
                                labirynth[current.Item1 - 1, current.Item2 + 1] != 1)
                        {
                            _path.Push(new Tuple<int, int>(current.Item1 - 1, current.Item2 + 1));
                            currentDirection = MoveDirection.Right;
                        }
                        else
                        {
                            currentDirection = MoveDirection.Left;
                            _path.Push(current);
                        }
                    }
                    else if (currentDirection == MoveDirection.Left)
                    {
                        if (labirynth[current.Item1, current.Item2 - 1] != 1 &&
                            labirynth[current.Item1 - 1, current.Item2 - 1] == 1)
                        {
                            _path.Push(new Tuple<int, int>(current.Item1, current.Item2 - 1));
                        }
                        else if (labirynth[current.Item1, current.Item2 - 1] != 1 &&
                                 labirynth[current.Item1 - 1, current.Item2 - 1] != 1)
                        {
                            _path.Push(new Tuple<int, int>(current.Item1 - 1, current.Item2 - 1));
                            currentDirection = MoveDirection.Up;
                        }
                        else
                        {
                            currentDirection = MoveDirection.Down;
                            _path.Push(current);
                        }
                    }
                    else if (currentDirection == MoveDirection.Down)
                    {
                        if (labirynth[current.Item1 + 1, current.Item2] != 1 &&
                            labirynth[current.Item1 + 1, current.Item2 - 1] == 1)
                        {
                            _path.Push(new Tuple<int, int>(current.Item1 + 1, current.Item2));
                        }
                        else if (labirynth[current.Item1 + 1, current.Item2] != 1 &&
                                 labirynth[current.Item1 + 1, current.Item2 - 1] != 1)
                        {
                            _path.Push(new Tuple<int, int>(current.Item1 + 1, current.Item2 - 1));
                            currentDirection = MoveDirection.Left;
                        }
                        else
                        {
                            currentDirection = MoveDirection.Right;
                            _path.Push(current);
                        }
                    }
                }
            }
            // Если не одного выхода не нашли
            return -1;
        }

        // Метод проверки на повторение координат выхода в списке
        static bool IsInListCount(out int count, List<Tuple<int, int>> list, Tuple<int, int> tuple)
        {
            if (list.Contains(tuple))
            {
                    count = list.Count;
                    return true;            
            }
            count = -1;
            return false;
        }
    }
}
