using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Sprite makeSprite(byte[] bytes)
    {
        Sprite sprite = null; //TODO мб
        //Texture2D tex = new Texture2D(Server.keyboard_x,Server.keyboard_y);
        //tex.LoadImage(bytes);
        //sprite = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0,0));
        return sprite;
    }
}
