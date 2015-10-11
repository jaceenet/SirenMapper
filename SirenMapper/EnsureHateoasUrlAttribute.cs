using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SirenMapper
{
	public abstract class EnsureHateoasUrlAttribute : ActionFilterAttribute
	{
	    private HateoasUrl url;

	    protected EnsureHateoasUrlAttribute(HateoasUrl url)
	    {
	        this.url = url;
	    }

		public string[] IgnoreParameters { get; set; }

		public EnsureHateoasUrlAttribute(string[] ignoreParameters = null)
		{
			IgnoreParameters = ignoreParameters;
		}

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (!url.ValidateUri(actionContext.Request.RequestUri, actionContext.Request.Method.Method.ToUpper(), this.IgnoreParameters))
			{
				actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Gone, 
					"Use the hateoas response url, valid for 48 hours", 
					actionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
			}

			base.OnActionExecuting(actionContext);
		}
	}
}