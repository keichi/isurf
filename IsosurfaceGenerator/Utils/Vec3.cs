/*
 * This file is part of "IsosurfaceGenerator"
 *
 * Copyright (C) 2013 Keichi TAKAHASHI. All Rights Reserved.
 * Please contact Keichi Takahashi <keichi.t@me.com> for further informations.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 */

using System;
using System.Runtime.InteropServices;

namespace IsosurfaceGenerator.Utils
{
	/// <summary>
	/// 3次元ベクトルを表現するクラス
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vec3
	{
		/// <summary>
		/// X成分
		/// </summary>
		public float X;
		/// <summary>
		/// Y成分
		/// </summary>
		public float Y;
		/// <summary>
		/// Z成分
		/// </summary>
		public float Z;
		
		public Vec3(float x, float y, float z) {
			X = x;
			Y = y;
			Z = z;
		}
		
		/// <summary>
		/// 内積を求める
		/// </summary>
		/// <param name="v"></param>
		/// <returns>内積の値</returns>
		public float Dot(Vec3 v) {
			return X * v.X + Y * v.Y + Z * v.Z;
		}

		/// <summary>
		/// 外積を求める
		/// </summary>
		/// <param name="v2"></param>
		/// <returns>外積ベクトル</returns>
		public Vec3 Cross(Vec3 v2) {
			Vec3 v;
			v.X = Y * v2.Z - Z * v2.Y;
			v.Y = Z * v2.X - X * v2.Z;
			v.Z = X * v2.Y - Y * v2.X;
			return v;
		}

		/// <summary>
		/// 正規化する
		/// </summary>
		/// <returns>正規化されたベクトル</returns>
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
	}
}

