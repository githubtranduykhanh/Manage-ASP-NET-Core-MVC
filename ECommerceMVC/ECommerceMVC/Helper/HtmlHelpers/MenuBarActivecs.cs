using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceMVC.Helper.HtmlHelpers
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

        public static string IsParentActive(this IHtmlHelper htmlHelper, string controllers = "", string actions = "")
        {
            ViewContext viewContext = htmlHelper.ViewContext;
            RouteValueDictionary routeValues = viewContext.RouteData.Values;

            string currentAction = routeValues["action"]?.ToString();
            string currentController = routeValues["controller"]?.ToString();

            var acceptedActions = (actions ?? currentAction)?.Split(',').Distinct().ToArray();
            var acceptedControllers = (controllers ?? currentController)?.Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? "active open" : string.Empty;
        }
    }
}
