using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;
using SensorService.Shared.Wrappers;
using SensorService.Socket.Dtos;

namespace SensorService.Socket
{
    public class SocketManager : ISocketManager
    {
        private const string LoginEndpoint = "api/user/login";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        private IMqttServer _mqttServer;
        
        public SocketManager(IHttpClientFactory httpClientFactory,IHttpContextAccessor contextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _contextAccessor = contextAccessor;
            Init();
        }

        private async Task Init()
        {
            var options = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithConnectionValidator(ValidateConnection)
                .WithDefaultEndpointPort(1883).Build();

            _mqttServer = new MqttFactory().CreateMqttServer();
            _mqttServer.ClientConnected += OnClientConnected;
            _mqttServer.ClientDisconnected += OnClientDisconnected;
            _mqttServer.ApplicationMessageReceived += OnMessage;
            _mqttServer.Started += OnStarted;
            await _mqttServer.StartAsync(options);
        }

        private void OnClientDisconnected(object sender, MqttClientDisconnectedEventArgs e)
        {
            Console.WriteLine("Client disconnected");
        }

        private void OnMessage(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ClientId);
        }

        private void OnStarted(object sender, MqttServerStartedEventArgs e)
        {
            Console.WriteLine("Started!");
        }

        private void OnClientConnected(object sender, MqttClientConnectedEventArgs e)
        {
            Console.WriteLine(e.Client);
        }

        private void ValidateConnection(MqttConnectionValidatorContext context)
        {
            var user = LoginUser(new LoginDto {UserName = context.Username, Password = context.Password});
            if (user == null)
            {
                context.ReturnCode = MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;
                return;
            }

            SetUserCookie(user).RunSynchronously();

            context.ReturnCode = MqttConnectReturnCode.ConnectionAccepted;
            return;
        }

        private async Task SetUserCookie(UserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            if (user.IsAdministrator)
            {
                var adminClaim = new Claim(ClaimTypes.Role, "Administrator");
                claims.Add(adminClaim);
            }

            var userIdentity = new ClaimsIdentity(claims, "login");

            var principal = new ClaimsPrincipal(userIdentity);
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public void SendMessage(string message, string topic = "/")
        {
            var mqttMessage = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(message).Build();
            _mqttServer.PublishAsync(mqttMessage);
        }

        private UserDto LoginUser(LoginDto loginDto)
        {
            using (var client = _httpClientFactory.Create(new Uri("localhost:8080")))
            {
                var json = JsonConvert.SerializeObject(loginDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync(LoginEndpoint, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<UserDto>(jsonResponse);
                }

                return null;
            }
        }
    }

    public interface ISocketManager
    {
        void SendMessage(string message, string topic = "/");
    }
}
