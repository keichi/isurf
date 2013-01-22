using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IsosurfaceGenerator
{
	public class STLExporter : IMeshExporter
	{
		private string _filename;

		private STLExporter()
		{
		}

		public STLExporter (string filename)
		{
			_filename = filename;
		}

		private unsafe void writeStruct<T>(Stream stream, T obj) where T : struct
		{
			var buf = new byte[sizeof(T)];
			fixed (byte* ptr = buf) {
				*(T*)ptr = obj;
			}
			stream.Write(buf, 0, buf.Length);
		}

		public void Export(List<Triangle> triangles) {
			using (var fs = File.OpenWrite(_filename)) {
				fs.Write(new byte[80], 0, 80);
				fs.Write(BitConverter.GetBytes(triangles.Count), 0, 4);

				foreach (var triangle in triangles) {
					var normal = (triangle.Vertex3 - triangle.Vertex1).Cross (triangle.Vertex2 - triangle.Vertex1).Normalize ();
					writeStruct(fs, normal);
					writeStruct(fs, triangle);
					fs.Write (new byte[2], 0, 2);
				}
			}
		}
	}
}

