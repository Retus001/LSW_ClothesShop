using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    public GameObject m_dialogueWindow;
    public TextMeshProUGUI m_dialogue_TMP;
    public Image m_dialogueSpeaker;
    public TextMeshProUGUI m_dialogueSpeakerName;
    public TextMeshProUGUI m_dialogueSpeakerTitle;
    public Image m_dialogueBoxBG;
    public Transform m_dialogueOptionsContainer;

    private Dictionary<string, GameObject> dialogueOptions = new Dictionary<string, GameObject>();
    private float speechSpeed = 1;

    private void Start()
    {
        m_dialogueWindow.SetActive(false);
    }

    public void AddDialogueOption(string _label, UnityEngine.Events.UnityAction _callback)
    {
        GameObject newDialogueOption = PoolManager.Instance.GetPoolObject(PoolObjectType.DialogueOption);
        Button newOptionBtn = newDialogueOption.GetComponent<Button>();
        TextMeshProUGUI newOptionLabel = newDialogueOption.GetComponentInChildren<TextMeshProUGUI>();

        newOptionBtn.onClick.AddListener(() => _callback());
        newOptionLabel.text = _label;
        newDialogueOption.SetActive(true);
        dialogueOptions.Add(_label, newDialogueOption);
    }

    public void RemoveDialogueOption(string _label)
    {
        PoolManager.Instance.ResetObjInstance(dialogueOptions[_label], PoolObjectType.DialogueOption);
        dialogueOptions.Remove(_label);
    }

    public void ClearDialogueOptions()
    {
        foreach(Transform option in m_dialogueOptionsContainer)
        {
            PoolManager.Instance.ResetObjInstance(option.gameObject, PoolObjectType.DialogueOption);
        }

        dialogueOptions.Clear();
    }

    public void ShowDialogueBox(NPCBehaviour _npc)
    {
        m_dialogueWindow.SetActive(true);

        m_dialogueSpeakerName.text = _npc.m_npcData.characterName;
        m_dialogueSpeakerTitle.text = _npc.m_npcData.characterTitle;
        m_dialogueSpeaker.sprite = _npc.m_npcData.speakerSprite;
        m_dialogueBoxBG.color = _npc.m_npcData.textBoxColor;
        speechSpeed = 0.1f/_npc.m_npcData.speechSpeed;
    }

    public void HideDialogueBox()
    {
        m_dialogueWindow.SetActive(false);
        ClearDialogueOptions();
    }

    public void DialogueSpeak(string _dialogue)
    {
        StopAllCoroutines();
        m_dialogue_TMP.text = _dialogue;
        m_dialogue_TMP.maxVisibleCharacters = 0;
        StartCoroutine(Typewriter());
    }

    private IEnumerator Typewriter()
    {
        m_dialogue_TMP.maxVisibleCharacters++;
        yield return new WaitForSeconds(speechSpeed);

        if (m_dialogue_TMP.maxVisibleCharacters < m_dialogue_TMP.text.Length)
            StartCoroutine(Typewriter());
    }
}
