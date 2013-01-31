using System;
using NUnit.Framework;

using IsosurfaceGenerator.Utils;

namespace IsosurfaceGenerator
{
	[TestFixture()]
	public class VectorTest
	{
		[Test()]
		public void TestDot ()
		{
			var vec1 = new Vec3(1.0f, 2.0f, 3.0f);
			var vec2 = new Vec3(3.0f, 2.0f, 1.0f);

			Assert.That(vec1.Dot(vec2), Is.EqualTo(10.0f));
		}

		[Test()]
		public void TestCross ()
		{
			var vec1 = new Vec3(1.0f, 0.0f, 0.0f);
			var vec2 = new Vec3(0.0f, 1.0f, 0.0f);
			var vec3 = new Vec3(0.0f, 0.0f, 1.0f);
			
			Assert.That(vec1.Cross(vec2), Is.EqualTo(vec3));
		}
	}
}

