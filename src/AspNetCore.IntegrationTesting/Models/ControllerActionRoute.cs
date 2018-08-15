using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using AspNetCore.IntegrationTesting.Contracts;
using Newtonsoft.Json;

namespace AspNetCore.IntegrationTesting.Models
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="IControllerActionRoute" />
    internal class ControllerActionRoute : IControllerActionRoute
    {
        /// <summary>
        /// The controller token
        /// </summary>
        private const string ControllerToken = "[controller]";

        /// <summary>
        /// The path separator
        /// </summary>
        private const string PathSeparator = "/";

        /// <summary>
        /// The media type json
        /// </summary>
        private const string MediaTypeJson = "application/json";

        /// <summary>
        /// The a list of RegEx matches for route parameters
        /// </summary>
        private readonly List<Match> _parameterMatches;

        /// <summary>
        /// A list of query string parameters
        /// </summary>
        private readonly List<string> _queryString = new List<string>();

        /// <summary>
        /// A list of header values
        /// </summary>
        private readonly List<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// The route string builder
        /// </summary>
        private readonly StringBuilder _routeStringBuilder;

        /// <summary>
        /// The model set by http method that send a body
        /// </summary>
        private object _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerActionRoute"/> class.
        /// </summary>
        public ControllerActionRoute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerActionRoute"/> class.
        /// </summary>
        /// <param name="controllerAction">The controller action.</param>
        public ControllerActionRoute(IControllerAction controllerAction)
        {
            ParameterizedTemplate = string.Join(PathSeparator, controllerAction.RouteSegments);
            _parameterMatches = MatchRouteTokens(ParameterizedTemplate).ToList();
            _routeStringBuilder = new StringBuilder(ParameterizedTemplate);
            _routeStringBuilder.Replace(ControllerToken, controllerAction.Controller);
        }

        /// <summary>
        /// Gets the parameterized template.
        /// </summary>
        /// <value>
        /// The parameterized template.
        /// </value>
        public string ParameterizedTemplate
        {
            get;
        }

        /// <summary>
        /// Adds a route segment to the templated route.
        /// </summary>
        /// <param name="routeSegment">The route segment.</param>
        public void AddToRoute(string routeSegment)
        {
            _routeStringBuilder.Append(routeSegment);
        }

        /// <summary>
        /// Builds the URI.
        /// </summary>
        /// <returns></returns>
        private string BuildUri()
        {
            var sb = new StringBuilder(_routeStringBuilder.ToString());
            if (_queryString.Any())
            {
                sb.Append($"?{string.Join("&", _queryString)}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Builds the request message.
        /// </summary>
        /// <param name="controllerAction"></param>
        /// <returns></returns>
        public HttpRequestMessage BuildRequestMessage(IControllerAction controllerAction)
        {
            var message = new HttpRequestMessage(controllerAction.Method, BuildUri());

            if (_model != null)
            {
                message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeJson));
                message.Content = new StringContent(JsonConvert.SerializeObject(_model));
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeJson);
            }
            if (_headers.Any())
            {
                foreach (var item in _headers)
                {
                    message.Headers.Add(item.Key, item.Value);
                }
            }
            return message;
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <returns></returns>
        public string GetModel()
        {
            if (_model != null)
            {
                return JsonConvert.SerializeObject(_model);
            }
            return null;
        }

        /// <summary>
        /// Sets the header value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void SetHeaderValue(string name, object value)
        {
            _headers.Add(new KeyValuePair<string, string>(name, value.ToString()));
        }

        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetModel(object value)
        {
            _model = value;
        }

        /// <summary>
        /// Sets the query string parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void SetQueryStringParameter(string name, string value)
        {
            if (!_queryString.Contains(name))
            {
                _queryString.Add($"{name}={value}");
            }
        }

        /// <summary>
        /// Sets the route value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void SetRouteValue(string name, object value)
        {
            //find the matching parameter
            var match = _parameterMatches.Find(m =>
             {
                 var tokens = m.Value.Split(":".ToCharArray()); //check for route constraints
                 var item = tokens[0];
                 return item.Equals(name, System.StringComparison.OrdinalIgnoreCase);
             });
            if (match != null)
            {
                _routeStringBuilder.Replace($"{{{match.Value}}}", $"{value}");
            }
        }

        /// <summary>
        /// Matches the route tokens.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        private IEnumerable<Match> MatchRouteTokens(string template)
        {
            string pattern = @"[^{\}]+(?=})";
            foreach (Match m in Regex.Matches(template, pattern))
            {
                yield return m;
            }
        }

        public override string ToString()
        {
            return BuildUri();
        }
    }
}