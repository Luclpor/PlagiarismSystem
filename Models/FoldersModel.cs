using System.ComponentModel.DataAnnotations;

namespace PlagiarismSystem.Models
{
    public class FoldersModel
    {
        //public string FolderName{get;set;}
        public Dictionary<string, Type> languageDic = new Dictionary<string, Type>()
        {
            {"Python",typeof(Xylab.PlagiarismDetect.Frontend.Python.Language)},
            {"Cpp",typeof(Xylab.PlagiarismDetect.Frontend.Cpp.Language)},
            {"CSharp",typeof(Xylab.PlagiarismDetect.Frontend.Csharp.Language)}
        };

        public string? Key1 {get;set;} //Language type
        public string? Key2 {get;set;} //Language type
        [Range(0,100)]
        [Display(Name ="Введите минимальный процент совпадения")]
        public int Percent1 {get;set;}
        [Range(0,100)]
        public int Percent2 {get;set;}
            
        public string Folder1{get;set;}
        public string File1{get;set;}

        public string Folder2 {get;set;}
        public string File2 {get;set;}
        public IEnumerable<string> Folders {get;set;} 

        public FoldersModel()
        {
            Folders = Directory.EnumerateDirectories(Directory.GetCurrentDirectory()+"/Upload").Select(x=>{
                string subString = "Upload";
                int indexOfSubstring = x.IndexOf(subString) + 7;
                x = x.Substring(indexOfSubstring);
                return x;
            });
            
        }
    }
    
}