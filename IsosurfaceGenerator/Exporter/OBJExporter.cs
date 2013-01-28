using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using IsosurfaceGenerator.Utils;

namespace IsosurfaceGenerator.Exporter
{
	public class OBJExporter : IMeshExporter
	{
		private string _filename;

		private OBJExporter()
		{
		}

		public OBJExporter (string filename)
		{
			_filename = filename;
		}

		public void Export(List<Triangle> triangles)
		{
			var sb = new StringBuilder();
			sb.Append("g isosurface1");

			foreach(var triangle in triangles) {
				sb.Append("v ");
				sb.Append(triangle.Vertex1.X);
				sb.Append(' ');
				sb.Append(triangle.Vertex1.Y);
				sb.Append(' ');
				sb.Append(triangle.Vertex1.Z);
				sb.AppendLine();

				sb.Append("v ");
				sb.Append(triangle.Vertex2.X);
				sb.Append(' ');
				sb.Append(triangle.Vertex2.Y);
				sb.Append(' ');
				sb.Append(triangle.Vertex2.Z);
				sb.AppendLine();

				sb.Append("v ");
				sb.Append(triangle.Vertex3.X);
				sb.Append(' ');
				sb.Append(triangle.Vertex3.Y);
				sb.Append(' ');
				sb.Append(triangle.Vertex3.Z);
				sb.AppendLine();
			}

			foreach(var triangle in triangles) {
				var normal = (triangle.Vertex3 - triangle.Vertex1).Cross (triangle.Vertex2 - triangle.Vertex1).Normalize ();
				sb.Append("vn ");
				sb.Append(normal.X);
				sb.Append(' ');
				sb.Append(normal.Y);
				sb.Append(' ');
				sb.Append(normal.Z);
				sb.AppendLine();
			}

			for (var i = 0; i < triangles.Count; i++) {
				sb.AppendFormat("f {0}//{3} {1}//{3} {2}//{3}", i * 3 + 1, i * 3 + 2, i * 3 + 3,  i + 1);
			}
		}
	}

}

