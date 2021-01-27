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
		#region Properties
		private List<Edge> edges = new List<Edge>();
		private List<char> nodes = new List<char>();
		private List<List<int>> adjMatrix = new List<List<int>>();
		private List<bool> visited = new List<bool>();
		#endregion

		#region Functions
		// Removes the edge from the edge list(char, char)
		public void RemoveEdge(char u, char v)
		{
			// finding the edge with u, v nodes
			for (int i = 0; i < edges.Count; i++)
			{
				// if this edge has been found
				if ((edges[i].u == u && edges[i].v == v) ||
					(edges[i].u == v && edges[i].v == u))
				{
					// remove it from the list
					edges.RemoveAt(i);

					// remove connection from adjustment matrix
					int i1 = nodes.IndexOf(u);
					int i2 = nodes.IndexOf(v);

					adjMatrix[i1][i2] = 0;
					adjMatrix[i2][i1] = 0;

					debug($"Edge ({u}; {v}) has been removed.");

					return;
				}
			}
			debug($"There is no edge ({u}; {v}).");
		}

		// Removes the edge from the edge list(Edge)
		public void RemoveEdge(Edge edge)
		{
			RemoveEdge(edge.u, edge.v);
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

			// explands adjustment matrix if new nodes have been added
			expandMatrix(count);

			// finds indexes of the new added nodes 
			int i1 = nodes.IndexOf(edge.u);
			int i2 = nodes.IndexOf(edge.v);

			// adds connection between nodes in the adjustment matrix
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
				Console.WriteLine("-------Graph:-------");
				foreach (var e in edges)
				{
					Console.Write($"{e.u} -> {e.v} = {e.w}\n");
				}
				Console.WriteLine("--------------------");
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

				Console.Write("# ");

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
				Console.WriteLine("--------------------");
			}
		}

		// Displays all graph nodes
		public void DisplayNodes()
		{
			Console.Write($"Nodes({nodes.Count}): ");
			foreach(var n in nodes)
			{
				Console.Write($"{n} ");
			}
			Console.WriteLine();
		}

		// Depth-First Search algorithm
		public void DFS(char start = 'A')
		{
			// if there is no such node, say about it
			if (nodes.IndexOf(start) == -1)
			{
				Console.WriteLine($"[DFS]: There is no \'{start}\' node.");
			}
			else
			{
				// initializing visited nodes
				visited = new List<bool>(nodes.Count);

				// set every visited node to false
				for (int i = 0; i < nodes.Count; i++)
					visited.Add(false);

				Console.Write("[DFS]: ");

				// start DFS algorithm
				dfs(nodes.IndexOf(start));

				Console.WriteLine();
			}
		}

		// Deletes all unconnected nodes
		public void DeleteUnconnectedNodes()
		{
			for(int i = 0; i < nodes.Count; i++)
			{
				checkForNodeDelete(nodes[i]);
			}
		}
		#endregion

		#region Private Functions
		// Debug information
		private void debug(string msg)
		{
			Console.WriteLine("[DEBUG]: " + msg);
		}

		// Expand matrix if new nodes have been added to the graph
		private void expandMatrix(int count)
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

		// Implements DFS
		private void dfs(int at)
		{
			// if we have visited this node already, skip it
			if (visited[at])
				return;

			// print current node
			Console.Write($"{nodes[at]} ");

			// set this node to visited
			visited[at] = true;

			// find neighbours of this node
			for (int i = 0; i < adjMatrix.Count; i++)
			{
				// if there is connection between nodes
				if (adjMatrix[at][i] == 1)
				{
					// do DFS for each neighbour
					dfs(i);
				}
			}
		}

		// Checks if there is no node connection between other nodes
		// And if it is, deletes this node
		private void checkForNodeDelete(char node)
		{
			bool delete = true;
			for (int j = 0; j < adjMatrix.Count; j++)
			{
				if (adjMatrix[nodes.IndexOf(node)][j] == 1)
				{
					delete = false;
				}
			}

			if (delete)
			{
				debug($"Deleting \'{node}\' node..");
				Console.WriteLine($"ADJ: {nodes.IndexOf(node)}");
				adjMatrix.RemoveAt(nodes.IndexOf(node));
				nodes.Remove(node);
			}
		}
		#endregion
	}

	class MainClass
	{
		static void Main(string[] args)
		{
			Graph graph = new Graph();

			graph.AddEdge('A', 'B', 27);
			graph.AddEdge('A', 'M', 15);
			graph.AddEdge('B', 'G', 9);
			graph.AddEdge('B', 'L', 7);
			graph.AddEdge('L', 'N', 10);
			graph.AddEdge('N', 'G', 8);
			graph.AddEdge('N', 'R', 31);
			graph.AddEdge('R', 'D', 32);
			graph.AddEdge('G', 'S', 11);
			graph.AddEdge('S', 'M', 15);
			graph.AddEdge('M', 'D', 21);
			graph.AddEdge('S', 'D', 17);

			graph.Display();
			graph.DisplayNodes();
			graph.DisplayMatrix();

			graph.DFS();
			graph.DFS('C');
			graph.DFS('L');

			Console.WriteLine("----------------------");

			graph.RemoveEdge('L', 'B');
			graph.RemoveEdge('N', 'L');
			graph.RemoveEdge('Z', 'X');

			graph.DisplayNodes();

			Console.WriteLine("----------------------");

			graph.DisplayMatrix();

			graph.DFS();

			Console.WriteLine("----------------------");

			Console.WriteLine("Press any key to exit program.");
			Console.ReadKey();
		}
	}
}

/*
-------Graph:-------
A -> B = 27
A -> M = 15
B -> G = 9
B -> L = 7
L -> N = 10
N -> G = 8
N -> R = 31
R -> D = 32
G -> S = 11
S -> M = 15
M -> D = 21
S -> D = 17
--------------------
Nodes(9): A B M G L N R D S
Adjustment matrix:
# A B M G L N R D S
A 0 1 1 0 0 0 0 0 0
B 1 0 0 1 1 0 0 0 0
M 1 0 0 0 0 0 0 1 1
G 0 1 0 0 0 1 0 0 1
L 0 1 0 0 0 1 0 0 0
N 0 0 0 1 1 0 1 0 0
R 0 0 0 0 0 1 0 1 0
D 0 0 1 0 0 0 1 0 1
S 0 0 1 1 0 0 0 1 0
--------------------
[DFS]: A B G N L R D M S
[DFS]: There is no 'C' node.
[DFS]: L B A M D R N G S
----------------------
[DEBUG]: Edge (L; B) has been removed.
[DEBUG]: Edge (N; L) has been removed.
[DEBUG]: There is no edge (Z; X).
Nodes(9): A B M G L N R D S
----------------------
Adjustment matrix:
# A B M G L N R D S
A 0 1 1 0 0 0 0 0 0
B 1 0 0 1 0 0 0 0 0
M 1 0 0 0 0 0 0 1 1
G 0 1 0 0 0 1 0 0 1
L 0 0 0 0 0 0 0 0 0
N 0 0 0 1 0 0 1 0 0
R 0 0 0 0 0 1 0 1 0
D 0 0 1 0 0 0 1 0 1
S 0 0 1 1 0 0 0 1 0
--------------------
[DFS]: A B G N R D M S
----------------------
Press any key to exit program. 
*/