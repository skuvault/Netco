using System.Collections.Generic;
using Machine.Specifications;
using Netco.Monads;

namespace Netco.Specs.Monads
{
	[ Subject( typeof( Maybe ) ) ]
	public class When_value_is_present_in_pipe_operations
	{
		private Establish context = () =>
			{
				_apple = new object();
				_orange = new object();
			};
		
		private Because of = () =>_salad = from a in _apple
							     from o in _orange
							     select Maybe.From( System.Tuple.Create( a, o ));

		private It should_have_result_with_value = () => _salad.HasValue.ShouldBeTrue();

		static Maybe< object > _apple;
		static Maybe< object > _orange;
		static Maybe< System.Tuple< object, object > > _salad;
	}

	[ Subject( typeof( Maybe ) ) ]
	public class When_value_is_null
	{
		private Establish context = () =>
			{
				_apple = new object();
				_orange = null;
			};

		private Because of = () =>_salad = from a in _apple
							     from o in _orange
							     select Maybe.From( System.Tuple.Create( a, o ));

		private It should_have_result_without_value = () => _salad.HasValue.ShouldBeFalse();

		static Maybe< object > _apple;
		static Maybe< object > _orange;
		static Maybe< System.Tuple< object, object > > _salad;
	}

	[ Subject( typeof( Maybe ) ) ]
	public class When_value_is_empty
	{
		private Establish context = () =>
			{
				_apple = new object();
				_orange = Maybe< object >.Empty;
			};

		private Because of = () =>
			{
				_salad = from a in _apple
				         from o in _orange
				         select Maybe.From( System.Tuple.Create( a, o ) );
			};

		private It should_have_result_without_value = () => _salad.HasValue.ShouldBeFalse();

		static Maybe< object > _apple;
		static Maybe< object > _orange;
		static Maybe< System.Tuple< object, object > > _salad;
	}

	[ Subject( "integration of Maybe with Dictionary" ) ]
	public class When_trying_to_get_existing_value
	{
		private Establish context = () =>
			{
				_dictionary = new Dictionary< string, object >{ { Key, _value }};
			};

		private Because of = () => _result = _dictionary.TryGetValue( Key );

		private It should_get_value_corresponding_to_key = () => _result.Value.ShouldBeTheSameAs( _value );

		private const string Key = "key";
		private static readonly object _value = new object();
		private static Dictionary< string, object > _dictionary;
		private static Maybe< object > _result;
	}

	[ Subject( "integration of Maybe with Dictionary" ) ]
	public class When_trying_to_get_non_existing_value
	{
		private Establish context = () =>
			{
				_dictionary = new Dictionary< string, object >();
			};

		private Because of = () => _result = _dictionary.TryGetValue( "someKey" );

		private It should_get_empty_value = () => _result.HasValue.ShouldBeFalse();

		private static Dictionary< string, object > _dictionary;
		private static Maybe< object > _result;
	}
}