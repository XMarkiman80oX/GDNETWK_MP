using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RPSGameManager : MonoBehaviour {

    private int _timesLoaded = 0;

    private static RPSGameManager _instance;
    public static RPSGameManager Instance {
        get { if (_instance == null) {}
            return _instance;
        }
    }

    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private GameObject _choices;

    [SerializeField]
    private TMP_Text _resultText;

    [SerializeField]
    private GameObject _resultTextObj;

    public Sprite RockSprite;

    public Sprite PaperSprite;

    public Sprite ScissorSprite;

    public Sprite QuestionSprite;

    [SerializeField]
    private TMP_Text _timerText;

    private bool _timerRunning = false;

    private float _timerLimit = 60.0f;

    private float _timeLeft = 60.0f;

    [SerializeField]
    private bool _isGameLoaded = false;
    public bool IsGameLoaded
    {
        get { return _isGameLoaded; } 
    }
    [SerializeField]
    private bool _isGameStarted = false;
    public bool IsGameStarted
    {
        get { return _isGameStarted; }
    }
    private void CheckChoices() {
        EChoice p1Choice = Player1Manager.Instance.Choice;
        EChoice p2Choice = Player2Manager.Instance.Choice;

        if(p1Choice == p2Choice) {
            this._resultText.text = "DRAW";
        }
        else {
            switch(p1Choice) {
                case (EChoice.ROCK) :
                    switch(p2Choice) {
                        case (EChoice.SCISSORS) :
                            Player1Manager.Instance.AddPoint();
                            this._resultText.text = "P1 wins";
                            break;
                        case (EChoice.PAPER) :
                            Player2Manager.Instance.AddPoint();
                            this._resultText.text = "P2 wins";
                            break;
                        default:
                            break;
                    }
                    break;
                case (EChoice.PAPER) :
                    switch(p2Choice) {
                        case (EChoice.ROCK) :
                            Player1Manager.Instance.AddPoint();
                            this._resultText.text = "P1 wins";
                            break;
                        case (EChoice.SCISSORS) :
                            Player2Manager.Instance.AddPoint();
                            this._resultText.text = "P2 wins";
                            break;
                        default:
                            break;
                    }
                    break;
                case (EChoice.SCISSORS) :
                    switch(p2Choice) {
                        case (EChoice.PAPER) :
                            Player1Manager.Instance.AddPoint();
                            this._resultText.text = "P1 wins";
                            break;
                        case (EChoice.ROCK) :
                            Player2Manager.Instance.AddPoint();
                            this._resultText.text = "P2 wins";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        this.ResetPlayers();
    }
    public void OnGameLoad(string playername) {//when first loading the game
        this._timesLoaded++;

        if (this._timesLoaded > 2)
            this._timesLoaded = 1;

        switch (this._timesLoaded)
        {
            case 1:
                Player1Manager.Instance.IntitializePlayer(playername);
                break;
            case 2:
                Player2Manager.Instance.IntitializePlayer(playername);
                break;
        }

        GameUIManager.Instance.ShowMainUI();
        this._playButton.gameObject.SetActive(true);
        this._playButton.interactable = true;
        this._choices.SetActive(false);
        //this._timerRunning = false;
        this._resultText.text = "";
        this._isGameLoaded = true;
        Debug.Log("IS GAME LOADED: " + this._isGameLoaded);
    }

    public void OnGameStart() {//when pressing play button
        this.ResetGame();
        this._timerRunning = true;
        this._playButton.gameObject.SetActive(false);
        this._playButton.interactable = false;
        this._choices.SetActive(true);
        //this._timerRunning = true;
        this._timeLeft = this._timerLimit;
        this._resultText.text = "";

        this._isGameStarted = true;
    }

    public void OnGameEnd() {
        this._playButton.enabled = true;
        this._playButton.interactable = true;
        this._choices.SetActive(false);
        int P1 = Player1Manager.Instance.Score;
        int P2 = Player2Manager.Instance.Score;
        string resultText = "Technical Difficulties folks!";
        int xPos = 0;

        if(P1 > P2) {
            resultText = "Player 1 wins!";
            xPos = -500;
        }
        else if(P1 < P2) {
            resultText = "Player 2 wins!";
            xPos = 500;
        }
        else if(P1 == P2) {
            resultText = "Draw!";
            
        }
        else if(P1 == 0 && P2 == 0) {
            resultText = "Uhh... were you two AFK?";
        }
        this._resultText.text = resultText;
        this._resultTextObj.transform.position = new Vector3(xPos, 350, 0);
        this._isGameLoaded = false;
        this._isGameStarted = false;
    }

    public void ResetGame() {
        Player1Manager.Instance.ResetPlayer();
        Player2Manager.Instance.ResetPlayer();
    }

    public void ResetPlayers() {
        Player1Manager.Instance.ResetPlayerState();
        Player2Manager.Instance.ResetPlayerState();

        this._choices.gameObject.SetActive(false);
    }

    public void IntitializePlayers() {
        Player1Manager.Instance.IntitializePlayer();
        Player2Manager.Instance.IntitializePlayer();
    }
    public void IntitializePlayers(string player1name, string player2name)
    {
        Player1Manager.Instance.IntitializePlayer(player1name);
        Player2Manager.Instance.IntitializePlayer(player2name);
    }

    private void UpdateTimeLeft() {
        if (_timerText != null && _timerRunning) {
            if (_timeLeft >= 0) {
                _timeLeft -= Time.deltaTime;
            }
            _timerText.text = $"{_timeLeft:F2}";
        }
        else {
            Debug.LogWarning("Time text not found.");
        }
    }

    void Update()
    {
        if(_timeLeft > 0) {
            if(Player1Manager.Instance.IsReady && Player2Manager.Instance.IsReady) {
                CheckChoices();
            }
            UpdateTimeLeft();
        }
        else {
            _timeLeft = 0;
            _timerText.text = $"{_timeLeft:F2}";
            Debug.Log("GAME ENDED");
            OnGameEnd();
        }
    }

    void Awake() {
        if(_instance == null){
            _instance = this;
        }
        else{
            Debug.Log("You should destroy yourself now.");
            Destroy(gameObject);
        }
    }
}
