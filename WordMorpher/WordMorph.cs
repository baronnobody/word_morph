using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WordMorpher
{
    class WordMorph
    {
        /// <summary>
        /// Main function, takes the input requirements and manages them.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static int Main(string[] args)
        {
            if(args.Length < 4)
            {
                Console.WriteLine("Please input a dictionary file location, start word, end word and result file location.");
                return 0;
            }

            try
            {
                var fileStream = new FileStream(args[0], FileMode.Open);
                fileStream.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Failed to find the file at: " + args[0]);
                Console.WriteLine(e.Message);
                throw;
            }
            catch (IOException e)
            {
                Console.WriteLine("The file at: " + args[0] + ", could not be read.");
                Console.WriteLine(e.Message);
                throw;
            }

            Graph g = new Graph(); //create new graph for solver to "navigate"
            DictionaryParser d = new DictionaryParser(g, args[0]); //parse the dictionary file and form a graph
            AStar aStar = new AStar(g.StringToCoord(args[1]), g.StringToCoord(args[2]));
            Node goal = aStar.PerformAStar(g);
            if (goal == null) Console.WriteLine("Cannot reach the word " + args[2] + " from the word " + args[1] + ".");
            else
            {
                List<string> path = goal.GetPath(g);
                aStar.SaveOutput(args[3], path);
            }

            return 1;
        }
    }
}
