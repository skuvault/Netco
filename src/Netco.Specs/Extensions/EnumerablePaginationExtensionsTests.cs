#pragma warning disable 1998
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Netco.Extensions;
using Ploeh.AutoFixture;

namespace Netco.Specs.Extensions.EnumerablePaginationExtensionsTestsContainer
{
	[ TestFixture ]
	public class EnumerablePaginationExtensionsTests
	{
		protected Fixture f;

		[ SetUp ]
		public void Init()
		{
			this.f = new Fixture();
		}

		public class ProcessWithPagesMethod : EnumerablePaginationExtensionsTests
		{
			[ Test ]
			public void SeveralPagesWithSameValueProcessor_ResultsUnioned()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >();
				//------------ Act
				var result = items.ProcessWithPages( 3, l => new List< int >( l ) );
				//------------ Assert
				result.Should().Equal( items );
			}

			[ Test ]
			public void SeveralPagesWithConstantValueProcessor_ResultsUnioned()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >( 8 );
				//------------ Act
				var result = items.ProcessWithPages( 3, l => new List< string > { "test" } );
				//------------ Assert
				var expected = Enumerable.Repeat( "test", 3 );
				result.Should().Equal( expected );
			}

			[ Test ]
			public void PageSmallerThanOnePage_NormalProcessing()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >( 8 );
				//------------ Act
				var result = items.ProcessWithPages( 10, l => new List< string > { "test" } );
				//------------ Assert
				var expected = Enumerable.Repeat( "test", 1 );
				result.Should().Equal( expected );
			}

			[ Test ]
			public void ProcessorReturnsNullForPage_NullResultIgnored()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >( 8 );
				//------------ Act
				var result = items.ProcessWithPages( 3, l => l.Count() == 3 ? null : new List< string > { "test" } );
				//------------ Assert
				var expected = Enumerable.Repeat( "test", 1 );
				result.Should().Equal( expected );
			}
		}

		public class ProcessWithPagesAsyncMethod : EnumerablePaginationExtensionsTests
		{
			[ Test ]
			public async Task SeveralPagesWithSameValueProcessor_ResultsUnioned()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >();
				//------------ Act
				var result = await items.ProcessWithPagesAsync< int, int >( 3, async l => new List< int >( l ) );
				//------------ Assert
				result.Should().Equal( items );
			}

			[ Test ]
			public async Task SeveralPagesWithConstantValueProcessor_ResultsUnioned()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >( 8 );
				//------------ Act
				var result = await items.ProcessWithPagesAsync< int, string >( 3, async l => new List< string > { "test" } );
				//------------ Assert
				var expected = Enumerable.Repeat( "test", 3 );
				result.Should().Equal( expected );
			}

			[ Test ]
			public async Task PageSmallerThanOnePage_NormalProcessing()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >( 8 );
				//------------ Act
				var result = await items.ProcessWithPagesAsync< int, string >( 10, async l => new List< string > { "test" } );
				//------------ Assert
				var expected = Enumerable.Repeat( "test", 1 );
				result.Should().Equal( expected );
			}

			[ Test ]
			public async Task ProcessorReturnsNullForPage_NullResultIgnored()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >( 8 );
				//------------ Act
				var result = await items.ProcessWithPagesAsync< int, string >( 3, async l => l.Count() == 3 ? null : new List< string > { "test" } );
				//------------ Assert
				var expected = Enumerable.Repeat( "test", 1 );
				result.Should().Equal( expected );
			}
		}

		public class DoWithPagesMethod : EnumerablePaginationExtensionsTests
		{
			[ Test ]
			public void SeveralPagesWithSameValueProcessor_ResultsUnioned()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >();
				var result = new ConcurrentBag< int >();
				//------------ Act
				items.DoWithPages( 3, l => l.ForEach( result.Add ) );
				//------------ Assert
				result.Should().Contain( items );
			}

			[ Test ]
			public void SeveralPagesWithConstantValueProcessor_ResultsUnioned()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >( 8 );
				var result = new ConcurrentBag< string >();
				//------------ Act
				items.DoWithPages( 3, l => result.Add( "test" ) );
				//------------ Assert
				var expected = Enumerable.Repeat( "test", 3 );
				result.Should().Equal( expected );
			}

			[ Test ]
			public void PageSmallerThanOnePage_NormalProcessing()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >( 8 );
				var result = new ConcurrentBag< string >();
				//------------ Act
				items.DoWithPages( 10, l => result.Add( "test" ) );
				//------------ Assert
				var expected = Enumerable.Repeat( "test", 1 );
				result.Should().Equal( expected );
			}
		}

		public class DoWithPagesMethodAsync : EnumerablePaginationExtensionsTests
		{
			[ Test ]
			public async Task SeveralPagesWithSameValueProcessor_ResultsUnioned()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >();
				var result = new ConcurrentBag< int >();
				//------------ Act
				await items.DoWithPagesAsync( 3, async l => l.ForEach( result.Add ) );
				//------------ Assert
				result.Should().Contain( items );
			}

			[ Test ]
			public async Task SeveralPagesWithConstantValueProcessor_ResultsUnioned()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >( 8 );
				var result = new ConcurrentBag< string >();
				//------------ Act
				await items.DoWithPagesAsync( 3, async l => result.Add( "test" ) );
				//------------ Assert
				var expected = Enumerable.Repeat( "test", 3 );
				result.Should().Equal( expected );
			}

			[ Test ]
			public async Task PageSmallerThanOnePage_NormalProcessing()
			{
				//------------ Arrange
				var items = this.f.CreateMany< int >( 8 );
				var result = new ConcurrentBag< string >();
				//------------ Act
				await items.DoWithPagesAsync( 10, async l => result.Add( "test" ) );
				//------------ Assert
				var expected = Enumerable.Repeat( "test", 1 );
				result.Should().Equal( expected );
			}
		}
	}
}
#pragma warning restore 1998