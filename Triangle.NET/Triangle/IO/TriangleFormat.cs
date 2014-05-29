﻿// -----------------------------------------------------------------------
// <copyright file="TriangleFormat.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;

    /// <summary>
    /// Implements geometry and mesh file formats of the the original Triangle code.
    /// </summary>
    public class TriangleFormat : IPolygonFormat, IMeshFormat
    {
        public bool IsSupported(string file)
        {
            string ext = Path.GetExtension(file);

            if (ext == ".node" || ext == ".poly" || ext == ".ele")
            {
                return true;
            }

            return false;
        }

        public Mesh Import(string filename)
        {
            string ext = Path.GetExtension(filename);

            if (ext == ".node" || ext == ".poly" || ext == ".ele")
            {
                List<ITriangle> triangles;
                InputGeometry geometry;

                TriangleReader.Read(filename, out geometry, out triangles);

                if (geometry != null && triangles != null)
                {
                    var converter = new Converter();

                    return converter.ToMesh(geometry, triangles.ToArray());
                }
            }

            throw new NotSupportedException("Could not load '" + filename + "' file.");
        }

        public void Write(Mesh mesh, string filename)
        {
            TriangleWriter.WritePoly((Mesh)mesh, Path.ChangeExtension(filename, ".poly"));
            TriangleWriter.WriteElements((Mesh)mesh, Path.ChangeExtension(filename, ".ele"));
        }

        public void Write(Mesh mesh, StreamWriter stream)
        {
            throw new NotImplementedException();
        }

        public InputGeometry Read(string filename)
        {
            string ext = Path.GetExtension(filename);

            if (ext == ".node")
            {
                return TriangleReader.ReadNodeFile(filename);
            }

            if (ext == ".poly")
            {
                return TriangleReader.ReadPolyFile(filename);
            }

            throw new NotSupportedException("File format '" + ext + "' not supported.");
        }


        public void Write(InputGeometry polygon, string filename)
        {
            TriangleWriter.WritePoly(polygon, filename);
        }

        public void Write(InputGeometry polygon, StreamWriter stream)
        {
            throw new NotImplementedException();
        }
    }
}
