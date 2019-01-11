﻿using GeoGen.Core;
using GeoGen.Generator;
using GeoGen.Utilities;
using System.Collections.Generic;
using System.IO;

namespace GeoGen.ConsoleTest
{
    class ConsoleInconstructibleObjectsTracer : IInconstructibleObjectsTracer
    {
        private List<ConfigurationObject> _objects = new List<ConfigurationObject>();

        public void TraceInconstructibleObject(ConfigurationObject configurationObject)
        {
            _objects.Add(configurationObject);
        }

        public void WriteReport(TextWriter writer)
        {
            writer.WriteLine("This file contains all the objects that have been resolved as inconstructible during the generation.");
            writer.WriteLine();
            writer.WriteLine("The last object in each list is an inconstructible one.");
            writer.WriteLine();
            _objects.ForEach((obj,i) => writer.WriteLine($"{i+1}. {ToStringHelper.ObjectToString(obj, displayId: false)}"));
        }
    }
}