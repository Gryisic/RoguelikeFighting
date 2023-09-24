using System;

namespace Common.Dialogues.Interfaces
{
    public interface IDialogueEndRequester
    {
        event Action DialogueEnded;
    }
}