using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour{
    [SerializeField]
    private Canvas _IPScreen;
    [SerializeField]
    private Canvas _pregameScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToPreGame() {//called by IP button
        this._IPScreen.gameObject.SetActive(false);
        this._pregameScreen.gameObject.SetActive(true);
    }
}
