using System.Web.Mvc;

namespace Events.web.Areas.QACoor
{
    public class QACoorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QACoor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QACoor_default",
                "QACoor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}