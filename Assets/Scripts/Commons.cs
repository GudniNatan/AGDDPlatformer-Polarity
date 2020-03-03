using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Commons
{
    public static IEnumerator DelayedAction(UnityAction lambda, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        lambda.Invoke();
    }
    
    public static IEnumerator DelayedNextFrame(UnityAction lambda)
    {
        yield return new WaitForEndOfFrame();
        lambda.Invoke();
    }

    public static Color ColorFromHex(string hex) {
        float r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        float g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        float b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color(r / 255, g / 255, b / 255);
    }
}