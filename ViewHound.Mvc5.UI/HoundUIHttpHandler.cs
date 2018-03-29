using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ViewHound.Mvc5.UI
{
    public class HoundUiHttpHandler : HttpMessageHandler
    {
        private readonly IHound _hound;

        public HoundUiHttpHandler(IHound hound)
        {
            _hound = hound;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var views = _hound.GetAllViewsLog()
                .Select(v => $"<p>View: {v.ViewPath}, Use count : {v.UseCount} </p>")
                .ToList();

            var httpContent = new StreamContent(CreateHtmlContent(views));

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(new HttpResponseMessage() { Content = httpContent, RequestMessage = request });
            return tsc.Task;

        }

        private static Stream CreateHtmlContent(IEnumerable<string> content)
        {
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            var scontent = $"{content.Select(s => $"<p>{s}</p>").Aggregate((s1, s2) => $"{s1}{s2}")}</>";
            writer.Write(scontent);
            writer.Flush();
            memoryStream.Position = 0;
            return memoryStream;

        }
    }
}