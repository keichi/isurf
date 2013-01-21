using System;
using System.Collections.Generic;

namespace IsosurfaceGenerator
{
	public interface IMeshExporter
	{
		void Export(List<Triangle> triangles);
	}
}

