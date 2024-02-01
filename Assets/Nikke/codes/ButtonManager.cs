using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    public Button button1, button2, button3;

    public Button burstButton;
    void Start()
    {
        button1.onClick.AddListener(() => gameManager.RequestChangeNikke(1));
        button2.onClick.AddListener(() => gameManager.RequestChangeNikke(2));
        button3.onClick.AddListener(() => gameManager.RequestChangeNikke(3));
    }


}
