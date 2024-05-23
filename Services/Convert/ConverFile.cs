using Microsoft.AspNetCore.Routing.Internal;
using PlagiarismSystem.Models;

namespace PlagiarismSystem.Services
{
    public static class ConvertFile
    {

        public static Dictionary<string,string> GetSourceFiles(FoldersModel fm)
        {
            var path = Directory.GetCurrentDirectory().Contains("bin") ? Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().IndexOf("\\bin\\Debug")) : Directory.GetCurrentDirectory();
            path = Path.Combine(path,"Upload");
           //path = path.Contains("bin") ? path.Remove(path.IndexOf("\\bin\\Debug")) : path;
            Dictionary<string, string> dicFiles = new Dictionary<string, string>();
            if(Directory.Exists(path))
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach(var dir in dirs)
                {
                    if(Path.GetFileName(dir) == fm.Folder1 || Path.GetFileName(dir) == fm.Folder2)
                    {
                        foreach(var d in Directory.EnumerateFiles(dir))
                        {
                            if(Path.GetFileName(d) == fm.File1 || Path.GetFileName(d) == fm.File2) dicFiles.Add(Path.GetFileName(d),new StreamReader(d).ReadToEnd());
                        }
                    }
                }
            }
            return dicFiles;
        }

        public static Dictionary<string,string> GetSourceFiles2(FoldersModel fm)
        {
            var path = Directory.GetCurrentDirectory().Contains("bin") ? Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().IndexOf("\\bin\\Debug")) : Directory.GetCurrentDirectory();
            path = Path.Combine(path,"Upload");
           //path = path.Contains("bin") ? path.Remove(path.IndexOf("\\bin\\Debug")) : path;
            Dictionary<string, string> dicFiles = new Dictionary<string, string>();
            if(Directory.Exists(path))
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach(var dir in dirs)
                {
                    if(Path.GetFileNameWithoutExtension(dir) == fm.Folder1)
                    {
                        foreach(var d in Directory.EnumerateFiles(dir))
                        {
                            if(Path.GetFileName(d) == fm.File1) dicFiles.Add(Path.GetFileName(d),new StreamReader(d).ReadToEnd());
                        }
                    }
                }
                foreach(var dir in dirs)
                {
                    if(Path.GetFileName(dir) == fm.Folder2)
                    {
                        foreach(var d in Directory.EnumerateFiles(dir))
                        {
                            if(Path.GetFileName(d) == fm.File1) continue;
                            dicFiles.Add(Path.GetFileName(d),new StreamReader(d).ReadToEnd());
                        }
                    }
                }
            }
            return dicFiles;
        }
    }
    }