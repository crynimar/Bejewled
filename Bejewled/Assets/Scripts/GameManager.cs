using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        //Create Singleton instance
        if (Instance == null || Instance != this)
            Instance = this; 
    }
}
