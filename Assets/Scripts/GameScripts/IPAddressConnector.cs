using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPAddressConnector : MonoBehaviour
{
    [SerializeField]
    private string _IPAddress;
    public string IPAddress {
        get { return this._IPAddress; }
        set { this._IPAddress = value; }
    }

    [SerializeField]
    private GameClient _client;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeInIPAddress(InputField _if)
    {
        int _stringLength = 15;
        if (_if.text.Length > _stringLength)
        {
            string newText = _if.text.Remove(_stringLength);
            _if.text = newText;
        }
        this._client.ip = _if.text;
    }
}
