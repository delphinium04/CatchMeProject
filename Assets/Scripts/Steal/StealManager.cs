using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class StealManager : MonoBehaviour
{
    public GameObject[] _people;
    public List<Tuple<int, int>> _products = new List<Tuple<int, int>>(); // item1 == weight, item2 == price

    private float time;
    public bool bag_item = false;

    void Awake()
    {
        
    }
 

    List<int> bag_Algorithm(List<int> heavy, List<int> price, int maxbagsize)
    {
        int n = heavy.Count;
        int[,] K = new int[n + 1, maxbagsize + 1]; 
        List<int> items = new List<int>(); 

        for (int i = 1; i <= n; i++) 
        {
            for (int w = 1; w <= maxbagsize; w++)
            {
                if (heavy[i - 1] <= w)
                {
                    
                    K[i, w] = Mathf.Max(K[i-1, w], K[i-1, w - heavy[i-1]] + price[i-1]);
                }
                else
                {
                    K[i, w] = K[i - 1, w];
                }
            }
        }

        int currw = maxbagsize;
        for (int i = n; i > 0; i--)
        {
            if (K[i, currw] != K[i-1, currw])
            {
                items.Add(i-1);
                currw -= heavy[i-1]; 
            }
        }
        return items; 
    }
}
