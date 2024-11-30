using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStealManager
{
    int currheavy { get; set; }
    List<int> heavy { get; set; }
    int maxbagsize { get; set; }
    bool? yesorno { get; set; }

    void getitem(int item);
    void hideitem(int item);
    void showitem(int item);
    void ShowWhyCantSteal(string why);
}