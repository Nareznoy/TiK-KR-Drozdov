using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRC
{
    static class CRC
    {
        private static long _poly; // Генераторный полином
        private static readonly int _polyLength = (int)Math.Log(_poly, 2) + 1;

        private static int _inputCodeLength;

        public static long SetPoly
        {
            set
            {
                _poly = value;
            }
        }

        public static long CalculateCRC(long inputCode, bool isCheck = false)
        {
            // Добавление в конце принятого сообщения нулей
            if (!isCheck)
                inputCode <<= _polyLength - 1;

            _inputCodeLength = (int)Math.Log(inputCode, 2) + 1;
            //_poly <<= (_inputCodeLength - _polyLength); // Сдвиг полинома до старшего бита принятого сообщения

            while ((int)Math.Log(inputCode, 2) + 1 >= _polyLength)
            {
                long subCode = Convert.ToInt64(Convert.ToString(inputCode, 2).Substring(0, ((int)Math.Log(_poly, 2) + 1)), 2);
                subCode ^= _poly;
                inputCode = Convert.ToInt64(Convert.ToString(subCode, 2) + Convert.ToString(inputCode, 2).Substring((int)Math.Log(_poly, 2) + 1), 2);
            }
            // Если размер текущего остатка равен размеру полинома, то выполнить XOR еще один раз
            if ((int)Math.Log(inputCode, 2) + 1 == (int)Math.Log(_poly, 2) + 1)
            {
                inputCode ^= _poly;
            }

            return inputCode;
        }

        public static bool CheckMessage(long message, long crc)
        {
            message <<= _polyLength - 1; // Добавление в конец сообщения нулей
            long messagePlusCrcInt = message + crc; // Прибавление к сообщению CRC кода

            return CalculateCRC(messagePlusCrcInt, true) == 0; // Равен ли CRC код нулю
        }

        //public static long GetMessagePlusCRC(long message, long crc)
        //{
        //    message <<= _polyLength - 1; // Добавление в конец сообщения нулей
        //    long messagePlusCrc = message + crc; // Прибавление к сообщению CRC кода

        //    return messagePlusCrc;
        //}
    }

}
