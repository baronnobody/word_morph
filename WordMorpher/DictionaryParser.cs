using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WordMorpher
{
    class DictionaryParser
    {

        public DictionaryParser(Graph g, string fileLoc)
        {

            int lineCount = File.ReadLines(fileLoc).Count();
            //Parse the input file in parallel.
            CreateTasks(8, g, fileLoc, lineCount);
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
            Console.WriteLine("Finished parsing dictionary file.");
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
            Console.WriteLine("Started to parse dictionary file at line: " + startLine);
            for (int i = 0; i < linesToRead; i++)
            {
                string line = File.ReadLines(file).Skip(startLine + i).First();
                g.InsertWord(line);
            }
        }
    }
}
