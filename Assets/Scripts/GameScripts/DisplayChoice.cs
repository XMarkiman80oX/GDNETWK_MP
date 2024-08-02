using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayChoice : MonoBehaviour
{
    [SerializeField]
    private Image Rock;
    [SerializeField]
    private Image Paper;
    [SerializeField]
    private Image Scissors;
    public void DisplayChoiceFunc(EChoice selectedMove)
    {
        switch (selectedMove)
        {
            case EChoice.ROCK:
                this.GetComponent<Image>().sprite = Rock.sprite;
                break;
            case EChoice.PAPER:
                this.GetComponent<Image>().sprite = Paper.sprite;
                break;
            case EChoice.SCISSORS:
                this.GetComponent<Image>().sprite = Scissors.sprite;
                break;
        }
    }
}
