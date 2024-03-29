﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Coordinates
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Coordinates() { }
        
        public Coordinates(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static implicit operator Coordinates(string value)
        {
            string[] split = value.Split(',', ';', StringSplitOptions.TrimEntries);
            double x = double.Parse(split[0]);
            double y = double.Parse(split[1]);
            return new(x, y);
        }

        public double Distance(Coordinates other)
        {
            double dX = X - other.X, dY = Y - other.Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }

        public static byte[] ToBytes(Coordinates coordinates)
        {
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream);
            writer.Write(coordinates.X);
            writer.Write(coordinates.Y);
            return stream.ToArray();
        }

        public static Coordinates FromBytes(byte[] bytes)
        {
            using MemoryStream stream = new(bytes);
            using BinaryReader reader = new(stream);
            double X = reader.ReadDouble();
            double Y = reader.ReadDouble();
            Coordinates result = new(X, Y);
            return result;
        }

        public override string ToString()
        {
            return $"(X: {X}; Y: {Y})";
        }
    }
}
