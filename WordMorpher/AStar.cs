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

        /// <summary>
        /// Create a new A* solver.
        /// </summary>
        /// <param name="args"></param>
        public AStar(string[] args)
        {
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

            resultFile = args[3];
            int lineCount = File.ReadLines(args[0]).Count();

            Graph g = new Graph(); //create new graph for solver to "navigate"

            //Parse the input file in parallel.
            CreateTasks(8, g, args[0], lineCount);
        }

        /// <summary>
        /// Creates a list of tasks that balance the workload between them.
        /// </summary>
        /// <param name="workers"></param>
        /// <param name="g"></param>
        /// <param name="fileLocation"></param>
        /// <param name="jobs"></param>
        /// <returns></returns>
        private void CreateTasks(int workers, Graph g, string fileLocation, int jobs)
        {
            Console.WriteLine(jobs);
            Task[] tasks = new Task[workers];
            if (workers <= 1) //return one worker if one or less are requested
            {
                tasks = new Task[1];
                tasks[0] = (Task.Factory.StartNew(() => ParseFile(g, fileLocation, 0, jobs)));
            }
            int jobsEach = (int)Math.Ceiling((double)jobs / (double)workers); //round up, read a few lines twice but doesn't matter.
            for (int i = 0; i < workers; i++)
            {
                int startJob = i * jobsEach;
                object jobsToDo = jobsEach;
                tasks[i] = (Task.Factory.StartNew(new Action<object>((j) => ParseFile(g, fileLocation, (int)j, jobsEach)), startJob));
            }

            Task.WaitAll(tasks);
        }

        /// <summary>
        /// Reads in a linesToRead lines from a file starting at line startLine and processes them.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="file"></param>
        /// <param name="startLine"></param>
        /// <param name="linesToRead"></param>
        public void ParseFile(Graph g, string file, int startLine, int linesToRead)
        {
            Console.WriteLine("Started at line: " + startLine);
            for (int i = 0; i < linesToRead; i++)
            {
                string line = File.ReadLines(file).Skip(startLine + i).First();
                g.InsertWord(line);
            }
            Console.WriteLine("Finished");
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
}
