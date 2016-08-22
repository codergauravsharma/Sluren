using System;
using System.Collections.Generic;
using System.Linq;

namespace Sluren.Decision {
	public class Operand<T> {
		public T Source { get; private set; }

		public Operand(T operand) {
			Source = operand;
		}
		/*
		public static implicit operator T(Operand<T> operand) {
			return operand.Source;
		}
		*/
		public override string ToString() { return Source.GetType().Name; }
		//*
		public bool IsEqualTo(T target) { return Source.Equals(target); }
		public bool IsNotEqualTo(T target) { return !IsEqualTo(target); }
		//public bool IsEqualTo(IComparable<T> target) { return target.CompareTo(Source) == 0; }
		//public bool IsNotEqualTo(IComparable<T> target) { return !IsEqualTo(target); }
		public bool IsLessThan(IComparable<T> target) { return target.CompareTo(Source) > 0; }
		public bool IsLessThanEqualTo(IComparable<T> target) { return target.CompareTo(Source) >= 0; }
		public bool IsGreaterThan(IComparable<T> target) { return target.CompareTo(Source) < 0; }
		public bool IsGreaterThanEqualTo(IComparable<T> target) { return target.CompareTo(Source) <= 0; }
		public bool IsIn(IEnumerable<T> collection) { return collection.Contains(Source); }
		public bool IsNotIn(IEnumerable<T> collection) { return !IsIn(collection); }
		public bool LiesBetween(IComparable<T> lowerLimit, IComparable<T> upperLimit) { return lowerLimit.CompareTo(Source) <= 0 && upperLimit.CompareTo(Source) >= 0; }
		public bool LiesOutside(IComparable<T> lowerLimit, IComparable<T> upperLimit) { return !LiesBetween(lowerLimit, upperLimit); }
		//*/
	}

	/*
	public static class Operators {
		public static bool IsEqualTo<T>(this T source, T target) {
			return source.Equals(target);
		}
		public static bool IsNotEqualTo<T>(this T source, T target) {
			return !source.IsEqualTo(target);
		}
		public static bool IsIn<T>(this T source, IEnumerable<T> collection) {
			return collection.Contains(source);
		}
		public static bool IsNotIn<T>(this T source, IEnumerable<T> collection) {
			return !source.IsIn(collection);
		}
		public static bool IsLessThan<T>(this T source, IComparable<T> target) {
			return target.CompareTo(source) > 0;
		}
		public static bool IsLessThanEqualTo<T>(this T source, IComparable<T> target) {
			return target.CompareTo(source) >= 0;
		}
		public static bool IsGreaterThan<T>(this T source, IComparable<T> target) {
			return target.CompareTo(source) < 0;
		}
		public static bool IsGreaterThanEqualTo<T>(this T source, IComparable<T> target) {
			return target.CompareTo(source) <= 0;
		}
		public static bool LiesBetween<T>(this T source, IComparable<T> lowerLimit, IComparable<T> upperLimit) {
			return lowerLimit.CompareTo(source) <= 0 && upperLimit.CompareTo(source) >= 0;
		}
		public static bool LiesOutside<T>(this T source, IComparable<T> lowerLimit, IComparable<T> upperLimit) {
			return !source.LiesBetween(lowerLimit, upperLimit);
		}
	}
	*/
}
