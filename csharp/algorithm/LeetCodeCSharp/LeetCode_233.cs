using System;
using System.Collections;
using System.Collections.Generic;

namespace LeetCodeCSharp
{
    public class LeetCode_233
    {
        private IList<string> BuildList(char[][] board)
        {
            var list = new List<string>();
            foreach (var row in board)
            {
                list.Add(new string(row));
            }

            return list;
        }

        private void Backtrack(int row, int n, IList<IList<string>> ans,
            char[][] board, bool[][] visited)
        {
            if (row == n)
            {
                ans.Add(BuildList(board));
                return;
            }

            for (int col = 0; col < n; ++col)
            {
                if (!visited[0][col] &&
                    !visited[1][row - col + n] &&
                    !visited[2][row + col])
                {
                    board[row][col] = 'Q';
                    visited[0][col] = visited[1][row - col + n] = visited[2][row + col] = true;
                    Backtrack(row + 1, n, ans, board, visited);
                    visited[0][col] = visited[1][row - col + n] = visited[2][row + col] = false;
                    board[row][col] = '.';
                }
            }
        }

        public IList<IList<string>> SolveNQueen(int n)
        {
            var result = new List<IList<string>>();
            bool[][] visited = new bool[3][]
            {
                new bool[2 * n],
                new bool[2 * n],
                new bool[2 * n]
            };

            char[][] board = new char[n][];
            for (int i = 0; i < n; i++)
            {
                var temp = new char[n];
                Array.Fill(temp, '.');
                board[i] = temp;
            }

            Backtrack(0, n, result, board, visited);

            return result;
        }
    }
}