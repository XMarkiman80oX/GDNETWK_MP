using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Canvas preGameCanvas;
    public Canvas mainCanvas;
    public GameObject startMenu;
    
    public Button readyBtn;
    public Button Choice1Btn;
    public Button Choice2Btn;
    public Button Choice3Btn;
    public Text Choice1Txt;
    public Text Choice2Txt;
    public Text Choice3Txt;
    public InputField usernameField;
    public InputField answerAttemptField;
    public InputField chatField;
    public Text connectedStatusText;
    public Text riddleQuestionText;
    public Text riddleAnswerText;
    public Text timerTxt;
    public Sprite CorrectMark;
    public Sprite WrongMark;
    public GameObject chatTemplate;
    public GameObject NotificationTemplate;
    public GameObject usersMarksTrackerUI;
    public GameObject usersPointsTrackerUI;
    public GameObject selfProfileUI;
    public GameObject usersChatUI;
    public GameObject notifUI;
    private int nUserUI = 0;
    public List<GameObject> usersMarksTrackerUIList;
    public List<GameObject> usersPointsTrackerUIList;
    public List<GameObject> notificationList;
    public List<Text> chatList;
    public List<ChatText> chatLog;
    public Transform usersMarksTrackerUITransform;
    public Transform usersPointsTrackerUITransform;

    private Text pointsText;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        usersMarksTrackerUIList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ConnectToServerTCP()
    {
        //startMenu.SetActive(false);
        //usernameField.interactable = false;
        Debug.Log("THIS RUNS LEGIT");
        Client.Instance.ConnectToServerTCP();
    }

    public void ConnectToServerUDP()
    {
        Client.Instance.ConnectToServerUDP();
    }

    public void PlayerReadyChangeTCP()
    {
        if (readyBtn != null && Client.Instance.isConnected)
        {
            if(Client.Instance.isReady) 
                readyBtn.image.color = new Color(1.0f, 0.392f, 0.392f);    //make red

            else
                readyBtn.image.color = new Color(0.392f, 1.0f, 0.392f);    //make green

        }
            
        ClientSend.TCPPlayerReadyStatusChangeSend();
    }

    public void DisplayChoices(string choice1, string choice2, string choice3)
    {
        Choice1Txt.text = choice1;
        Choice2Txt.text = choice2;
        Choice3Txt.text = choice3;

        Choice1Btn.gameObject.SetActive(true);
        Choice2Btn.gameObject.SetActive(true);
        Choice3Btn.gameObject.SetActive(true);

        
    }

    public void HideChoices()
    {

        Choice1Btn.gameObject.SetActive(false);
        Choice2Btn.gameObject.SetActive(false);
        Choice3Btn.gameObject.SetActive(false);

    }

    public void SelectChoice1()
    {
        ClientSend.TCPPromptSelectSend(0);
        HideChoices();
    }

    public void SelectChoice2()
    {
        ClientSend.TCPPromptSelectSend(1);
        HideChoices();
    }

    public void SelectChoice3()
    {
        ClientSend.TCPPromptSelectSend(2);
        HideChoices();
    }

    public void setupProfile(string _username)
    {
        selfProfileUI.transform.GetChild(0).gameObject.GetComponent<Text>().text = _username;
        selfProfileUI.transform.GetChild(1).gameObject.GetComponent<Text>().text = "0";
    }

    public void setSelfProfilePointsText(int _newPoints)
    {
        selfProfileUI.transform.GetChild(1).gameObject.GetComponent<Text>().text = _newPoints.ToString();
    }

    public void SetRiddleText(string _text)
    {
        riddleQuestionText.text = _text;
    }

    public void ShowRiddleAnswer(string _text)
    {
        riddleAnswerText.text = _text;
    }

    public void ResetRiddleAnswer()
    {
        riddleAnswerText.text = "";
    }

    public void SetConnectedText(bool _isConnected)
    {
        if(connectedStatusText != null)
        {
            if (_isConnected)
            {
                connectedStatusText.text = "Status to the server: Connnected";
            }

            else
            {
                connectedStatusText.text = "Status to the server: Disconnected";
            }
        }
        
    }

    public void LimitTextLength(InputField _if)
    {
        int _stringLength = 8;
        if (_if.text.Length > _stringLength)
        {
            string newText = _if.text.Remove(_stringLength);
            _if.text = newText;
        }
    }

    public void SetTimer(int _txt)
    {
        timerTxt.text = _txt.ToString();
    }

    public void DisableAnswerField()
    {
        answerAttemptField.interactable = false;
    }

    public void EnableAnswerField()
    {
        answerAttemptField.interactable = true;
    }

    public void ShowMainUI()
    {
        preGameCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
        
    }

    public void EnterAnswerAttempt()
    {
        //if(Input.GetKeyDown(KeyCode.Return))
        //{
        //    GameManager.Instance.EvaluateAnswer(answerAttemptField.text);
        //    answerAttemptField.text = "";

        //    if (answerAttemptField.IsInteractable())
        //        answerAttemptField.ActivateInputField();
        //}

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ClientSend.TCPPromptReplySend(answerAttemptField.text);
            answerAttemptField.interactable = false;
        }    



    }

    public void AddUserUI(string username, int id, int points)
    {
        Debug.Log("Adding user ui");

        //GameObject newUserMarksUI = Instantiate(usersMarksTrackerUI, answerAttemptField.gameObject.transform.position + new Vector3(0.0f, -45.0f - (nUserUI * 25.5f), 0.0f), Quaternion.identity, usersMarksTrackerUITransform);



        //newUserMarksUI.GetComponent<UserUIBehaviour>().SetUsernametext(username);
        //newUserMarksUI.GetComponent<UserUIBehaviour>().SetUserID(id);

        //usersMarksTrackerUIList.Add(newUserMarksUI);

        GameObject newUserPointsUI = Instantiate(usersPointsTrackerUI, usersPointsTrackerUITransform);
        newUserPointsUI.GetComponent<Text>().text = username;
        newUserPointsUI.transform.GetChild(0).gameObject.GetComponent<Text>().text = points.ToString();
        usersPointsTrackerUIList.Add(newUserPointsUI);
        SortPlayerListByScore();

    }

    public void RemoveUserUI(int userId)
    {
        Debug.Log("Removing user ui");

        foreach(GameObject uiObj in usersMarksTrackerUIList)
        {
            if (uiObj.GetComponent<UserUIBehaviour>().GetUserID() == userId)
            {
                usersMarksTrackerUIList.Remove(uiObj);
                Destroy(uiObj);
                Debug.Log("Removing a player's ui1");
                break;
            }
 
        }

        foreach(GameObject uiObj in usersPointsTrackerUIList)
        {
            if (uiObj.GetComponent<Text>().text == GameManager.Instance.playerList[userId].username && uiObj.transform.GetChild(0).gameObject.GetComponent<Text>().text == GameManager.Instance.playerList[userId].points.ToString())
            {
                usersPointsTrackerUIList.Remove(uiObj);
                Destroy(uiObj);
                Debug.Log("Removing a player's ui2");
                break;
            }
        }

    }

    public void AddUserReplyUI(int _id, string _name, string _reply)
    {
        Debug.Log("Adding user ui");

        GameObject newUserReplyUI = Instantiate(usersMarksTrackerUI, answerAttemptField.gameObject.transform.position + new Vector3(0.0f, 400.0f - (nUserUI * 200.5f), 0.0f), Quaternion.identity, usersMarksTrackerUITransform);
        nUserUI++;
        newUserReplyUI.GetComponent<UserReplyUIBehaviour>().SetUserID(_id);
        newUserReplyUI.GetComponent<UserReplyUIBehaviour>().SetName(_name);
        newUserReplyUI.GetComponent<UserReplyUIBehaviour>().SetReply(_reply);
        usersMarksTrackerUIList.Add(newUserReplyUI);

    }

    public void ClearUserReplyUIList()
    {
        

        for (int i = 0; i < usersMarksTrackerUIList.Count; i++)
        {
            Destroy(usersMarksTrackerUIList[i]);
            
            
        }

        usersMarksTrackerUIList.Clear();
        nUserUI = 0;
    }

    public void RefreshPlayerScores()
    {
        Dictionary<string, int> userPointsList = GameManager.Instance.GetAllPlayerPoints();

        foreach(string username in userPointsList.Keys)
        {
            for (int i = 0; i < usersPointsTrackerUIList.Count; i++)
            {
                if (usersPointsTrackerUIList[i].GetComponent<Text>().text == username)
                {
                    usersPointsTrackerUIList[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = userPointsList[username].ToString();
                    //Debug.Log(username + " now has " + userPointsList[username] + " points");
                }
            }
            
        }
    }

    public void SortPlayerListByScore()
    {
        usersPointsTrackerUIList.Sort((a, b) => a.transform.GetChild(0).GetComponent<Text>().text.CompareTo(b.transform.GetChild(0).GetComponent<Text>().text));
        usersPointsTrackerUIList.Reverse();
        int nElements = usersPointsTrackerUITransform.childCount;

        foreach(GameObject obj in usersPointsTrackerUIList)
        {
            Debug.Log(obj.GetComponent<Text>().text + ": " + obj.transform.GetChild(0).GetComponent<Text>().text);
        }

        Debug.Log("sorting");
        for (int i = 0; i < nElements; i++)
        {
            for(int j = 0; j < usersPointsTrackerUIList.Count; j++)
            {
                Debug.Log(usersPointsTrackerUITransform.GetChild(i).GetComponent<Text>().text + "vs" + usersPointsTrackerUIList[j].GetComponent<Text>().text);
                Debug.Log(usersPointsTrackerUITransform.GetChild(i).gameObject == usersPointsTrackerUIList[j]);
                if (usersPointsTrackerUITransform.GetChild(i).gameObject == usersPointsTrackerUIList[j])
                {
                    usersPointsTrackerUITransform.GetChild(i).SetSiblingIndex(j);
                    Debug.Log("modifying child index");
                }
            }
        }
    }

    public void AddCorrectMark(int _id)
    {
        foreach (GameObject _userUI in usersMarksTrackerUIList)
        {
            
            if(_userUI.GetComponent<UserUIBehaviour>().GetUserID() == _id)
            {
                _userUI.GetComponent<UserUIBehaviour>().addCorrectMark();
            }
            
        }
    }

    public void AddWrongMark(int _id)
    {
        foreach (GameObject _userUI in usersMarksTrackerUIList)
        {

            if (_userUI.GetComponent<UserUIBehaviour>().GetUserID() == _id)
            {
                _userUI.GetComponent<UserUIBehaviour>().addWrongMark();
            }

        }
    }


    public void ResetUserUIMarks()
    {
        foreach(GameObject _userUI in usersMarksTrackerUIList)
        {
            for (int i = 0; i < 5; i++)
            {
                _userUI.GetComponent<UserUIBehaviour>().ResetMarks();
            }
        }
    }

    public void SendChatMessage()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            string _chat = chatField.text;
            //GameObject newChat = Instantiate(chatTemplate, usersChatUI.transform);
            //Text newChatTxt = newChat.GetComponent<Text>();
            //newChatTxt.text = _chat;


            //chatList.Add(newChatTxt);
            ClientSend.TCPChatMessageSend(_chat);
            chatField.ActivateInputField();
        }
        
    }

    public void ReceiveChatMessage(string _fromPlayer, string _chatMsg)
    {
        GameObject newChat = Instantiate(chatTemplate, usersChatUI.transform);
        Text newChatTxt = newChat.GetComponent<Text>();
        newChatTxt.text = _fromPlayer + ": "+ _chatMsg;


        chatList.Add(newChatTxt);
    }

    public void UpdateAttemptLog(string _username, string _answerGuess, bool _isAnswerCorrect)
    {
        GameObject newAttemptNotif = Instantiate(NotificationTemplate, notifUI.transform);
        Text newAttemptTxt = newAttemptNotif.GetComponent<Text>();
        if (_isAnswerCorrect)
            newAttemptTxt.text = _username + " found the answer to the riddle";

        else
            newAttemptTxt.text = _username + " attempted to answer with their guess: \n" + _answerGuess;

        notificationList.Add(newAttemptNotif);
    }

    public void ClearAttemptLog()
    {
        foreach(GameObject _obj in notificationList)
        {
            Destroy(_obj);
        }

        notificationList.Clear();
    }

    public void DisableAllLikeButtonExcept(int _id)
    {
        foreach(GameObject obj in usersMarksTrackerUIList)
        {
            if(obj.GetComponent<UserReplyUIBehaviour>().id != _id)
                obj.GetComponent<UserReplyUIBehaviour>().DisableLikeBtn();
        }
    }

    public void EnableAllLikeButton()
    {
        foreach (GameObject obj in usersMarksTrackerUIList)
        {
            obj.GetComponent<UserReplyUIBehaviour>().EnableLikeButton();
        }
    }

    public void SetVoteForReplyUI(int _id, int _votes)
    {
        foreach (GameObject obj in usersMarksTrackerUIList)
        {
            if (obj.GetComponent<UserReplyUIBehaviour>().id == _id)
                obj.GetComponent<UserReplyUIBehaviour>().SetVotes(_votes);
        }
    }

    public void SetHighestVotesUI(int _id)
    {
        foreach (GameObject obj in usersMarksTrackerUIList)
        {
            if (obj.GetComponent<UserReplyUIBehaviour>().id == _id)
                obj.GetComponent<UserReplyUIBehaviour>().ChanegBGColorToGreen();
        }
    }
    
    /*
     * Classes
     */
    
    public class Message
    {
        public Text msgTxt;
        public string msgString;

        public Message(string _msgString)
        {

            msgTxt.text = _msgString;
        }
    }
}
