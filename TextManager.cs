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

    //public Text speakerName;  

    //answereddialogue = left or top speech
    //current dialog = answereddialogue[currentSpeechIndex] 
    public Text dialogueText;
    public Text playerRightDialogueText;
    public Text playerLeftDialogueText;
    public int currentDayIndex;
    public int currentSpeechIndex;
    public int currentPlayerSpeechIndex;

    //reference to the scriptable objects script


   // [HideInInspector]
   // public SpeechTree nPCSpeechTreeLeft;
   // [HideInInspector]
    //public SpeechTree nPCSpeechTreeRight;
   // [HideInInspector]    
    public SpeechScriptableObjects nPCAnswerSpeech;
   // [HideInInspector]
    public SpeechScriptableObjects playerAnswer;
    //[HideInInspector]
    //public SpeechScriptableObjects sellOffer;
    //[HideInInspector]
    //public SpeechScriptableObjects endDialogue;

    [HideInInspector]
    public bool speechInProgress = false;
    [HideInInspector]
    public bool playerSpeechInProgress = false;

    public static TextManager instance;

    
    private RayCast ray;
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
        if (nPCAnswerSpeech.speechGroup.Count > currentSpeechIndex)        
            FillSpeech();        
        else        
            StartCoroutine(EndSpeechCroutine());        
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
        nPCAnswerSpeech = speech;
    }
    
    IEnumerator EndSpeechCroutine()
    {
        //ResetDialogues();
        speechInProgress = false;
        dialogueText.text = null;
        yield return null;
        
        
    }

    //I'll probably need to change the from void to send and receive.
    /*public int FillSpeech()
        
    */
    public void FillSpeech()
    {
        SpeechGroup s = nPCAnswerSpeech.speechGroup[currentSpeechIndex];       
        dialogueText.text = s.speechText;
    }
    #endregion

    #region PlayerDialogueMethods

    public void SetPlayerLeftSpeechNext()
    {
        currentPlayerSpeechIndex++;
        if (playerAnswer.speechGroup.Count > currentPlayerSpeechIndex)
        {            
            FillPlayerLeftSpeech();
        }
        else
        {
            StartCoroutine(EndPlayerSpeechCroutine());
            //playerDialogueText.text = defaultGreeting;
        }
    }
    public void SetPlayerRightSpeechNext()
    {
        currentPlayerSpeechIndex++;
        if (playerAnswer.speechGroup.Count > currentPlayerSpeechIndex)
        {
            FillPlayerRightSpeech();
        }
        else
        {
            StartCoroutine(EndPlayerSpeechCroutine());
            //playerDialogueText.text = defaultGreeting;
        }
    }

    private void FillPlayerRightSpeech()
    {
        SpeechGroup pSpeech = playerAnswer.speechGroup[currentPlayerSpeechIndex];
        playerRightDialogueText.text = pSpeech.speechText;
    }


    private void FillPlayerLeftSpeech()
    {
        SpeechGroup pSpeech = playerAnswer.speechGroup[currentPlayerSpeechIndex];
        playerLeftDialogueText.text = pSpeech.speechText;
    }
     

    public void StartPlayerLeftSpeech(SpeechScriptableObjects playerSpeech)
        {
            StartCoroutine(StartPlayerLeftSpeechCroutine(playerSpeech));
        }

    
    public void StartPlayerRightSpeech(SpeechScriptableObjects playerSpeech)
        {
            StartCoroutine(StartPlayerRightSpeechCroutine(playerSpeech));
        }
     

    IEnumerator StartPlayerRightSpeechCroutine(SpeechScriptableObjects playerSpeech)
    {
        while (playerSpeechInProgress)
            yield return null;

        playerSpeechInProgress = true;
        playerAnswer = playerSpeech;
        
        currentPlayerSpeechIndex = 0;
        FillPlayerRightSpeech();
        
    }

    
     IEnumerator StartPlayerLeftSpeechCroutine(SpeechScriptableObjects playerSpeech)
    {
        while (playerSpeechInProgress)
            yield return null;

        playerSpeechInProgress = true;
        playerAnswer = playerSpeech;
        
        currentPlayerSpeechIndex = 0;
        FillPlayerRightSpeech();
    }
         

    IEnumerator EndPlayerSpeechCroutine()
    {        
        playerSpeechInProgress = false;
        playerRightDialogueText.text = null;
        playerLeftDialogueText.text = null;

        yield return null;
    }
    
   
    #endregion

}

//currentSpeech = ray.hitObj.GetComponent<NPCDialogue>().npcSpeech;

/*
public void ResetDialogues()
{
    currentSpeechIndex = 0;
    currentPlayerSpeechIndex = 0;
    //interaction.image = null;
}
*/
