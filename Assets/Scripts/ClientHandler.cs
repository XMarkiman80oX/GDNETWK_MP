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

    public static void TCPPromptChoicesReceived(Packet _packet)
    {
        
        if (!GameManager.Instance.isGameStarted)
            GameManager.Instance.StartGame();
        string _choice1 = _packet.ReadString();
        string _choice2 = _packet.ReadString();
        string _choice3 = _packet.ReadString();

        Debug.Log($"Prompt Choices received {_choice1}, {_choice2}, {_choice3}");
        UIManager.Instance.DisplayChoices(_choice1.ToString(), _choice2.ToString(), _choice3.ToString());
        UIManager.Instance.ReplyDisplayUI.SetActive(false);
        UIManager.Instance.ReplyInputUI.SetActive(false);
        UIManager.Instance.userPrompt.SetActive(false);

        UIManager.Instance.SetRiddleText("");
        UIManager.Instance.ClearUserReplyUIList();
        
    }


    public static void TCPRiddleReceived(Packet _packet)
    {

        
        string _prompt = _packet.ReadString();

        Debug.Log($"The riddle is this: {_prompt}");

        UIManager.Instance.ReplyInputUI.SetActive(true);
        UIManager.Instance.ReplyDisplayUI.SetActive(false);
        UIManager.Instance.userPrompt.SetActive(true);

        GameManager.Instance.SetPrompt(_prompt);
        GameManager.Instance.AssignNewRiddle();
        UIManager.Instance.RandomizeProfileUIColor();
        UIManager.Instance.RefreshPlayerScores();
        UIManager.Instance.ResetAnswerAttemptFieldText();
        UIManager.Instance.submitBtn.interactable = true;
        UIManager.Instance.HideChoices();
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

    //public static void TCPHostPromptRelayReceived(Packet _packet)
    //{
    //    string _hostPrompt = _packet.ReadString();

    //    GameManager.Instance.SetPrompt(_hostPrompt);
    //}

    public static void TCPPromptReplyRelayReceived(Packet _packet)
    {
        int _replySenderId = _packet.ReadInt();
        string _promptReply = _packet.ReadString();
        //UIManager.Instance.UpdateAttemptLog(_replySenderId.ToString(), _promptReply, false);

        if(GameManager.Instance.playerList.ContainsKey(_replySenderId))
        {
            string username = GameManager.Instance.playerList[_replySenderId].username;
            UIManager.Instance.AddUserReplyUI(_replySenderId, username, _promptReply);
        }
        
    }

    public static void TCPAllPlayersRepliedReceived(Packet _packet)
    {
        string _msg = _packet.ReadString();
        Debug.Log(_msg);

        UIManager.Instance.ReplyInputUI.SetActive(false);
        UIManager.Instance.ReplyDisplayUI.SetActive(true);

        UIManager.Instance.EnableAllLikeButton();
    }

    public static void TCPVoteForReplyRelayReceived(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _votes = _packet.ReadInt();

        UIManager.Instance.SetVoteForReplyUI(_id, _votes);
    }

    public static void TCPHighestVotesReceived(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _votes = _packet.ReadInt();

        Debug.Log(_id);

        GameManager.Instance.FinishRound(_id, _votes);
        
    }

    public static void TCPTimerReceived(Packet _packet)
    {
        float _currentTimerValue = _packet.ReadFloat();


        Debug.Log(_currentTimerValue);
        UIManager.Instance.SetTimer((int)_currentTimerValue);
    }

}
