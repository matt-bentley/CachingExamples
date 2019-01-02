﻿using System;
using CachingExamples.Cache.Serialization.Abstract;

namespace CachingExamples.Cache.Serialization.Serializers
{
    /// <summary>
    /// Null implementaion of <see cref="ISerializer"/>
    /// </summary>
    public class NullSerializer : ISerializer
    {
        public SerializationFormat Format
        {
            get { return SerializationFormat.None; }
        }

        public object Deserialize(Type type, object serializedValue)
        {
            return serializedValue;
        }

        public object Serialize(object returnValue)
        {
            return returnValue;
        }
    }
}
