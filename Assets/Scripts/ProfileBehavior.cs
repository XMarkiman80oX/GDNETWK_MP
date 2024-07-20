using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileBehavior : MonoBehaviour
{
    private Vector4[] colors;
    public Image profileImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeColor()
    {
        colors = new Vector4[5] { new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 0.7f, 0.7f, 1.0f), new Vector4(1.0f, 1.0f, 0.6f, 1.0f), new Vector4(0.6f, 1.0f, 0.7f, 1.0f), new Vector4(0.5f, 0.8f, 0.9f, 1.0f) };
        int colorIndex = Random.Range(0, 4);
        profileImage.color = colors[colorIndex];
    }
}
