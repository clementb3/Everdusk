using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;

namespace EverduskServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<WebSocket> webSockets = new List<WebSocket>();
            int idWebsocket = 0;
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        idWebsocket++;
                        webSockets.Add(webSocket);
                        var message = "Connected id : " + idWebsocket;
                        Console.WriteLine("Websocket Connected");
                        var bytes = Encoding.UTF8.GetBytes(message);
                        var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                        await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                        var messageBuffer = WebSocket.CreateClientBuffer(8192, 8192);
                        WebSocketReceiveResult result;
                        var msgStringTemp = "";
                        try
                        {
                            while (true)
                            {
                                messageBuffer = WebSocket.CreateClientBuffer(8192, 8192);
                                result = await webSocket.ReceiveAsync(messageBuffer, CancellationToken.None);
                                if (result.MessageType == WebSocketMessageType.Text)
                                {
                                    var msgString = Encoding.UTF8.GetString(messageBuffer.ToArray()).Replace("\0", "");
                                    if (msgString != msgStringTemp)
                                    {
                                        Console.WriteLine(msgString);
                                        bytes = Encoding.UTF8.GetBytes(msgString);
                                        arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                                        foreach (WebSocket socket in webSockets)
                                        {
                                            if (webSocket != socket)
                                            {
                                                try
                                                {
                                                    await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                                                }
                                                catch { }
                                            }
                                        }
                                        msgStringTemp = msgString;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            webSockets.Remove(webSocket);
                            Console.WriteLine("Websocket Closed");
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    }
                }
                else
                {
                    await next(context);
                }

            });
            app.MapRazorPages();

            app.Run();
        }
    }
}
