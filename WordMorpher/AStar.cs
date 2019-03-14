using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WordMorpher
{
    /// <summary>
    /// Class for managing the a* implementation and functions.
    /// </summary>
    class AStar
    {
        readonly string resultFile; //file location of the output file

        List<int> start;
        List<int> goal;
        
        public AStar(List<int> input, List<int> output)
        {
            // Convert the words into coordinates
            start = input;
            goal = output;
        }

        /// <summary>
        /// Perform the a* algorithm
        /// </summary>
        /// <param name="g"></param>
        /// <param name="start"></param>
        /// <param name="goal"></param>
        public Node PerformAStar(Graph g)
        {
            if (!g.CheckLoc(goal) || !g.CheckLoc(start)) return null;
            //Initialise the two sets needed.
            List<Node> closedSet = new List<Node>();
            List<Node> openSet = new List<Node>();

            openSet.Add(new Node(start, 0, null));

            while (openSet.Count > 0)
            {
                //Remove lowest f node from open list
                int min = openSet.Min(x => x.f);
                Node q = openSet.Find(x => x.f == min);
                openSet.Remove(q);
                List<Node> neighbours = GenerateNeighbours(g, q);
                foreach (Node n in neighbours)
                {
                    if(n.coords.SequenceEqual(goal)) //reached goal
                    {
                        return n;
                    }
                    n.g = q.g + 1; //cost of move is always one: changing one letter.
                    n.h = HammingDistance(n.coords, goal); //distance is the hamming distance, number of changes required.
                    n.f = n.g + n.h;

                    //don't add to closed if a better alternative already exists.
                    int openIndex = openSet.FindIndex(x => x.coords.Equals(n.coords) && x.f <= n.f);
                    int closedIndex = closedSet.FindIndex(x => x.coords.Equals(n.coords) && x.f <= n.f);
                    if (openIndex < 0 && closedIndex < 0)
                    {
                        openSet.Add(n);
                    }
                }
                closedSet.Add(q);
            }

            return null;
        }
        
        /// <summary>
        /// Generate all the neighbours reachable from a node N.
        /// This means any word reachable by changing one letter.
        /// Run loop four times to loop through all four dimensions.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private List<Node> GenerateNeighbours(Graph g, Node n)
        {
            List<Node> neighbours = new List<Node>();
            List<int> startCoords = n.coords;
            //First dimension
            for (int i = 0; i < 26; i++)
            {
                if (i != startCoords[0]) //don't add start node to neighbours
                {
                    List<int> testCoords = new List<int> { i, n.coords[1], n.coords[2], n.coords[3] };
                    if (g.CheckLoc(testCoords)) neighbours.Add(new Node(testCoords, 0, n));
                }
            }
            //Second dimension
            for (int i = 0; i < 26; i++)
            {
                if (i != startCoords[1]) //don't add start node to neighbours
                {
                    List<int> testCoords = new List<int> { n.coords[0], i, n.coords[2], n.coords[3] };
                    if (g.CheckLoc(testCoords)) neighbours.Add(new Node(testCoords, 0, n));
                }
            }
            //Third dimension
            for (int i = 0; i < 26; i++)
            {
                if (i != startCoords[2]) //don't add start node to neighbours
                {
                    List<int> testCoords = new List<int> { n.coords[0], n.coords[1], i, n.coords[3] };
                    if (g.CheckLoc(testCoords)) neighbours.Add(new Node(testCoords, 0, n));
                }
            }
            //Fourth dimension
            for (int i = 0; i < 26; i++)
            {
                if (i != startCoords[3]) //don't add start node to neighbours
                {
                    List<int> testCoords = new List<int> { n.coords[0], n.coords[1], n.coords[2], i };
                    if (g.CheckLoc(testCoords)) neighbours.Add(new Node(testCoords, 0, n));
                }
            }

            return neighbours;
        }

        /// <summary>
        /// The hamming distance between two "words" of the same length
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int HammingDistance(List<int> a, List<int> b)
        {
            if (a.Count != b.Count) return int.MaxValue; //don't perform if non-identical lengths
            int dist = 0;
            for(int i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i]) dist++;
            }
            return dist;
        }

        /// <summary>
        /// Save the output to the file requested.
        /// </summary>
        private void SaveOutput()
        {
            try
            {
                var fileStream = new FileStream(resultFile, FileMode.Open);
                fileStream.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Creating the file at: " + resultFile);
                File.Create(resultFile);
            }

        }
        
    }

    /// <summary>
    /// Class for representing points on a graph.
    /// </summary>
    class Node
    {
        public List<int> coords;
        public int f = 0, g = 0, h = 0;
        public Node parent;
        public Node(List<int> coords, int f, Node p)
        {
            this.coords = coords;
            this.f = f;
            parent = p;
        }

        /// <summary>
        /// Iterates through parents of a node and returns the path of coordinates
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public List<string> GetPath(Graph g)
        {
            List<string> path = new List<string>();
            Node x = this;
            while(x != null)
            {
                path.Insert(0,g.CoordToString(x.coords));
                x = x.parent;
            }
            return path;
        }
    }
}
