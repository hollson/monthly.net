//
//  Monthly.cs
//
//  Author:
//       sybs <hollson@live.cn>
//
//  Copyright (c) 2020 
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 与月份相关的对象，如账单、账期、月刊、月报等
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Auto)]
    public struct Monthly : IComparable<Monthly>, IEquatable<Monthly>
    {
        private int _year;
        private int _month;

        #region Property
        /// <summary>
        ///  获取当前实例的年
        /// </summary>
        public int Year => _year;

        /// <summary>
        ///  获取当前实例的月
        /// </summary>
        public int Month => _month;

        /// <summary>
        /// 获取当前实例的年月标记值，如2018年1月记为 : 201801
        /// </summary>
        public int Dot => this._year * 100 + this._month;

        /// <summary>
        /// 获取当前实例从公元零年一月开始的月份累计值
        /// </summary>
        public int Tickes => this._year * 12 + this._month;

        /// <summary>
        ///  获取当前实例所在的季度
        /// </summary>
        public int Quarter => (this._month - 1) / 3 + 1;
        #endregion

        #region Ctor
        /// <summary>
        ///  以指定的年和月初始化Monthly实例。
        /// </summary>
        /// <param name="year"> 年（0 到 9999）</param>
        /// <param name="month"> 月（1 到 12）</param>
        public Monthly(int year, int month)
        {
            CheckYear(year);
            CheckMonth(month);
            this._year = year;
            this._month = month;
        }

        /// <summary>
        /// 获取以当前时间点为依据的新实例
        /// </summary>
        public static Monthly Current => new Monthly() { _year = DateTime.Now.Year, _month = DateTime.Now.Month };

        /// <summary>
        /// 获取当前时间点的上月为依据的新实例
        /// </summary>
        public Monthly Previous => Monthly.fromTickes(this.Tickes - 1);

        /// <summary>
        /// 获取当前时间点的下月为依据的新实例
        /// </summary>
        public Monthly Next => Monthly.fromTickes(this.Tickes + 1);

        /// <summary>
        /// 获取当前年份的一月为依据的新实例  
        /// </summary>
        public Monthly First => new Monthly() { _year = this._year, _month = 1 };

        /// <summary>
        /// 获取当前年份的十二月为依据的新实例
        /// </summary>
        public Monthly Last => new Monthly() { _year = this._year, _month = 12 };

        /// <summary>
        /// 获取Monthly的最小值实例
        /// </summary>
        public static Monthly MinValue => new Monthly() { _year = 0, _month = 1 };

        /// <summary>
        /// 获取Monthly的最大值实例
        /// </summary>
        public static Monthly MaxValue => new Monthly() { _year = 9999, _month = 12 };
        #endregion

        #region Method
        private static int YearOfDot(int dot) => dot / 100;

        private static int MonthOfDot(int dot) => dot % 100;

        /// <summary>
        /// 获取当前实例的年月标记值，如2018年1月记为 : 201801
        /// </summary>
        /// <returns></returns>
        public int ToDot() => this.Dot;

        /// <summary>
        /// 以当前实例与years的和值为依据创建一个新实例
        /// </summary>
        public Monthly AddYears(int years) => FromTickes(Tickes + years * 12);

        /// <summary>
        /// 以当前实例与months的和值为依据创建一个新实例
        /// </summary>
        public Monthly AddMonths(int months) => FromTickes(Tickes + months);

        /// <summary>
        /// 判断当前实例的值与给定实例的值是否相等
        /// </summary>
        public bool Equals(Monthly other) => this.Tickes == other.Tickes;

        /// <summary>
        /// 获取当前实例与给定实例的月份差值
        /// </summary>
        public int SpanMonths(Monthly other) => this - other;

        /// <summary>
        /// 获取当前实例与DateTime实例的月份差值
        /// </summary>
        public int SpanMonths(DateTime date) => Tickes - date.Year * 12 - date.Month;

        /// <summary>
        /// 获取当前实例与给定实例的大小比较的结果标识
        /// </summary>
        /// <param name="other"></param>
        /// <returns>-1:小于other实例值 ； 0 等于other实例值 ； 1：大于other实例值</returns>
        public int CompareTo(Monthly other)
        {
            if (this.Tickes < other.Tickes) return -1;
            if (this.Tickes > other.Tickes) return 1;
            else return 0;
        }

        /// <summary>
        /// 以年月标记值创建一个Monthly新实例
        /// </summary>
        /// <param name="dot">格式：201801</param>
        /// <returns></returns>
        public static Monthly FromDot(int dot)
        {
            var year = YearOfDot(dot);
            var month = MonthOfDot(dot);
            if (year < 0 || year > 9999 || month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(dot), dot, "Please enter correct dot format such as \'201801\'.");

            return new Monthly
            {
                _year = YearOfDot(dot),
                _month = MonthOfDot(dot)
            };
        }

        private static Monthly fromTickes(int tickes)
        {
            return new Monthly
            {
                _year = (tickes - 1) / 12,
                _month = tickes % 12 == 0 ? 12 : tickes % 12
            };
        }

        /// <summary>
        /// 以年月累计值创建一个Monthly新实例
        /// </summary>
        /// <param name="tickes">以公元零年一月为起点的月份计数值（1-120000）</param>
        public static Monthly FromTickes(int tickes)
        {
            if (tickes < 1 || tickes > 120000)
                throw new ArgumentOutOfRangeException(nameof(tickes), tickes, "The tickes must beteen 1 and 120000 .");
            return fromTickes(tickes);
        }

        /// <summary>
        /// 以DateTime实例创建一个Monthly新实例
        /// </summary>
        public static Monthly FromDate(DateTime time) => new Monthly() { _year = time.Year, _month = time.Month };

        /// <summary>
        /// 以诸如"2018/01"格式的字符串创建一个Monthly新实例
        /// <param name="s">"2018/01"格式的字符串</param>
        /// </summary>
        public static Monthly FromString(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new Exception("The parameter cannot be null or empty.");

            var nums = Regex.Matches(s, "[0-9]+");
            if (nums.Count == 0)
                throw new Exception("Please give the correct parameters, such as '2018/01' .");

            if (nums.Count == 1)
                return new Monthly(0, Convert.ToInt32(nums[0].ToString().TrimStart('0')));
            return new Monthly(Convert.ToInt32(nums[0].ToString().TrimStart('0')), Convert.ToInt32(nums[1].ToString().TrimStart('0')));
        }

        /// <summary>
        /// 获取一段时间内的Monthly数轴(包含开始与结束月份)
        /// </summary>
        /// <param name="from">开始月份</param>
        /// <param name="to">结束月份</param>
        /// <returns></returns>
        public static List<Monthly> Axis(Monthly from, Monthly to)
        {
            var result = new List<Monthly>();
            var span = from - to;
            var len = (span ^ (span >> 31)) - (span >> 31) + 1;
            for (int i = 0; i < len; i++)
            {
                if (span > 0) result.Add(from - i);
                else result.Add(from + i);
            }
            return result;
        }

        /// <summary>
        /// 获取给定时间段内的Monthly集合(包含开始与结束月份)
        /// </summary>
        /// <param name="from">开始月份</param>
        /// <param name="to">结束月份</param>
        /// <returns></returns>
        public static List<Monthly> Axis(int from, int to)
        {
            return Axis(Monthly.FromDot(from), Monthly.FromDot(to));
        }

        /// <summary>
        /// 检查year的合法性
        /// </summary>
        private static void CheckYear(int year)
        {
            if (year < 0 || year > 9999)
                throw new ArgumentOutOfRangeException("year", year, "The year must beteen 0 and 9999 .");
        }

        /// <summary>
        /// 检查month的合法性
        /// </summary>
        private static void CheckMonth(int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), month, "The month must beteen 1 and 12 .");
        }
        #endregion

        #region Operator
        /// <summary>
        /// 以给定实例与months的和值创建一个新实例
        /// </summary>
        /// <param name="months">月分数</param>
        public static Monthly operator +(Monthly m, int months) => FromTickes(m.Tickes + months);

        /// <summary>
        /// 以给定实例与months的差值创建一个新实例
        /// </summary>
        /// <param name="months">月分数</param>
        public static Monthly operator -(Monthly m, int months) => FromTickes(m.Tickes - months);

        /// <summary>
        /// 获取当前实例与给定实例的月份差值
        /// </summary>
        public static int operator -(Monthly m1, Monthly m2) => m1.Tickes - m2.Tickes;

        /// <summary>
        ///获取当前实例与DateTime实例的月份差值
        /// </summary>
        public static int operator -(Monthly m, DateTime d) => m.SpanMonths(d);

        public static Monthly operator ++(Monthly m) => m + 1;

        public static Monthly operator --(Monthly m) => m - 1;

        /// <summary>
        ///判断m1是否等于m2
        /// </summary>
        public static bool operator ==(Monthly m1, Monthly m2) => m1.Tickes == m2.Tickes;

        /// <summary>
        /// 判断m1是否不等于m2
        /// </summary>
        public static bool operator !=(Monthly m1, Monthly m2) => m1.Tickes != m2.Tickes;

        /// <summary>
        /// 判断m1是否小于m2
        /// </summary>
        public static bool operator <(Monthly m1, Monthly m2) => m1.Tickes < m2.Tickes;

        /// <summary>
        /// 判断m1是否大于m2
        /// </summary>
        public static bool operator >(Monthly m1, Monthly m2)
        {
            return m1.Tickes > m2.Tickes; ;
        }

        /// <summary>
        /// 判断m1是否小于等于m2
        /// </summary>
        public static bool operator <=(Monthly m1, Monthly m2)
        {
            return m1.Tickes <= m2.Tickes; ;
        }

        /// <summary>
        /// 判断m1是否大于等于m2
        /// </summary>
        public static bool operator >=(Monthly m1, Monthly m2)
        {
            return m1.Tickes >= m2.Tickes; ;
        }

        /// <summary>
        /// 以年月标识的Monthly实例
        /// </summary>
        /// <param name="dot">格式：201801</param>
        public static implicit operator Monthly(int dot)
        {
            return Monthly.FromDot(dot);
        }
        #endregion

        #region Override
        /// <summary>
        /// 获取包含"Y、y、M、m"字符格式的自定义Monthly字符串
        /// </summary>
        /// <param name="format">
        /// 如：yyyy/mm ; yy/mm ; yyyy年mm月 ;YYYY-Mm...
        /// 不区分大小写
        /// </param>
        /// <returns></returns>
        public string ToString(string format = "yyyy/mm")
        {
            return Format(this, format);
        }

        /// <summary>
        /// 判断当前实例的值与给定实例的转换值是否相等
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is null) throw new ArgumentNullException("obj", "The parameter cannot be null.");
            if (obj is Monthly) return this == (Monthly)obj;
            if (obj is DateTime) return this == Monthly.FromDate((DateTime)obj);
            throw new ArgumentException("The parameter must be System.DateTime type or System.Monthly type .", "obj");
        }

        public override int GetHashCode()
        {
            Int64 ticks = Tickes;
            return unchecked((int)ticks) ^ (int)(ticks >> 32);
        }

        private static string Format(Monthly m, string format)
        {
            string _y = m.Year.ToString();
            string _m = m.Month.ToString();
            format = format.ToLower();
            if (!(format.Contains("yyyy") || format.Contains("yyyy")) && !(format.Contains("mm") || format.Contains("m")))
                throw new ArgumentException("The format expression error. ", nameof(format));
            if (format.Contains("yyyy"))
                format = format.Replace("yyyy", m.Year < 10 ? $"0{_y}" : _y);
            else if (format.Contains("yy"))
                format = format.Replace("yy", m.Year < 10 ? $"0{_y}" : _y.PadLeft(4, '0').Substring(2));
            if (format.Contains("mm"))
                format = format.Replace("mm", m.Month < 10 ? $"0{_m}" : _m);
            else if (format.Contains("m"))
                format = format.Replace("m", _m.TrimStart('0'));
            return format;
        }
        #endregion
    }
}
