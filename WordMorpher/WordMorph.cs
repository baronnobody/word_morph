using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            AStar aStar = new AStar(args);

            return 1;
        }
    }
}
