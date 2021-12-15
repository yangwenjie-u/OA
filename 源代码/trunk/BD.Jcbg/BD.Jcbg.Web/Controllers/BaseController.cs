using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spring.Context;
using Spring.Context.Support;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        public IApplicationContext webApplicationContext { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            webApplicationContext = ContextRegistry.GetContext();
        }
    }
}
