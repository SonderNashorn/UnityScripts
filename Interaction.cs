using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    #region Variables
    //---Public---//

    public enum SpeechSelection
    {
        Start,
        Left,
        Right,
        End
    }
    public SpeechSelection speechSelection;
    [Header("DrugSymbol")]
    public RawImage image;
    public bool offeredToSell = false;

    //---Private---//    
    [Header("NPCSpeech")]
    [SerializeField]
    SpeechTree npcAnswerTree;
    [SerializeField]
    SpeechScriptableObjects npcAnswer;

    [Header("Players response")]
    [SerializeField]
    SpeechTree playerAnswerTree;
    [SerializeField]
    SpeechScriptableObjects playerAnswer;


    [Header("Selloffer dialogue")]
    [SerializeField]
    SpeechScriptableObjects sellOffer;

    [Header("End disussion")]
    [SerializeField]
    SpeechScriptableObjects endDialogue;

    private RayCast ray;
    private Text npcName;
    private Text npcDialogueText;
    private GameManager gm;
    private TextManager textManager;
    private Inventory inventory;
    private PlayerDialogue playerDialogue;
    KeyCode key;
    #endregion

    void Start()
    {
        ray = FindObjectOfType<Camera>().GetComponent<RayCast>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        textManager = TextManager.instance;
        inventory = gm.GetComponent<Inventory>();
        playerDialogue = GetComponent<PlayerDialogue>();
    }

    void Update()
    {        
        LoadSpeech();
        CurrentTree();
        if (textManager.speechInProgress)
            CheckSellOffer();
    }

    void LoadSpeech()
    {
        if (ray.NPCTrigger() == true)
        {
            ray.hitObj.GetComponent<NPCDialogue>().isNPCClose = true;
            playerDialogue.target = ray.hitObj.GetComponent<Transform>();
            textManager.dialogueText = ray.hitObj.GetComponentInChildren<Text>();
            
        }

        if (ray.NPCTrigger() == false)
        {
            image.texture = null;
            playerDialogue.OpenSellDialog(false);
        }
    }

    void CheckState()
    {
        if (textManager.speechInProgress == true && Input.GetKeyDown(KeyCode.Q))
        {            
            speechSelection = SpeechSelection.Left;
        }
        else if (textManager.speechInProgress == true && Input.GetKeyDown(KeyCode.E))
        {            
            speechSelection = SpeechSelection.Right;
        }        
    }

    void StartConversation()
    {
        //if Raycast hit NPC and Space key is hit
        if (ray.NPCTrigger() == true && Input.GetKeyDown(KeyCode.Space) && textManager.speechInProgress == false)
        {
            //---NPC---//            
            //Begin speech
            textManager.StartSpeech(npcAnswer);
            textManager.currentSpeechIndex = 0;

            //---Player---//
            textManager.currentPlayerSpeechIndex = 0;

            //open player dialogue at the same time as you begin to speak
            playerDialogue.isPlayerClose = true;                      
        }
    }

    void LoadLeft()
    {
        npcAnswerTree = ray.hitObj.GetComponent<NPCDialogue>().nPCLeftSpeechTree;
        npcAnswer = npcAnswerTree.dailyScripts[gm.dayCount];        
        textManager.nPCAnswerSpeech = npcAnswer;
        
        playerAnswerTree = ray.hitObj.GetComponent<NPCDialogue>().playerLeftSpeechTree;
        playerAnswer = playerAnswerTree.dailyScripts[gm.dayCount];
        textManager.playerAnswer = playerAnswer;
        
        image.texture = npcAnswer.drugSprite;
    }

    void LoadRight()
    {
        playerAnswerTree = ray.hitObj.GetComponent<NPCDialogue>().playerRightSpeechTree;
        playerAnswer = playerAnswerTree.dailyScripts[gm.dayCount];        
        textManager.playerAnswer = playerAnswer;

        npcAnswerTree = ray.hitObj.GetComponent<NPCDialogue>().nPCRightSpeechTree;
        npcAnswer = npcAnswerTree.dailyScripts[npcAnswerTree.dayCount];
        textManager.nPCAnswerSpeech = npcAnswer;

        image.texture = npcAnswer.drugSprite;
    }
    
    void CheckSellOffer()
    {

        if (image.texture == null)
        {
            return;
        }

        if (textManager.currentSpeechIndex >= npcAnswer.offerToSell && npcAnswer.offerToSell > 0)
        {
            int checkDrug = npcAnswer.drugID;
            InventoryDrug masterDrug = inventory.CheckForItem(checkDrug);

            playerDialogue.OpenSellDialog(true);
            if (Input.GetKeyDown(KeyCode.F) && masterDrug.quant > 0)
            {
                inventory.SellItem(checkDrug, 1);
                offeredToSell = true;
            }

            if (npcAnswer.offerToSell == 0)
            {
                playerDialogue.OpenSellDialog(false);
            }
        }
    }

    void EndDialogue()
    {
        playerDialogue.isPlayerClose = false;
        playerDialogue.openDialog = false;
        textManager.speechInProgress = false;
    }

    void BackToStart()
    {
        if (textManager.speechInProgress == false)
        {
            speechSelection = SpeechSelection.Start;
        }
    }

    void OptionStopConversation()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            speechSelection = SpeechSelection.End;
        }
    }

    void CurrentTree()
    {
        switch (speechSelection)
        {
            default:
            case SpeechSelection.Start:
                {                    
                    StartConversation();
                    CheckState();
                    Debug.Log("Start Convo");
                    OptionStopConversation();
                    break;
                }

            case SpeechSelection.Left:
                {
                    key = KeyCode.Q;
                   
                    LoadLeft();
                    StartConversation();
                    
                    ContinueConversation();
                    playerDialogue.CloseDialog(true);

                    if (textManager.currentSpeechIndex == npcAnswerTree.dailyScripts[gm.dayCount].speechGroup.Count)
                    {
                        speechSelection = SpeechSelection.End;
                    }

                    OptionStopConversation();
                    break;
                }

            case SpeechSelection.Right:
                {
                    key = KeyCode.E;

                    LoadRight();
                    StartConversation();

                    ContinueConversation();
                    playerDialogue.CloseDialog(false);

                    if (textManager.currentSpeechIndex == npcAnswerTree.dailyScripts[gm.dayCount].speechGroup.Count)
                    {
                        speechSelection = SpeechSelection.End;
                    }

                    OptionStopConversation();
                    break;
                }

            case SpeechSelection.End:
                {
                    //read end dialogue
                    EndDialogue();
                    Debug.Log("EndConvo" + textManager.speechInProgress);
                    BackToStart();
                    break;
                }
        }

        void ContinueConversation()/*itt a bibi*/
        {
            if (textManager.speechInProgress == true && Input.GetKeyDown(key))
            {
                if (key == KeyCode.Q)
                {
                    textManager.SetSpeechNext();
                    if (textManager.playerSpeechInProgress == false)
                        textManager.StartPlayerLeftSpeech(playerAnswer);
                    else
                        textManager.SetPlayerLeftSpeechNext();
                }
                if (key == KeyCode.E)
                {
                    textManager.SetSpeechNext();
                    if (textManager.playerSpeechInProgress == false)
                        textManager.StartPlayerRightSpeech(playerAnswer);
                    else
                        textManager.SetPlayerRightSpeechNext();
                }
            }         
        }
    }   
}


