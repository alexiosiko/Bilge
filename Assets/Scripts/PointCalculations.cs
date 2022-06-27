using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCalculations
{
    public static int CalculatePoint(int count)
    {
        if (count == 1)
            return 0;
        if (count == 2)
            return 0;
        if (count == 3)
            return 2;
        if (count == 4)
            return 2;
        if (count == 5)
            return 2;
        if (count == 6)
            return 4;
        if (count == 7)
            return 4;
        if (count == 8)
            return 6 * 2 * 2;
        if (count == 9)
            return 6 * 2 * 2;
        if (count == 10)
            return 6 * 2 * 2;
        if (count == 11)
            return 7 * 2 * 2;
        if (count == 12)
            return 7 * 4 * 2;
        if (count == 13)
            return 7 * 4 * 2;
        if (count == 14)
            return 8 * 5 * 4;
        if (count == 15)
            return 8 * 5 * 4;
        if (count == 16)
            return 10 * 8 * 4;
        if (count == 17)
            return 10 * 8 * 4;
        if (count == 18)
            return 12 * 8 * 4;
        else
            return 14 * 10;
        
    }
}
