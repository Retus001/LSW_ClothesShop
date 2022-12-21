using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Shopkeeper : NPCBehaviour
{
    public override void Interact(DialogueType _dialogueIntention = DialogueType.BASE)
    {
        base.Interact(_dialogueIntention);

        if (!known)
        {
            _dialogueIntention = DialogueType.GREETING;
            known = true;
        }

        string dialogueAppend = "";

        Debug.Log("Switching with " + _dialogueIntention);

        switch (_dialogueIntention)
        {
            default:
                break;

            case DialogueType.BASE:
                DialogueManager.Instance.AddDialogueOption("Talk", () => Interact(DialogueType.SMALLTALK));
                DialogueManager.Instance.AddDialogueOption("Buy", () => Interact(DialogueType.PURCHASE));
                DialogueManager.Instance.AddDialogueOption("Sell", () => Interact(DialogueType.SELL));
                break;

            case DialogueType.PURCHASE:
                if (InventoryManager.Instance.cartClothingItems.Count <= 0)
                    _dialogueIntention = DialogueType.PURCHASE_MISSINGITEMS;
                else
                {
                    dialogueAppend = "$" + InventoryManager.Instance.GetCartTotalCost().ToString();
                    DialogueManager.Instance.AddDialogueOption("Confirm", () => Interact(DialogueType.PURCHASE_COMPLETE));
                }
                break;

            case DialogueType.PURCHASE_COMPLETE:
                if (InventoryManager.Instance.money < InventoryManager.Instance.GetCartTotalCost())
                    _dialogueIntention = DialogueType.PURCHASE_MISSINGMONEY;
                break;

            case DialogueType.SELL:
                if (InventoryManager.Instance.ownedClothingItems.Count <= 0)
                    _dialogueIntention = DialogueType.SELL_MISSINGITEMS;
                else
                    DialogueManager.Instance.AddDialogueOption("Confirm", () => Interact(DialogueType.SELL_COMPLETE));
                break;
        }

        previousDialogueType = _dialogueIntention;
        DialogueManager.Instance.DialogueSpeak(m_dialogues.GetRandomDialogue(_dialogueIntention) + dialogueAppend);
    }
}
