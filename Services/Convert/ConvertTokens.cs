using PlagiarismSystem.Models;
using Xylab.PlagiarismDetect.Frontend;

namespace PlagiarismSystem.Services.Convert
{
    public class ConvertTokens
    {
        private Matching compareResults;
        public ConvertTokens(Matching compareRes,int userPercent)
        {
            compareResults = compareRes;
            FileA = ((SubmissionString)compareRes.SubmissionA.File).Content;
            FileB = ((SubmissionString)compareRes.SubmissionB.File).Content;
            charsFileA = FileA.ToList();
            charsFileB = FileB.ToList();
            FileALines = (FileA.Split('\n')); //.Where(str=> str != "").ToArray();
            FileBLines = FileB.Split('\n');
            FileATokens = compareRes.SubmissionA.IL.tokens;
            FileBTokens = compareRes.SubmissionB.IL.tokens;
            MatchPairs = compareRes.ToList();
            FileAName = ((SubmissionString)compareRes.SubmissionA.File).Path;
            FileBName = ((SubmissionString)compareRes.SubmissionB.File).Path;
            Percent = compareRes.Percent;
            this.userPercent = userPercent;
        }

        public int userPercent {get;set;}
        public double Percent {get;set;}
        public string FileA {get;}
        public string FileAName {get;}
        public string FileBName {get;}
        public List<Token> FileATokens {get;}
        public string[] FileALines {get;}
        public string[] FileBLines {get;}
        public List<char> charsFileA {get;} 
        public string FileB {get;}
        public List<char> charsFileB {get;}
        public List<Token> FileBTokens {get;}
        public List<MatchPair> MatchPairs {get;}

        public void FileAMatchPair(out string resultStr)
        {
            resultStr = "";
            foreach(var pair in MatchPairs)
            {
                var StartA = FileATokens[pair.StartA].Column;
                var Length =FileATokens[pair.Length].Column+FileATokens[pair.Length].Length;
                for(int i=StartA; i < Length-1;i++)
                {
                    resultStr = $"{resultStr}{charsFileA[i]}";
                }
            }
           
        }

        public Dictionary<ConvertFileModel, ConvertFileModel> FilesMatchPair()
        {
            if(userPercent > Percent)
            {
                return new Dictionary<ConvertFileModel, ConvertFileModel>{
                {new ConvertFileModel{FileName = FileAName, ParseContent = "Совпадений не найдено", Percent = this.Percent},
                new ConvertFileModel{FileName = FileBName, ParseContent = "Совпадений не найдено",Percent = this.Percent}}
            };
            }
            var resultStrA = "";
            var resultStrB = "";
            foreach(var pair in MatchPairs)
            {
                resultStrA = $"{resultStrA}{pair.ToString()}\n";
                var StartALine = FileATokens[pair.StartA].Line - 1;
                var tokenEndA = FileATokens[pair.Length + pair.StartA - 1];
                var EndALine = (FileATokens[pair.Length + pair.StartA].Line == tokenEndA.Line + 1 || FileATokens[pair.Length + pair.StartA].Line == tokenEndA.Line) ?  tokenEndA.Line - 1 : FileATokens[pair.Length + pair.StartA].Line - 2;
                for(int i=StartALine; i <= EndALine;i++)
                {
                    resultStrA = $"{resultStrA}{FileALines[i]}\n";
                }
                resultStrB = $"{resultStrB}{pair.ToString()}\n";
                var StartBLine = FileBTokens[pair.StartB].Line - 1;
                var tokenEndB = FileBTokens[pair.Length + pair.StartB - 1];
                var EndBLine = (FileBTokens[pair.Length + pair.StartB].Line == tokenEndB.Line + 1 || FileBTokens[pair.Length + pair.StartB].Line == tokenEndB.Line) ?  tokenEndB.Line - 1 : FileBTokens[pair.Length + pair.StartB].Line - 2;
                for(int i=StartBLine; i <= EndBLine;i++)
                {
                   resultStrB = $"{resultStrB}{FileBLines[i]}\n";
                }
            }
            resultStrA = resultStrA == "" ? "Совпадений не найдено" : resultStrA;
            resultStrB = resultStrB == "" ? "Совпадений не найдено" : resultStrB;
           
            return new Dictionary<ConvertFileModel, ConvertFileModel>{
                {new ConvertFileModel{FileName = FileAName, ParseContent = resultStrA, Percent = this.Percent},
                new ConvertFileModel{FileName = FileBName, ParseContent = resultStrB,Percent = this.Percent}}
            };
        }

        // public Dictionary<ConvertFileModel, ConvertFileModel> FilesMatchPair()
        // {
        //     var resultStrA = "";
        //     var resultStrB = "";
        //     foreach(var pair in MatchPairs)
        //     {
        //         var StartA = FileATokens[pair.StartA].Column;
        //         var LengthA =FileATokens[pair.Length + pair.StartA].Column+FileATokens[pair.Length + pair.StartA].Length;
        //         for(int i=StartA; i < LengthA-1;i++)
        //         {
        //             resultStrA = $"{resultStrA}{charsFileA[i]}";
        //         }
        //         var StartB = FileBTokens[pair.StartB].Column;
        //         var LengthB = FileBTokens[pair.Length + pair.StartB].Column + FileBTokens[pair.Length + pair.StartB].Length;
        //         for(int i=StartB; i < LengthB-1;i++)
        //         {
        //             resultStrB = $"{resultStrB}{charsFileB[i]}";
        //         }
        //     }
        //     resultStrA = resultStrA == "" ? "Совпадений не найдено" : resultStrA;
        //     resultStrB = resultStrB == "" ? "Совпадений не найдено" : resultStrB;
           
        //     return new Dictionary<ConvertFileModel, ConvertFileModel>{
        //         {new ConvertFileModel{FileName = FileAName, ParseContent = resultStrA},
        //         new ConvertFileModel{FileName = FileBName, ParseContent = resultStrB}}
        //     };
        // }
    }
}