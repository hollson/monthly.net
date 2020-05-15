//
//  MonthlyTest.cs
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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Monthly_Test
{
	[TestClass]
	public class MonthlyTest
	{
		[TestMethod]
		public void TestProps()
		{
			var tar = DateTime.Now;
            Monthly plan = Monthly.FromDate(tar);

			Assert.AreEqual(Monthly.Current, new Monthly(tar.Year, tar.Month));

			Assert.AreEqual(plan.Year, tar.Year);
			Assert.AreEqual(plan.Month, tar.Month);
			Assert.AreEqual(plan.Dot, tar.Year * 100 + tar.Month);
			Assert.AreEqual(plan.Tickes, tar.Year * 12 + tar.Month);

			Assert.AreEqual(plan.First.ToDot(), tar.Year * 100 + 1);
			Assert.AreEqual(plan.Last.ToDot(), tar.Year * 100 + 12);


			Assert.AreEqual(plan.First.Previous.ToDot(), tar.AddYears(-1).Year * 100 + 12);
			Assert.AreEqual(plan.Last.Next.ToDot(), tar.AddYears(1).Year * 100 + 1);

			Assert.AreEqual(plan.Quarter, GetQuarter(tar.Month));

			Assert.AreEqual(Monthly.MinValue, new Monthly(0, 1));
			Assert.AreEqual(Monthly.MaxValue, new Monthly(9999, 12));
		}

		private int GetQuarter(int q)
		{
			if (new System.Collections.Generic.List<int>() { 1, 2, 3 }.Contains(q)) return 1;
			if (new System.Collections.Generic.List<int>() { 4, 5, 6 }.Contains(q)) return 2;
			if (new System.Collections.Generic.List<int>() { 7, 8, 9 }.Contains(q)) return 3;
			if (new System.Collections.Generic.List<int>() { 10, 11, 12 }.Contains(q)) return 4;
			return 0;
		}


        [TestMethod]
		public void TestMethods()
		{
			Monthly plan = 201801;
			var tar = new DateTime(2018, 1, 1);
			var tip = false;

			//Dot
			Assert.AreEqual(new Monthly(0, 11), 11);
			Assert.AreEqual(new Monthly(1, 1), 101);
			Assert.AreEqual(new Monthly(100, 12), 10012);
			Assert.AreEqual(new Monthly(2018, 12), 201812);

			//Tickes
			Assert.AreEqual(((Monthly)101).Tickes, 13);
			Assert.AreEqual(((Monthly)201811).Tickes, 2018 * 12 + 11);

			//加月
			Assert.AreEqual(plan.AddMonths(-1), 201712);
			Assert.AreEqual(plan.AddMonths(-23), 201602);
			Assert.AreEqual(plan.AddMonths(22), 201911);

			//加月(随机)
			for (int i = 0; i < 100; i++)
			{
				var rd = new Random(Guid.NewGuid().GetHashCode()).Next(100);
				Assert.AreEqual(plan.AddMonths(rd), Monthly.FromDate(tar.AddMonths(rd)));
				Assert.AreEqual(plan.AddMonths(rd).Dot, tar.AddMonths(rd).Year * 100 + tar.AddMonths(rd).Month);
			}

			//加年
			Assert.IsTrue(plan.AddYears(6) == 202401);
			Assert.IsTrue(plan.AddYears(-18) == 200001);

			//加年（异常）
			try { var m = Monthly.Current.AddYears(-3000); }
			catch (Exception e) { tip |= e.Message.Contains("beteen 1 and 120000"); }
			Assert.IsTrue(tip);

			//月份差
			Assert.AreEqual(plan.SpanMonths(201711), 2);
			Assert.AreEqual(plan.SpanMonths(201902), -13);

			//比较大小
			Assert.AreEqual(plan.CompareTo(201801), 0);
			Assert.AreEqual(plan.CompareTo(201701), 1);
			Assert.AreEqual(plan.CompareTo(202001), -1);

			//构造
			Assert.AreEqual(Monthly.FromDot(3), 3);
			Assert.AreEqual(Monthly.FromTickes(13), 101);
			Assert.AreEqual(Monthly.FromDate(new DateTime(2018, 12, 12)), 201812);
			Assert.AreEqual(Monthly.FromString("2018/01"), 201801);
			Assert.AreEqual(Monthly.FromString("2018年01月"), 201801);
            Assert.AreEqual(Monthly.FromString("2018@01/01"), 201801);
            Assert.AreEqual(Monthly.FromString(new DateTime(2018, 1, 1).ToString("yyyy-MM")), 201801);
            Assert.AreEqual(Monthly.FromString("3"), 3);

			//月份轴
			var axis = Monthly.Axis(201711, 201901);
			Assert.IsTrue(axis.Count == 15);
			Assert.AreEqual(axis[0], 201711);
			Assert.AreEqual(axis[3], 201802);
			Assert.AreEqual(axis[14], 201901);

			axis = Monthly.Axis(201812, 201712);
			Assert.IsTrue(axis.Count == 13);
			Assert.AreEqual(axis[0], 201812);
			Assert.AreEqual(axis[12], 201712);

			//异常
			tip = false;
			try { Monthly m = 201800; }
			catch (Exception e) { tip |= e.Message.Contains("correct dot format"); }  //dot format
			Assert.IsTrue(tip);

			tip = false;
			try { Monthly m = Monthly.FromDot(13); }
			catch (Exception e) { tip |= e.Message.Contains("correct dot format"); }  //13月
			Assert.IsTrue(tip);

			tip = false;
			try { Monthly m = Monthly.FromTickes(999999); }
			catch (Exception e) { tip |= e.Message.Contains("must beteen 1 and 120000"); }  //越界
			Assert.IsTrue(tip);

			tip = false;
			try { Monthly m = Monthly.FromString(null); }
			catch (Exception e) { tip |= e.Message.Contains("null or empty"); }  //IsNullOrEmpty
			Assert.IsTrue(tip);

			tip = false;
			try { Monthly m = Monthly.FromString("abc"); }
			catch (Exception e) { tip |= e.Message.Contains("parameters"); }  //格式错误
			Assert.IsTrue(tip);

			tip = false;
			try { Monthly m = Monthly.FromString("88"); }
			catch (Exception e) { tip |= e.Message.Contains("must beteen"); }  //越界
			Assert.IsTrue(tip);
		}

		[TestMethod]
		public void TestOps()
		{
            Monthly plan = 201801;

            Assert.AreEqual(plan + 12, 201901);
			Assert.AreEqual(plan - 13, 201612);

			Assert.AreEqual(plan - (Monthly)201701, 12);
			Assert.AreEqual(plan - (new DateTime(2017, 12, 12)), 1);

			Assert.AreEqual(--plan, 201712);
			Assert.AreEqual(++plan, 201801);

			Assert.IsTrue(plan == Monthly.FromDot(201801));
			Assert.IsTrue(plan != Monthly.FromDot(201802));

			Assert.IsTrue(plan >= Monthly.FromDot(201801));
			Assert.IsTrue(plan < Monthly.FromDot(201803));
		}

		[TestMethod]
		public void TestOvr()
		{
			Monthly plan = 201801;
			var tar = Monthly.FromString("2018.01");

			//哈希码(相同dot具有相同的哈希码)
			Assert.AreEqual(plan.GetHashCode(), tar.GetHashCode());
			tar++;
			Assert.AreNotEqual(plan.GetHashCode(), tar.GetHashCode());

			//格式化
			Assert.AreEqual(plan.ToString(), "2018/01");
			Assert.AreEqual(plan.ToString("yy/mm"), "18/01");
			Assert.AreEqual(Monthly.FromDot(501).ToString("yy/mm"), "05/01");
			Assert.AreEqual(plan.ToString("YYYY年m月"), "2018年1月");
			Assert.AreEqual(plan.ToString("公元YyYy年mM月,哈哈..."), "公元2018年01月,哈哈...");

			//比较相等
			Assert.IsTrue(plan.Equals(Monthly.FromDot(201801)));
			Assert.IsTrue(plan.Equals(new DateTime(2018, 1, 1)));
			Assert.IsTrue(plan.Equals((object)Monthly.FromDot(201801)));
			Assert.IsFalse(plan.Equals(Monthly.FromDot(201901)));
		}
	}
}
