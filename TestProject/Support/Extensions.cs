using System;

namespace BFF.Tests.Support;

public static class Extensions
{
    public static void Times(this int count, Action action)
    {
        for (var i = 0; i < count; i++)
        {
            action();
        }
    }
}