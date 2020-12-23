using System;
using System.Collections.Generic;

namespace Hsluv
{
	public class HsluvConverter
	{
		protected static double[][] M = new double[][]
		{
			new double[] {  3.240969941904521, -1.537383177570093, -0.498610760293    },
			new double[] { -0.96924363628087,   1.87596750150772,   0.041555057407175 },
			new double[] {  0.055630079696993, -0.20397695888897,   1.056971514242878 },
		};

		protected static double RefX = 0.95045592705167;
		protected static double RefY = 1.0;

		protected static double RefU = 0.19783000664283;
		protected static double RefV = 0.46831999493879;

		protected static double Kappa   = 903.2962962;
		protected static double Epsilon = 0.0088564516;

		protected static IList<double[]> GetBounds(double L)
		{
			var result = new List<double[]>();

			double sub1 = Math.Pow(L + 16, 3) / 1560896;
			double sub2 = sub1 > Epsilon ? sub1 : L / Kappa;

			for (int c = 0; c < 3; ++c)
			{
				var m1 = M[c][0];
				var m2 = M[c][1]; 
				var m3 = M[c][2];

				for (int t = 0; t < 2; ++t)
				{
					var top1 = (284517 * m1 - 94839 * m3) * sub2;
					var top2 = (838422 * m3 + 769860 * m2 + 731718 * m1) * L * sub2 - 769860 * t * L;
					var bottom = (632260 * m3 - 126452 * m2) * sub2 + 126452 * t;

					result.Add(new double[] { top1 / bottom, top2 / bottom });
				}
			}

			return result;
		}

		protected static bool LengthOfRayUntilIntersect(double theta, 
			IList<double> line,
			out double length)
		{
			length = line[1] / (Math.Sin(theta) - line[0] * Math.Cos(theta));

			return length >= 0;
		}

		protected static double MaxChromaForLH(double L, double H) 
		{
			double hrad = H / 360 * Math.PI * 2;

			var bounds = GetBounds(L);
			double min = Double.MaxValue;

			foreach (var bound in bounds)
			{
				double length;

				if (LengthOfRayUntilIntersect(hrad, bound, out length))
				{
					min = Math.Min(min, length);
				}
			}

			return min;
		}

		protected static double DotProduct(IList<double> a, 
			IList<double> b) 
		{
			double sum = 0;

			for (int i = 0; i < a.Count; ++i)
			{
				sum += a[i] * b[i];
			}

			return sum;
		}

		protected static double FromLinear(double c) 
		{
			if (c <= 0.0031308)
			{
				return 12.92 * c;
			}
			else 
			{
				return 1.055 * Math.Pow(c, 1 / 2.4) - 0.055;
			}
		}

		public static IList<double> XyzToRgb(IList<double> tuple)
		{
			return new double[]
			{
				FromLinear(DotProduct(M[0], tuple)),
				FromLinear(DotProduct(M[1], tuple)),
				FromLinear(DotProduct(M[2], tuple)),
			};
		}

		protected static double LToY(double L) 
		{
			if (L <= 8) 
			{
				return RefY * L / Kappa;
			} 
			else 
			{
				return RefY * Math.Pow((L + 16) / 116, 3);
			}
		}

		public static IList<double> LuvToXyz(IList<double> tuple)
		{
			double L = tuple[0];
			double U = tuple[1];
			double V = tuple[2];

			if (L == 0) 
			{
				return new double[] { 0, 0, 0 };
			}

			double varU = U / (13 * L) + RefU;
			double varV = V / (13 * L) + RefV;

			double Y = LToY(L);
			double X = 0 - (9 * Y * varU) / ((varU - 4) * varV - varU * varV);
			double Z = (9 * Y - (15 * varV * Y) - (varV * X)) / (3 * varV);

			return new double[] { X, Y, Z };
		}

		public static IList<double> LchToLuv(IList<double> tuple)
		{
			double L = tuple[0];
			double C = tuple[1];
			double H = tuple[2];

			double Hrad = H / 360.0 * 2 * Math.PI;
			double U = Math.Cos(Hrad) * C;
			double V = Math.Sin(Hrad) * C;

			return new Double [] { L, U, V };
		}

		public static IList<double> HsluvToLch(IList<double> tuple)
		{
			double H = tuple[0];
			double S = tuple[1]; 
			double L = tuple[2];

			if (L > 99.9999999)
			{
				return new Double[] { 100, 0, H };
			}

			if (L < 0.00000001) 
			{
				return new Double[] { 0, 0, H };
			}

			double max = MaxChromaForLH(L, H);
			double C = max / 100 * S;

			return new double[] { L, C, H };
		}

		public static IList<double> LchToRgb(IList<double> tuple)
		{
			return XyzToRgb(LuvToXyz(LchToLuv(tuple)));
		}

		public static IList<double> HsluvToRgb(IList<double> tuple)
		{
			return LchToRgb(HsluvToLch(tuple));
		}
	}
}

