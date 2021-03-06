using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BD.Jcbg.Web.Filter
{
    public class JsonNetFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is JsonResult == false)
                return;

            filterContext.Result = new CustomJsonResult((JsonResult)filterContext.Result);
        }

        /// <summary>
        /// 自定义JsonResult返回的json为小写格式
        /// </summary>
        private class CustomJsonResult : JsonResult
        {
            public CustomJsonResult(JsonResult jsonResult)
            {
                this.ContentEncoding = jsonResult.ContentEncoding;
                this.ContentType = jsonResult.ContentType;
                this.Data = jsonResult.Data;
                this.JsonRequestBehavior = jsonResult.JsonRequestBehavior;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                    throw new ArgumentNullException("context");

                if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet
                    && String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("GET not allowed! Change JsonRequestBehavior to AllowGet.");

                var response = context.HttpContext.Response;

                response.ContentType = String.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

                if (this.ContentEncoding != null)
                    response.ContentEncoding = this.ContentEncoding;

                if (this.Data != null)
                {
                    var json = JsonConvert.SerializeObject(
                        this.Data,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });

                    response.Write(json);
                }
            }
        }
    }
}