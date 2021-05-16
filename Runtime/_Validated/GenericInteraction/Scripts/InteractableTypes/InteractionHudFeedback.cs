using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InteractionHudFeedback : Interactable
{
    [SerializeField]
    PlayerHudActions TargetAction;

    public string MessageText = "This is a Tutorial Interactable, how much can we fit in here?";
    public float PromptDuration = 3.14f;
    public override void StartInteraction(bool isPlayer = false)
    {
        if (PlayerHudManager.instance != null)
        {
            switch (TargetAction) 
            {
                case PlayerHudActions.ShowMessage:
                    PlayerHudManager.instance.DisplayPlayerMessage(MessageText, PromptDuration);
                    break;

                case PlayerHudActions.ShowObjective:
                    PlayerHudManager.instance.DisplayPlayerObjective(MessageText, PromptDuration);
                    break;
                case PlayerHudActions.ShowObjectiveComplete:
                    PlayerHudManager.instance.DisplayObjectiveComplete();
                    break;
                case PlayerHudActions.Showprompt:
                    PlayerHudManager.instance.DisplayPlayerPrompt(MessageText, false, PromptDuration);
                    break;
                case PlayerHudActions.ShowTutorial:
                    PlayerHudManager.instance.DisplayPlayerMessage(MessageText, PromptDuration);
                    break;
            }
           
        }
    }

    public override void StopInteraction(bool isPlayer = false)
    {
        base.StopInteraction();
        if (PlayerHudManager.instance != null)
        {
           // PlayerHudManager.instance.StartCoroutine(PlayerHudManager.instance.HidePlayerPrompt(MessageText));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "stabbed-note.png", true);
    }
}
