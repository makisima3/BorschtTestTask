using System;

namespace Code.Player.Utils
{
    public static class EnumConvertor
    {
        public static bool TryGetValue<T1, T2>(T1 value1, out T2 result)
        where T1 : Enum
        where T2 : Enum
        {
            var value1String = value1.ToString();
            result = (T2)(Enum.GetValues(typeof(T2)).GetValue(0));
            foreach (T2 value in Enum.GetValues(typeof(T2)))
            {
                if (value1String == value.ToString())
                {
                    result = value;
                    return true;
                }
            }

            return false;
        }
    }
}