using System;
using System.Threading.Tasks;
using HavocBot.DAL;
using HavocBot.Datastore;
using HavocBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HavocBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string LineBreak = "\n\r";
        private const string CommandHelp = "help";
        private const string CommandQuestion = "question";
        private const string CommandAnswer = "answer";

        private const string HelpMessage = 
            "Here's what you can do:" + LineBreak
            + "* Type \"{CommandQuestion}\" to get a trivia question";

        private TriviaApiClient _triviaApiClient = new TriviaApiClient();
        private InMemoryTriviaDatastore _triviaDatastore = WebApiConfig.TriviaDatastore;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            string messageText = activity.Text?.ToLower();

            if (!string.IsNullOrEmpty(messageText))
            {
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
                            await _triviaApiClient.GetQuestionAsync(triviaPlayer);

                        if (triviaQuestion != null
                            && !string.IsNullOrEmpty(triviaQuestion.Text)
                            && triviaQuestion.QuestionOptions != null
                            && triviaQuestion.QuestionOptions.Count > 0)
                        {
                            if (_triviaDatastore.PendingTriviaQuestions.ContainsKey(triviaPlayer.Id))
                            {
                                _triviaDatastore.PendingTriviaQuestions[triviaPlayer.Id] = triviaQuestion.Id;
                            }
                            else
                            {
                                _triviaDatastore.PendingTriviaQuestions.Add(triviaPlayer.Id, triviaQuestion.Id);
                            }

                            await context.PostAsync($"{triviaPlayer.Name.Split(' ')}, your question is: {triviaQuestion.Text}");

                            string questionOptions = $"Options are:{LineBreak}";

                            foreach (TriviaQuestionOption triviaQuestionOption in triviaQuestion.QuestionOptions)
                            {
                                questionOptions += $"{triviaQuestionOption.Id}: {triviaQuestionOption.Text}{LineBreak}";
                            }

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
                else if (messageText.StartsWith(CommandAnswer))
                {
                    int answerId = -1;
                    string answerAsString = messageText.Remove(0, CommandAnswer.Length).Trim();

                    try
                    {
                        answerId = int.Parse(answerAsString);
                    }
                    catch (Exception)
                    {
                        await context.PostAsync("Invalid answer, try again");
                    }

                    if (answerId != -1)
                    {
                        await context.PostAsync("The answer is missing");
                    }
                    else
                    {
                        TriviaPlayer triviaPlayer = await GetPlayer(activity);

                        if (triviaPlayer != null)
                        {
                            if (_triviaDatastore.PendingTriviaQuestions.ContainsKey(triviaPlayer.Id))
                            {
                                TriviaAnswer triviaAnswer = new TriviaAnswer()
                                {
                                    UserId = triviaPlayer.Id,
                                    QuestionId = _triviaDatastore.PendingTriviaQuestions[triviaPlayer.Id],
                                    AnswerId = answerId
                                };

                                TriviaAnswerResponse triviaAnswerResponse =
                                    await _triviaApiClient.PostAnswerAsync(triviaAnswer);

                                if (triviaAnswerResponse != null)
                                {
                                    _triviaDatastore.PendingTriviaQuestions.Remove(triviaPlayer.Id);

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

        /// <summary>
        /// Tries to find the trivia player matching the sender of the given activity.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>The trivia player or null if not found.</returns>
        private async Task<TriviaPlayer> GetPlayer(IActivity activity)
        {
            TriviaPlayer[] triviaPlayers =
                await _triviaApiClient.SearchPlayerAsync(activity.From.Name);

            if (triviaPlayers != null && triviaPlayers.Length > 0)
            {
                // Just pick the first found player for now
                return triviaPlayers[0];
            }

            return null;
        }
    }
}