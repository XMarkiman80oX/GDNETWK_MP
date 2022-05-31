using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject startMenu;
    public InputField usernameField;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectToServerTCP()
    {
        //startMenu.SetActive(false);
        //usernameField.interactable = false;
        Client.Instance.ConnectToServerTCP();
    }

    public void ConnectToServerUDP()
    {
        Client.Instance.ConnectToServerUDP();

    }
}
