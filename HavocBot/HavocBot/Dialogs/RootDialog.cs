using System; 
using System.Threading.Tasks;
using HavocBot.DAL;
using HavocBot.Datastore;
using HavocBot.Models;
using HavocBot.Utils;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HavocBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const int MaxQuestionOptionNumber = 5;
        private const string LineBreak = "\n\r";
        private const string CommandHelp = "help";
        private const string CommandQuestion = "question";
        private const string CommandAnswer = "answer";
        private const string CommandRegister = "register";

        private const string HelpMessage =
            "Here's what you can do:" + LineBreak
            + "* To register a trivia team, type \"" + CommandRegister + "\"" + LineBreak
            + "* Type \"" + CommandQuestion + "\" to get a trivia question" + LineBreak
            + "* To answer a question simply type the option number" + LineBreak;

        private const string HavocTeamId = "19:7f0240ce5cd64e8ea7c04cf6f1ccb693@thread.skype";

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            string messageText = CleanMessage(activity);

            if (!string.IsNullOrEmpty(messageText))
            {
                TriviaApiClient triviaApiClient = WebApiConfig.TriviaApiClient;
                InMemoryTriviaDatastore triviaDatastore = WebApiConfig.TriviaDatastore;

                if (messageText.Equals(CommandHelp))
                {
                    await context.PostAsync(HelpMessage);
                }
                else if (messageText.Equals(CommandQuestion))
                {
                    TriviaPlayer triviaPlayer = await GetPlayer(activity);

                    if (triviaPlayer != null)
                    {
                        TriviaQuestion triviaQuestion =
                            await triviaApiClient.GetQuestionAsync(triviaPlayer);

                        if (triviaQuestion != null
                            && !string.IsNullOrEmpty(triviaQuestion.Text)
                            && triviaQuestion.QuestionOptions != null
                            && triviaQuestion.QuestionOptions.Count > 0)
                        {
                            if (triviaDatastore.PendingTriviaQuestions.ContainsKey(triviaPlayer.Id))
                            {
                                triviaDatastore.PendingTriviaQuestions[triviaPlayer.Id] = triviaQuestion.Id;
                            }
                            else
                            {
                                triviaDatastore.PendingTriviaQuestions.Add(triviaPlayer.Id, triviaQuestion.Id);
                            }

                            await context.PostAsync($"{triviaPlayer.Name.Split(' ')}, your question is: {triviaQuestion.Text}");

                            string questionOptions = $"Options are:{LineBreak}";

                            foreach (TriviaQuestionOption triviaQuestionOption in triviaQuestion.QuestionOptions)
                            {
                                questionOptions += $"{triviaQuestionOption.Id}: {triviaQuestionOption.Text}{LineBreak}";
                            }

                            await context.PostAsync(questionOptions);
                            await context.PostAsync($"When answering, start your answer with \"{CommandAnswer}\"");
                        }
                        else
                        {
                            await context.PostAsync("I'm sorry, I couldn't find a trivia question. Please try again later");
                        }
                    }
                    else
                    {
                        await context.PostAsync("I'm sorry, I couldn't find your player profile. Are you sure you have registered?");
                    }
                }
                else if (messageText.StartsWith(CommandRegister))
                {
                    string serviceUrl = activity.ServiceUrl;
                    var memebers = await TeamsApiClient.GetTeamsMembers(activity, serviceUrl, HavocTeamId);

                    if (memebers?.Count > 0)
                    {
                        var triviaRoster = new TriviaRoster
                        {
                            TeamId = HavocTeamId,
                            Members = memebers
                        };

                        TriviaRegister registerResponse = await triviaApiClient.RegisterAsync(triviaRoster);

                        if (registerResponse != null)
                        {
                            if (registerResponse.Success)
                            {
                                await context.PostAsync("Team registered successfully!");
                            }
                            else
                            {
                                await context.PostAsync($"Failed to register the team: {registerResponse.Message}");
                            }
                        }
                        else
                        {
                            await context.PostAsync($"Failed to register the team, please try again later");
                        }
                    }
                }
                else
                {
                    // Check for answer
                    int answerId = -1;
                    string answerAsString = messageText.Remove(0, CommandAnswer.Length).Trim();
                    bool numberParsedSuccessfully = int.TryParse(answerAsString, out answerId);

                    if (numberParsedSuccessfully && answerId >= 0 && answerId <= MaxQuestionOptionNumber)
                    {
                        TriviaPlayer triviaPlayer = await GetPlayer(activity);

                        if (triviaPlayer != null)
                        {
                            if (triviaDatastore.PendingTriviaQuestions.ContainsKey(triviaPlayer.Id))
                            {
                                TriviaAnswer triviaAnswer = new TriviaAnswer()
                                {
                                    UserId = triviaPlayer.Id,
                                    QuestionId = triviaDatastore.PendingTriviaQuestions[triviaPlayer.Id],
                                    AnswerId = answerId
                                };

                                TriviaAnswerResponse triviaAnswerResponse =
                                    await triviaApiClient.PostAnswerAsync(triviaAnswer);

                                if (triviaAnswerResponse != null)
                                {
                                    triviaDatastore.PendingTriviaQuestions.Remove(triviaPlayer.Id);

                                    if (triviaAnswerResponse.Correct)
                                    {
                                        await context.PostAsync($"{triviaPlayer.Name.Split(' ')}: That is correct!");
                                    }
                                    else
                                    {
                                        await context.PostAsync($"{triviaPlayer.Name.Split(' ')}: I'm afraid that is not the correct answer. Better luck next time!");
                                    }
                                }
                                else
                                {
                                    await context.PostAsync("Sorry, something went wrong. Try again later");
                                }
                            }
                            else
                            {
                                await context.PostAsync("I haven't asked you a question yet.");
                                await context.PostAsync(HelpMessage);
                            }
                        }
                        else
                        {
                            await context.PostAsync("I'm sorry, I couldn't find your player profile. Are you sure you have registered?");
                        }
                    }
                }
            }

            context.Wait(MessageReceivedAsync);
        }

        private string CleanMessage(Activity activity)
        {
            string cleanedMessage = string.Empty;
            string botName = activity.Recipient?.Name?.ToLower();
            string messageText = activity.Text?.Trim().ToLower();

            if (!string.IsNullOrEmpty(messageText))
            {
                if (!string.IsNullOrEmpty(botName)
                    && messageText.Contains(botName))
                {
                    // Remove bot name - here we assume it's in the beginning of the message
                    string[] elements = messageText.Split(' ');
                    
                    if (elements.Length > 1)
                    {
                        for (int i = 1; i < elements.Length; ++i)
                        {
                            cleanedMessage += elements[i];

                            if (i < elements.Length - 1)
                            {
                                cleanedMessage += ' ';
                            }
                        }
                    }
                }
                else
                {
                    cleanedMessage = messageText;
                }

                System.Diagnostics.Debug.WriteLine($"\"{messageText}\" -> \"{cleanedMessage}\"");
            }

            return cleanedMessage;
        }

        /// <summary>
        /// Tries to find the trivia player matching the sender of the given activity.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>The trivia player or null if not found.</returns>
        private async Task<TriviaPlayer> GetPlayer(IActivity activity)
        {
            TriviaApiClient triviaApiClient = WebApiConfig.TriviaApiClient;
            TriviaPlayer[] triviaPlayers =
                await triviaApiClient.SearchPlayerAsync(activity.From.Name);

            if (triviaPlayers != null && triviaPlayers.Length > 0)
            {
                // Just pick the first found player for now
                return triviaPlayers[0];
            }

            return null;
        }
    }
}