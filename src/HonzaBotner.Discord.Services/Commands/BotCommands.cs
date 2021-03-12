﻿using System;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using HonzaBotner.Discord.Services.Attributes;
using HonzaBotner.Discord.Services.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HonzaBotner.Discord.Services.Commands
{
    [Group("bot")]
    [Description("Commands to get you info about Discord bot and other things.")]
    public class BotCommands : BaseCommandModule
    {
        private readonly ILogger<BotCommands> _logger;
        private readonly InfoOptions _infoOptions;

        public BotCommands(ILogger<BotCommands> logger, IOptions<InfoOptions> infoOptions)
        {
            _logger = logger;
            _infoOptions = infoOptions.Value;
        }

        [Command("activity")]
        [Description("Changes bot's activity status.")]
        [RequireMod]
        public async Task Activity(CommandContext ctx,
            [Description("Type of the activity. Allowed values: competing, playing, streaming, watching or listening.")]
            string type,
            [Description("Status value of the activity."), RemainingText]
            string status
        )
        {
            try
            {
                DiscordActivity activity = new()
                {
                    ActivityType = type.ToLower() switch
                    {
                        "competing" => ActivityType.Competing,
                        "playing" => ActivityType.Playing,
                        "streaming" => ActivityType.Streaming,
                        "watching" => ActivityType.Watching,
                        "listening" => ActivityType.ListeningTo,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    Name = status,
                };

                await ctx.Client.UpdateStatusAsync(activity);
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:"));
            }
            catch (ArgumentOutOfRangeException)
            {
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":-1:"));
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Couldn't update bot's status");
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":-1:"));
            }
        }

        [Command("info")]
        [Description("Command to get you info about Discord bot and other things.")]
        public async Task BotInfo(CommandContext ctx)
        {
            StringBuilder stringBuilder = new();
            stringBuilder
                .Append("Tohoto bota vyvíjí komunita. " +
                        "Budeme rádi, pokud se k vývoji přidáš a pomůžeš nám bota dále vylepšovat.")
                .Append("\n\n")
                .Append($"Zdrojový kód najdeš [zde]({_infoOptions.RepositoryUrl}).")
                .Append("\n")
                .Append($"Hlásit chyby můžeš [tady]({_infoOptions.IssueTrackerUrl}).")
                ;

            try
            {
                DiscordEmbedBuilder embed = new()
                {
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        Name = ctx.Client.CurrentUser.Username, IconUrl = ctx.Client.CurrentUser.AvatarUrl
                    },
                    Title = "Informace o botovi",
                    Description = stringBuilder.ToString()
                };

                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Couldn't build and send a embed message");
            }
        }
    }
}
