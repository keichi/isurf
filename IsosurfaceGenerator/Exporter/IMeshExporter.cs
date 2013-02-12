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
using System.Collections.Generic;

using IsosurfaceGenerator.Utils;

namespace IsosurfaceGenerator.Exporter
{
	/// <summary>
	/// メッシュデータをエクスポートするクラスのインターフェース
	/// </summary>
	public interface IMeshExporter : IDisposable
	{
		/// <summary>
		/// メッシュをファイルへエクスポートする
		/// </summary>
		/// <param name="triangles">メッシュデータ</param>
		/// <param name="isoValue">等値曲面の値</param>
		void Export(List<Triangle> triangles, float isoValue);
	}
}

