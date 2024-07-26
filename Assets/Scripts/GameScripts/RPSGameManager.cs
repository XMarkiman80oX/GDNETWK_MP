using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RPSGameManager : MonoBehaviour {
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
    public Sprite RockSprite { get; }

    [SerializeField]
    public Sprite ScissorSprite { get; }

    [SerializeField]
    public Sprite PaperSprite { get; }

    [SerializeField]
    public Sprite QuestionSprite { get; }

    [SerializeField]
    private TMP_Text _timerText;

    private bool _timerRunning;

    private float _timerLimit = 60.0f;

    private float _timeLeft;

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
    }

    public void OnGameStart() {
        this._playButton.enabled = false;
        this._playButton.interactable = false;
        this._choices.SetActive(true);
        this._timerRunning = true;
        this._timeLeft = this._timerLimit;
    }

    public void OnGameEnd() {
        this._playButton.enabled = true;
        this._playButton.interactable = true;
        this._choices.SetActive(false);
        int P1 = Player1Manager.Instance.Score;
        int P2 = Player2Manager.Instance.Score;
        string resultText = "Technical Difficulties folks!";
        if(P1 > P2) {
            resultText = "Player 1 wins!";
        }
        if(P1 < P2) {
            resultText = "Player 2 wins!";
        }
        else if(P1 == P2) {
            resultText = "Draw!";
        }
        else if(P1 == 0 && P2 == 0) {
            resultText = "Uhh... were you two AFK?";
        }
        this._resultText.text = resultText;
    }

    public void ResetGame() {
        Player1Manager.Instance.ResetPlayer();
        Player2Manager.Instance.ResetPlayer();
    }

    public void ResetPlayers() {
        Player1Manager.Instance.ResetPlayerState();
        Player2Manager.Instance.ResetPlayerState();
    }

    private void UpdateTimeLeft() {
        if (_timerText != null && _timerRunning) {
            if (_timeLeft >= 0) {
                _timeLeft -= Time.deltaTime;
            }
            _timerText.text = $"Time Left: {_timeLeft:F2}";
        }
        else {
            Debug.LogWarning("Time text not found.");
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_timeLeft > 0) {
            CheckChoices();
            UpdateTimeLeft();
        }
        else {
            _timeLeft = 0;
            _timerText.text = $"Time Left: {_timeLeft:F2}";
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
