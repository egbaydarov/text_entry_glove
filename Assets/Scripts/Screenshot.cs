using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    [SerializeField]
    Vector2 upperLeftSource = new Vector2(0, 0);

    [SerializeField]
    Vector2 screenSize = new Vector2(1920, 1080);

    [SerializeField]
    Vector2 upperLeftDestination = new Vector2(0, 0);

    [SerializeField]
    UnityEngine.UI.Image img;
    byte[] bytes;


    volatile bool isWaiting = false;

    volatile bool updateTexture = false;

    Texture2D tex;

    void Start()
    {
        tex = new Texture2D((int)screenSize.x, (int)screenSize.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting)
            StartCoroutine(updateImage(0.03f));
        if (updateTexture)
        {
            tex.LoadImage(bytes);
            try
            {
                img.sprite = Sprite.Create(tex, new Rect(0, 0, 610, 480),
                    new Vector2(0, 0));
                updateTexture = false;
            }
            catch
            {
                Debug.Log($"Wrong SIZE. {screenSize.x}:{screenSize.y}");
            }
        }
    }

    IEnumerator updateImage(float delaySec)
    {
        isWaiting = true;

        yield return new WaitForSeconds(delaySec);
        new Thread(() =>
       {
           makeScreenshot(
               new Point((int)upperLeftSource.x, (int)upperLeftSource.y),
               new Point((int)upperLeftDestination.x, (int)upperLeftDestination.y),
               new Size((int)screenSize.x, (int)screenSize.y));

           isWaiting = false;
       }).Start();
    }

    public void makeScreenshot(Point upperLeftSource, Point upperLeftDestination, Size screenSize)
    {
        System.Drawing.Bitmap keyboardBitmap;
        using (keyboardBitmap = new Bitmap(780, 480)) // 780 480
        {
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(keyboardBitmap))
            {
                g.CopyFromScreen(upperLeftSource, upperLeftDestination, screenSize);
            }
            ImageConverter converter = new ImageConverter();
            bytes = (byte[])converter.ConvertTo(keyboardBitmap, typeof(byte[]));
            updateTexture = true;
        }
    }

    public void updateImageSynchronisly()
    {
        makeScreenshot(
               new Point((int)upperLeftSource.x, (int)upperLeftSource.y),
               new Point((int)upperLeftDestination.x, (int)upperLeftDestination.y),
               new Size((int)screenSize.x, (int)screenSize.y));

        tex.LoadImage(bytes);
        img.sprite = Sprite.Create(tex, new Rect(0, 0, 1080, 729), new Vector2(0, 0));
        updateTexture = false;
    }

}
