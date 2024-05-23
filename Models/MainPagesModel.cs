namespace PlagiarismSystem.Models
{
    public class MainPagesModel 
    {
        public IEnumerable<PageInfo> Pages {get;set;}
        public MainPagesModel()
        {
            Pages = new List<PageInfo>
            {
                {new PageInfo{PageName="Загрузить",AreaName="Upload",ControllerName="Upload", ActionName="Upload"}},
                {new PageInfo{PageName="Сравнить",AreaName="Convert",ControllerName="Convert",ActionName="Create"}}
            };
        }
        
    }

    public class PageInfo
    {
        public string PageName {get;set;}
        public string AreaName {get;set;}
        public string ControllerName {get;set;}
        public string ActionName {get;set;}
    }
}