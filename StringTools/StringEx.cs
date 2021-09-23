using System;

namespace StringTools
{
    public static class StringEx
    {
        /// <summary>
        /// Возвращает подсторку между превыми вхождениями начальной и конечной строки
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startString">Начальная строка</param>
        /// <param name="endString">Конечная строка</param>
        /// <returns>Строка между startString и endString</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string Substring(this string str, string startString, string endString)
        {
            string buffer = str;
            if (str.Contains(startString))
            {
                buffer = buffer.Split(startString)[1];
                if (buffer.Contains(endString))
                {
                    return buffer.Split(endString)[0];
                }
                else
                {
                    throw new ArgumentException("startString not found");
                }
            }
            else
            {
                throw new ArgumentException("endString not found or before startString");
            }

        }
    }
}
