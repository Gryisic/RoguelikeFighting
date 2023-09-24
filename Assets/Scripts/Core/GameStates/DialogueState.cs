using System;
using Common.Dialogues.Interfaces;
using Common.Utils;
using Common.Utils.Extensions;
using Core.Interfaces;

namespace Core.GameStates
{
    public class DialogueState : StatesChanger<IDialogueState>, IGameState, IDeactivatable
    {
        private readonly IGameStateSwitcher _stateSwitcher;

        private DialogueStateArgs _args;
        
        public DialogueState(IGameStateSwitcher stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }
        
        public void Activate(GameStateArgs args)
        {
            if (args is DialogueStateArgs dialogueStateArgs)
            {
                _args = dialogueStateArgs;
                
                for (var i = 0; i < states.Count; i++)
                {
                    IDialogueState state = states[i];
                    
                    if (state is IDialogueArgsRequester argsRequester)
                        argsRequester.RequestArgs += GetArgs;
                    
                    if (state is IDialogueEndRequester dialogueEndRequester)
                        dialogueEndRequester.DialogueEnded += EndDialogueState;
                }

                //ChangeState<DialogueInitializationState>();
            }
            else
            {
                throw new InvalidOperationException("Trying to initiate dialogue via non DialogueStateArgs");
            }
        }

        public void Deactivate()
        {
            for (var i = 0; i < states.Count; i++)
            {
                IDialogueState state = states[i];
                    
                if (state is IDialogueArgsRequester argsRequester)
                    argsRequester.RequestArgs -= GetArgs;
                
                if (state is IDialogueEndRequester dialogueEndRequester)
                    dialogueEndRequester.DialogueEnded -= EndDialogueState;
                
                state.Deactivate();
            }
        }

        private void EndDialogueState() => _stateSwitcher.SwitchState<GameplayState>(new GameStateArgs());

        private DialogueStateArgs GetArgs() => _args;
    }
}