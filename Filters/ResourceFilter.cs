using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Filters;
using PlagiarismSystem.Models;

namespace PlagiarismSystem.Filters
{
    public class ResourceFilter : Attribute, IResourceFilter
    {
        public FoldersModel FoldersModel {get;set;}
        public ResourceFilter()
        {
            FoldersModel = new FoldersModel();
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Dictionary<string, string> filesDic = new Dictionary<string, string>();
            foreach(var file in FoldersModel.Folders.Where(x => x != "Выберите папку для сравения"))
            {
                var newUploadPath = $"{Directory.GetCurrentDirectory()}/Upload/{file}";
                var filesOnFolder=Directory.GetFiles(newUploadPath);
                var strFilesOnFolder = "";   
                foreach(var strFile in filesOnFolder)
                {
                    var fn = Path.GetFileName(strFile);
                    if(strFilesOnFolder == "") { strFilesOnFolder += WebUtility.UrlEncode(fn); continue;}
                    strFilesOnFolder += "," + WebUtility.UrlEncode(fn);
                }
                filesDic.Add(file,strFilesOnFolder);
            }
            var keyValuePairs = filesDic.Select(kvp => string.Format("\"{0}\":\"{1}\"", kvp.Key, kvp.Value));
            string json = "{" + string.Join(",", keyValuePairs) + "}";
            context.HttpContext.Response.Headers.Append("FoldersAndFiles", json);

            var uploadPath = $"{Directory.GetCurrentDirectory()}/Upload";
            if(!Directory.Exists(Directory.GetCurrentDirectory()+"/Upload"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory()+"/Upload");
            }
            var strArr =Directory.GetFiles(uploadPath);
            var resultStr = "";
            //var encodedLocationName = WebUtility.UrlEncode(locationName);
            foreach(var str in strArr){
                var fn = Path.GetFileName(str);
                if(resultStr == "") { resultStr += WebUtility.UrlEncode(fn); continue;}
                resultStr += "," + WebUtility.UrlEncode(fn);
            }
            if(resultStr == "")
            {
                context.HttpContext.Response.Headers.Append("files_server", "NotFind");
            }else
            {
                context.HttpContext.Response.Headers.Append(
                "files_server", resultStr);
            }
            
        }

        public static void GetFilesOnFolder(string folder,HttpContext context)
        {
            var uploadPath = Path.Combine($"{Directory.GetCurrentDirectory()}/Upload/",folder);
            if(!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var strArr =Directory.GetFiles(uploadPath);
            var resultStr = "";
            //var encodedLocationName = WebUtility.UrlEncode(locationName);
            foreach(var str in strArr){
                var fn = Path.GetFileName(str);
                if(resultStr == "") { resultStr += WebUtility.UrlEncode(fn); continue;}
                resultStr += "," + WebUtility.UrlEncode(fn);
            }
            if(resultStr == "")
            {
                context.Response.Headers.Append("FilesOnFolder", "NotFind");
            }else
            {
                context.Response.Headers.Append(
                "FilesOnFolder", resultStr);
            }
        }
    }
}