using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    
    private RayCast ray;
    [SerializeField]
    SpeechScriptableObjects speech;
    
    public SpeechScriptableObjects continueConversation;
    
    public SpeechScriptableObjects sellOffer;
    
    public SpeechScriptableObjects endDialogue;
    private Text npcName;
    private Text npcDialogue;
    private GameManager gm;
    private TextManager textManager;
    public RawImage image;

    void Start()
    {
        ray = FindObjectOfType<Camera>().GetComponent<RayCast>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        textManager = TextManager.instance;
    }

    void Update()
    {
        Interact();
        NameHower();
        if (textManager.speechInProgress == true && Input.GetKeyDown(KeyCode.Space))
        {
            textManager.SetSpeechNext();
            if (textManager.playerSpeechInProgress == false)
                textManager.StartPlayerSpeech(continueConversation);
            else
                textManager.SetPlayerSpeechNext();
            //TextManager.instance.StartSpeech(continueConversation);
            //TextManager.instance.SetSpeechNext();

            /*if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                
                Debug.Log("Alpha3 key is pressed");
                ray.hitObj.GetComponent<NPCDialogue>().closeDialogue = false;
            }*/
        }        
    }

    private void NameHower()
    {
        if (ray.NPCTrigger() == true)
        {            
            ray.hitObj.GetComponent<NPCDialogue>().closeDialogue = true;
            GetComponent<PlayerDialogue>().target = ray.hitObj.GetComponent<Transform>();
            textManager.dialogueText = ray.hitObj.GetComponentInChildren<Text>();
            speech = ray.hitObj.GetComponent<NPCDialogue>().npcSpeech;
            //GetComponent<PlayerDialogue>().drugSprite = ray.hitObj.GetComponent<NPCDialogue>().npcSpeech.drugSprite;
            //GetComponent<Image>().sprite = ray.hitObj.GetComponent<NPCDialogue>().npcSpeech.drugSprite;
            image.texture = ray.hitObj.GetComponent<NPCDialogue>().npcSpeech.drugSprite;
            /*TextManager.instance.currentSpeechIndex = 0;
            TextManager.instance.currentPlayerSpeechIndex = 0;*/

        }

        if (ray.NPCTrigger() == false)
        {
            textManager.ResetDialogues();
            image.texture = null;
            GetComponent<PlayerDialogue>().OpenSellDialog(false);
            /*speech = null;
            continueConversation = null;*/

        }
    }

    private void Interact()
    {
        
        //if Ray hit NPC and F key is hit
        if (ray.NPCTrigger() == true && Input.GetKeyDown(KeyCode.Space) && textManager.speechInProgress == false)
        {
            //---NPC---//
            // get targetNPCs speech

            //Begin speech
            textManager.StartSpeech(speech);
            textManager.currentSpeechIndex = 0;

            //setpup up text as target
            

            //---Player---//
            //open player dialogue at the same time as you being to speak
            GetComponent<PlayerDialogue>().closePlayerDialogue = true;
            //Get the players conversation stored under the npc to make sure conversations are NPC Specific.
            continueConversation = ray.hitObj.GetComponent<NPCDialogue>().continueConversation;
            textManager.currentPlayerSpeechIndex = 0;
        }

        if (textManager.currentSpeechIndex == sellOffer.offerToSell)
        {
            GetComponent<PlayerDialogue>().OpenSellDialog(true);
            if (sellOffer.offerToSell == 0)
            {
                GetComponent<PlayerDialogue>().OpenSellDialog(false);
            }
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Sold drug");
            gm.addMoney(50);
            //GetComponent<PlayerDialogue>().OpenSellDialog(true);
        }
        /*if (ray.NPCTrigger() == false && Input.GetKeyDown(KeyCode.F))
        {

            //close player dialogue
            GetComponent<PlayerDialogue>().closePlayerDialogue = false;
        }*/
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
}




//TextManager.instance.speakerName = ray.hitObj.GetComponentInChildren<Text>();
//TextManager.instance.currentSpeech = speech;
//GetComponent<PlayerDialogue>().continueConversation = ray.hitObj.GetComponent<NPCDialogue>().playerContinueConversation;
//GetComponentInChildren<CanvasGroup>().interactable = true;