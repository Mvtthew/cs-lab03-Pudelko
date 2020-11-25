
using System;

namespace PudelkoLibrary
{
	class Pudelko : IFormattable
	{
		// private a,b,c always in mm for easier calculations
		private double _A;
		private double _B; 
		private double _C; // in mm
		private UnitOfMeasure unit;

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
			switch(unit) {
				case UnitOfMeasure.milimeter:
					return value;
				case UnitOfMeasure.centimeter:
					return value * 10;
				case UnitOfMeasure.meter:
					return value * 1000;
				default: 
					throw new ArgumentOutOfRangeException();
			}
		}

		public Pudelko (double a = 0.1, double b = 0.1, double c = 0.1, UnitOfMeasure unit = UnitOfMeasure.meter) {
			if (a <= 0 || b <= 0 || c <= 0) 
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

	}

	interface IEquatable<Pudelko> {
		bool Equals(Pudelko obj);
	}
}
