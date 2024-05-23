namespace PlagiarismSystem.Models
{
    public class ConvertResult
    {
        public Dictionary<ConvertFileModel, ConvertFileModel> dictionaryFiles = new();
        public string Language {get;set;}
    }
}