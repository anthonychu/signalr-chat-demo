using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRChatDemo.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hubConnection = new HubConnectionBuilder().WithTransport(TransportType.All)
                .WithUrl("http://localhost:5000/chat?access_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImpvaG5kb2VAYW50aG9ueWNodS5jYSIsImV4cCI6MTUyNTI0NjcwMCwiaXNzIjoiU2lnbmFsUlRlc3RTZXJ2ZXIiLCJhdWQiOiJTaWduYWxSVGVzdHMifQ.m5HpKWSHsMVuZ2f10QNBfyzBYEevFVRMYxxaLjGK79s")
                //.WithAccessToken(() => "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFudGhvbnlAYW50aG9ueWNodS5jYSIsImV4cCI6MTUyNTI0MzczNiwiaXNzIjoiU2lnbmFsUlRlc3RTZXJ2ZXIiLCJhdWQiOiJTaWduYWxSVGVzdHMifQ.xGVosVgevMRipSPkE9Z1XU7xsOmoAecIdjgq9NGnC5M")
                .WithJsonProtocol().Build();

            hubConnection.Closed += e => System.Console.WriteLine(e.ToString());

            hubConnection.On<string, string>("newMessage", 
                (sender, message) => System.Console.WriteLine($"{sender}: {message}"));

            await hubConnection.StartAsync();

            System.Console.WriteLine("connected!");

            System.Console.ReadLine();
        
        }
    }
}
