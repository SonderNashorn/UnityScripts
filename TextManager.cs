using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    #region variables
    
    [System.Serializable]
    public struct SpeechGroup
    {        
        [TextArea(2, 5)]
        public string speechText;
    }

    
    //[SerializeField] CanvasGroup TextboxCanvasGroup;    No need for Buttons
    //public Text speakerName;                  
    public Text dialogueText;
    public Text playerDialogueText;
    //[SerializeField] AudioSource voiceLines;

    //reference to the scriptable objects script
    public SpeechScriptableObjects currentSpeech;
    [HideInInspector]
    public SpeechScriptableObjects continueConversation;
    [HideInInspector]
    public SpeechScriptableObjects sellOffer;
    [HideInInspector]
    public SpeechScriptableObjects endDialogue;

    private RayCast ray;

    public int currentSpeechIndex;
    public int currentPlayerSpeechIndex;

    [HideInInspector]
    public bool speechInProgress = false;
    [HideInInspector]
    public bool playerSpeechInProgress = false;

    public static TextManager instance;

    private Interaction interaction;
    //private string defaultGreeting;
    #endregion



    #region NPCDialogueMethods
    private void Awake()
    {
        instance = this;
        
    }

    public void SetSpeechNext()
    {
        currentSpeechIndex++;
        if (currentSpeech.speechGroup.Count > currentSpeechIndex)
        {
            FillSpeech();
        }
        else
        {
            StartCoroutine(EndSpeechCroutine());
            //dialogueText.text = null;
        }
    }

    public void StartSpeech(SpeechScriptableObjects speech)
    {
        StartCoroutine(StartSpeechCroutine(speech));        
    }

    IEnumerator StartSpeechCroutine(SpeechScriptableObjects speech)
    {
        while (speechInProgress)
            yield return null;

        speechInProgress = true;
        currentSpeech = speech;

        
        FillSpeech();

    }
    
    IEnumerator EndSpeechCroutine()
    {
        ResetDialogues();

        yield return null;
        
        
    }

    public void FillSpeech()
    {
        SpeechGroup s = currentSpeech.speechGroup[currentSpeechIndex];       
        dialogueText.text = s.speechText;
        
    }
    #endregion

    #region PlayerDialogueMethods
    public void SetPlayerSpeechNext()
    {
        currentPlayerSpeechIndex++;
        if (continueConversation.speechGroup.Count > currentPlayerSpeechIndex)
        {
            FillPlayerSpeech();
        }
        else
        {
            StartCoroutine(EndPlayerSpeechCroutine());
            //playerDialogueText.text = defaultGreeting;
        }
    }

    public void StartPlayerSpeech(SpeechScriptableObjects pspeech)
    {
        StartCoroutine(StartPlayerSpeechCroutine(pspeech));
    }

    IEnumerator StartPlayerSpeechCroutine(SpeechScriptableObjects pspeech)
    {
        while (playerSpeechInProgress)
            yield return null;

        playerSpeechInProgress = true;
        continueConversation = pspeech;

        currentPlayerSpeechIndex = 0;
        FillPlayerSpeech();

    }
    IEnumerator EndPlayerSpeechCroutine()
    {
        
        playerSpeechInProgress = false;
        yield return null;            
    }

    public void ResetDialogues()
    {
        currentSpeechIndex = 0;
        currentPlayerSpeechIndex = 0;
        
    }

    private void FillPlayerSpeech()
    {        
        SpeechGroup s = continueConversation.speechGroup[currentPlayerSpeechIndex];
        playerDialogueText.text = s.speechText;

    }
    #endregion

}

//currentSpeech = ray.hitObj.GetComponent<NPCDialogue>().npcSpeech;