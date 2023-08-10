namespace MRA.Jobs.Infrastructure.Identity.Utils;

internal class PaswordHelper
{
    public static string RandomPassword(
        int length = 8,
        bool requireNonAlphanumeric = true,
        bool requireLowercase = true,
        bool requireUppercase = true,
        bool requireDigit = true)
    {
        string[] randomChars = new[]
        {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",
                "abcdefghijkmnopqrstuvwxyz",
                "0123456789",
                "!@$?_-&*"
            };

        Random rand = new Random(Environment.TickCount);
        List<char> chars = new List<char>();

        if (requireUppercase)
            chars.Insert(rand.Next(0, chars.Count), randomChars[0][rand.Next(0, randomChars[0].Length)]);

        if (requireLowercase)
            chars.Insert(rand.Next(0, chars.Count), randomChars[1][rand.Next(0, randomChars[1].Length)]);

        if (requireDigit)
            chars.Insert(rand.Next(0, chars.Count), randomChars[2][rand.Next(0, randomChars[2].Length)]);

        if (requireNonAlphanumeric)
            chars.Insert(rand.Next(0, chars.Count), randomChars[3][rand.Next(0, randomChars[3].Length)]);

        for (int i = chars.Count; i < length; i++)
        {
            string rcs = randomChars[rand.Next(0, randomChars.Length)];
            chars.Insert(rand.Next(0, chars.Count),
                rcs[rand.Next(0, rcs.Length)]);
        }

        return new string(chars.ToArray());
    }
}
