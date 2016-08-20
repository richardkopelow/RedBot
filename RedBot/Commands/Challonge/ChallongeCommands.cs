﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using RedBot.Commands.Challonge;

namespace RedBot.Commands
{
	class ChallongeCommands
	{
		private ChallongeAPIHandler challonge;

		public ChallongeCommands(CommandService cmd)
		{
			try
			{
				challonge = new ChallongeAPIHandler(Properties.Settings.Default.challongeAccount, Properties.Settings.Default.challongeToken);
				InitCommands(cmd);
				Console.WriteLine("[ OK ] Challonge Support");
			}
			catch
			{
				Console.WriteLine("[FAIL] Challonge Support");
			}
		}

		private void InitCommands(CommandService cmd)
		{
			cmd.CreateCommand("signup")
					.Description("Adds the user to the current C2GS tournament.")
					.Parameter("player_name", Discord.Commands.ParameterType.Required)
					.Do(async e =>
					{
						string player_name = e.GetArg("player_name");

						System.Net.HttpStatusCode responseCode = await challonge.AddParticipantAsync(player_name);

						Console.WriteLine(responseCode);

						if (responseCode == System.Net.HttpStatusCode.OK)
						{
							await e.Channel.SendMessage(string.Format("{0} has been added to the tournament!", player_name));
						}
						else
						{
							await e.Channel.SendMessage(string.Format("{0}, there was a problem adding you to the torunament, try again in a sec or message the TO if the problem persists.", player_name));
						}
					});
			cmd.CreateCommand("select-tourney")
				.Description("MODERATOR/EBOARD ONLY: Sets the focus to the specified tourney. id = challonge.com/tourneyID")
				.Parameter("id", Discord.Commands.ParameterType.Required)
				.Do(async e =>
				{
					challonge.SelectedTournament = e.GetArg("id");
					await e.Channel.SendMessage(string.Format("The bot's focus has been set to chalonge.com/{0}", e.GetArg("id")));
				});
		}
	}
}
