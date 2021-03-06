using System;
using System.Text;
using System.Collections;

namespace Refal.Runtime
{
	/* Passive expression is expression that don't contain
	   execution braces: <>

	   It is basically a collection of symbols (single characters,
	   strings - called 'compound characters' and treated as symbols,
	   macrodigits and identifiers, which can be thought of as a
	   special case of compound characters)
	*/

	public class PassiveExpression : CollectionBase
	{
		public PassiveExpression()
		{
		}

		public PassiveExpression(params object[] symbols)
		{
			foreach (object symbol in symbols)
				Add(symbol);
		}

		public static PassiveExpression Build(params object[] objects)
		{
			PassiveExpression result = new PassiveExpression();

			// flatten expressions, if needed
			foreach (object obj in objects)
			{
				if (obj is PassiveExpression)
				{
					foreach (object symbol in (PassiveExpression)obj)
						result.Add(symbol);
				}
				else
					result.Add(obj);
			}

			return result;
		}

		public object this[int index]
		{
			get { return List[index]; }
		}

		public virtual int Add(object symbol)
		{
			if (symbol is char[])
			{
				int index = -1;

				foreach (char c in (char[])symbol)
					index = List.Add(c);

				return index;
			}
			else if (symbol != null)
				return List.Add(symbol);

			return -1;
		}

		public bool IsEmpty
		{
			get { return List.Count == 0; }
		}

		public override int GetHashCode()
		{
			int hashCode = this.List.Count ^ (int)0xBAD1DEA;

			if (this.List.Count >= 1)
			{
				hashCode ^= List[0].GetHashCode();
			}

			return hashCode;
		}

		public override bool Equals(object o)
		{
			bool equals = true;

			if (o is PassiveExpression)
			{
				PassiveExpression ex = (PassiveExpression)o;

				if (ex.Count != Count)
					return false;

				for (int i = 0; i < Count; i++)
				{
					object my = this[i];
					object his = ex[i];

					if (my == null)
					{
						if (his != null)
							return false;
					}
					else // my != null
					{
						if (his == null)
							return false;

						equals = my.Equals(his);
					}

					if (!equals)
						return false;
				}

				return equals;
			}

			// ex
			return false;
		}

		public override string ToString()
		{
			return ToStringBuilder().ToString();
		}

		private StringBuilder ToStringBuilder()
		{
			StringBuilder sb = new StringBuilder();
			
			foreach (object value in this)
			{
				if (value is PassiveExpression)
					sb.AppendFormat("({0}) ", (value as PassiveExpression).ToStringBuilder().ToString());
				else if (value is char)
					sb.AppendFormat("{0}", value);
				else
					sb.AppendFormat("{0} ", value);
			}

			return sb;
		}

/*		public void Print()
		{
			foreach (object value in this)
			{
				Console.WriteLine("-> {0}", value);
			}
		}

		static void Main()
		{
			PassiveExpression ex = new PassiveExpression("Hello".ToCharArray(), 1, 2, 3, "World");
			ex.Print();
		}
*/
	}

	public class StructureBrace
	{
		public StructureBrace()
		{
		}
	}

	public class OpeningBrace
	{
		public OpeningBrace()
		{
		}
	}

	public class ClosingBrace
	{
		public ClosingBrace()
		{
		}
	}
}
