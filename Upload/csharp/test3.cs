using System.Collections.Generic; 
using System.Text; 
namespace TableParser 
{ 
 public class FieldsParserTask 
 { 
    public static List<string> ParseLine(string line) 
    { 
        int i = 0; 
        var fields = new List<string>(); 
        while (i < line.Length) 
        { 
            if (line[i] == ' ') 
                i++; 
            else 
            { 
                var field = ReadField(line, i); 
                fields.Add(field.Value); 
                i += field.Length;
            } 
        } 
        return fields; 
    } 
    
    private static Token ReadField(string line, int startIndex) 
    { 
        if (line[startIndex] == '\"' || line[startIndex] == '\'') 
            return HaveQ(line, startIndex, line[startIndex]); 
        else return DontHaveQ(line, startIndex); 
    } 
    
    public static Token DontHaveQ(string line, int startInd) 
    { 
        int count = 0; 
        int index = startInd; 
        StringBuilder result = new StringBuilder(); 
        var stop = "\'\" "; 
        while (index < line.Length && !stop.Contains(line[index].ToString()))
        { 
            result.Append(line[index]); 
            index++; 
        } 
        return new Token(result.ToString(), startInd, result.Length + count); 
    } 
    
    public static Token HaveQ(string line, int startInd, char stop) 
    { 
        int count = 0;
        int index = startInd + 1; 
        StringBuilder result = new StringBuilder(); 
        while (index < line.Length && line[index] != stop) 
        { 
            if (line[index] == '\\') 
            { 
                count++; index++; 
            } 
            result.Append(line[index]); 
            index++; 
        } 
        return new Token(result.ToString(), startInd++, result.Length + count + 2); 
    } 
 } 
}
