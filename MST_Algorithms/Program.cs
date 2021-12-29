using System;
using System.Collections.Generic;

namespace Derevo
{
    public struct graphEdges
    {
        public int beginning;
        public int end;
        public int value;
    };
    
    class Program
    {

       
        public static void printGraphEdges(ref graphEdges[] edgesArr)  //Outputting an array of edges
        {
            for (int i = 0; i < edgesArr.Length; i++)
            {
                Console.WriteLine("Edge: {0}-{1} | Weight: {2}", edgesArr[i].beginning, edgesArr[i].end, edgesArr[i].value);
            }
            Console.WriteLine();
        }

       
        public static void sortValues(ref graphEdges[] edgesArr)  //Sort edges by weight
        {
            for (int i = 0; i < edgesArr.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < edgesArr.Length; j++)
                {
                    if (edgesArr[min].value > edgesArr[j].value)
                    {
                        min = j;
                    }
                }
                graphEdges tmp = edgesArr[i];
                edgesArr[i] = edgesArr[min];
                edgesArr[min] = tmp;
            }

        }

         
        public static void sortVert(ref graphEdges[] adgesArr) //Sort edges by weight
        {
            for (int i = 0; i < adgesArr.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < adgesArr.Length; j++)
                {
                    if (adgesArr[min].beginning > adgesArr[j].beginning)
                    {
                        min = j;
                    }
                }
                graphEdges tmp = adgesArr[i];
                adgesArr[i] = adgesArr[min];
                adgesArr[min] = tmp;
            }

        }

         
        public static void algKraskala(ref graphEdges[] edgesArr)//minimal skeleton by Kruskal's Algorithm
        {
            sortValues(ref edgesArr);
            //The number of edges in the matrix -> in the branch tree is 1 less
            int edgesInMatrix = 7;
            //Skeleton
            int[,] skeleton = new int[edgesInMatrix, edgesInMatrix];
            //Skeleton formation
            foreach (graphEdges edges in edgesArr)
            {
                //Branches value = 0
                int count = 0;
                //walking along the edges
                for (int i = 0; i < edgesInMatrix; i++)
                {
                    //If there is no edges
                    if (skeleton[i, edges.end - 1] == 0)
                    {
                        count++;
                    }
                    //if the available edge is greater than the one with which to compare
                    else if (skeleton[i, edges.end - 1] > edges.value)
                    {
                        skeleton[i, edges.end - 1] = 0;
                        skeleton[edges.end - 1, i] = 0;
                        skeleton[edges.end - 1, edges.beginning - 1] = edges.value;
                        skeleton[edges.beginning - 1, edges.end - 1] = edges.value;
                        break;
                    }
                    //if everyone passed
                    if (count == edgesInMatrix)
                    {
                        skeleton[edges.end - 1, edges.beginning - 1] = edges.value;
                        skeleton[edges.beginning - 1, edges.end - 1] = edges.value;
                    }
                }
            }

            //Skeleton output
            Console.WriteLine("The skeleton of Kruskal");
            int ves = 0;
            for (int i = 0; i < skeleton.GetLength(0); i++)
            {
                for (int j = i; j < skeleton.GetLength(0); j++)
                {
                    //Console.Write("{0,2} ", ostov[i, j]);
                    if (skeleton[i, j] != 0)
                    {
                        Console.WriteLine("Ребро {0}-{1}", i + 1, j + 1);
                        ves += skeleton[i, j];
                    }
                }
            }
            Console.WriteLine("Final minimum tree weight = {0}", ves);
        }

        //minimal skeleton by Prim's Algorithm
        public static void algPrima(ref graphEdges[] edgesArr)
        {
            sortVert(ref edgesArr);
            //The number of edges in the matrix -> in the branch tree is 1 less
            int edgesInMatrix = 7;
            //Skeleton
            int[,] skeleton = new int[edgesInMatrix, edgesInMatrix];
            //Already added vertices
            int[] check = new int[edgesInMatrix];
            //Array of adjacent edges
            int n = 0;
            graphEdges[] adjacentEdges = new graphEdges[0];

            //initial vertex
            int vertex = 0;
        //Write down all adjacent to the vertex
        Algoritm:
            for (int i = 0; i < edgesArr.Length; i++)
            {
                //If the vertex is the same as the vertex in question. Add its edges to the adjacent edge graph
                if (edgesArr[i].beginning - 1 == vertex)
                {
                    n++;
                    Array.Resize(ref adjacentEdges, n);
                    adjacentEdges[n - 1] = edgesArr[i];
                    check[vertex] = 1;

                }
            }
            sortValues(ref adjacentEdges);
            //we go through adjacent branches
            foreach (var adjacent in adjacentEdges)
            {
                //If the vertex has not yet been added to the graph
                if (check[adjacent.end - 1] == 0)
                {
                    vertex = adjacent.end - 1;
                    skeleton[adjacent.beginning - 1, adjacent.end - 1] = adjacent.value;
                    skeleton[adjacent.end - 1, adjacent.beginning - 1] = adjacent.value;
                    goto Algoritm;
                }
                else
                {
                    for (int i = 0; i < edgesInMatrix; i++)
                    {
                        //If the investigated edge of incidence is less than the one that connects the existing branch in the skeleton
                        if (skeleton[i, adjacent.end - 1] > adjacent.value & vertex == adjacent.end - 1 & check[i] != 0)
                        {
                            skeleton[i, adjacent.end - 1] = 0;
                            skeleton[adjacent.end - 1, i] = 0;
                            skeleton[adjacent.end - 1, adjacent.beginning - 1] = adjacent.value;
                            skeleton[adjacent.beginning - 1, adjacent.end - 1] = adjacent.value;
                            break;
                        }
                    }
                }
            }

            //Skeleton output
            Console.WriteLine("Prima skeleton");
            int ves = 0;
            for (int i = 0; i < skeleton.GetLength(0); i++)
            {
                for (int j = i; j < skeleton.GetLength(0); j++)
                {
                    //Console.Write("{0,2} ", ostov[i, j]);
                    if (skeleton[i, j] != 0)
                    {
                        Console.WriteLine("Edge {0}-{1}", i, j);
                        ves += skeleton[i, j];
                    }
                }
            }
            Console.WriteLine("Final minimum tree weight = {0}", ves);
        }

        //Incidents
        public static void Incident(int[,] matr, ref graphEdges[] edgesArr)
        {
            //Filling Sorting Edges
            int size = 0;
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < matr.GetLength(1); j++)
                {
                    //If an edge exists then add to the array
                    if (matr[i, j] != 0)
                    {
                        size++;
                        Array.Resize(ref edgesArr, size);
                        edgesArr[size - 1].beginning = i + 1;
                        edgesArr[size - 1].end = j + 1;
                        edgesArr[size - 1].value = matr[i, j];
                    }
                    //Console.Write(matr[i,j] + " " );
                }
                //Console.WriteLine();
            }
        }

        //Matrix output
        static void printaMatrix(int[,] matr)
        {
            Console.WriteLine("Adjacency matrix");
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < matr.GetLength(0); j++)
                {
                    Console.Write("{0,2} ", matr[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            //Undirected graph
            //int[,] matrix = {
               //{ 0, 3, 8, 4, 0, 0, 0 },
               //{ 3, 0, 13, 0, 0, 6, 0 },
               //{ 8, 13, 0, 10, 7, 6, 8 },
               //{ 4, 0, 10, 0, 9, 0, 0 },
               //{ 0, 0, 7, 9, 0, 0, 2 },
               //{ 0, 6, 6, 0, 0, 0, 4 },
               //{ 0, 0, 8, 0, 2, 4, 0 }
            //};
            int[,] matrix1 = {
                { 0,  1,  0,  2,  1,  4, 0  },
                { 3,  0,  1,  2,  0,  0, 0  },
                { 0,  2,  2,  0,  2,  1, 0  },
                { 5,  2,  1,  0,  6,  1, 0  },
                { 0,  0,  3,  6,  0,  4, 0  },
                { 0,  0,  3,  0,  8,  0, 1  },
                { 0,  0,  0,  0,  3,  2, 0  }
            };

            //Incidence matrix
            graphEdges[] edgesArr = new graphEdges[0];

            //Console.WriteLine("Matrix №1");
            //printaMatrix(matrix);
            //Incident(matrix, ref edgesArr);
            //Console.WriteLine();
            //algKraskala(ref edgesArr);
            //Console.WriteLine();
            //algPrima(ref edgesArr);

            Console.WriteLine("Matrix №2");
  
            Console.WriteLine();
            printaMatrix(matrix1);
            Incident(matrix1, ref edgesArr);
            Console.WriteLine();
            algKraskala(ref edgesArr);
            Console.WriteLine();
            algPrima(ref edgesArr);
        }
    }
}
