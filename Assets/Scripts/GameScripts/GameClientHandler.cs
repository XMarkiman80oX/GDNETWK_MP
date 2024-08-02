using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class GameClientHandler : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Recieved packet via TCP from server. Contains message: {_msg}. Your id is {_myId}");

        GameClient.Instance.myId = _myId;
        GameClientSend.WelcomeReceived();

        GameUIManager.Instance.setupProfile(GameUIManager.Instance.usernameField.text);
        GameClient.Instance.isConnected = true;
        GameUIManager.Instance.SetConnectedText(true);
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
    public static void TCPPressedPlayReceivedConfirm(Packet packet)
    {
        string _msg = packet.ReadString();
        bool _hasAllPressedPlay = packet.ReadBool();

        Debug.Log($"Message from server: {_msg}. Have all pressed play: {_hasAllPressedPlay}");

        Debug.Log("PLAY BUTTON CLICKED " + _msg);
        if (!RPSGameManager.Instance.IsGameStarted && RPSGameManager.Instance.IsGameLoaded)
        {
            Debug.Log("PLAY BUTTON CLICKED 2");
            RPSGameManager.Instance.OnGameStart();
        }
    }

    public static void TCPPromptChoicesReceived(Packet _packet)
    {
        string player1Username = _packet.ReadString();
        string player2Username = _packet.ReadString();

        //Debug.Log("Inside TCPPromptChoicesReceived-> player2Username:" + player2Username);
        if (!RPSGameManager.Instance.IsGameLoaded)
        {
            RPSGameManager.Instance.OnGameLoad(player1Username, player2Username);
        }
        //string _choice1 = _packet.ReadString();
        //string _choice2 = _packet.ReadString();
        //string _choice3 = _packet.ReadString();

        //Debug.Log($"Prompt Choices received {_choice1}, {_choice2}, {_choice3}");
        ////GameUIManager.Instance.DisplayChoices(_choice1.ToString(), _choice2.ToString(), _choice3.ToString());
        //GameUIManager.Instance.ReplyDisplayUI.SetActive(false);
        //GameUIManager.Instance.ReplyInputUI.SetActive(false);
        //GameUIManager.Instance.userPrompt.SetActive(false);

        ////GameUIManager.Instance.SetRiddleText("");
        //GameUIManager.Instance.ClearUserReplyUIList();

    }

    public static void TCPChoiceReceived(Packet _packet)
    {
        int _prompt = _packet.ReadInt();
        int _playerIndex = _packet.ReadInt();

        EChoice choice = (EChoice)_prompt;

        Debug.Log("CHOSEN: " + choice);
        //TODO SHU TOO: Set the game objects here:
        /*
         
        */
        GameUIManager.Instance.SendChoice(_playerIndex, choice);
        
        //Debug.Log($"The riddle is this: {_prompt}");

        //UIManager.Instance.ReplyInputUI.SetActive(true);
        //UIManager.Instance.ReplyDisplayUI.SetActive(false);
        //UIManager.Instance.userPrompt.SetActive(true);

        //GameManager.Instance.SetPrompt(_prompt);
        //GameManager.Instance.AssignNewRiddle();
        //UIManager.Instance.RandomizeProfileUIColor();
        //UIManager.Instance.RefreshPlayerScores();
        //UIManager.Instance.ResetAnswerAttemptFieldText();
        //UIManager.Instance.submitBtn.interactable = true;
        //UIManager.Instance.HideChoices();
    }

    public static void TCPPlayerReceived(Packet _packet)
    {
        int _playerId = _packet.ReadInt();
        string _username = _packet.ReadString();
        int _points = _packet.ReadInt();

        if (!(GameManager.Instance.playerList.ContainsKey(_playerId)))
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
        if (GameManager.Instance.isGameStarted)
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

        if (GameManager.Instance.isGameStarted)
        {
            GameManager.Instance.playerList.Remove(_disconnectedPlayer);
            UIManager.Instance.RemoveUserUI(_disconnectedPlayer);
        }
    }

    public static void TCPPromptReplyRelayReceived(Packet _packet)
    {
        int _replySenderId = _packet.ReadInt();
        string _promptReply = _packet.ReadString();
        //UIManager.Instance.UpdateAttemptLog(_replySenderId.ToString(), _promptReply, false);

        if (GameManager.Instance.playerList.ContainsKey(_replySenderId))
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
        GameUIManager.Instance.SetTimer((int)_currentTimerValue);
    }

}
