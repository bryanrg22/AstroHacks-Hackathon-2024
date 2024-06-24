using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPTManagerManager : MonoBehaviour
{
    public static ChatGPTManager instance;

    void Awake()
    {
        instance = new ChatGPTManager();
    }
}
