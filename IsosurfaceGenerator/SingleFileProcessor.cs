using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

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
		private string _ctlPath;
		private string _meshPath;
		private float[] _isoValues;

		private Dictionary<MeshFileType, string> MESH_FILE_EXTENSIONS = new Dictionary<MeshFileType, string>() {
			{MeshFileType.OBJ, ".obj"},
			{MeshFileType.STL, ".stl"},
		};
		private Dictionary<MeshFileType, Func<string, IMeshExporter>> MESH_EXPORTERS = new Dictionary<MeshFileType, Func<string, IMeshExporter>>() {
			{MeshFileType.OBJ, s => new OBJExporter(s)},
			{MeshFileType.STL, s => new STLExporter(s)},
		};

		public SingleFileProcessor (string ctlPath, string outputPath, float isoValue, MeshFileType meshFileType)
			: this(ctlPath, outputPath, new float[] {isoValue}, meshFileType)
		{
		}

		public SingleFileProcessor (string ctlPath, string outputPath, float[] isoValues, MeshFileType meshFileType)
		{
			_meshFileType = meshFileType;
			_ctlPath = ctlPath;
			_meshPath = getMeshPath(outputPath, ctlPath);
			_isoValues = isoValues;
			
			if (!Directory.Exists(outputPath)) {
				Directory.CreateDirectory(outputPath);
			}
		}

		private string getMeshPath(string outputPath, string ctlFilename) {
			var meshFilename = Path.Combine(
				outputPath,
				Path.GetFileNameWithoutExtension(ctlFilename)
				);
			meshFilename += MESH_FILE_EXTENSIONS[_meshFileType];
			
			return meshFilename;
		}

		public void Process ()
		{
			var sw = new Stopwatch ();
			sw.Start ();
			var reader = new GradsFileReader (_ctlPath);
			using (var data = reader.ReadData()) {
				sw.Stop ();
				Debug.WriteLine ("Read/parsed GrADS file. ({0}[ms])", sw.ElapsedMilliseconds);
			
				sw.Restart();
				using (var mc = new MarchingCubes(data)) {
					sw.Stop();
					Debug.WriteLine("Initialized isosurface generator. ({0}[ms])", sw.ElapsedMilliseconds);

					using (var exporter = MESH_EXPORTERS[_meshFileType](_meshPath)) {
						foreach (var isoValue in _isoValues) {
								sw.Restart();
								mc.UpdateIsosurfaceValue(isoValue);
								var mesh = mc.CalculateIsosurface();
								sw.Stop();
								Debug.WriteLine("Generated isosurface for value = {1}. ({0}[ms])", sw.ElapsedMilliseconds, isoValue);
								
								sw.Restart();
								exporter.Export(mesh, isoValue);
								sw.Stop();
								Debug.WriteLine("Wrote isosurface mesh data to \"{1}\". ({0}[ms])", sw.ElapsedMilliseconds, _meshPath);
								Debug.WriteLine("Resulted in {0} triangles.", mesh.Count);
						}
					}
				}
			}
		}
	}
}

