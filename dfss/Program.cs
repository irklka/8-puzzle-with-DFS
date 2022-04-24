using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFSGame
{
    public class DfsSolver
    {
        int iterator = 0;
        bool solved = false;

        private static HashSet<string> visited = new HashSet<string>();
        private Stack<string> stack = new Stack<string>();

        //             Up Left Right Down
        readonly int[] offsetI = { -1, 0, 0, 1 };

        readonly int[] offsetJ = { 0, -1, 1, 0 };

        readonly int[][] finalMatrix = new int[3][]
        {
                new int[]   { 1, 2, 3 },
                new int[]   { 4, 5, 6 },
                new int[]   { 7, 8, 0 }
        };

        /// <summary>
        /// Final matrix's string representation.
        /// </summary>
        public string FinalMat { get => SerializeMatrix(finalMatrix); }

        /// <summary>
        /// Swaps two elements and returns new matrix.
        /// </summary>
        /// <param name="mat1">Initial matrix</param>
        /// <param name="x1">old x coordinate</param>
        /// <param name="x2">old y coordinate</param>
        /// <param name="y1">new x coordinate</param>
        /// <param name="y2">new x coordinate</param>
        /// <returns></returns>
        public int[][] SwapMatrix(int[][] mat1, int x1, int x2, int y1, int y2)
        {
            var newMat = new int[3][]{
                new int[3],
                new int[3],
                new int[3]
            };
            for (int i = 0; i < 3; i++)
            {
                Array.Copy(mat1[i], newMat[i], 3);
            }
            var tmp = newMat[x1][y1];
            newMat[x1][y1] = newMat[x2][y2];
            newMat[x2][y2] = tmp;
            return newMat;
        }

        /// <summary>
        /// Prints matrix.
        /// </summary>
        /// <param name="matrix">matrix to print</param>
        public void PrintMatrix(int[][] matrix)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write($"{matrix[i][j],2}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Matrix to string.
        /// </summary>
        /// <param name="matrix"> matrix to serialize</param>
        /// <returns>returns string for specific matrix 0 is replaced with '#'.</returns>
        public string SerializeMatrix(int[][] matrix)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i][j] == 0)
                    {
                        str.Append('#');
                    }
                    else
                    {
                        str.Append(matrix[i][j]);
                    }
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// Deserializes string to int[][] (jagged array).
        /// </summary>
        /// <param name="str">matrix string to deserialize.</param>
        /// <returns>Jagged array int[][],</returns>
        public int[][] DeserializeMatrix(string str)
        {
            var newMat = new int[3][]{
                new int[3],
                new int[3],
                new int[3]
            };

            for (int i = 0; i < 9; i++)
            {
                newMat[i / 3][i % 3] = str[i] == '#' ? 0 : str[i] - '0';
            }

            return newMat;
        }

        /// <summary>
        /// Gets possible child iterations for specific parent array.
        /// </summary>
        /// <param name="matrix">Parent matrix.</param>
        /// <returns>list of child matrices</returns>
        private List<int[][]> GetChilds(int[][] matrix)
        {
            var tmpAxis = GetAxis(matrix);
            int i = tmpAxis.Item1;
            int j = tmpAxis.Item2;
            List<int[][]> childs = new List<int[][]>();
            for (int k = 0; k < 4; k++)
            {
                int newI = i + offsetI[k];
                int newJ = j + offsetJ[k];

                if (newI >= 3 || newI < 0 || newJ >= 3 || newJ < 0)
                    continue;

                var tmpMat = SwapMatrix(matrix, i, newI, j, newJ);
                childs.Add(tmpMat);
            }
            return childs;
        }
        
        /// <summary>
        /// Prints path to final matrix.
        /// </summary>
        public void PrintPath()
        {
            foreach (var item in stack.Reverse())
            {
                PrintMatrix(DeserializeMatrix(item));
                Console.WriteLine(item);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// recursive DFS algorithm.
        /// </summary>
        /// <param name="m">matrix for dfs.</param>
        /// <param name="depth">0 is passed for initial depth.</param>
        public void DFS(int[][] m, int depth)
        {
            
            if (solved)
            {
                // return if solved is true.
                return;
            }

            // restrict depth of the algorithm.
            if (depth > 50)
            {
                return;
            }

            // string representation of matrix.
            var mat = SerializeMatrix(m);

            // check if matrix is final matrix.
            if (mat == FinalMat)
            {
                PrintPath();
                Console.WriteLine("Final matrix");
                PrintMatrix(m);
                Console.Write(mat);
                Console.WriteLine($" ---------{depth} depth, {iterator} iterations.");
                solved = true;
                return;
            }

            // generate child matrices.
            var childMat = GetChilds(m);
            foreach (var child in childMat)
            {
                if (solved)
                {
                    // return if solved is true.
                    return;
                }

                // string representation of child matrix.
                var tmpM = SerializeMatrix(child);

                // check if matrix was already visited.
                if (visited.Contains(tmpM))
                {
                    continue;
                }
                iterator++;

                // add string representation of matrix to the array.
                visited.Add(tmpM);
                stack.Push(tmpM);
                // recursive call to dfs.
                DFS(child, depth+1);
                stack.Pop();
            }
        }

        /// <summary>
        /// Gets coordinates of 0 for specific array.
        /// </summary>
        /// <param name="matrix">matrix of int[][]</param>
        /// <returns> tuple(i,j) for coordinates of 0</returns>
        private (int, int) GetAxis(int[][] matrix)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i][j] == 0)
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }
    }
    public static class Program
    {

        static void Main(string[] args)
        {
            int[][] matrix1 = new int[3][]
            {
                new int[]   { 1, 2, 3 },
                new int[]   { 4, 5, 6 },
                new int[]   { 7, 0, 8 }
            };

            int[][] matrix = new int[3][]
            {
                new int[]   { 4, 1, 3 },
                new int[]   { 7, 2, 6 },
                new int[]   { 0, 5, 8 }
            };

            var dfs = new DfsSolver();
            Console.WriteLine("Start matrix");
            dfs.PrintMatrix(matrix);
            Console.WriteLine();
            dfs.DFS(matrix, 0);
        }
    }
}
