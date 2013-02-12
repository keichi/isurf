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

namespace IsosurfaceGenerator
{
	/// <summary>
	/// フェーズドアレーレーダ(PAR)のデータを格納するクラス
	/// </summary>
	public class PARData : IDisposable
	{
		/// <summary>
		/// X方向の解像度
		/// </summary>
		public float StepX { get; set; }
		/// <summary>
		/// Y方向の解像度
		/// </summary>
		public float StepY { get; set; }
		/// <summary>
		/// Z方向の解像度
		/// </summary>
		public float StepZ { get; set; }
		
		public float StartX { get; set; }
		public float StartY { get; set; }
		public float StartZ { get; set; }
		
		public int SizeX { get; set; }
		public int SizeY { get; set; }
		public int SizeZ { get; set; }
		
		/// <summary>
		/// データの存在しない格子点に割り当てられる値
		/// </summary>
		public float NoDataValue { get; set; }
		
		/// <summary>
		/// 全ての格子点のデータへのポインタ<para/>
		/// float[,,]へのIntPtrで表現される。
		/// </summary>
		public IntPtr RawData { get; set; }
		
		public PARData ()
		{
		}

		public void Dispose()
		{
			Marshal.FreeHGlobal(RawData);
			GC.RemoveMemoryPressure(SizeX * SizeY * SizeZ * 4);
		}
	}
}

