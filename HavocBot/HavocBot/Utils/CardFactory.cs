using HavocApiClients.Models;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace HavocBot.Utils
{
    public class CardFactory
    {
        public static HeroCard CreateQuestionCard(TriviaQuestion triviaQuestion)
        {
            List<CardAction> buttons = new List<CardAction>();

            foreach (TriviaQuestionOption triviaQuestionOption in triviaQuestion.QuestionOptions)
            {
                buttons.Add(new CardAction()
                {
                    Title = triviaQuestionOption.Text,
                    Type = ActionTypes.ImBack,
                    Value = triviaQuestionOption.Id
                });
            }

            HeroCard card = new HeroCard()
            {
                Subtitle = triviaQuestion.Text,
                Buttons = buttons,
            };

            return card;
        }
    }
}
