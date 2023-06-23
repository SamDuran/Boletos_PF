namespace BoletosAPI.Utilities
{
    public static class Utils
    {
        public static string ToSqlString(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return "\'\'";
            
            var result = "\'";
            foreach(var caracter in input)
            {
                if(caracter != '\'')
                    result += caracter;
            }
            result += "\'";
            return result;
        }

        public static string ToSqlLikeString(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return "\'\'";

            var result = "\'%";
            foreach (var caracter in input)
            {
                if (caracter != '%' || caracter != '\'')
                    result += caracter;
            }
            result += "%'";
            return result;
        }

        public static string ToSqlIntList(this List<int> intList)
        {
            if (intList is null ||  intList.Count == 0) return 0.ToString();

            var result = "(";

            foreach( var caracter in intList)
            {
                if (caracter != 0)
                {
                    result += caracter +", ";
                }
            }

            result += ")";
            result.Replace(", )", ")");
            return result;
        }
    }
}
