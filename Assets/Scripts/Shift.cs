using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net.Mime;
using UnityEngine.Serialization;

public class Shift : MonoBehaviour
{
    public Sprite small;
    public Sprite capital;
    public Sprite constCapital;


    public static Sprite sSmall;
    public static Sprite sCapital;
    public static Sprite sConstCapital;


    public static int i = 1;
    private static Image im;
    void Start()
    {
        sSmall = small;
        sCapital = capital;
        sConstCapital = constCapital;

        im = GetComponent<Image>();
        im.sprite = capital;
    }

    public static void ToCapital()
    {
        Debug.Log("a");
        im.sprite = sCapital;
        Debug.Log("A");
        i = 1;

        im.GraphicUpdateComplete();
    }

    public static void ToSmall()
    {
        Debug.Log("A");
        im.sprite = sSmall;
        Debug.Log("a");
        i = 0;
    }

    public void Swap()
    {
        if (i == 0)
        {
            im.sprite = capital;
            i++;
            return;
        }

        if (i == 1)
        {
            im.sprite = constCapital;
            i++;
            return;
        }

        if (i == 2)
        {
            im.sprite = small;
            i = 0;
            return;
        }
    }

    public static void SizeReset()
    {
        if (i == 1)
        {
            im.sprite = sSmall;
            i = 0;
        }
    }
}