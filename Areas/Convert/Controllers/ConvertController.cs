using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xylab.PlagiarismDetect.Frontend;
using Antlr4;
using System.Reflection;
using PlagiarismSystem.Services.Convert;
using System.Text;
using PlagiarismSystem.Services;
using PlagiarismSystem.Models;
using System.Runtime.Intrinsics.X86;
using PlagiarismSystem.Filters;
using Xylab.PlagiarismDetect.Frontend.Cpp;
using System.Reflection.Emit;

namespace PlagiarismSystem.Areas.Convert{
    [Area("Convert")]
    public class ConvertController : Controller{
        string folder;
        public FoldersModel foldersModel {get;set;}
        public ConvertController(FoldersModel fm)
        {
                foldersModel = fm;
        }
        [HttpPost]
        public IActionResult DefaultCompare(FoldersModel foldersModel)
        {
                Dictionary<string, string> dicFiles = ConvertFile.GetSourceFiles(foldersModel);
                string keyLanguage = "." + (ModelState["Key1"].AttemptedValue.ToLower() == "python" ? "py":ModelState["Key1"].AttemptedValue.ToLower());
                if( !keyLanguage.Contains(Path.GetExtension(dicFiles.First().Key)) || !dicFiles.All(kvp => Path.GetExtension(kvp.Key) == Path.GetExtension(dicFiles.First().Key)))
                { 
                        ModelState.AddModelError(nameof(foldersModel.Key1),"Расширение файлов не совпадает с указанным языком");
                        string? folder = HttpContext.Request.Headers["folder"];
                        if(folder != null) ResourceFilter.GetFilesOnFolder(HttpContext.Request.Headers["folder"],HttpContext);
                        return View("Create",foldersModel);
                }
                Type language = foldersModel.languageDic.FirstOrDefault(t => t.Key == foldersModel.Key1).Value;
                var lang = (ILanguage)Activator.CreateInstance(language);
                List<Submission> submissionsList = new List<Submission>();
                foreach(var kvp in dicFiles)
                {
                        submissionsList.Add(new Submission(lang, new SubmissionString(kvp.Key, kvp.Value)));

                }
                for(int i=0;i < submissionsList.Count();i+=2)
                {
                        var compareResult = GreedyStringTiling.Compare(submissionsList[i],submissionsList[i+1],lang.MinimalTokenMatch);
                        var convertTokens = new ConvertTokens(compareResult, foldersModel.Percent1);
                        //convertTokens.FileAMatchPair(out string resStr);
                        ConvertResult fileModel = new ConvertResult
                        {
                                dictionaryFiles = convertTokens.FilesMatchPair()
                        };
                        switch (language.ToString())
                        {
                                case "Xylab.PlagiarismDetect.Frontend.Cpp.Language":
                                        fileModel.Language = "c";
                                        break;
                                case "Xylab.PlagiarismDetect.Frontend.Python.Language":
                                        fileModel.Language = "python";
                                        break;
                                case "Xylab.PlagiarismDetect.Frontend.Csharp.Language":
                                        fileModel.Language = "csharp";
                                        break;
                                default:
                                        fileModel.Language = "c";
                                        break;
                        }
                        return View("Convert",fileModel);

                }
                return View("Convert");
        }

        public IActionResult ComparePairs(FoldersModel foldersModel)
        {
                Type language = foldersModel.languageDic.FirstOrDefault(t => t.Key == foldersModel.Key2).Value;
                var lang = (ILanguage)Activator.CreateInstance(language);
                Dictionary<string, string> dicFiles = ConvertFile.GetSourceFiles2(foldersModel);
                string keyLanguage = "." + (ModelState["Key2"].AttemptedValue.ToLower() == "python" ? "py":ModelState["Key2"].AttemptedValue.ToLower());
                if(!keyLanguage.Contains(Path.GetExtension(dicFiles.First().Key)) || !dicFiles.All(kvp => Path.GetExtension(kvp.Key) == Path.GetExtension(dicFiles.First().Key)))
                { 
                        ModelState.AddModelError(nameof(foldersModel.Key2),"Расширение файлов не совпадает с указанным языком");
                        string? folder = HttpContext.Request.Headers["folder"];
                        if(folder != null) ResourceFilter.GetFilesOnFolder(HttpContext.Request.Headers["folder"],HttpContext);
                        return View("Create",foldersModel);
                }
                var submissionMain = new Submission(lang, new SubmissionString(dicFiles.First().Key, dicFiles.First().Value));
                List<Submission> submissionsList = new List<Submission>();
                dicFiles.Remove(dicFiles.First().Key);
                foreach(var kvp in dicFiles)
                {
                        submissionsList.Add(new Submission(lang, new SubmissionString(kvp.Key, kvp.Value)));
                }

                ConvertResult fileModel = new ConvertResult();
                switch (language.ToString())
                {
                        case "Xylab.PlagiarismDetect.Frontend.Cpp.Language":
                                fileModel.Language = "c";
                                break;
                        case "Xylab.PlagiarismDetect.Frontend.Python.Language":
                                fileModel.Language = "python";
                                break;
                        default:
                                fileModel.Language = "c";
                                break;
                }

                for(int i=0;i < submissionsList.Count();i++)
                {
                        var compareResult = GreedyStringTiling.Compare(submissionMain,submissionsList[i],lang.MinimalTokenMatch);
                        var convertTokens = new ConvertTokens(compareResult,foldersModel.Percent2);
                        var dic = convertTokens.FilesMatchPair();
                        foreach(var kvp in dic)
                        {
                                if(!fileModel.dictionaryFiles.ContainsKey(kvp.Key))
                                {
                                        fileModel.dictionaryFiles.Add(kvp.Key,kvp.Value);
                                }
                        }
                        //return View("Convert",fileModel);

                }
                return View("Convert",fileModel);
        }

        [HttpPost]
        public IActionResult CreateFiles()
        {
                ViewBag.Folder = HttpContext.Request.Headers["folder"];

                return RedirectToAction("Create");
        }
        public ViewResult Create()
        {
                // if(fm != null)
                // {
                //         string? folder_ = HttpContext.Request.Headers["folder"];
                //         if(folder_ != null) ResourceFilter.GetFilesOnFolder(HttpContext.Request.Headers["folder"],HttpContext);
                //         return View("Create",fm);
                // }
                string? folder = HttpContext.Request.Headers["folder"];
                if(folder != null) ResourceFilter.GetFilesOnFolder(HttpContext.Request.Headers["folder"],HttpContext);
                return View("Create",foldersModel);
        }

    }
}
