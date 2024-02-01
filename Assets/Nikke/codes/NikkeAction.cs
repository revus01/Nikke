using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NikkeAction : MonoBehaviour
{
    [SerializeField]
    private GunSystem gun;

    private GameManager gameManager;

    bool isAutoFire, isFocused;
    public int nikkeNum;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

    }

    private void Update()
    {
        isAutoFire = gameManager.isAutoFire;

        isFocused = gameManager.focusedNikke == nikkeNum;


        if (isFocused && !isAutoFire)
        {
            gun.MyInput();
        }
        else
        {
            gun.AiInput(isFocused);
        }
    }

}
