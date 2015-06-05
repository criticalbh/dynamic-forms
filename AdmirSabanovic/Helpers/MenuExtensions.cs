using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AdmirSabanovic.Helpers
{
    public static class MenuExtensions
    {
        public static MvcHtmlString MenuItem(
            this HtmlHelper htmlHelper,
            string text,
            string action,
            string path,
            string controller,
            string icon = null
        )
        {
            var li = new TagBuilder("li");
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentAction = routeData.GetRequiredString("action");
            var currentController = routeData.GetRequiredString("controller");
            if (string.Equals(currentAction, action, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(currentController, controller, StringComparison.OrdinalIgnoreCase))
            {
                li.AddCssClass("active");
            }
            if (icon == null)
            {
                li.InnerHtml = htmlHelper.ActionLink(text, action, controller).ToHtmlString();
            }
            else
            {
                li.InnerHtml = createAnchor(createIconTag(icon), createUrl(path, action), text);
            }
            return MvcHtmlString.Create(li.ToString());
        }
        private static string createIconTag(String icon) {
            TagBuilder i = new TagBuilder("i");
            i.AddCssClass(icon);
            return i.ToString();
        }
        private static string createAnchor(string innerHtml, string url, string text) 
        {
            TagBuilder anchorBuilder = new TagBuilder("a");
            anchorBuilder.Attributes.Add("href", url);
            anchorBuilder.InnerHtml = innerHtml + text;
            return anchorBuilder.ToString();
        }
        private static string createUrl(string controller, string action)
        {
           if (action.CompareTo("Index") == 0)
            {
                action = null;
            }
            return controller + "/" + action;
        }
    }
}
