﻿namespace Adapter.PersonalInformation
{
    public interface IPersonalInformationGettable
    {
        string QuestionTopic { get; }
        string GetQuestion();
        string GetAnswer();
    }
}
