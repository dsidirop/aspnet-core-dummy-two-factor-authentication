namespace TwoFactorAuth.Common
{
    using System.Linq;
    using System.Text;

    static public class UStrings
    {
        static public string Asciify(this string input)
        {
            return string.Join(
                separator: " ",
                values: Encoding.Unicode.GetBytes(input).Select(x => x.ToString())
            );
        }
    }
}
