using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.Instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.Instance.udp.SendData(_packet);
    }

    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.Instance.myId);
            _packet.Write(UIManager.Instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void UDPTestReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.udpTestReceived))
        {
            _packet.Write("Received a UDP packet");

            SendUDPData(_packet);
        }
    }

    public static void TCPPlayerReadyStatusChangeSend()
    {
        Client.Instance.isReady = !Client.Instance.isReady;
        bool _isReady = Client.Instance.isReady;
        using (Packet _packet = new Packet((int)ClientPackets.playerReadysend))
        {

            _packet.Write(_isReady);


            SendTCPData(_packet);
        }
    }

    public static void TCPPromptSelectSend(int _choice)
    {
        using (Packet _packet = new Packet((int)ClientPackets.PromptSelectSend))
        {

            _packet.Write(_choice);


            SendTCPData(_packet);
        }
    }
    public static void TCPFinishedRoundSend()
    {
        bool _finishedROund = true;
        using (Packet _packet = new Packet((int)ClientPackets.FinishedRoundSend))
        {
            _packet.Write(_finishedROund);
            SendTCPData(_packet);
        }
    }

    public static void TCPAsnwerAttempt(string _answerGuess, bool _isAnswerCorrect)
    {
        using (Packet _packet = new Packet((int)ClientPackets.AnswerAttemptSend))
        {
            _packet.Write(_answerGuess);
            _packet.Write(_isAnswerCorrect);
            SendTCPData(_packet);
        }
    }

    public static void TCPRequestPlayerList()
    {
        string _msg = "Requesting player list";
        using (Packet _packet = new Packet((int)ClientPackets.PlayerListRequested))
        {
            _packet.Write(_msg);
            SendTCPData(_packet);
        }
    }

    public static void TCPChatMessageSend(string _msg)
    {
        using(Packet _packet = new Packet((int)ClientPackets.ChatMessageSend))
        {
            _packet.Write(_msg);
            SendTCPData(_packet);
        }
    }

    //public static void TCPHostSetPromptSend(string _prompt)
    //{
    //    using (Packet _packet = new Packet((int)ClientPackets.HostSetPromptSend))
    //    {
    //        _packet.Write(_prompt);
    //        SendTCPData(_packet);
    //    }
    //}

    public static void TCPPromptReplySend(string _reply)
    {
        using (Packet _packet = new Packet((int)ClientPackets.PromptReplySend))
        {
            _packet.Write(_reply);
            SendTCPData(_packet);
        }
    }

    public static void TCPSendVoteForReply(int idReceiver)
    {
        using (Packet _packet = new Packet((int)ClientPackets.VoteForReplySend))
        {
            
            _packet.Write(idReceiver);
            SendTCPData(_packet);
        }
    }
}
