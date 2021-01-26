using System;
using System.Collections.Generic;

namespace Graph
{
	public struct Edge
	{
		public char u, v;
		public int w;

		public Edge(char u, char v, int w)
		{
			this.u = u;
			this.v = v;
			this.w = w;
		}
	}

	public class Graph
	{
		private List<Edge> edges = new List<Edge>();
		private List<char> nodes = new List<char>();
		private List<List<int>> adjMatrix = new List<List<int>>();

		public void RemoveEdge(char a, char b)
		{

		}

		// Addes new edge to the edge list
		public void AddEdge(char v1, char v2, int w)
		{
			// adding new edge to the edge list
			AddEdge(new Edge(v1, v2, w));
		}

		// Addes new edge to the edge list
		public void AddEdge(Edge edge)
		{
			edges.Add(edge);

			// count of new added nodes 
			int count = 0;

			// if there is no such node, add it and increase count.
			if (!nodes.Contains(edge.u))
			{
				nodes.Add(edge.u);
				count++;
			}
			if (!nodes.Contains(edge.v))
			{
				nodes.Add(edge.v);
				count++;
			}

			// expland adjustment matrix if new nodes have been added
			ExpandMatrix(count);

			// find indexes of the new added nodes 
			int i1 = nodes.IndexOf(edge.u);
			int i2 = nodes.IndexOf(edge.v);

			if ((i1 != -1) && (i2 != -1))
			{
				adjMatrix[i1][i2] = 1;
				adjMatrix[i2][i1] = 1;
			}
		}

		// Displays all the edges and their weigths
		public void Display()
		{
			if (edges.Count == 0)
			{
				Console.WriteLine("The graph is empty.");
			}
			else
			{
				Console.WriteLine("Graph:\n");
				foreach (var e in edges)
				{
					Console.Write($"{e.u} -> {e.v} = {e.w}\n");
				}
				Console.WriteLine();
			}
		}

		// Displays all adjustment matrix
		public void DisplayMatrix()
		{
			if (edges.Count == 0)
			{
				Console.WriteLine("The matrix is empty.");
			}
			else
			{
				Console.WriteLine("Adjustment matrix:");

				Console.Write("  ");

				foreach (var n in nodes)
				{
					Console.Write($"{n} ");
				}

				Console.WriteLine();

				for (int i = 0; i < adjMatrix.Count; i++)
				{
					Console.Write($"{nodes[i]} ");
					for (int j = 0; j < adjMatrix.Count; j++)
					{
						Console.Write($"{adjMatrix[i][j]} ");
					}
					Console.WriteLine();
				}
			}
		}

		private void ExpandMatrix(int count)
		{
			// for each node that we added
			for (int c = 0; c < count; c++)
			{
				// we are expanding matrix by 1 and filling it with zeroes
				for (int i = 0; i < adjMatrix.Count; i++)
				{
					adjMatrix[i].Add(0);
				}

				// adding new field in matrix
				adjMatrix.Add(new List<int>(adjMatrix.Count + 1));

				// fill it with zeroes for new nodes
				for (int j = 0; j < adjMatrix.Count; j++)
				{
					adjMatrix[adjMatrix.Count - 1].Add(0);
				}
			}
		}
	}

	class MainClass
	{
		static void Main(string[] args)
		{
			Graph graph = new Graph();

			graph.Display();
			graph.DisplayMatrix();

			Console.ReadKey();
		}
	}
}