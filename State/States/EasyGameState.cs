﻿using System;

namespace State.States
{
    public class EasyGameState : GameState
    {
        private const int NumberLimit = 100;
        private const int EasyModeLimit = 5;
        private static Random Random = new Random();

        public EasyGameState(GameState gameState) :
            this(gameState.QuestionsAttempted, gameState.QuestionsCorrect, gameState.MathGame)
        { }

        public EasyGameState(int questionsAttempted, int questionsCorrect, MathGame mathGame)
        {
            QuestionsAttempted = questionsAttempted;
            QuestionsCorrect = questionsCorrect;
            MathGame = mathGame;
        }

        public override void AskQuestion()
        {
            var questionsTillNextLevel = EasyModeLimit - QuestionsCorrect;
            Console.Write("\nYou're in the ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("easy");
            Console.ResetColor();
            Console.WriteLine($" mode. Answer {questionsTillNextLevel} " +
                $"{(QuestionsCorrect == 0 ? string.Empty : "more ")}{(questionsTillNextLevel > 1 ? "questions " : "question ")}" +
                $"correctly to get to the next level.\n");

            var (question, answer) = GetQuestionAndCorrectAnswer();
            var answerGiven = GetAnswer(question);

            EvaluateAnswer(answer, answerGiven);
            StateChangeCheck();
        }

        private (string, int) GetQuestionAndCorrectAnswer()
        {
            var operation = GetRandomOperation();
            var digitOne = 0;
            var digitTwo = 0;
            var answer = 0;
            string opSign;
            switch (operation)
            {
                case Operation.Add:
                    digitOne = Random.Next(1, NumberLimit);
                    digitTwo = Random.Next(1, NumberLimit);
                    answer = Add(digitOne, digitTwo);
                    opSign = "+";
                    break;
                case Operation.Subtract:
                    digitOne = Random.Next(NumberLimit / 2, NumberLimit);
                    digitTwo = Random.Next(1, NumberLimit / 2);
                    answer = Subtract(digitOne, digitTwo);
                    opSign = "-";
                    break;
                default:
                    opSign = string.Empty;
                    break;
            }

            return ($"What is {digitOne} {opSign} {digitTwo}?", answer);
        }

        private Operation GetRandomOperation()
        {
            var operation = Random.Next(0, Enum.GetNames(typeof(Operation)).Length / 2);

            return (Operation)operation;
        }

        private void StateChangeCheck()
        {
            if (QuestionsCorrect >= EasyModeLimit)
            {
                MathGame.ChangeState(new ModerateGameState(this));
            }
        }
    }
}
