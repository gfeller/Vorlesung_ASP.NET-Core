using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace Pizza.Test.Helpers
{
    //https://gist.github.com/stefanhendriks/f7fe45d2e3bce09d002f3328d44915bc   
    public class AntiForgeryHelper
    {
        public static string ExtractAntiForgeryToken(string htmlResponseText)
        {
            if (htmlResponseText == null) throw new ArgumentNullException("htmlResponseText");

            System.Text.RegularExpressions.Match match = Regex.Match(htmlResponseText, @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");
            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }

        public static async Task<string> ExtractAntiForgeryToken(HttpResponseMessage response)
        {
            string responseAsString = await response.Content.ReadAsStringAsync();
            return await Task.FromResult(ExtractAntiForgeryToken(responseAsString));
        }
    }


    public class PostRequestHelper
    {
        public static HttpRequestMessage Create(String path, Dictionary<string, string> formPostBodyData)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = new FormUrlEncodedContent(ToFormPostData(formPostBodyData))
            };
            return httpRequestMessage;
        }

        public static List<KeyValuePair<string, string>> ToFormPostData(Dictionary<string, string> formPostBodyData)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            formPostBodyData.Keys.ToList().ForEach(key =>
            {
                result.Add(new KeyValuePair<string, string>(key, formPostBodyData[key]));
            });
            return result;
        }

        public static HttpRequestMessage CreateWithCookiesFromResponse(string path, Dictionary<string, string> formPostBodyData,
            HttpResponseMessage response)
        {
            var httpRequestMessage = Create(path, formPostBodyData);
            return CookiesHelper.CopyCookiesFromResponse(httpRequestMessage, response);
        }
    }

    public class CookiesHelper
    {

        // Inspired from:
        // https://github.com/aspnet/Mvc/blob/538cd9c19121f8d3171cbfddd5d842cbb756df3e/test/Microsoft.AspNet.Mvc.FunctionalTests/TempDataTest.cs#L201-L202

        public static IDictionary<string, string> ExtractCookiesFromResponse(HttpResponseMessage response)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            IEnumerable<string> values;
            if (response.Headers.TryGetValues("Set-Cookie", out values))
            {
                SetCookieHeaderValue.ParseList(values.ToList()).ToList().ForEach(cookie =>
                {
                    result.Add(cookie.Name, cookie.Value);
                });
            }
            return result;
        }

        public static HttpRequestMessage PutCookiesOnRequest(HttpRequestMessage request, IDictionary<string, string> cookies)
        {
            cookies.Keys.ToList().ForEach(key =>
            {
                request.Headers.Add("Cookie", new CookieHeaderValue(key, cookies[key]).ToString());
            });

            return request;
        }

        public static HttpRequestMessage CopyCookiesFromResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            return PutCookiesOnRequest(request, ExtractCookiesFromResponse(response));
        }
    }
}
