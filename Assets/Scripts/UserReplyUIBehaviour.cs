using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserReplyUIBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    public Button likeBtn;
    public Text nameTxt;
    public Text replyTxt;
    public Text votesTxt;
    public Image bgImage;
    public int id;
    public string username;
    public string reply;
    public int votes;

    void Start()
    {
        nameTxt.text = username;
        replyTxt.text = reply;
        likeBtn.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetName(string _name)
    {
        username = _name;
    }

    public void SetReply(string _reply)
    {
        reply = _reply;
    }

    public void SetUserID(int _id)
    {
        id = _id;
    }

    public void EnableLikeButton()
    {

        StartCoroutine(EnableLikeButtonCoroutine());

    }

    public IEnumerator EnableLikeButtonCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        likeBtn.interactable = true;
        Debug.Log(id + "Like btn enabled");
    }
    public void DisableLikeBtn()
    {
        likeBtn.image.color = Color.grey;
        likeBtn.interactable = false;
        Debug.Log(id + " Like btn disabled");
    }

    public void Like()
    {
        likeBtn.image.color = Color.blue;
        likeBtn.interactable = false;

        GameManager.Instance.Like(id);
    }

    public void SetVotes(int _votes)
    {
        votes = _votes;
        votesTxt.text = votes.ToString();
        
    }

    public void ChanegBGColorToGreen()
    {
        bgImage.color = new Color(0.3f, 1.0f, 0.3f);
    }
}
