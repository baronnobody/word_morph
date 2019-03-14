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
        public int words = 0;

        /// <summary>
        /// Convert a 4-length word into a coordinate space bounded by 0-25 and set it as an obstacle.
        /// </summary>
        /// <param name="word"></param>
        public void InsertWord(string word)
        {
            if(word.Length == 4 && word.All(char.IsLetter)) //check word length is valid, and only contains letters (symbols should not count as "letter changes" in my opinion)
            {
                graph[CharToCoord(word[0]), CharToCoord(word[1]), CharToCoord(word[2]), CharToCoord(word[3])] = true;
                words++;
            }
        }
        
        public char CoordToChar(int coord)
        {
            return (char)(coord + 65);
        }

        public string CoordToString(List<int> coords)
        {
            string output = "";
            foreach(int c in coords)
            {
                output = String.Concat(output, CoordToChar(c));
            }
            return output;
        }

        /// <summary>
        /// Converts a character into a number between 0-25.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int CharToCoord(char c)
        {
            return char.ToUpper(c) - 65;
        }

        /// <summary>
        /// Converts a string into an array of length s.length
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<int> StringToCoord(string s)
        {
            List<int> coord = new List<int>();
            foreach(char c in s)
            {
                coord.Add(CharToCoord(c));
            }
            return coord;
        }
        
        /// <summary>
        /// Check if an existing location is a real word or not.
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public bool CheckLoc(List<int> coords)
        {
            return graph[coords[0], coords[1], coords[2], coords[3]];
        }
    }
}
