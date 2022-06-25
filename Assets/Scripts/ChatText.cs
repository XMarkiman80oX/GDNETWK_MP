using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatText : MonoBehaviour
{
    // Start is called before the first frame update

    public Text chatObj;
    public string chatMessage;

    public void Start()
    {
        chatObj.text = chatMessage;
    }
}
