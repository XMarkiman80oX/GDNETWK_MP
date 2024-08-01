using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player1Manager : MonoBehaviour {
    private static Player1Manager _instance;
    public static Player1Manager Instance {
        get { if (_instance == null) {}
            return _instance;
        }
    }

    [SerializeField]
    private string _name;
    public string Name {
        get { return this._name; }
        set { this._name = value; }
    }

    [SerializeField]
    private int _score;
    public int Score {
        get { return this._score; }
        set { this._score = value; }
    }

    [SerializeField]
    private TMP_Text _scoreBoard;

    [SerializeField]
    private Image _playerChoice;

    [SerializeField]
    private TMP_Text _playerNameTag;

    private EChoice _choice;
    public EChoice Choice {
        get { return this._choice; }
        set { this._choice = value; }
    }

    private bool _isReady;
    public bool IsReady {
        get { return this._isReady; }
        set { this._isReady = value; }
    }

    public void AddPoint() {
        this._score++;
    }

    public void Rock() {
        this._isReady = true;
        this._choice = EChoice.ROCK;
        this._playerChoice.sprite = RPSGameManager.Instance.RockSprite;
    }

    public void Paper() {
        this._isReady = true;
        this._choice = EChoice.PAPER;
        this._playerChoice.sprite = RPSGameManager.Instance.PaperSprite;
    }

    public void Scissors() {
        this._isReady = true;
        this._choice = EChoice.SCISSORS;
        this._playerChoice.sprite = RPSGameManager.Instance.ScissorSprite;
    }

    public void ResetPlayer() {
        this._score = 0;
        this._isReady = false;
        this._choice = EChoice.NONE;
        this._playerChoice.sprite = RPSGameManager.Instance.QuestionSprite;
    }

    public void ResetPlayerState() {
        this._isReady = false;
        this._choice = EChoice.NONE;
        this._playerChoice.sprite = RPSGameManager.Instance.QuestionSprite;
    }

    public void IntitializePlayer() {
        this._score = 0;
        this._playerNameTag.text = this._name;
        this._choice = EChoice.NONE;
        this._playerChoice.sprite = RPSGameManager.Instance.QuestionSprite;
    }
    public void IntitializePlayer(string playerName)
    {
        Debug.Log("New Player Name: " + playerName);
        this._score = 0;
        this._playerNameTag.text = playerName;
        this._choice = EChoice.NONE;
        this._playerChoice.sprite = RPSGameManager.Instance.QuestionSprite;
    }

    void Start() {
        this.ResetPlayer();
    }

    void FixedUpdate() {
        this._scoreBoard.text = "Player 1 Score : " + this._score.ToString();
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
