
using System;
using System.Collections;

namespace PudelkoLibrary
{
	class Pudelko : IFormattable
	{
		// private a,b,c always in mm for easier calculations
		private readonly  double _A;
		private readonly double _B; 
		private readonly double _C; // in mm
		private readonly UnitOfMeasure unit;

		// convert to m from private mm on the fly
		public double A {
			get => _A / 1000; 
		}
		public double B {
			get => _B / 1000; 
		}
		public double C {
			get => _C / 1000; 
		}
		public double Objetosc {
			get => Math.Round((_A / 1000) * (_B / 1000) * (_C / 1000), 9);
		}
		public double Pole {
			get => Math.Round(
			(((_A / 1000) * (_B / 1000)) * 2) + (((_A / 1000) * (_C / 1000)) * 2) + (((_B / 1000) * (_C / 1000)) * 2)
			, 6);
		}

		public Pudelko () {
			this._A = 100;
			this._B = 100;
			this._C = 100;
			this.unit = UnitOfMeasure.meter;
		}

		private double ParseToMM(double value, UnitOfMeasure unit) {
			switch (unit) {
				case UnitOfMeasure.milimeter:
					return Math.Truncate(value);
				case UnitOfMeasure.centimeter:
					return value < 0.1 ? 0 : Math.Round(value * 10, 1);
				case UnitOfMeasure.meter:
					return value < 0.001 ? 0 : Math.Round(value * 1000, 3);
				default: 
					throw new ArgumentOutOfRangeException();
			}
		}
		public Pudelko(double a, UnitOfMeasure unit = UnitOfMeasure.meter)
		{
			if (this.ParseToMM(a, unit) <= 0)
				throw new ArgumentOutOfRangeException();
			if (this.ParseToMM(a, unit) >= 10000)
				throw new ArgumentOutOfRangeException();
			this._A = this.ParseToMM(a, unit);
			this._B = this.ParseToMM(0.1, UnitOfMeasure.meter);
			this._C = this.ParseToMM(0.1, UnitOfMeasure.meter);
		}
		public Pudelko(double a, double b, UnitOfMeasure unit = UnitOfMeasure.meter)
		{
			if (this.ParseToMM(a, unit) <= 0 || this.ParseToMM(b, unit) <= 0)
				throw new ArgumentOutOfRangeException();
			if (this.ParseToMM(a, unit) > 10000 || this.ParseToMM(b, unit) > 10000)
				throw new ArgumentOutOfRangeException();
			this._A = this.ParseToMM(a, unit);
			this._B = this.ParseToMM(b, unit);
			this._C = this.ParseToMM(0.1, UnitOfMeasure.meter);
		}
		public Pudelko (double a, double b, double c, UnitOfMeasure unit = UnitOfMeasure.meter) {
			if (this.ParseToMM(a, unit) <= 0 || this.ParseToMM(b, unit) <= 0 || this.ParseToMM(c, unit) <= 0) 
				throw new ArgumentOutOfRangeException();
			if (this.ParseToMM(a, unit) > 10000 || this.ParseToMM(b, unit) > 10000 || this.ParseToMM(c, unit) > 10000)
				throw new ArgumentOutOfRangeException();
			this._A = this.ParseToMM(a, unit);
			this._B = this.ParseToMM(b, unit);
			this._C = this.ParseToMM(c, unit);
		}


		public override string ToString()
		{
			return this.ToString("m");
		}

		public string ToString(string format) {
			return this.ToString(format, null);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			switch (format)
			{
				case "mm":
					return $"{_A} mm × {_B} mm × {_C} mm";
				case "cm":
					return $"{_A / 10} cm × {_B / 10} cm × {_C / 10} cm";
				case "m":
					return $"{_A / 1000} m × {_B / 1000} m × {_C} m";
				default:
					throw new FormatException();
			}
		}

		public virtual bool Equals(Pudelko obj) {
			if (this._A == obj._A) {
				if (this._B == obj._B) {
					if (this._C == obj._C) {
						return true;
					}
				}
				if (this._B == obj._C) {
					if (this._C == obj._B) {
						return true;
					}
				}
			}
			if (this._B == obj._A) {
				if (this._A == obj._B) {
					if (this._C == obj._C) {
						return true;
					}
				}
				if (this._A == obj._C) {
					if (this._C == obj._B) {
						return true;
					}
				}
			}
			if (this._C == obj._A) {
				if (this._A == obj._B) {
					if (this._C == obj._C) {
						return true;
					}
				}
				if (this._B == obj._C) {
					if (this._C == obj._B) {
						return true;
					}
				}
			}
			return false;
		}
		public static bool operator ==(Pudelko obj, Pudelko obj2)
		{
			return obj.Equals(obj2);
		}
		public static bool operator !=(Pudelko obj, Pudelko obj2)
		{
			return obj.Equals(obj2);
		}

		public static Pudelko operator +(Pudelko obj, Pudelko obj2)
		{
			double a = obj._A + obj2._A;
			double b = obj._B + obj2._B;
			double c = obj._C > obj2._C ? obj._C : obj2._C;
			return new Pudelko(a, b, c, UnitOfMeasure.milimeter);
		}

		public static explicit operator double[](Pudelko obj) {
			var r = new double[3] { obj.A, obj.B, obj.C };
			return r;
		}

		public static implicit operator Pudelko(ValueTuple<int, int, int> wymiary) {
			return new Pudelko((double)wymiary.Item1, (double)wymiary.Item2, (double)wymiary.Item3, UnitOfMeasure.milimeter);
		}

		public double this[int index]
		{
			get {
				switch (index) {
					case 0:
						return _A;
					case 1:
						return _B;
					case 2:
						return _C;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		int position = 0;
		public bool MoveNext()
		{
			position++;
			return (position < 3);
		}
		public void Reset()
		{
			position = 0;
		}
		public object Current
		{
			get {
				switch(position) {
					case 0:
						return _A;
					case 1:
						return _B;
					case 2:
						return _C;
					default:
						return 0;
				}
			}
		}

		public IEnumerator GetEnumerator() {
			return (IEnumerator)this;
		}

		public static Pudelko Parse(string toParse) {
			var splitedToParse = toParse.Split(" ");
			if (splitedToParse.Length == 8) {
				var a = double.Parse(splitedToParse[0]);
				var b = double.Parse(splitedToParse[3]);
				var c = double.Parse(splitedToParse[6]);
				var unitChar = splitedToParse[1];
				switch(unitChar) {
					case "mm":
						return new Pudelko(a, b, c, UnitOfMeasure.milimeter);
					case "cm":
						return new Pudelko(a, b, c, UnitOfMeasure.centimeter);
					case "m":
						return new Pudelko(a, b, c, UnitOfMeasure.meter);
					default:
						throw new FormatException();
				}
			} else {
				throw new FormatException();
			}
		}

	}

	public interface IEquatable<Pudelko> { }

	public interface IEnumerable<Pudelko> : System.Collections.IEnumerable { }
}
