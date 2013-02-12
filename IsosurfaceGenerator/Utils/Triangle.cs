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
	/// 3次元空間内の三角形を表現する構造体
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Triangle
	{
		/// <summary>
		/// 1つ目の頂点
		/// </summary>
		public Vec3 Vertex1;
		/// <summary>
		/// 2つ目の頂点
		/// </summary>
		public Vec3 Vertex2;
		/// <summary>
		/// 3つ目の頂点
		/// </summary>
		public Vec3 Vertex3;

		public Triangle (Vec3 v1, Vec3 v2, Vec3 v3)
		{
			Vertex1 = v1;
			Vertex2 = v2;
			Vertex3 = v3;
		}
	}
}

