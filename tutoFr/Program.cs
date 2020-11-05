using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace tutoFr
{
    class Program
    {
        private DiscordSocketClient client;
        private CommandService commands;
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient( new DiscordSocketConfig 
            { 
                LogLevel = LogSeverity.Debug
            });

            commands = new CommandService();

            client.Log += Log;

            client.Ready += () =>
            {
                Console.WriteLine("je suis prêt bro");
                return Task.CompletedTask;
            };

            await InstallCommandsAsync();

            //attendre que leclient(bot) se connecte en allant chercher le token dans mes variables d'environnements
            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("discordToken", EnvironmentVariableTarget.User));

            //attendre que le client démare
            await client.StartAsync();

            //fait en sorte que le programme ne s trrmine jamais . vu que on veux que notre bot continu de tourner
            await Task.Delay(-1);
        }
        //delimite les comandes supporter
        public async Task InstallCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            // récupère tte nos commandes en cherchant un fichier gérant tt ça dans le rep
        }

        private async Task HandleCommandAsync(SocketMessage pMessage)
        {
            var message = pMessage as SocketUserMessage;//conversion de type pour analyser les messages de l'utilisateur

            if (message == null) return;

            int argPos = 0;//indexation du  premier caractère de la chaine de caractère de la commande

            if (!message.HasCharPrefix('!', ref argPos)) return;

            //création d'un contexte
            var context = new SocketCommandContext(client, message);

            // gère le resulta de la commande
            var result = await commands.ExecuteAsync(context, argPos, null);

            // si il y a eu erreur pendant l'exécution de la commande , envoyer un message d'erreur
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);

        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }
    }
}
