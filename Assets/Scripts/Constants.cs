using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static int[] ExpTable = { 0, 280, 660, 1140, 1720, 2400, 3180, 4060, 5040, 6120, 7300, 8580, 9960, 11440, 13020, 14700, 16480, 18360 };
    public static int MaxLevel = 18;
    public static float[] GarenEDamageByLevelTable = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 8.25f, 8.5f, 8.75f, 9, 9.25f, 9.5f, 9.75f, 10f, 10.25f };
    public static float[] AatroxPassiveCooldownByLevelTable = { 24, 23.29f, 22.59f, 21.88f, 21.18f, 20.47f, 19.76f, 19.06f, 18.35f, 17.65f, 16.94f, 16.24f, 15.53f, 14.82f, 14.12f, 13.41f, 12.71f, 12f};
    public static float[] TibbersEnragedAttackSpeeds = { 0.625f, 0.739f, 1.043f, 1.307f, 1.536f, 1.736f };
    public static float GetAnnieStunDurationByLevel(int level)
    {
        if (level < 6) return 1.25f;
        if (level < 11) return 1.5f;
        return 1.75f;
    }
}