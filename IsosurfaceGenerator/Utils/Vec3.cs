using System;
using System.Runtime.InteropServices;

namespace IsosurfaceGenerator.Utils
{
	/// <summary>
	/// A 3-dimensional vector
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vec3
	{
		/// <summary>
		/// x-component
		/// </summary>
		public float X;
		/// <summary>
		/// y-component
		/// </summary>
		public float Y;
		/// <summary>
		/// z-component
		/// </summary>
		public float Z;
		
		public Vec3(float x, float y, float z) {
			X = x;
			Y = y;
			Z = z;
		}
		
		public float Dot(Vec3 v) {
			return X * v.X + Y * v.Y + Z * v.Z;
		}

		public Vec3 Cross(Vec3 v2) {
			Vec3 v;
			v.X = Y * v2.Z - Z * v2.Y;
			v.Y = Z * v2.X - X * v2.Z;
			v.Z = X * v2.Y - Y * v2.X;
			return v;
		}

		public Vec3 Normalize() {
			var length = (float)Math.Sqrt(Dot(this));
			if (length == 0.0f) return this;

			return new Vec3(X / length, Y / length, Z / length);
		}

		public override string ToString ()
		{
			return string.Format ("({0}, {1}, {2})", X, Y, Z);
		}

		public static Vec3 operator-(Vec3 v1, Vec3 v2) {
			Vec3 v;
			v.X = v1.X - v2.X;
			v.Y = v1.Y - v2.Y;
			v.Z = v1.Z - v2.Z;
			return v;
		}

		public static Vec3 operator+(Vec3 v1, Vec3 v2) {
			Vec3 v;
			v.X = v1.X + v2.X;
			v.Y = v1.Y + v2.Y;
			v.Z = v1.Z + v2.Z;
			return v;
		}
		
		public static Vec3 Interpolate(float isoValue, Vec3 v1, Vec3 v2, float f1, float f2) {
			Vec3 v;
			
			if (Math.Abs(isoValue - f1) < 0.00001) {
				return v1;
			}
			if (Math.Abs(isoValue - f2) < 0.00001) {
				return v2;
			}
			if (Math.Abs(f1 - f2) < 0.00001) {
				return v1;
			}

			var diff = (isoValue -  f1) / (f2 - f1);
			
			v.X = v1.X + (v2.X - v1.X) * diff;
			v.Y = v1.Y + (v2.Y - v1.Y) * diff;
			v.Z = v1.Z + (v2.Z - v1.Z) * diff;
			
			return v;
		}
	}
}

