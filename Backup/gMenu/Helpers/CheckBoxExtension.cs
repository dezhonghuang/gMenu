using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace gMenu.Helpers
{
    public static class CheckBoxExtension
    {
        public static MvcHtmlString MyCheckBox<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            //string html = "<input type=\"checkbox\"";

            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.Attributes["type"] = "checkbox";
            tagBuilder.Attributes["name"] = name;
            //tagBuilder.Attributes["value"] = (metaData.Model.ToString() == "True")? "true" : "false";
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(tagBuilder.ToString());
        }
    }
}