﻿namespace ChainOfResponsibility
{
    public class Clergyman : IClergyman
    {
        public string Rank { get; }
        private DegreeOfPhilosophicalDepth DepthLimit { get; }

        public Clergyman(string rank, DegreeOfPhilosophicalDepth depthLimit)
        {
            Rank = rank;
            DepthLimit = depthLimit;
        }

        public bool CanAnswer(DegreeOfPhilosophicalDepth questionDepth)
        {
            return DepthLimit <= questionDepth;
        }

        public string GetAnswer(Question question)
        {
            return $"The degree of philosophical depth of this question is {question.PhilosophicalDepth.ToString()}.\n" +
                   $"Therefore, the {Rank} can, and will, answer it.\n" +
                   $"Here is the answer to your question:\n{question.Answer}";
        }
    }
}
