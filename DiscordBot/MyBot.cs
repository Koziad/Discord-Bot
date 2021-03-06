﻿using Discord;
using Discord.Commands;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class MyBot
    {
        DiscordClient discord;
        CommandService commands;

        // Random Number gen
        Random rand;
        Random Rand_roll;

        string[] randomGreetings;
        string[] randomTexts;
        string[] freshestMemes;


        public MyBot()
        {
            rand = new Random();

            // Freshest memes.
            freshestMemes = new string[]
            {
                "Memes/imgres.jpg",
                "Memes/scum.jpg",
                "Memes/really.png"
            };

            // Random text
            randomTexts = new string[]
            {
                "Stop using this command",
                "Some random text",
                "Do you even care?",
                "Do you like it?",
                "Quotes in 'quotes'",
                "I am a Hanzo main",
                "I need healing",
                "Do you even main Mercy?",
                "Bastion is the most skillful hero",
                "My body is 60% made out of salt",
                "Konami..........",
                "Played me like a damn fiddle!",
                "U Casul",
                "Did you lvl in DEX?",
                "Filthy magic user",
                "The legend never dies(Giant Dad)",
                "Chug your estus",
                "Plz fix Miyazaki",
                "It's definitely on the table",
                "Ash seeketh ember",
                "Men are props on the stage of life, and no matter how tender, how exquisite… A LIE WILL REMAIN A LIE!",
                 "Hmmmmm... Mhmmmm... Hmmmmmmmmmm...",
            };

            // Different greetings
            randomGreetings = new string[]
            {
                "Hello ",
                "Hi ",
                "Howdy ",
                "Yo "
            };

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            // Command prefix
            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
                x.HelpMode = HelpMode.Public;
            });

            // Command lines
            commands = discord.GetService<CommandService>();

            /*              Command List          */
            RegisterMemeCommand(); // The meme command.
            RegisterTextCommand(); // The random text command.
            GreetingsCommand();    // Greets back or greets the mentioned user.
            BotGreeting();         // Greets the user back.
            RolesCommand();        // Give a certain role to a user.
            PingCommand();         // Ping/Pong command.
            MeeseeksCommand();     // A Mr.Meeseeks Commands that uses a loop.
            RegisterPurge();       // Purge at most 100 messages.
            RegisterDice();        // A dice that will roll between 1 to 100.
            RegisterInvite();      // A simple command to make an invite for users.
            CreateChannel();       // Creates a text/voice channel, depends on the parameters.
            CreateCommandList();   // Creates and sends a private message to the user containing a list of the commands.

            // Token
            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("", TokenType.Bot); //Bot Token here.....
            });

        }

        // Dank memes
        private void RegisterMemeCommand()
        {
            commands.CreateCommand("meme")
              .Description("Provides the user memes.")
              .Do(async (e) =>
              {
                  int randomMemeIndex = rand.Next(freshestMemes.Length);
                  string memeToPost = freshestMemes[randomMemeIndex];
                  await e.Channel.SendFile(memeToPost);
              });
        }

        // Random text
        private void RegisterTextCommand()
        {
            commands.CreateCommand("text")
              .Description("Returns text")
              .Parameter("user", ParameterType.Unparsed)
              .Do(async (e) =>
              {
                  // Randomize the index
                  int randomTextIndex = rand.Next(randomTexts.Length);
                  // Pick a random string from the array.
                  string textToPost = randomTexts[randomTextIndex];
                  var toReturn = $"{e.GetArg("user")}";

                  await e.Channel.SendMessage(textToPost + toReturn);
              });
        }

        // Greets the user that was mentioned
        private void GreetingsCommand()
        {
            commands.CreateCommand("hello")
                .Alias(new string[] { "hi","hey" })
                .Description("Greets the person who is mentioned")
                .Parameter("user", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    int randomGreetIndex = rand.Next(randomGreetings.Length);
                    string greetToPost = randomGreetings[randomGreetIndex];
                    var toReturn = $"{e.GetArg("user")}";

                    await e.Channel.SendMessage(greetToPost + toReturn);
                });
        }

        // Bot greets the user back if the target was the bot
        private void BotGreeting()
        {
            commands.CreateCommand("greet")
                .Description("Greets the user back.")
                .Parameter("user", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    // It is a simple command....
                    await e.Channel.SendMessage("Hello " + e.User.Mention);
                });
        }

        // Role command.
        private void RolesCommand()
        {
            commands.CreateCommand("Role")
                .Description("Assign a role to a user")
                .Parameter("role", ParameterType.Optional)
                .Parameter("user", ParameterType.Optional)
                .Do(async (e) =>
                {
                    var user = $"{e.GetArg("user")}";
                    var RoleName = $"{e.GetArg("role")}";
                    var Admin = e.User.ServerPermissions.Administrator;

                    // Must have Admin priveleges to execute this command.
                    if (Admin)
                    {
                        try
                        {
                            // Finds a certain role in the server.
                            Discord.Role SearchRole = e.Server.FindRoles(RoleName).FirstOrDefault();
                            await e.User.AddRoles(SearchRole);
                        }
                        catch (Exception )
                        {
                            await e.Channel.SendMessage("Role does not exist");
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage(":no_entry:You don't have the permissions to execute this command.:no_entry:");
                    }
                });
        }

        // Do I have to explain this one?
        private void PingCommand()
        {
            commands.CreateCommand("ping")
                .Description("Returns 'pong'")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage(":ping_pong: Pong");
                });
        }

        // Keep it simple for a meeseek.
        private void MeeseeksCommand()
        {
            commands.CreateCommand("Meeseeks")
                .Description("Makes trouble")
                .Parameter("FirstUser", ParameterType.Required)
                .Parameter("SecondUser",ParameterType.Optional)
                .Do(async (e) =>
                {

                    var toReturn = $"{e.GetArg("FirstUser")}";
                    var SecondUser = $"{e.GetArg("SecondUser")}";

                    for (int i = 1; i < 3 ; i++)
                    {
                        // Check if there is no second user in this cycle.
                        if (SecondUser == "")
                        {
                            await e.Channel.SendMessage("You have roped me into this. " + toReturn);
                            await e.Channel.SendMessage("No, he roped me into this. " + e.User.Mention);
                        }
                        else
                        {
                            // If the var in the loop reaches 2,change the outcome
                            if (i == 2)
                            {
                                await e.Channel.SendMessage("You have roped me into this. " + toReturn);
                                await e.Channel.SendMessage("No, he roped me into this. " + e.User.Mention);
                            }
                            else
                            {
                                await e.Channel.SendMessage("You have roped me into this. " + toReturn);
                                await e.Channel.SendMessage("No, he roped me into this. " + SecondUser);
                            }
                        }
                    }
                    // Send Speech message with a classic Rick and Morty Quote.
                    await e.Channel.SendTTSMessage("EXISTENCE IS PAIN!");

                    //Images will be a bit delayed, but it does the job.
                    await e.Channel.SendMessage("https://i.redd.it/kla43n9eb4az.jpg");
                });
        }

        // Delete 100 messages. MAX = 100 messages
        private void RegisterPurge()
        {
            commands.CreateCommand("purge")
                .Description("Deletes chat messages")
                .Do(async (e) =>
                {
                    var ChannelManage = e.User.ServerPermissions.ManageChannels;
                    var Admin = e.User.ServerPermissions.Administrator;

                    // Either have permissions to manage channels or to be an admin
                    if (Admin || ChannelManage )
                    {
                        Message[] messageToDelete;
                        // Download the last 100 messages
                        messageToDelete = await e.Channel.DownloadMessages(100);

                        // Delete the messages.
                        await e.Channel.DeleteMessages(messageToDelete);

                        //Add a delay to prevent an overload of requests.
                        await Task.Delay(2000);
                    }
                    else
                    {
                        await e.Channel.SendMessage(":no_entry:You don't have the permissions to execute this command.:no_entry:");
                    }
                });
        }

        // Time to get addicted to rolling the dices
        private void RegisterDice()
        {
            commands.CreateCommand("dice")
                .Description("Will roll a dice. 1 - 100")
                .Parameter("Opponent",ParameterType.Optional)
                .Do(async (e) =>
                {
                    // The user's opponent
                    var opponent = $"{e.GetArg("Opponent")}";

                    Rand_roll = new Random();

                    // First roll for the opponent or the single user, depending on the parameters.
                    int roll = Rand_roll.Next(1,100);
                    //Second roll
                    int roll2 = Rand_roll.Next(1, 100);

                    // if there is no opponent and the user just wants to roll by himself
                    if(opponent == "")
                    {
                        if (roll >= 50)
                        {
                            await e.Channel.SendMessage(":game_die: " + e.User.Mention + " has rolled : " + roll + ". Lucky you!");
                        }
                        else
                        {
                            await e.Channel.SendMessage(":game_die: " + e.User.Mention + " has rolled : " + roll + ". Better luck next time");
                        }
                    }
                    else
                    {
                      if (roll > roll2)
                        {
                            await e.Channel.SendMessage(":game_die: " + opponent + " has rolled: " + roll + ". \n:game_die:" +
                                e.User.Mention + " has rolled " + roll2 + ". \n:medal:" + opponent + " has won.");
                        }
                      else
                        {
                            await e.Channel.SendMessage(":game_die: " + opponent + " has rolled: " + roll + ". \n:game_die:" +
                                 e.User.Mention + " has rolled " + roll2 + ". \n:medal:" + e.User.Mention + " has won.");
                        }
                    }

                });
        }

        // Invite your friends
        private void RegisterInvite()
        {
            commands.CreateCommand("inv")
                .Description("Creates an invite")
                .Parameter("DurationType",ParameterType.Required)
                .Parameter("Duration",ParameterType.Optional)
                .Parameter("Uses", ParameterType.Optional)
                .Do(async (e) =>
                {
                   var DurationType = $"{e.GetArg("DurationType")}".ToLower();

                   switch (DurationType)
                    {
                        // An invite that never expires
                        case "max":
                            await e.Channel.CreateInvite(maxAge: null,maxUses: null);
                            break;
                        // Time limit invite with a certain amount of uses
                        case "timer":
                            try
                            {
                                var Duration = Int32.Parse($"{e.GetArg("Duration")}");
                                var Uses = Int32.Parse($"{e.GetArg("Uses")}");
                                await e.Channel.CreateInvite(maxAge: Duration, maxUses: Uses);
                            }
                            catch (Exception)
                            {
                                await e.Channel.SendMessage("No parameters given. \nDefault values will be used.");
                                int Uses = 20;
                                await e.Channel.CreateInvite( maxUses: Uses);
                            }
                            break;
                        // Error if something went wrong with the parameters.
                        default:
                            await e.Channel.SendMessage("An error has occured");
                            break;
                    }

                   // Notify the user that it has been created.
                   await e.Channel.SendMessage("An invite has been created");
                });
        }

        // More typing, less clicking for a channel
        private void CreateChannel()
        {
            commands.CreateCommand("CreateChannel")
                .Description("Creates a channel")
                .Parameter("Channel type",ParameterType.Optional)
                .Parameter("Channel name", ParameterType.Optional)
                .Do(async (e) =>
                {
                    // Make it lower case to prevent going to the default.
                    var type = $"{e.GetArg("Channel type")}".ToLower();

                    var argument = $"{e.GetArg("Channel name")}";

                    // Check the first parameter to decide if the user wants a Voice or Text channel.
                    switch(type)
                    {
                        case "voice":
                            await e.Server.CreateChannel(argument, ChannelType.Voice);
                            break;
                        case "text":
                            await e.Server.CreateChannel(argument, ChannelType.Text);
                            break;
                        default:
                            await e.Server.CreateChannel("TextChannel", ChannelType.Text);
                            break;
                    }
                });
        }

        // Command list
        private void CreateCommandList()
        {
            commands.CreateCommand("cmd")
                .Description("Gives you a list with commands")
                .Do(async (e) =>
                {
                   var commandlist = await e.User.SendMessage("|-**Commands**-|" +
                       "\n \n -**!meme** : *Provides the user memes.* " +
                       "\n -**!text** [user]: Provides random jokes/quotes from games."  +
                       "\n -**!hello** [user] : *Greets the user if there was a @user mention. The bot will just say 'hello' if there was no mention used.*" +
                       "\n -**!greet** : *The bot will greet you nicely back.*" +
                       "\n -**!role** [Role name] [User]: *This will give a certain role to the mentioned user.* " +
                       "\n -**!pong** : *Returns Pong.*" +
                       "\n -**!meeseeks** <First User> [Optional Second User] : *This command will cause trouble for the mentioned Users.*" +
                       "\n -**!purge** : *This will delete 100 messages*" +
                       "\n -**!dice** : *Rolls a dice between the numbers 1 - 100* " +
                       "\n -**!inv** <invite type: timer/max> [timer Duration] [timer Uses] : *Creates an invite*" +
                       "\n -**!createchannel** [Channel type: voice/text] [Channel name]  : This will create either a text or voice channel. The default is a text channel");
                });
        }

        // Console log
        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] [{e.Source}] [{e.Message}]");

        }
    }
}
