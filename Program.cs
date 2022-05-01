using System;
using System.Reactive.Linq;
using System.Reflection;
using Biko;
using Guilded;
using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Base.Users;
using Newtonsoft.Json.Linq;

// Get the configuration values
JObject config = JObject.Parse(await File.ReadAllTextAsync("./config/config.json"));

string auth = config.Value<string>("auth")!,
       prefix = config.Value<string>("prefix")!;

using GuildedBotClient client = new(auth);

client.Prepared
      .Subscribe(me =>
          Console.WriteLine("The bot is prepared!\nLogged in as \"{0}\" with the ID \"{1}\"", me.Name, me.Id)
      );

// message awaiting functions
client.MessageCreated
    .Where(msgCreated => msgCreated.Content.StartsWith(prefix + "ping"))
    .Subscribe(async msgCreated => {
        Embed embed = new Embed();
        embed.Title = "Pong!";
       // embed.SetImage(new EmbedMedia("https://img.guildedcdn.com/UserAvatar/ccc500d83a66329ead7313f8e872ef02-Medium.png"));
        embed.AddField("Bot Author ID", client.Me.CreatedBy);
        embed.SetFooter(client.Me.Name, "https://img.guildedcdn.com/UserAvatar/ccc500d83a66329ead7313f8e872ef02-Medium.png");
    
        await msgCreated.ReplyAsync(embeds: embed);
        
    }
);
try
{

    client.MessageCreated
        .Where(msgCreated => msgCreated.Content.StartsWith(prefix + "purge"))
        .Subscribe(async msgCreated =>
        {
            var user = await client.GetMemberAsync(msgCreated.ServerId ?? new HashId(" "), msgCreated.CreatedBy);
            if (user.Type == UserType.User && msgCreated.CreatedBy == client.Me.CreatedBy)
            {
                Embed embed = new Embed();
                embed.Title = "Purge";
                embed.SetDescription(msgCreated.Content.Replace(prefix + "purge", ""));
                // embed.SetImage(new EmbedMedia("https://img.guildedcdn.com/UserAvatar/ccc500d83a66329ead7313f8e872ef02-Medium.png"));
                    var author = await client.GetMemberAsync(msgCreated.ServerId ?? new HashId(" "), client.Me.CreatedBy);

            embed.AddField("Bot Author", author.Name);
                embed.SetFooter(client.Me.Name, "https://img.guildedcdn.com/UserAvatar/ccc500d83a66329ead7313f8e872ef02-Medium.png");

                await msgCreated.ReplyAsync(embeds: embed);
            }
            else
            {
                await msgCreated.ReplyAsync(content:"Command Not implemented for non owner");
            }


        }
    );

    client.MessageCreated
        .Where(msgCreated => msgCreated.Content.StartsWith(prefix + "say"))
        .Subscribe(async msgCreated =>
        {
            Embed embed = new Embed();
            embed.Title = "Say";
            embed.SetDescription(msgCreated.Content.Replace(prefix + "say", ""));
            // embed.SetImage(new EmbedMedia("https://img.guildedcdn.com/UserAvatar/ccc500d83a66329ead7313f8e872ef02-Medium.png"));
            var author = await client.GetMemberAsync(msgCreated.ServerId ?? new HashId(" "), client.Me.CreatedBy);

            embed.AddField("Bot Author", author.Name);
            embed.SetFooter(client.Me.Name, "https://img.guildedcdn.com/UserAvatar/ccc500d83a66329ead7313f8e872ef02-Medium.png");

            await msgCreated.ReplyAsync(embeds: embed);



        }
    );
    client.MessageCreated
        .Where(msgCreated => msgCreated.Content.StartsWith(prefix + "userinfo"))
        .Subscribe(async msgCreated =>
        {
            var user = await client.GetMemberAsync(msgCreated.ServerId ?? new HashId(" "), msgCreated.CreatedBy);
            var args = msgCreated.Content.Split(" ");
            if (args.Length > 1)
            {
                try
                {
                    if (args[1].StartsWith("@"))
                    {
                        Embed embed1 = new Embed();
                        embed1.SetTitle("⚠️Error⚠️");
                        embed1.SetDescription("Tagging users not implemented currendly");
                        await msgCreated.ReplyAsync(embeds: embed1);
                        return;


                    }
                    user = await client.GetMemberAsync(msgCreated.ServerId ?? new HashId(" "), new HashId(args[1]));
                }
                catch (Exception ex)
                {


                }
            

            }
            Embed embed = new Embed();
            embed.Title = "";
            embed.SetDescription("");

            // embed.SetImage(new EmbedMedia("https://img.guildedcdn.com/UserAvatar/ccc500d83a66329ead7313f8e872ef02-Medium.png"));
            embed.AddField("UserID:", user.Id);
            embed.AddField("Name", user.Name);
            if (user.Nickname != null)
            {
                embed.AddField("Nickname", user.Nickname ?? " ");
            }

            embed.AddField("Join Date", "`"+user.JoinedAt.ToString()+"`");
            embed.AddField("Is Bot", user.IsBot);
            var author = await client.GetMemberAsync(msgCreated.ServerId ?? new HashId(" "), client.Me.CreatedBy);

            embed.AddField("Bot Author", author.Name);
            embed.SetFooter(client.Me.Name, "https://img.guildedcdn.com/UserAvatar/ccc500d83a66329ead7313f8e872ef02-Medium.png");

            await msgCreated.ReplyAsync(embeds: embed);


        }
    );

}
catch (Exception ex)
{
   Console.WriteLine(ex.Message);

}
/*
client.MessageCreated
    .Where(msgCreated => msgCreated.Content.StartsWith(prefix + "eval"))
    .Subscribe(async msgCreated => {
        if (false) {

            Embed embed = new Embed();
            embed.Title = "⚠️Failed Eval Attempt⚠️" ;
            embed.SetDescription("Please do not use this command it's for my owner only");
            // embed.SetImage(new EmbedMedia("https://img.guildedcdn.com/UserAvatar/ccc500d83a66329ead7313f8e872ef02-Medium.png"));
            embed.AddField("Bot Author ID", client.Me.CreatedBy);
            embed.SetFooter(client.Me.Name, "https://img.guildedcdn.com/UserAvatar/ccc500d83a66329ead7313f8e872ef02-Medium.png");
            
            await msgCreated.ReplyAsync(embeds: embed);

        }
        else
        {

            Util.Eval(msgCreated.Content.Replace(prefix + "eval", ""));
            
            await msgCreated.ReplyAsync("Error: Required Function not implemented");

          

        }

    }
);
*/
await client.ConnectAsync();

// Don't close the program when the bot connects
await Task.Delay(-1);