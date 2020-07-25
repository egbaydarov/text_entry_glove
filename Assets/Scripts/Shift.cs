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

    public int i = 1;
    private Image im;
    void Start()
    {
        im = GetComponent<Image>();
        im.sprite = capital;
    }

    public void ToCapital()
    {
        i = 1;
        im.sprite = capital;
        Debug.Log("CAPITAl");
    }

    public void ToSmall()
    {
        i = 0;
        im.sprite = small;
        Debug.Log("small");

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


}