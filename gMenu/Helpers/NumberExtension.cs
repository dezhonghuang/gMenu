using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace gMenu.Helpers
{
    public static class NumberExtension
    {
        public static MvcHtmlString NumberFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            TagBuilder tagBuilder = new TagBuilder("input");

            tagBuilder.Attributes["type"] = "number";
            tagBuilder.Attributes["name"] = name;
            tagBuilder.Attributes["value"] = metaData.Model.ToString();
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(tagBuilder.ToString());
        }
    }
}