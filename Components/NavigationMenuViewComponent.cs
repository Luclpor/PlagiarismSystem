using Microsoft.AspNetCore.Mvc;
using PlagiarismSystem.Models;

namespace PlagiarismSystem.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        public MainPagesModel pagesModel {get;set;} = new MainPagesModel();
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedArea = RouteData?.Values["area"];
            return View(pagesModel.Pages);
        }
    }
}