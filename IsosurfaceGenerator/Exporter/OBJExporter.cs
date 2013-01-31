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
			var dict = new Dictionary<Vec3, int>();
			var sb = new StringBuilder();
			sb.AppendLine("g isosurface");

			var count = 1;
			foreach(var triangle in triangles) {
				var vertex1 = triangle.Vertex1;
				if (!dict.ContainsKey(vertex1)) {
					dict.Add(vertex1, count++);
					sb.Append("v ");
					sb.Append(vertex1.X);
					sb.Append(' ');
					sb.Append(vertex1.Y);
					sb.Append(' ');
					sb.Append(vertex1.Z);
					sb.AppendLine();
				}

				var vertex2 = triangle.Vertex2;
				if (!dict.ContainsKey(vertex2)) {
					dict.Add(vertex2, count++);
					sb.Append("v ");
					sb.Append(vertex2.X);
					sb.Append(' ');
					sb.Append(vertex2.Y);
					sb.Append(' ');
					sb.Append(vertex2.Z);
					sb.AppendLine();
				}

				var vertex3 = triangle.Vertex3;
				if (!dict.ContainsKey(vertex3)) {
					dict.Add(vertex3, count++);
					sb.Append("v ");
					sb.Append(vertex3.X);
					sb.Append(' ');
					sb.Append(vertex3.Y);
					sb.Append(' ');
					sb.Append(vertex3.Z);
					sb.AppendLine();
				}
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
				var i1 = dict[triangles[i].Vertex1];
				var i2 = dict[triangles[i].Vertex2];
				var i3 = dict[triangles[i].Vertex3];

				if (i1 == i2 || i1 == i3 || i2 == i3) {
					continue;
				}
				
				sb.AppendFormat("f {0}//{3} {1}//{3} {2}//{3}\n", i1, i2, i3,  i + 1);
			}

			File.WriteAllText(_filename, sb.ToString());
		}
	}

}

