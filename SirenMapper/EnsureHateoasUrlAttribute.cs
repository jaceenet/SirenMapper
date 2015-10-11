using System;
using System.Collections.Specialized;
using System.Net;

namespace SirenMapper
{
	public class EnsureHateoasUrlAttribute : ActionFilterAttribute
	{
		public string[] IgnoreParameters { get; set; }

		public EnsureHateoasUrlAttribute(string[] ignoreParameters = null)
		{
			IgnoreParameters = ignoreParameters;
		}

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (!ValidateUri(actionContext.Request.RequestUri, actionContext.Request.Method.Method.ToUpper()))
			{
				actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Gone, 
					"Use the hateoas response url, valid for 48 hours", 
					actionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
			}

			base.OnActionExecuting(actionContext);
		}

		private bool ValidateUri(Uri uri, string method)
		{
			string checksum = HttpUtility.ParseQueryString(uri.Query)["checksum"];

			var validateUrl = ModifyQuery(uri, x =>
			{
				if (IgnoreParameters != null)
				{
					foreach (var q in this.IgnoreParameters)
					{
						x.Remove(q);
					}
				}

				x.Remove("checksum");
			});
			
			return Checksum(method, validateUrl.ToString(), DateTime.UtcNow.Date.ToShortDateString()).Equals(checksum, StringComparison.Ordinal) 
				|| Checksum(method, validateUrl.ToString(), DateTime.UtcNow.AddDays(-1).Date.ToShortDateString()).Equals(checksum, StringComparison.Ordinal);
		}

		public static string Checksum(string method, string s, string timeoutOption = "")
		{
			return String.Concat(method.ToUpper(),"6B9B2389B5ED44388", timeoutOption, s, "9B19CF3E7732F").Sha1();
		}

		public static string GetApiUrl(string relative, string[] ignoredquerykeys = null, bool noChecksum = false, string method = "GET")
		{
			var uri = new Uri(string.Format("{0}/{1}", GluuAppProcess.ApiUrl, relative.TrimStart('/')));

			if (noChecksum)
			{
				return uri.ToString();
			}

			Uri safeUrl = uri;

			if (ignoredquerykeys != null && ignoredquerykeys.Length > 0)
			{
				safeUrl = ModifyQuery(uri, x =>
				{
					foreach (var key in ignoredquerykeys)
					{
						x.Remove(key);
					}
				});
			}
			

			return ModifyQuery(uri, x =>
			{
				x.Add("checksum", HttpUtility.UrlEncode(Checksum(method, safeUrl.ToString(), DateTime.UtcNow.Date.ToShortDateString())));
			}).ToString();
		}

		private static Uri ModifyQuery(Uri uri, Action<NameValueCollection> modify)
		{
			var query = HttpUtility.ParseQueryString(uri.Query);
			modify(query);

			var url = uri.GetLeftPart(UriPartial.Path);
			for (int i = 0; i < query.Count; i++)
			{
				url += string.Format("{0}{1}={2}", (i == 0 ? "?" : "&"), query.Keys[i], query[i]);
			}

			return new Uri(url);
		}
	}
}