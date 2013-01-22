using System;
using System.Collections.Generic;

using IsosurfaceGenerator.Utils;

namespace IsosurfaceGenerator.Exporter
{
	/// <summary>
	/// Interface of mesh exporters
	/// </summary>
	public interface IMeshExporter
	{
		/// <summary>
		/// Export the given mesh data to a file
		/// </summary>
		/// <param name="triangles">Triangles.</param>
		void Export(List<Triangle> triangles);
	}
}

