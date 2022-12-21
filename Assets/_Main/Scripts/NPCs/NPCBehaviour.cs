using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct NPCData
{
    public string characterName;
    public string characterTitle;
    public float speechSpeed;
    public Color textBoxColor;
    public Sprite speakerSprite;
}

public class NPCBehaviour : MonoBehaviour
{
    public NPCData m_npcData;
    public SO_Dialogue m_dialogues;

    protected bool known = false;
    protected DialogueType previousDialogueType;

    private GameObject interactionPrompt;
    private Button promptBtn;

    public virtual void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.CompareTag("Player"))
        {
            ShowInteractionPrompt();
        }
    }

    public virtual void OnTriggerExit2D(Collider2D _col)
    {
        if (_col.CompareTag("Player"))
        {
            HideInteractionPrompt();
            DialogueManager.Instance.HideDialogueBox();
        }
    }

    public virtual void ShowInteractionPrompt()
    {
        interactionPrompt = PoolManager.Instance.GetPoolObject(PoolObjectType.InteractionPrompt);
        promptBtn = interactionPrompt.GetComponent<Button>();
        promptBtn.onClick.AddListener(() => Interact());
        interactionPrompt.transform.position = transform.position + new Vector3(0, 2, 0);
        interactionPrompt.SetActive(true);
    }

    public virtual void HideInteractionPrompt()
    {
        promptBtn.onClick.RemoveAllListeners();
        PoolManager.Instance.ResetObjInstance(interactionPrompt, PoolObjectType.InteractionPrompt);
    }

    public virtual void Interact(DialogueType _dialogueIntention = DialogueType.BASE)
    {
        if(!DialogueManager.Instance.m_dialogueWindow.activeSelf)
            DialogueManager.Instance.ShowDialogueBox(this);

        if(previousDialogueType != _dialogueIntention)
            DialogueManager.Instance.ClearDialogueOptions();
    }
}
