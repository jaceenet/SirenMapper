using System;
using System.Collections.Specialized;
using System.Web;

namespace SirenMapper
{
    public class HateoasUrl
    {
        public HateoasUrl(string rootUrl, string secret)
        {
            RootUrl = rootUrl;
            Secret = secret;
        }

        public string RootUrl { get; private set; }

        public string Secret { get; private set; }

        public string GetApiUrl(string relative, string[] ignoredquerykeys = null, bool noChecksum = false, string method = "GET")
        {
            var uri = new Uri(string.Format("{0}/{1}", RootUrl, relative.TrimStart('/')));

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

        
        public string Checksum(string method, string s, string timeoutOption = "")
        {
            return String.Concat(method.ToUpper(), Secret, timeoutOption, s).Sha1();
        }

        public bool ValidateUri(Uri uri, string method, string[] IgnoreParameters = null)
        {
            string checksum = HttpUtility.ParseQueryString(uri.Query)["checksum"];

            var validateUrl = ModifyQuery(uri, x =>
            {
                if (IgnoreParameters != null)
                {
                    foreach (var q in IgnoreParameters)
                    {
                        x.Remove(q);
                    }
                }

                x.Remove("checksum");
            });

            return Checksum(method, validateUrl.ToString(), DateTime.UtcNow.Date.ToShortDateString()).Equals(checksum, StringComparison.Ordinal)
                   || Checksum(method, validateUrl.ToString(), DateTime.UtcNow.AddDays(-1).Date.ToShortDateString()).Equals(checksum, StringComparison.Ordinal);
        }


    }
}