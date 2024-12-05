using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace TestEnum
{
	enum MyEnum
	{
		A, B, C,
	}

	internal class Program
	{
		public static void Main(string[] args)
		{
			MyEnum myEnum = MyEnum.A;
			Console.WriteLine(myEnum.ToString());
			if (myEnum == (MyEnum)1)
			{

			}
			myEnum.CompareTo(1);
		}
	}
}

namespace Model
{
	public enum NumericType
	{
		Max = 10000, Speed = 1000, SpeedBase = Speed * 10 + 1,
		SpeedAdd = Speed * 10 + 2, SpeedPct = Speed * 10 + 3, SpeedFinalAdd = Speed * 10 + 4,
		SpeedFinalPct = Speed * 10 + 5, Hp = 1001, HpBase = Hp * 10 + 1,
		MaxHp = 1002, MaxHpBase = MaxHp * 10 + 1, MaxHpAdd = MaxHp * 10 + 2,
		MaxHpPct = MaxHp * 10 + 3, MaxHpFinalAdd = MaxHp * 10 + 4, MaxHpFinalPct = MaxHp * 10 + 5,
	}

	internal static class NumericTypeTool
	{
		public static TEnum[] GetEnumValue<TEnum>() where TEnum : Enum
		{
			return (TEnum[])Enum.GetValues(typeof(TEnum));
		}
	}

	public class NumericComponent
	{
		public readonly Dictionary<int, int> NumericDic = new Dictionary<int, int>();

		public void Awake()
		{
			var numericTypes = NumericTypeTool.GetEnumValue<NumericType>();
			foreach (var numericType in numericTypes)
			{
				NumericDic[(int)numericType] = 0;
			}
		}

		public float GetAsFloat(NumericType numericType)
		{
			return (float)GetByKey((int)numericType) / 10000;
		}

		public int GetAsInt(NumericType numericType)
		{
			return GetByKey((int)numericType);
		}

		public void Set(NumericType nt, float value)
		{
			this[nt] = (int)(value * 10000);
		}

		public void Set(NumericType nt, int value)
		{
			this[nt] = value;
		}

		public int this[NumericType numericType]
		{
			get { return GetByKey((int)numericType); }
			set
			{
				int v = GetByKey((int)numericType);
				if (v == value)
				{
					return;
				}

				NumericDic[(int)numericType] = value;

				Update(numericType);
			}
		}

		private int GetByKey(int key)
		{
			int value = 0;
			NumericDic.TryGetValue(key, out value);
			return value;
		}

		public void Update(NumericType numericType)
		{
			if (numericType > NumericType.Max)
			{
				return;
			}

			int final = (int)numericType / 10;
			int bas = final * 10 + 1;
			int add = final * 10 + 2;
			int pct = final * 10 + 3;
			int finalAdd = final * 10 + 4;
			int finalPct = final * 10 + 5;

			// 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
			// final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;
			NumericDic[final] =
				((GetByKey(bas) + GetByKey(add)) * (100 + GetByKey(pct)) / 100 +
				 GetByKey(finalAdd)) * (100 + GetByKey(finalPct)) / 100;

			// Game.EventSystem.Run(EventIdType.NumbericChange, this.Entity.Id, numericType, final);
		}
	}
}
