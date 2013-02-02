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
		private StreamWriter _writer;
		private int _verticesCount = 1;
		private int _normalVecsCount = 1;

		private OBJExporter()
		{
		}

		public OBJExporter (string filename)
		{
			_writer = new StreamWriter(filename);
		}

		public void Export(List<Triangle> triangles, float isoValue)
		{
			var dict = new Dictionary<Vec3, int>();
			_writer.WriteLine("g " + isoValue.ToString());

			foreach(var triangle in triangles) {
				var vertex1 = triangle.Vertex1;
				if (!dict.ContainsKey(vertex1)) {
					dict.Add(vertex1, _verticesCount++);
					_writer.Write("v ");
					_writer.Write(vertex1.X);
					_writer.Write(' ');
					_writer.Write(vertex1.Y);
					_writer.Write(' ');
					_writer.Write(vertex1.Z);
					_writer.WriteLine();
				}

				var vertex2 = triangle.Vertex2;
				if (!dict.ContainsKey(vertex2)) {
					dict.Add(vertex2, _verticesCount++);
					_writer.Write("v ");
					_writer.Write(vertex2.X);
					_writer.Write(' ');
					_writer.Write(vertex2.Y);
					_writer.Write(' ');
					_writer.Write(vertex2.Z);
					_writer.WriteLine();
				}

				var vertex3 = triangle.Vertex3;
				if (!dict.ContainsKey(vertex3)) {
					dict.Add(vertex3, _verticesCount++);
					_writer.Write("v ");
					_writer.Write(vertex3.X);
					_writer.Write(' ');
					_writer.Write(vertex3.Y);
					_writer.Write(' ');
					_writer.Write(vertex3.Z);
					_writer.WriteLine();
				}
			}

			foreach(var triangle in triangles) {
				_normalVecsCount++;
				var normal = (triangle.Vertex3 - triangle.Vertex1).Cross (triangle.Vertex2 - triangle.Vertex1).Normalize ();
				_writer.Write("vn ");
				_writer.Write(normal.X);
				_writer.Write(' ');
				_writer.Write(normal.Y);
				_writer.Write(' ');
				_writer.Write(normal.Z);
				_writer.WriteLine();
			}

			for (var i = 0; i < triangles.Count; i++) {
				var i1 = dict[triangles[i].Vertex1];
				var i2 = dict[triangles[i].Vertex2];
				var i3 = dict[triangles[i].Vertex3];

				if (i1 == i2 || i1 == i3 || i2 == i3) {
					continue;
				}

				_writer.Write("f ");

				_writer.Write(i1);
				_writer.Write("//");
				_writer.Write(i + 1);
				
				_writer.Write(' ');
				_writer.Write(i2);
				_writer.Write("//");
				_writer.Write(i + 1);
				
				_writer.Write(' ');
				_writer.Write(i3);
				_writer.Write("//");
				_writer.Write(i + 1);
				
				_writer.WriteLine();
			}
		}

		public void Dispose()
		{
			_writer.Dispose();
		}
	}

}

