using Microsoft.AspNetCore.Mvc;
using Xylab.PlagiarismDetect.Frontend;

namespace PlagiarismSystem.Services.Convert
{
    public class ConvertMatchPair{
    public ConvertMatchPair(){}
         public string Convert(Matching compareResults, string file1, string file2){
            var testGet = (SubmissionString)compareResults.SubmissionA.File;
            string a = testGet.Content;
            string resultStrA = "";
            string resultStrB = "";
            string test ="";
            Dictionary<int,int> startA =new Dictionary<int, int>();
            Dictionary<int,int> startB =new Dictionary<int, int>();
            foreach(var matchpair in compareResults){
                startA.Add(matchpair.StartA, matchpair.Length);
                startB.Add(matchpair.StartB, matchpair.Length);
            }
            List<char> file1List = new List<char>();
            List<char> file2List = new List<char>();
            foreach(var c in file1){
                file1List.Add(c);
            }
            foreach(var c in file2){
                file2List.Add(c);
            }
            var file1Tokens = compareResults.SubmissionA.IL.tokens;
            var file2Tokens = compareResults.SubmissionB.IL.tokens;
            resultStrA += "File: a.cs\n";
            foreach(var item in startA)
            {
                resultStrA += "MatchPair: [" + item.Key + "..." + item.Value + "]"; 
                for(int i =file1Tokens[item.Key].Column;i < file1Tokens[item.Value + item.Key].Column + file1Tokens[item.Value + item.Key].Length; i++)
                {
                    //if(i > item.Value) break;
                    resultStrA += file2List[i];
                    if(i>880 && i< 913){
                     test = file2List[i].ToString();
                    }
                   // if(i==1009) Console.WriteLine("here" + file1List[1012]);
                }
                resultStrA += "\n";

            }

            resultStrB += "File: b.cs\n";
            foreach(var item in startB)
            {   

                resultStrB += "MatchPair: [" + item.Key + "..." + item.Value + "]"; 
                for(int i =file2Tokens[item.Key].Column;i<file2List.Count && i < file2Tokens[item.Value + item.Key].Column +  file2Tokens[item.Value + item.Key].Length; i++)
                {
                    //if(i > item.Value) break;
                    resultStrB += file1List[i];
                }
                resultStrB += "\n";
               

            }
            return resultStrA + "\n" + resultStrB;
         }   
    }
}
