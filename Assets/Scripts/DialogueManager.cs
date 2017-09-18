using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text targetNameText;
    public Text dialogueText;
    public Image[] targetImages;

    public Animator[] targetAnimators;
    public GameObject selectionInstance;
    public List<GameObject> selections;
    public Transform content;

    public Animator animator;
    private Queue<string> sentences;
    private Queue<Conversation> conversations;
    [SerializeField]
    private int currentTargetIndex;
    [SerializeField]
    private string previousTargetName;
	// Use this for initialization
	void Start () {

        sentences = new Queue<string>();
        conversations = new Queue<Conversation>();
        currentTargetIndex = 0;

    }
	
    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        targetNameText.text = dialogue.targetName;

        Debug.Log("Starting conversation with " + dialogue.targetName);

        sentences.Clear();

        foreach(Conversation conversation in dialogue.conversations)
        {
            conversations.Enqueue(conversation);
        }

        DisplayNextConversation();
    }

    public void DisplayNextConversation()
    {
        

        if(conversations.Count == 0)
        {
            EndDialogue();
            return;
        }
        Conversation conversation = conversations.Dequeue();
        if(conversation.targetName != previousTargetName)
        {
            targetAnimators[currentTargetIndex].SetBool("isOpen", false);
            currentTargetIndex = (currentTargetIndex + 1) % 2;
            targetAnimators[currentTargetIndex].SetBool("isOpen", true);
        }
        targetImages[currentTargetIndex].sprite = conversation.targetSprite;
        targetNameText.text = conversation.targetName;
        // Debug.Log(sentence);
        StopAllCoroutines();
        Sentence normalSentence = conversation.sentence[0];
        StartCoroutine(TypeSentence(normalSentence.s));
        if (conversation.sentence.Length > 1)
        {
            
            for(int i = 1; i < conversation.sentence.Length; ++i)
            {
                selections.Add(Instantiate(selectionInstance) as GameObject);
                selections[selections.Count - 1].transform.SetParent(content);
                selections[selections.Count - 1].transform.localScale = new Vector3(1, 1, 1);
                selections[selections.Count - 1].GetComponentInChildren<Text>().text = conversation.sentence[i].s;
            }
        }
        previousTargetName = conversation.targetName;
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence)
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        targetAnimators[currentTargetIndex].SetBool("isOpen", false);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
