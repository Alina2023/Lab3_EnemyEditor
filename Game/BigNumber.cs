using System;
using System.Collections.Generic;
using System.Text;
using Lab2_EnemyEditor;


namespace Lab2_EnemyEditor.Game
{
    public class BigNumber : IComparable<BigNumber>
    {
        private const int BASE = 1000; // каждая "ячейка" хранит 0..999
        private List<int> digits = new List<int>(); // младший разряд в нулевом индексе

        public BigNumber() { digits.Add(0); }

        public BigNumber(long value)
        {
            if (value < 0) throw new ArgumentException("Only non-negative supported");
            if (value == 0) { digits.Add(0); return; }
            while (value > 0)
            {
                digits.Add((int)(value % BASE));
                value /= BASE;
            }
        }

        public static BigNumber FromInt(int v) => new BigNumber(v);

        public override string ToString()
        {
            // вывод как обычное число: старший разряд без ведущих нулей, остальные по 3 цифры
            var sb = new StringBuilder();
            for (int i = digits.Count - 1; i >= 0; i--)
            {
                if (i == digits.Count - 1) sb.Append(digits[i].ToString());
                else sb.Append(digits[i].ToString("D3"));
            }
            return sb.ToString();
        }

        private void Normalize()
        {
            int carry = 0;
            for (int i = 0; i < digits.Count || carry != 0; i++)
            {
                if (i == digits.Count) digits.Add(0);
                long cur = (long)digits[i] + carry;
                carry = 0;
                if (cur < 0)
                {
                    // borrow
                    long need = (-cur + (BASE - 1)) / BASE;
                    cur += need * BASE;
                    carry = (int)-need;
                }
                digits[i] = (int)(cur % BASE);
                carry = (int)(cur / BASE);
            }
            // trim leading zeros
            while (digits.Count > 1 && digits[digits.Count - 1] == 0) digits.RemoveAt(digits.Count - 1);
        }

        public BigNumber Add(BigNumber other)
        {
            var res = new BigNumber();
            res.digits = new List<int>();
            int max = Math.Max(digits.Count, other.digits.Count);
            int carry = 0;
            for (int i = 0; i < max || carry != 0; i++)
            {
                int a = i < digits.Count ? digits[i] : 0;
                int b = i < other.digits.Count ? other.digits[i] : 0;
                int sum = a + b + carry;
                res.digits.Add(sum % BASE);
                carry = sum / BASE;
            }
            return res;
        }

        public BigNumber Subtract(BigNumber other)
        {
            // assumes this >= other
            if (this.CompareTo(other) < 0) throw new InvalidOperationException("Result would be negative");
            var res = new BigNumber();
            res.digits = new List<int>(digits);
            int borrow = 0;
            for (int i = 0; i < res.digits.Count; i++)
            {
                int b = i < other.digits.Count ? other.digits[i] : 0;
                int cur = res.digits[i] - b - borrow;
                if (cur < 0) { cur += BASE; borrow = 1; } else borrow = 0;
                res.digits[i] = cur;
            }
            res.Normalize();
            return res;
        }

        public BigNumber MultiplyByInt(int m)
        {
            if (m < 0) throw new ArgumentException("Only non-negative multiplier supported");
            var res = new BigNumber();
            res.digits = new List<int>();
            long carry = 0;
            for (int i = 0; i < digits.Count || carry != 0; i++)
            {
                long a = i < digits.Count ? digits[i] : 0;
                long prod = a * (long)m + carry;
                res.digits.Add((int)(prod % BASE));
                carry = prod / BASE;
            }
            res.Normalize();
            return res;
        }

        public int CompareTo(BigNumber other)
        {
            if (digits.Count != other.digits.Count) return digits.Count.CompareTo(other.digits.Count);
            for (int i = digits.Count - 1; i >= 0; i--)
            {
                if (digits[i] != other.digits[i]) return digits[i].CompareTo(other.digits[i]);
            }
            return 0;
        }

        public bool IsZero() => digits.Count == 1 && digits[0] == 0;
    }
}
