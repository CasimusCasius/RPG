
using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class BrazierCurve 
{
    public static long BinomCoefficient(long n, long k)
    {
        if (k > n) { return 0; }
        if (n == k) { return 1; } // only one way to chose when n == k
        if (k > n - k) { k = n - k; } // Everything is symmetric around n-k, so it is quicker to iterate over a smaller k than a larger one.
        long c = 1;
        for (long i = 1; i <= k; i++)
        {
            c *= n--;
            c /= i;
        }
        return c;
    }



    public static Vector3 brazierCurvePoint(Vector3 startPoint, Vector3 endPoint,  float t)
    {
        




        return new Vector3();
    }
}
