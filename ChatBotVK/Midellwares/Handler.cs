using static System.Net.Mime.MediaTypeNames;

namespace ChatBotVK.Midellwares
{
    public class Handler
    {
        private readonly RequestDelegate _next;

        public Handler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var content = context.Request;
            await _next.Invoke(context);
        }
    }
}
