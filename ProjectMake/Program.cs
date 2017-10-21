using Fclp;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Text;

namespace ProjectMake
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new FluentCommandLineParser();
            string projectName = "";
            string directory = Directory.GetCurrentDirectory();
            bool js = false;

            directory = Path.GetFullPath(Path.Combine(directory, "..\\"));
            p.Setup<string>('p', "project")
             .Callback(value => projectName = value)
             .Required();

            p.Setup<string>('d', "dir")
             .Callback(value => directory = value);
            
            p.Setup<bool>('a', "apps")
             .Callback(value => js = value);

            p.SetupHelp("?","h", "help")
             .Callback(value => {
                 Console.WriteLine("-p, -project: Project Name\n"+
                    "-d, -dir: Target directory to create the project\n"+
                    "-a, -app: Include javascript frontend App\n");
             });

            var result = p.Parse(args);

            if (!result.HasErrors)
            {
                if (string.IsNullOrEmpty(projectName))
                {
                    Console.Write("Project Name:");
                    projectName = Console.ReadLine();
                    Console.Write("Directory Name [default: {0}]:", directory);
                    string dir = Console.ReadLine();
                    Console.WriteLine();
                    Console.Write("Create javascript app? (yes/no):");
                    string apps = Console.ReadLine()?.ToLower().Trim();

                    if(!string.IsNullOrEmpty(apps))
                    {
                        if(apps=="y" || apps=="yes" || apps=="true")
                        {
                            js = true;
                        }
                    }
                    if(!string.IsNullOrEmpty(dir))
                    {
                        directory = dir;
                    }

                    Run(projectName, directory, js);
                }
            }
            
            Console.WriteLine("Press <enter> to exit");
            Console.ReadLine();
        }

        static void Run(string projectName, string targetDir,bool apps=false)
        {
            string appDir = AppContext.BaseDirectory;
            string sourceZip = appDir + "blank.zip";
            string appsZip = appDir + "apps.zip";
            string destProjectDir = Path.Combine(targetDir, projectName);

            if(!Directory.Exists(destProjectDir))
            {
                Directory.CreateDirectory(destProjectDir);

                ExtractZipFile(projectName,sourceZip, destProjectDir);

                if (apps)
                {
                    Console.WriteLine("Unpacking Apps");
                    fastZipUnpack(appsZip, destProjectDir);
                }

                Console.WriteLine("Completed. Project is created in {0}", destProjectDir);
            }
            else
            {
                Console.WriteLine("Error! Project Directory {0} already exists.", destProjectDir);
            }

        }

        public static void ExtractZipFile(string projectName,string archiveFilenameIn, string outFolder)
        {
            ZipFile zf = null;

            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);

                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    zipStream = replaceReferences("Blank.NancyCore", projectName, zipStream);

                    // Manipulate the output filename here as desired.
                    
                    entryFileName = entryFileName.Replace("Blank.NancyCore", projectName);

                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }

                    Console.Write(".");
                }
                Console.WriteLine();
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }

        private static void fastZipUnpack(string zipFileName, string targetDir)
        {

            FastZip fastZip = new FastZip();

            // Will always overwrite if target filenames already exist
            fastZip.ExtractZip(zipFileName, targetDir, null);
        }

        private static Stream replaceReferences(string oldRef,string newRef,Stream stream)
        {
            StreamReader sr = new StreamReader(stream);

            string contents = sr.ReadToEnd();
            contents = contents.Replace(oldRef, newRef, StringComparison.InvariantCultureIgnoreCase);

            byte[] bytes = Encoding.ASCII.GetBytes(contents);
            var newStream = new MemoryStream(bytes);
            return newStream;
        }
    }
}
