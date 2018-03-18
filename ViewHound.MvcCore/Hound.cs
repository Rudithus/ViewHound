using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewHound
{
    public class Hound 
    {
        private readonly ILogger<Hound> _logger;
        private readonly RequestDelegate _next;        

        public Hound(ILogger<Hound> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Items.Add("Views", new Dictionary<string, int>());

            await _next(context);

            var views = context.Items["Views"] as Dictionary<string, int>;
            _logger.LogInformation(string.Join(Environment.NewLine, views.Select(v => $"{v.Key} : {v.Value}")));
        }
    }
}
