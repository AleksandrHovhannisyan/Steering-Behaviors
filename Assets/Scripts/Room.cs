using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool walls = true;

    // Start is called before the first frame update
    void Awake()
    {
        if(!walls)
        {
            foreach(Transform child in transform)
            {
                if(child.gameObject.CompareTag("Wall"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
