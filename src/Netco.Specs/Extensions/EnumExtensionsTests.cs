using System;
using Netco.Extensions;
using NUnit.Framework;

namespace Netco.Specs.Extensions
{
	public class EnumExtensionsTests
	{
		[ Test ]
		public void ToEnum_WhenSourceRepresentedByWrongNumber()
		{
			//------------ Arrange
			var str = "123";

			//------------ Act
			TestDelegate testDelegate = () => str.ToValidEnum< TestEnum >();

			//------------ Assert	
			Assert.Throws< ArgumentException >( testDelegate );
		}

		[ Test ]
		public void ToEnum_WhenSourceRepresentedByCorrectNumber()
		{
			//------------ Arrange
			var str = "2";

			//------------ Act
			var parsed = str.ToValidEnum< TestEnum >();

			//------------ Assert	
			Assert.IsTrue( parsed == TestEnum.Value2 );
		}

		[ Test ]
		public void ToEnum_WhenSourceRepresentedByWrongString()
		{
			//------------ Arrange
			var str = "value1df";

			//------------ Act
			TestDelegate testDelegate = () => str.ToValidEnum< TestEnum >();

			//------------ Assert	
			Assert.Throws< ArgumentException >( testDelegate );
		}

		[ Test ]
		public void ToEnum_WhenSourceRepresentedByCorrectString()
		{
			//------------ Arrange
			var str = "value1";

			//------------ Act
			var parsed = str.ToValidEnum< TestEnum >();

			//------------ Assert	
			Assert.IsTrue( parsed == TestEnum.Value1 );
		}

		[ Test ]
		public void ToEnumWithDefaultValue_WhenSourceRepresentedByWrongNumber()
		{
			//------------ Arrange
			var str = "123";

			//------------ Act
			var parsed = str.ToValidEnum( TestEnum.Undefined );

			//------------ Assert	
			Assert.IsTrue( parsed == TestEnum.Undefined );
		}

		[ Test ]
		public void ToEnumWithDefaultValue_WhenSourceRepresentedByCorrectNumber()
		{
			//------------ Arrange
			var str = "2";

			//------------ Act
			var parsed = str.ToValidEnum( TestEnum.Undefined );

			//------------ Assert	
			Assert.IsTrue( parsed == TestEnum.Value2 );
		}

		[ Test ]
		public void ToEnumWithDefaultValue_WhenSourceRepresentedByWrongString()
		{
			//------------ Arrange
			var str = "value1df";

			//------------ Act
			var parsed = str.ToValidEnum( TestEnum.Undefined );

			//------------ Assert	
			Assert.IsTrue( parsed == TestEnum.Undefined );
		}

		[ Test ]
		public void ToEnumWithDefaultValue_WhenSourceRepresentedByCorrectString()
		{
			//------------ Arrange
			var str = "value1";

			//------------ Act
			var parsed = str.ToValidEnum( TestEnum.Undefined );

			//------------ Assert	
			Assert.IsTrue( parsed == TestEnum.Value1 );
		}

		private enum TestEnum
		{
			Undefined = 0,
			Value1 = 1,
			Value2 = 2,
			Value3 = 3
		}
	}
}