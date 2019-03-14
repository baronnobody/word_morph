using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordMorpher
{
    /// <summary>
    /// Class to handle representing words as a graph
    /// </summary>
    class Graph
    {

        bool[,,,] graph = new bool[26,26,26,26]; //boolean array to represent the graph

        /// <summary>
        /// Convert a 4-length word into a coordinate space bounded by 0-25 and set it as an obstacle.
        /// </summary>
        /// <param name="word"></param>
        public void InsertWord(string word)
        {
            if(word.Length == 4 && word.All(char.IsLetter)) //check word length is valid, and only contains letters (symbols should not count as "letter changes" in my opinion)
            {
                graph[(char.ToUpper(word[0]) - 65), (char.ToUpper(word[1]) - 65), (char.ToUpper(word[2]) - 65), (char.ToUpper(word[3]) - 65)] = true;
            }
        }
        

        public bool CheckLoc(int a, int b, int c, int d)
        {
            return graph[a, b, c, d];
        }
    }
}
