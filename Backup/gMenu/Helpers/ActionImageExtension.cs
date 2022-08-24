using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gMenu.Helpers
{
    public static class ActionImageExtension
    {
        public static MvcHtmlString ActionImage(this HtmlHelper html, string actionName, string controllerName, object routeValues, string imagePath, string imgClass, string alt)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            //build the <img> tag
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url.Content(imagePath));
            imgBuilder.MergeAttribute("class", imgClass);
            imgBuilder.MergeAttribute("alt", alt);
            string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

            //build the <a> tag
            var anchorBuilder = new TagBuilder("a");
            anchorBuilder.MergeAttribute("href", url.Action(actionName, controllerName, routeValues));
            anchorBuilder.InnerHtml = imgHtml;

            string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(anchorHtml);
        }
    }
}