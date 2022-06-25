using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance;
    public string riddleQuestion;
    public string riddleAnswer;
    private int maxAttempts = 5;
    private int currentAttempts = 0;
    private int CurrentPoints = 0;
    public Dictionary<int, Player> playerList;
    private int currentPlayerIndex = 0;

    public bool isGameStarted = false;

    public class Player
    {
        public int id { get; set; }
        public string username { get; set; }
        public int points { get; set; }

        public Player(int _id, string _username, int _points)
        {
            id = _id;
            username = _username;
            points = _points;
        }
            
        public void addPoint()
        {
            this.points += 1;
            Debug.Log(this.points);
    
        }

        public int GetPoints()
        {
            return points;
        }
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(this);
    }

    void Start()
    {
        playerList = new Dictionary<int, Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupPlayerList()
    {

    }
    
    public void StartGame()
    {
        if(!isGameStarted)
        {
            UIManager.Instance.ShowMainUI();
            ClientSend.TCPRequestPlayerList();
            isGameStarted = true;
        }
        
    }

    public void AddPlayerToList(int _id, string _username, int _points)
    {
        Player newPlayer = new Player(_id, _username, _points);

        playerList.Add(_id, newPlayer);
        UIManager.Instance.AddUserUI(_username, _id, _points);
    }

    public void AssignNewRiddle()
    {
        StartCoroutine(AssignNewRiddleCoroutine());
        ClientSend.TCPRequestPlayerList();
    }

    private IEnumerator AssignNewRiddleCoroutine()
    {
        
        yield return new WaitForSeconds(0.85f);
        currentAttempts = 0;
        UIManager.Instance.SetRiddleText(riddleQuestion);
        UIManager.Instance.SetRiddleText(riddleQuestion);
        UIManager.Instance.ResetRiddleAnswer();
        UIManager.Instance.ResetUserUIMarks();
        UIManager.Instance.SortPlayerListByScore();
        UIManager.Instance.ClearAttemptLog();
        UIManager.Instance.EnableAnswerField();
    }
    public void SetRiddleAnswer(string _riddleAnswer)
    {
        riddleAnswer = _riddleAnswer;
    }

    public void SetRiddleQuestion(string _riddleQuestion)
    {
        riddleQuestion = _riddleQuestion;
    }

    public void EvaluateAnswer(string _answer)
    {
        if(string.Compare(_answer, riddleAnswer, true) == 0)
        {
    
            CurrentPoints++;
            ClientSend.TCPFinishedRoundSend();
            ClientSend.TCPAsnwerAttempt(_answer, true);
            OnPlayerAttemptRight(Client.Instance.myId);
            UIManager.Instance.setSelfProfilePointsText(CurrentPoints);
            UIManager.Instance.DisableAnswerField();
            UIManager.Instance.ShowRiddleAnswer(riddleAnswer);
        }

        else
        {
            currentAttempts++;
            Debug.Log($"Wrong answer. You have {maxAttempts-currentAttempts} attempts left");
            if(maxAttempts - currentAttempts == 0)
            {
                ClientSend.TCPFinishedRoundSend();
                UIManager.Instance.DisableAnswerField();
                UIManager.Instance.ShowRiddleAnswer(riddleAnswer);
            }
               

            ClientSend.TCPAsnwerAttempt(_answer, false);
            OnPlayerAttemptWrong(Client.Instance.myId, _answer);
        }
    }

    public void OnPlayerAttemptWrong(int player, string _answerGuess)
    {
        if (playerList.ContainsKey(player))
        {
            UIManager.Instance.AddWrongMark(player);
            UIManager.Instance.UpdateAttemptLog(playerList[player].username, _answerGuess, false);
        }
    }

    public void OnPlayerAttemptRight(int player)
    {
        if (playerList.ContainsKey(player))
        {
            UIManager.Instance.AddCorrectMark(player);
            playerList[player].addPoint();
            UIManager.Instance.UpdateAttemptLog(playerList[player].username, "", true);
            //Debug.Log("player " + playerList[player].id + "points: " + playerList[player].GetPoints());
        }

        else
        {
            Debug.Log("cant find player");
        }
        
    }

    

    public Dictionary<string, int> GetAllPlayerPoints()
    {
        Dictionary<string, int> userList = new Dictionary<string, int>();
        foreach (KeyValuePair<int, Player> p in playerList)
        {
            userList.Add(p.Value.username, p.Value.GetPoints());
        }

        return userList;
        
    }
}
