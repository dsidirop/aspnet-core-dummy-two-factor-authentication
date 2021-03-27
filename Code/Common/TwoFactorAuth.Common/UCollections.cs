namespace TwoFactorAuth.Common
{
    using System.Collections;
    using System.Collections.Generic;

    static public class UCollections
    {
        static public IEnumerable Enumify(this object input)
        {
            yield return input;
        }

        static public IEnumerable<T> Enumify<T>(this T input)
        {
            yield return input;
        }
    }
}
