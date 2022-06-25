using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUIBehaviour : MonoBehaviour
{
    private string usernameString;
    private int id = 0;

    private Text usernameText;
    private Image[] ticks = new Image[5];
    private int tickIndex = 0;
    [SerializeField] private Sprite correctMark;
    [SerializeField] private Sprite wrongMark;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("user ui start");
        usernameText = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        usernameText.text = usernameString;

        if(usernameText == null)
        {
            Debug.Log("text null");
        }
        for(int i = 0; i < 5; i++)
        {
            ticks[i] = gameObject.transform.GetChild(i + 1).gameObject.GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUsernametext(string _usernameText)
    {
        usernameString = _usernameText;
    }
    public void addCorrectMark()
    {
        Debug.Log("adding correct mark to user ui");
        ticks[tickIndex].sprite = correctMark;
        tickIndex++;
    }

    public void addWrongMark()
    {
        Debug.Log("adding wrong mark to user ui");
        ticks[tickIndex].sprite = wrongMark;
        tickIndex++;
    }

    public void ResetMarks()
    {
        for (int i = 0; i < 5; i++)
        {
            ticks[i].sprite = null;
            tickIndex = 0;
        }
    }

    public int GetUserID()
    {
        return id;
    }

    public void SetUserID(int _id)
    {
        id = _id;
    }
}
