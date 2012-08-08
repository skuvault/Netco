#region (c)2009 Lokad - New BSD license

// Copyright (c) Lokad 2009 
// Company: http://www.lokad.com
// This code is released under the terms of the new BSD licence

#endregion

#if !SILVERLIGHT2
using System;
using System.IO;
using System.Xml.Serialization;
using Lokad.Quality;

namespace Lokad
{
	/// <summary>
	/// Simple static class that caches <see cref="XmlSerializer"/> instances.
	/// </summary>
	/// <typeparam name="TXml"></typeparam>
	[NoCodeCoverage]
	public static class XmlUtil<TXml>
	{
		static readonly XmlSerializer Serializer = new XmlSerializer(typeof (TXml));

		/// <summary> Serializes instance to the provided writer </summary>
		public static void Serialize(TXml instance, TextWriter writer)
		{
			Serializer.Serialize(writer, instance);
		}

		/// <summary> Serializes instance to the provided stream </summary>
		public static void Serialize(TXml instance, Stream stream)
		{
			Serializer.Serialize(stream, instance);
		}

		/// <summary>
		/// Serializes instance to the Xml string
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static string Serialize(TXml instance)
		{
			using (var writer = new StringWriter())
			{
				Serialize(instance, writer);
				writer.Flush();
				return writer.ToString();
			}
		}

		/// <summary>
		/// Helper method for testing - quickly creates object from string
		/// </summary>
		/// <param name="source">xml string</param>
		/// <returns></returns>
		public static TXml Deserialize(string source)
		{
			using (var reader = new StringReader(source))
			{
				return (TXml) Serializer.Deserialize(reader);
			}
		}

		/// <summary>
		/// Helper method to deserialize from the stream using
		/// the cached serializer.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static TXml Deserialize(Stream stream)
		{
			return (TXml) Serializer.Deserialize(stream);
		}
	}

	/// <summary>
	/// Helper class for the xml operations
	/// </summary>
	public static class XmlUtil
	{
		/// <summary>
		/// Helper method for testing - checks if the object can be serialized
		/// </summary>
		/// <typeparam name="TData">The type of the data.</typeparam>
		/// <param name="item">The item.</param>
		/// <returns>deserialized item</returns>
		public static TData TestXmlSerialization<TData>(TData item)
		{
			var ser = new XmlSerializer(typeof (TData));

			using (var stream = new MemoryStream())
			{
				ser.Serialize(stream, item);
				stream.Seek(0, SeekOrigin.Begin);
				var deserialize = (TData) ser.Deserialize(stream);
				Enforce.That(!deserialize.Equals(default(TData)), "deserialize!=default(T)");
				return deserialize;
			}
		}

		/// <summary>
		/// Helper method for testing - checks if the class can be serialized
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static void TestXmlSerialization<T>() where T : class, new()
		{
			TestXmlSerialization(new T());
		}

		/// <summary>
		/// Tests the XML serialization.
		/// </summary>
		/// <param name="type">The type.</param>
		public static void TestXmlSerialization(Type type)
		{
			try
			{
				var item = Activator.CreateInstance(type);

				var ser = new XmlSerializer(type);

				using (var stream = new MemoryStream())
				{
					ser.Serialize(stream, item);
					stream.Seek(0, SeekOrigin.Begin);
					var deserialized = ser.Deserialize(stream);
					Enforce.NotNull(() => deserialized);
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("XML Serialization not supported for " + type.FullName, ex);
			}
		}

		/// <summary>
		/// <see cref="XmlUtil{T}.Serialize(T, Stream)"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="instance"></param>
		public static void Serialize<T>(T instance, TextWriter stream) where T : new()
		{
			XmlUtil<T>.Serialize(instance, stream);
		}

		/// <summary>
		/// 	<see cref="XmlUtil{T}.Serialize(T, Stream)"/>
		/// </summary>
		/// <typeparam name="T">type of the item to serialize</typeparam>
		/// <param name="stream">The stream.</param>
		/// <param name="array">The array.</param>
		public static void SerializeArray<T>(T[] array, TextWriter stream) where T : new()
		{
			XmlUtil<T[]>.Serialize(array, stream);
		}

		/// <summary>
		/// <see cref="XmlUtil{T}.Serialize(T,Stream)"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="instance"></param>
		public static void Serialize<T>(T instance, Stream stream) where T : new()
		{
			XmlUtil<T>.Serialize(instance, stream);
		}

		/// <summary>
		/// 	<see cref="XmlUtil{T}.Serialize(T, Stream)"/>
		/// </summary>
		/// <typeparam name="T">type of the item to serialize</typeparam>
		/// <param name="stream">The stream.</param>
		/// <param name="array">The array.</param>
		public static void SerializeArray<T>(T[] array, Stream stream) where T : new()
		{
			XmlUtil<T[]>.Serialize(array, stream);
		}

		/// <summary>
		/// <see cref="XmlUtil{T}.Deserialize(Stream)"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static T Deserialize<T>(Stream stream) where T : new()
		{
			return XmlUtil<T>.Deserialize(stream);
		}

		/// <summary> Serializes the specified instance. </summary>
		/// <typeparam name="T">type of the item to serialize</typeparam>
		/// <param name="instance">The instance.</param>
		/// <returns>String representation</returns>
		public static string Serialize<T>(T instance) where T : new()
		{
			return XmlUtil<T>.Serialize(instance);
		}

		/// <summary>
		/// Serializes the specified instance to the specified file
		/// </summary>
		/// <typeparam name="TXml">The type of the XML-serializable object.</typeparam>
		/// <param name="instance">The instance.</param>
		/// <param name="fileName">Name of the file.</param>
		[NoCodeCoverage]
		public static void SerializeTo<TXml>(TXml instance, string fileName) 
			where TXml : new()
		{
			using (var writer = File.CreateText(fileName))
			{
				XmlUtil<TXml>.Serialize(instance, writer);
			}
		}

		/// <summary>
		/// Serializes the specified instance.
		/// </summary>
		/// <typeparam name="T">type of the item to serialize</typeparam>
		/// <param name="array">The array.</param>
		/// <returns>String representation</returns>
		public static string SerializeArray<T>(T[] array) where T : new()
		{
			return XmlUtil<T[]>.Serialize(array);
		}
	}
}

#endif