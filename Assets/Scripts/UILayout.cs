using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayout : MonoBehaviour
{
    [SerializeField]
    private GameObject keyboard;
    [SerializeField]
    private GameObject keys;
    [SerializeField]
    private GameObject spaceBar;
    [SerializeField]
    private GameObject predictionBar;

    private Server server;

    private GridLayoutGroup gridLayoutGroup;

    private RectTransform keyboardTransform;
    private RectTransform keysTransform;
    private RectTransform spaceBarTransform;
    private RectTransform predictionBarTransform;

    private int keyboard_x, keyboard_y;
    // Start is called before the first frame update
    void Start()
    {
        server = keyboard.GetComponent<Server>();
        gridLayoutGroup = keys.GetComponent<GridLayoutGroup>();
        keyboardTransform = keyboard.GetComponent<RectTransform>();
        keysTransform = keys.GetComponent<RectTransform>();
        spaceBarTransform = spaceBar.GetComponent<RectTransform>();
        predictionBarTransform = predictionBar.GetComponent<RectTransform>();
        //keyboard_x = 1080;
        //keyboard_y = 401;
        //Server.isSizeSet = true;


    }

    // Update is called once per frame
    void Update()
    {
        if (Server.isSizeSet)
        {
            keyboard_x = Server.keyboard_x;
            keyboard_y = Server.keyboard_y;
            ChangeKeyboard();
            ChangeKeys();
            ChangePredictionBar();
            ChangeSpaceBar();
        }
    }
    void ChangeKeyboard()
    {
        keyboardTransform.sizeDelta = new Vector2(keyboard_x,keyboard_y);
        
    }
   // Меняю элементы GridLayoutGroup
   // Меняю размер панели keys
    void ChangeKeys()
    {
        keysTransform.sizeDelta = new Vector2(keyboard_x,(float)(0.62625*keyboard_y)); 
        keysTransform.anchoredPosition = new Vector2();
        gridLayoutGroup.cellSize = new Vector2((keyboard_x-120)/11,(float) ((0.835*keyboard_y-45)/4)); 
        
    }
    
    void ChangePredictionBar()
    {
        predictionBarTransform.sizeDelta = new Vector2(keyboard_x,(float) (0.165*keyboard_y)); 
        // TODO: change size of three buttons 
    }
    
    void ChangeSpaceBar()
    {
        spaceBarTransform.sizeDelta = new Vector2(keyboard_x,(float) (0.20875*keyboard_y)); 
        // space button 
    }
}
