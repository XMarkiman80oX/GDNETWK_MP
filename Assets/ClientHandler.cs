using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class ClientHandler : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg =_packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Recieved packet via TCP from server. Contains message: {_msg}");
        Debug.Log($"You are now player {_myId}");

        Client.Instance.myId = _myId;
        ClientSend.WelcomeReceived();

        
    }

    public static void UDPTest(Packet _packet)
    {
        string _msg = _packet.ReadString();

        Debug.Log($"Recieved packet via UDP from server. Contains message: {_msg}");
        ClientSend.UDPTestReceived();
    }
}
