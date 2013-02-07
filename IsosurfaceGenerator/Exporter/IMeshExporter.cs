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

