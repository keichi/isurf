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

namespace IsosurfaceGenerator.Utils
{
	/// <summary>
	/// 1つの格子点を表現する構造体
	/// </summary>
	public struct Vertex
	{
		/// <summary>
		/// 座標
		/// </summary>
		public Vec3 Point;
		/// <summary>
		/// 格子点の持つ値
		/// </summary>
		public float Value;
		/// <summary>
		/// 格子点が等値曲面の内側に存在するか
		/// </summary>
		public bool IsInside;

		/// <summary>
		/// 2つのVertexを内分するVertexを求める
		/// </summary>
		/// <param name="v1">1つ目のVertex</param>
		/// <param name="v2">2つ目のVertex</param>
		/// <param name="isoValue">等値曲面の値</param>
		/// <returns>内分されたVertex</returns>
		public static Vertex Interpolate(Vertex v1, Vertex v2, float isoValue) {
			if (Math.Abs(isoValue - v1.Value) < 0.00001f) {
				return v1;
			}
			if (Math.Abs(isoValue - v2.Value) < 0.00001f) {
				return v2;
			}
			if (Math.Abs(v1.Value - v2.Value) < 0.00001f) {
				return v1;
			}
			
			var diff = (isoValue -  v1.Value) / (v2.Value - v1.Value);

			Vertex v;
			v.Point = new Vec3(
				v1.Point.X  + (v2.Point.X - v1.Point.X) * diff,
				v1.Point.Y + (v2.Point.Y - v1.Point.Y) * diff,
				v1.Point.Z + (v2.Point.Z - v1.Point.Z) * diff
			);
			v.Value = (v1.Value + v2.Value) * 0.5f;
			v.IsInside = false;

			return v;
		}
	}
}

