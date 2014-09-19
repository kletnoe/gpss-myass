using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler.CLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("You should pass at least one parameter!");
            }

            string sourceModelPath = args[0];
            List<string> passedRefsPaths = args.Skip(1).ToList();

            if (!passedRefsPaths.Any())
            {
                passedRefsPaths = Compiler.AssemblyCompiler.DefaultRefs.ToList();
            }

            try {
                FileInfo modelFile;
                List<FileInfo> libraries;
                modelFile = GetFile(sourceModelPath);
                libraries = new List<FileInfo>();

                foreach (var passedRef in passedRefsPaths)
                {
                    libraries.Add(GetFile(passedRef));
                }

                AssemblyCompiler compiler = new AssemblyCompiler(modelFile, libraries);
                compiler.Compile(false);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static FileInfo GetFile(string path)
        {
            FileInfo fileinfo = new FileInfo(path);
            if (fileinfo.Exists)
            {
                return fileinfo;
            }
            else
            {
                throw new ArgumentException("File does not exists: " + path);
            }
        } 
    }
}
