using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Player;
    public GameObject Enemy;

    private void Awake()
    {
        Instance = this;
    }

}
