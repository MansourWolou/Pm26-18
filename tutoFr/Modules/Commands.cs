using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace tutoFr.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>// héritage de tte les méthodes propres a discord
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            var guess = Context.Message.Content;
            await ReplyAsync("tu as dis " + guess.ToString() + " alors je te répond Pong!") ;
        }
        // 
        [Command("avatar")]
        public async Task AvatarAsync(ushort size = 512)
        {
            await ReplyAsync(Context.User.Username + " la photo de ton avatar est :");
            await ReplyAsync(CDN.GetUserAvatarUrl(Context.User.Id, Context.User.AvatarId, size, ImageFormat.Auto));
        }

        [Command("react")]
        public async Task ReactAsync(string pMessage , string pEmoji)
        {
            var message = await Context.Channel.SendMessageAsync(pMessage);
            var emoji = new Emoji(pEmoji);

            await message.AddReactionAsync(emoji);
        }
        [Command("spam")]
        public async Task SpamAsync()
        {
            while (true) 
            {
                await ReplyAsync("on est arrivé ? ");
            } 
        }

        [Command("StartTchat")]
        public async Task StartTchatAsync(string pmessage)
        {

            var umessage = Context.Message.Content;
            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: umessage,
                from: new Twilio.Types.PhoneNumber("+15017122661"),
                to: new Twilio.Types.PhoneNumber("+15558675310")
            );

            Console.WriteLine(message.Sid);

        }

    }
}
