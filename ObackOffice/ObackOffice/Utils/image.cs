
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;


namespace ObackOffice.Helpers
{
    public class FileResult : IHttpActionResult
    {
        private HttpResponseMessage response;
        public FileResult(string file_path, string content_type = null)
        {
            this.getImage(file_path, content_type);
        }
        public FileResult(byte[] fileinBytes, string content_type = null)
        {
            this.getImage(fileinBytes, content_type);
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return response;
            }, cancellationToken);
        }
        public void getImage(string filePath, string contentType)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(File.OpenRead(filePath))
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            this.response = response;
        }
        public void getImage(byte[] fileinBytes, string contentType)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileinBytes)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            this.response = response;
        }
    }
}

