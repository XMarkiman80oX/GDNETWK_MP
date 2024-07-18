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
    private TMP_Text _resultText;

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

    public void ResetGame() {
        Player1Manager.Instance.ResetPlayer();
        Player2Manager.Instance.ResetPlayer();
    }

    public void ResetPlayers() {
        Player1Manager.Instance.ResetPlayerState();
        Player2Manager.Instance.ResetPlayerState();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
