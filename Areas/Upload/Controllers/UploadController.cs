using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;
using PlagiarismSystem.Filters;
using PlagiarismSystem.Models;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace PlagiarismSystem.Area.Upload.Controllers
{
    [Area("Upload")]
    [ResourceFilter]
    [ViewComponent(Name ="CreateNewFolder")]
    public class UploadController : Controller
    {
        public UploadController()
        {
        }

        public IViewComponentResult Invoke() => new ViewViewComponentResult()
        {
            ViewData = new ViewDataDictionary<NewFolderModel>(ViewData,new NewFolderModel())
        };

        [HttpPost]
        public IActionResult CreateFolder(NewFolderModel fm)
        {
            if(ModelState.IsValid){
                var path = Directory.GetCurrentDirectory().Contains("bin") ? Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().IndexOf("\\bin\\Debug")) : Directory.GetCurrentDirectory();
                    path = Path.Combine(path,"Upload",fm.FolderName);
                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
            }else
            {
                ModelState.AddModelError(nameof(fm.FolderName),"Пожалуйста, введите название папки");
            }
             return RedirectToAction(nameof(Upload));
            //return View("UploadFile",  new FoldersModel());
            // if(string.IsNullOrEmpty(fm.FolderName))
            // {
            //     ModelState.AddModelError(nameof(fm.FolderName),"Пожалуйста, введите название папки");
            // }
            // if(ModelState.IsValid)
            // {
            //     var path = Directory.GetCurrentDirectory().Contains("bin") ? Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().IndexOf("\\bin\\Debug")) : Directory.GetCurrentDirectory();
            //     path = Path.Combine(path,"Upload",fm.FolderName);
            //     if(!Directory.Exists(path))
            //     {
            //         Directory.CreateDirectory(path);
            //     }
            //     return RedirectToAction(nameof(Upload));
            // }else
            // {
            //     return RedirectToAction(nameof(Upload));
            // }
        }
        
        [HttpGet]
        public IActionResult Upload()
        {
            FoldersModel fm = new FoldersModel();
            return View("UploadFile", fm);
        }

   
        [HttpPost]
        public async void UploadFile()
        {
            var files = ControllerContext.HttpContext.Request.Form.Files;
            var folderName = ControllerContext.HttpContext.Request.Headers["folder"];

            foreach(var file in files)
            {
                using(var fileStream = new FileStream(Directory.GetCurrentDirectory()+"/Upload"+"/"+ folderName + "/" +file.FileName,FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                    
            }  
        }
    }
}