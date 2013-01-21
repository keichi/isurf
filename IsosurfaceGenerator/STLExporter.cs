using System;
using System.IO;
using System.Collections.Generic;

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

		public void Export(List<Triangle> triangles) {
			using (var writer = new BinaryWriter(File.OpenWrite(_filename))) {
				writer.Write (new byte[80]);
				writer.Write (triangles.Count);
				
				foreach (var triangle in triangles) {
					var normal = (triangle.Vertex3 - triangle.Vertex1).Cross (triangle.Vertex2 - triangle.Vertex1).Normalize ();
					writer.Write (normal.X);
					writer.Write (normal.Y);
					writer.Write (normal.Z);
					
					writer.Write (triangle.Vertex1.X);
					writer.Write (triangle.Vertex1.Y);
					writer.Write (triangle.Vertex1.Z);
					
					writer.Write (triangle.Vertex2.X);
					writer.Write (triangle.Vertex2.Y);
					writer.Write (triangle.Vertex2.Z);
					
					writer.Write (triangle.Vertex3.X);
					writer.Write (triangle.Vertex3.Y);
					writer.Write (triangle.Vertex3.Z);
					
					writer.Write ((short)0);
				}
			}
		}
	}
}