//---future project ---//
//making responses with an enum switch state
/*public void CurrentResponse()
{
    switch (response)
    {
        default:
        case Responses.topResponse:
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Debug.Log("Alpha1 key is pressed");
                    TextManager.instance.SetSpeechNext();
                }
                break;
            }
        case Responses.leftResponse:
            {
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Debug.Log("Alpha2 key is pressed");
                    TextManager.instance.SetSpeechNext();
                }
                break;
            }
        case Responses.rightResponse:
            {
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    Debug.Log("Alpha3 key is pressed");
                    TextManager.instance.SetSpeechNext();
                }
                break;
            }

    }       
}*/
//TextManager.instance.speakerName = ray.hitObj.GetComponentInChildren<Text>();
//TextManager.instance.currentSpeech = speech;
//GetComponent<PlayerDialogue>().continueConversation = ray.hitObj.GetComponent<NPCDialogue>().playerContinueConversation;
//GetComponentInChildren<CanvasGroup>().interactable = true;
/*if (ray.NPCTrigger() == false && Input.GetKeyDown(KeyCode.F))
            {

                //close player dialogue
                GetComponent<PlayerDialogue>().closePlayerDialogue = false;
            }*/
//TextManager.instance.StartSpeech(continueConversation);
//TextManager.instance.SetSpeechNext();
/*if (Input.GetKeyDown(KeyCode.Alpha3))
{

    Debug.Log("Alpha3 key is pressed");
    ray.hitObj.GetComponent<NPCDialogue>().closeDialogue = false;
}*/
//OLD CODE
/*

                        /*if (textManager.speechInProgress == true && textManager.currentSpeechIndex == npcAnswer.speechGroup.Count)
                    {
                        speechSelection = SpeechSelection.End;
                    }




 if (Input.GetKeyDown(KeyCode.X))
    {
        EndDialogue();
    } //close dialogue

 */