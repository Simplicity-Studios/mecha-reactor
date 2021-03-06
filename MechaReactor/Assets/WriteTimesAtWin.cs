using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class WriteTimesAtWin : MonoBehaviour
{
    public Text bestTime, recentTime;
    void Start()
    {
        float min = float.PositiveInfinity;
        float last = 0;
        foreach (string line in File.ReadAllLines("savegame.txt"))
        {
            last = float.Parse(line);
            min = Mathf.Min(min, last);
        }

        recentTime.text = last.ToString("0.00");
        bestTime.text = min.ToString("0.00");
        print(bestTime.text.ToString());
    }
}
