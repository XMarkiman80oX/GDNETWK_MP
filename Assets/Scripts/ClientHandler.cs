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

        Debug.Log($"Recieved packet via TCP from server. Contains message: {_msg}. You id is {_myId}");

        Client.Instance.myId = _myId;
        ClientSend.WelcomeReceived();

        UIManager.Instance.setupProfile(UIManager.Instance.usernameField.text);
        Client.Instance.isConnected = true;
        UIManager.Instance.SetConnectedText(true);
    }

    public static void UDPTest(Packet _packet)
    {
        string _msg = _packet.ReadString();

        Debug.Log($"Recieved packet via UDP from server. Contains message: {_msg}");
        ClientSend.UDPTestReceived();
    }

    public static void TCPPlayerReadyReceivedConfirmReceived(Packet _packet)
    {
        string _msg = _packet.ReadString();
        bool _isAllPlayerReady = _packet.ReadBool();

        Debug.Log($"Message from server: {_msg}. Are all players ready: {_isAllPlayerReady}");

    }

    public static void TCPRiddleReceived(Packet _packet)
    {
        if (!GameManager.Instance.isGameStarted)
            GameManager.Instance.StartGame();

        string _riddle = _packet.ReadString();
        string _answer = _packet.ReadString();

        Debug.Log($"The riddle is this: {_riddle}");
        Debug.Log($"The answer is this: {_answer}");

        GameManager.Instance.SetRiddleQuestion(_riddle);
        GameManager.Instance.SetRiddleAnswer(_answer);
        GameManager.Instance.AssignNewRiddle();
        UIManager.Instance.RefreshPlayerScores();
    }

    public static void TCPPlayerReceived(Packet _packet)
    {
        int _playerId = _packet.ReadInt();
        string _username = _packet.ReadString();
        int _points = _packet.ReadInt();

        if(!(GameManager.Instance.playerList.ContainsKey(_playerId)))
        {
            Debug.Log($"adding player with id {_playerId} and username {_username} to player list");
            GameManager.Instance.AddPlayerToList(_playerId, _username, _points);
        }
        
    }

    public static void TCPAnswerAttemptReceivedConfirmed(Packet _packet)
    {
        int _player = _packet.ReadInt();
        string _answerGuess = _packet.ReadString();
        bool _isAnswerCorrect = _packet.ReadBool();
        if(GameManager.Instance.isGameStarted)
        {
            if (_isAnswerCorrect)
            {
                GameManager.Instance.OnPlayerAttemptRight(_player);
            }

            else
            {
                GameManager.Instance.OnPlayerAttemptWrong(_player, _answerGuess);
            }
        }
        
    }

    public static void TCPChatMessageForwardReceived(Packet _packet)
    {
        string _chatMessage = _packet.ReadString();
        int _playerSender = _packet.ReadInt();

        Debug.Log(GameManager.Instance.playerList[_playerSender].username + "sent a chat message: " + _chatMessage);
        UIManager.Instance.ReceiveChatMessage(GameManager.Instance.playerList[_playerSender].username, _chatMessage);
    }

    public static void TCPPlayerDisconnectReceived(Packet _packet)
    {
        int _disconnectedPlayer = _packet.ReadInt();

        if(GameManager.Instance.isGameStarted)
        {
            GameManager.Instance.playerList.Remove(_disconnectedPlayer);
            UIManager.Instance.RemoveUserUI(_disconnectedPlayer);
        }
    }
}
