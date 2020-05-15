# monthly.net
> A month data type for applications such as bills, invoices, etc.

Monthly是一个跟`Datetime`类似的**月份数据类型**，适用于表达年月数据，如账单、账期、月刊等信息。

<br/>

## 使用介绍

### 1.Monthly构造
```c#
  //创建一个“2018年1月”的账期
   Monthly m1 = 201801;
   Monthly m2 = new Monthly(2018, 1);
   Monthly m3 = Monthly.FromDate(new DateTime(2018, 1, 1));
   Monthly m4 = Monthly.FromDot(201801);
   Monthly m5 = Monthly.FromTickes(2018 * 12 + 1);
   Monthly m6 = Monthly.FromString("2018年01月");
   
   Monthly cur = Monthly.Current;	//当前时间实例
   Monthly min = Monthly.MinValue;	//Monthly最小实例
   Monthly max = Monthly.MaxValue;	//Monthly最大实例
```

### 2. Monthly属性
|  属性    |   说明   |
| ---- | ---- |
| Year | 获取当前实例的年 |
| Month | 获取当前实例的月 |
| Dot | 获取当前实例的年月标记值，如2018年1月记为 : 201801 |
| Tickes | 获取当前实例从公元零年一月开始的月份累计值 |
| First | 获取当前年份的一月为依据的新实例 |
| Last | 获取当前年份的十二月为依据的新实例 |
| Previous | 获取当前时间点的上月为依据的新实例 |
| Next | 获取当前时间点的下月为依据的新实例 |
| Quarter | 获取当前实例所在的季度 |


### 3.Monthly方法
- `ToDot() `  获取当前实例的年月标记值，如2018年1月记为 : `201801`
  ​
- `AddYears(int years)`  以当前实例与years的和值为依据创建一个新实例

- `AddMonths(int months)`  以当前实例与months的和值为依据创建一个新实例
  ​
- `Equals(Monthly other)`  判断当前实例的值与给定实例的值是否相等
​
- `Equals(object obj)`. 判断当前实例的值与给定实例的转换值是否相等，obj可以是DateTime类型
​
- `SpanMonths(Monthly other)`  获取当前实例与给定实例的月份差值
 ​
- `SpanMonths(DateTime date)`  获取当前实例与DateTime实例的月份差值
获取当前实例与DateTime实例的月份差值
 ​
- `CompareTo(Monthly other)`  获取当前实例与给定实例的大小比较的结果标识， -1:小于other实例值 ； 0 等于other实例值 ； 1：大于other实例值
 ​
- `List<Monthly> Axis(int from, int to)`  获取一段时间内的Monthly数轴(包含开始与结束月份)
 ​
- `List<Monthly> Axis(Monthly from, Monthly to)`  同`List<Monthly> Axis(int from, int to)`
 ​
- `ToString(string format = "yyyy/mm")`  获取包含"Y、y、M、m"字符格式的自定义Monthly字符串，format 格式如：`yyyy/mm ; yy/mm `; `yyyy年mm月` ;`YYYY-Mm`...，不区分大小写

<br/>

**示例：**
```c#
   Monthly m = 201801;
   m.CompareTo(201701);            
   m.Equals(DateTime.Now);
   m.Equals(201701);
   m.SpanMonths(new DateTime(2017, 1, 1));
   m.SpanMonths(201701);

   m.ToString();
   m.ToString("yy/mm");
   Monthly.FromDot(501).ToString("yy/mm");
   m.ToString("YYYY年m月");
   m.ToString("公元YyYy年mM月,哈哈...");
```

###  4.Monthly操作符
Monthly支持`+、- 、* 、/ 、> 、>= 、< 、<= 、++ 、-- 、== 、!=` 运算符操作。
> 特别注意:`-`操作，他有`operator -(Monthly m, int months)`和`operator -(Monthly m1, Monthly m2)`两个重载版本，且方法功能不同，如果是第二个版本，则必须显式标注被减对象的数据类型，如`m-(Monthly)201701`


<br/>

### 参考:
Datetime:  https://referencesource.microsoft.com/#mscorlib/system/datetime.cs,df6b1eba7461813b	
