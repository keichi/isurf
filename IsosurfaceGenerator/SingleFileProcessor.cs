using System;
using System.IO;
using System.Collections.Generic;

using IsosurfaceGenerator.Exporter;
using IsosurfaceGenerator.Utils;

namespace IsosurfaceGenerator
{
	public enum MeshFileType {
		STL,
		OBJ,
	}

	public class SingleFileProcessor
	{
		private MeshFileType _meshFileType;
		private string _ctlFilename;
		private string _meshFilename;
		private float _isoValue;

		private Dictionary<MeshFileType, string> MESH_FILE_EXTENSIONS = new Dictionary<MeshFileType, string>() {
			{MeshFileType.OBJ, ".obj"},
			{MeshFileType.STL, ".stl"},
		};
		private Dictionary<MeshFileType, Func<string, IMeshExporter>> MESH_EXPORTERS = new Dictionary<MeshFileType, Func<string, IMeshExporter>>() {
			{MeshFileType.OBJ, s => new OBJExporter(s)},
			{MeshFileType.STL, s => new STLExporter(s)},
		};

		public SingleFileProcessor (string ctlFilename, float isoValue, MeshFileType meshFileType)
		{
			_meshFileType = meshFileType;
			_ctlFilename = ctlFilename;
			_meshFilename = getMeshFilename(_ctlFilename);
			_isoValue = isoValue;
		}

		private string getMeshFilename(string ctlFilename) {
			var meshFilename = Path.Combine(
				Path.GetDirectoryName(ctlFilename),
				Path.GetFileNameWithoutExtension(ctlFilename)
				);
			meshFilename += MESH_FILE_EXTENSIONS[_meshFileType];
			
			return meshFilename;
		}

		public void Process()
		{
			Console.WriteLine("====================");
			Console.WriteLine("Start processing CTL file \"{0}\"", _ctlFilename);

			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			var reader = new GradsFileReader(_ctlFilename);
			var data = reader.ReadData();
			sw.Stop();
			Console.WriteLine("[1/4] Read/parse GrADS file. ({0}[ms])", sw.ElapsedMilliseconds);
			
			sw.Restart();
			var mc = new MarchingCubes(data, _isoValue);
			data.Dispose();
			sw.Stop();
			Console.WriteLine("[2/4] Initializing isosurface generator. ({0}[ms])", sw.ElapsedMilliseconds);

			sw.Restart();
			var mesh = mc.CalculateIsosurface();
			mc.Dispose();
			sw.Stop();

			Console.WriteLine("[3/4] Generating isosurface where value is {1}. ({0}[ms])", sw.ElapsedMilliseconds, _isoValue);
			sw.Restart();
			var  exporter = MESH_EXPORTERS[_meshFileType](_meshFilename);
			exporter.Export(mesh);
			Console.WriteLine("[4/4] Writing isosurface mesh data to \"{1}\". ({0}[ms])", sw.ElapsedMilliseconds, _meshFilename);
			
			Console.WriteLine();
			Console.WriteLine("Resulted in {0} triangles.", mesh.Count);
		}
	}
}

