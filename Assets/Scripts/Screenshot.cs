using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

    public static byte[] makeScreenshot(Point upperLeftSource, Point upperLeftDestination, Size screenSize)
    {
        Bitmap keyboardBitmap;

        using (keyboardBitmap = new Bitmap(screenSize.Width, screenSize.Height))
        {
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(keyboardBitmap))
            {
                g.CopyFromScreen(upperLeftSource, upperLeftDestination, screenSize);
            }
            ImageConverter converter = new ImageConverter();

            return (byte[])converter.ConvertTo(keyboardBitmap, typeof(byte[]));
        }
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
