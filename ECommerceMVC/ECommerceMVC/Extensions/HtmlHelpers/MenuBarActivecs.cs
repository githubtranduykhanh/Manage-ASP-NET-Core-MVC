using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceMVC.UI.Extensions.HtmlHelpers
{
    public static class MenuBarActivecs
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controllers = "", string actions = "", string cssClass = "active")
        {
            ViewContext viewContext = htmlHelper.ViewContext;
            RouteValueDictionary routeValues = viewContext.RouteData.Values;

            string currentAction = routeValues["action"]?.ToString();
            string currentController = routeValues["controller"]?.ToString();

            var acceptedActions = (actions ?? currentAction)?.Split(',').Distinct().ToArray();
            var acceptedControllers = (controllers ?? currentController)?.Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? cssClass : string.Empty;
        }

        //public static string IsParentActive(this IHtmlHelper htmlHelper, string controllers = "", string actions = "")
        //{
        //    ViewContext viewContext = htmlHelper.ViewContext;
        //    RouteValueDictionary routeValues = viewContext.RouteData.Values;

        //    string currentAction = routeValues["action"]?.ToString();
        //    string currentController = routeValues["controller"]?.ToString();

        //    var acceptedActions = (actions ?? currentAction)?.Split(',').Distinct().ToArray();
        //    var acceptedControllers = (controllers ?? currentController)?.Split(',').Distinct().ToArray();

        //    return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? "active open" : string.Empty;
        //}


        public static string IsParentActive(this IHtmlHelper htmlHelper, string areas = "", string controllers = "", string actions = "")
        {
            ViewContext viewContext = htmlHelper.ViewContext;
            RouteValueDictionary routeValues = viewContext.RouteData.Values;

            string currentAction = routeValues["action"]?.ToString();
            string currentController = routeValues["controller"]?.ToString();
            string currentArea = routeValues["area"]?.ToString();

            var acceptedActions = string.IsNullOrEmpty(actions) ? new[] { currentAction } : actions.Split(',').Distinct().ToArray();
            var acceptedControllers = string.IsNullOrEmpty(controllers) ? new[] { currentController } : controllers.Split(',').Distinct().ToArray();
            var acceptedAreas = string.IsNullOrEmpty(areas) ? new[] { currentArea } : areas.Split(',').Distinct().ToArray();


            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) && acceptedAreas.Contains(currentArea) ? "active open" : string.Empty;
        }

    }
}
