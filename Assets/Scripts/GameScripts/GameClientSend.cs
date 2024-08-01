using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        GameClient.Instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        GameClient.Instance.udp.SendData(_packet);
    }

    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)GameClientPackets.welcomeReceived))
        {
            _packet.Write(GameClient.Instance.myId);
            _packet.Write(GameUIManager.Instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void UDPTestReceived()
    {
        using (Packet _packet = new Packet((int)GameClientPackets.udpTestReceived))
        {
            _packet.Write("Received a UDP packet");

            SendUDPData(_packet);
        }
    }

    public static void TCPPlayerReadyStatusChangeSend()
    {
        GameClient.Instance.isReady = !GameClient.Instance.isReady;
        bool _isReady = GameClient.Instance.isReady;
        using (Packet _packet = new Packet((int)GameClientPackets.playerReadysend))
        {
            _packet.Write(_isReady);

            SendTCPData(_packet);
        }
    }
    public static void TCPPlayerPressedPlay()
    {
        GameClient.Instance.clickedPlay = true;
        bool clickedPlay = GameClient.Instance.clickedPlay;

        using (Packet _packet = new Packet((int)GameClientPackets.PromptPlayButton))
        {
            _packet.Write(clickedPlay);

            SendTCPData(_packet);
        }

    }
    public static void TCPPromptSelectSend(int _choice)
    {
        using (Packet _packet = new Packet((int)GameClientPackets.PromptSelectSend))
        {
            _packet.Write(_choice);

            SendTCPData(_packet);
        }
    }
    public static void TCPFinishedRoundSend()
    {
        bool _finishedROund = true;
        using (Packet _packet = new Packet((int)GameClientPackets.FinishedRoundSend))
        {
            _packet.Write(_finishedROund);
            SendTCPData(_packet);
        }
    }

    public static void TCPAsnwerAttempt(string _answerGuess, bool _isAnswerCorrect)
    {
        using (Packet _packet = new Packet((int)GameClientPackets.AnswerAttemptSend))
        {
            _packet.Write(_answerGuess);
            _packet.Write(_isAnswerCorrect);
            SendTCPData(_packet);
        }
    }

    public static void TCPRequestPlayerList()
    {
        string _msg = "Requesting player list";
        using (Packet _packet = new Packet((int)GameClientPackets.PlayerListRequested))
        {
            _packet.Write(_msg);
            SendTCPData(_packet);
        }
    }

    public static void TCPChatMessageSend(string _msg)
    {
        using (Packet _packet = new Packet((int)GameClientPackets.ChatMessageSend))
        {
            _packet.Write(_msg);
            SendTCPData(_packet);
        }
    }

    //public static void TCPHostSetPromptSend(string _prompt)
    //{
    //    using (Packet _packet = new Packet((int)GameClientPackets.HostSetPromptSend))
    //    {
    //        _packet.Write(_prompt);
    //        SendTCPData(_packet);
    //    }
    //}

    public static void TCPPromptReplySend(string _reply)
    {
        using (Packet _packet = new Packet((int)GameClientPackets.PromptReplySend))
        {
            _packet.Write(_reply);
            SendTCPData(_packet);
        }
    }

    public static void TCPSendVoteForReply(int idReceiver)
    {
        using (Packet _packet = new Packet((int)GameClientPackets.VoteForReplySend))
        {
            _packet.Write(idReceiver);

            SendTCPData(_packet);
        }
    }
}
