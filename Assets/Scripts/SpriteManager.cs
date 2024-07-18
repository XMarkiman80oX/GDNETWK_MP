using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteManager : MonoBehaviour
{
    [System.Serializable]
    public class SpriteSheet
    {
        [SerializeField] public Sprite[] Img;
        [SerializeField] public string[] PossibleTitles;

        public bool Contains(string _title)
        {
            foreach(string _acceptTitle in PossibleTitles)
            {
                if (_title == _acceptTitle)
                    return true;
            }

            return false;
        }
    }

    private Dictionary<string, SpriteSheet> spriteSheetList;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Image currentSpriteDisplayed;
    [SerializeField] private SpriteSheet[] spriteSheetArr;
    private SpriteSheet currentSpriteSheet;
    void Start()
    {
        foreach (SpriteSheet _ss in spriteSheetArr)
        {
            foreach (string _title in _ss.PossibleTitles)
            {
                spriteSheetList.Add(_title, _ss);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SetSpriteAccordingToTitle(string _title)
    {
        if (spriteSheetList.ContainsKey(_title))
        {
            currentSpriteSheet = spriteSheetList[_title];
            currentSpriteDisplayed.sprite= currentSpriteSheet.Img[0];
            return true;
        }

        else
        {
            Debug.Log("No sprite sheet with title: " + _title);
            return false;
        }
        
    }
}
