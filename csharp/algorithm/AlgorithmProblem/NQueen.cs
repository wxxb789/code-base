using System;
using System.Collections.Generic;

namespace AlgorithmProblem
{
    // back tracking
    public class NQueen
    {
        public List<List<string>> solveNQueens(int n)
        {
            var solutions = new List<List<string>>();
            var queens = new int[n];
            Array.Fill(queens, -1);
            var columns = new HashSet<int>();
            var diagonals1 = new HashSet<int>();
            var diagonals2 = new HashSet<int>();
            backtrack(solutions, queens, n, 0, columns, diagonals1, diagonals2);
            return solutions;
        }

        public void backtrack(List<List<string>> solutions, int[] queens, int n, int row, HashSet<int> columns,
            HashSet<int> diagonals1, HashSet<int> diagonals2)
        {
            if (row == n)
            {
                var board = generateBoard(queens, n);
                solutions.Add(board);
            }
            else
            {
                for (var i = 0; i < n; i++)
                {
                    if (columns.Contains(i)) continue;

                    var diagonal1 = row - i;
                    if (diagonals1.Contains(diagonal1)) continue;

                    var diagonal2 = row + i;
                    if (diagonals2.Contains(diagonal2)) continue;

                    queens[row] = i;
                    columns.Add(i);
                    diagonals1.Add(diagonal1);
                    diagonals2.Add(diagonal2);

                    backtrack(solutions, queens, n, row + 1, columns, diagonals1, diagonals2);

                    queens[row] = -1;
                    columns.Remove(i);
                    diagonals1.Remove(diagonal1);
                    diagonals2.Remove(diagonal2);
                }
            }
        }

        public List<string> generateBoard(int[] queens, int n)
        {
            var board = new List<string>();
            for (var i = 0; i < n; i++)
            {
                var row = new char[n];
                Array.Fill(row, '.');
                row[queens[i]] = 'Q';
                board.Add(new string(row));
            }

            return board;
        }
    }
}