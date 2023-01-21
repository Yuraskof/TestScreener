namespace Screener.Utilities
{
    public static class StringUtils
    {
        public static decimal Trim(string volumeCoin)
        {
            if (volumeCoin.Contains("K"))
            {
                string replace = volumeCoin.Replace("-", "").Replace("K", "").Replace("₮", "");
                return Convert.ToDecimal(replace) * 1000;
            }
            else
            {
                string replace = volumeCoin.Replace("-", "").Replace("₮", "");
                return Convert.ToDecimal(replace);
            }
        }
    }
}
