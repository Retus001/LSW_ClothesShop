using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueType
{
    GREETING,
    SMALLTALK,
    PURCHASE,
    PURCHASE_MISSINGITEMS,
    PURCHASE_MISSINGMONEY,
    PURCHASE_COMPLETE,
    SELL,
    SELL_MISSINGITEMS,
    SELL_COMPLETE,
    BASE
}

[Serializable]
public class DialogueData
{
    public DialogueType dialogueType;
    [TextArea]
    public string[] dialogueOptions;
}

[CreateAssetMenu(fileName = "newNPCDialogue", menuName = "NPC Dialogue", order = 0)]

public class SO_Dialogue : ScriptableObject
{
    public DialogueData[] dialogues;

    public string GetRandomDialogue(DialogueType _type)
    {
        foreach(DialogueData dData in dialogues)
        {
            if(dData.dialogueType == _type)
            {
                return dData.dialogueOptions[UnityEngine.Random.Range(0, dData.dialogueOptions.Length)];
            }
        }

        return null;
    }
}
