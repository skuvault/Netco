using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Netco.Monads
{
	/// <summary>
	/// Simplifies acessing a chain of properties when one of the property can be null. Instead of exception thrown <c>null</c> is still returned.
	/// </summary>
	/// <example>
	/// string code = licensePlate.MaybeDeep( lp => lp.Car.Owner.Address.PostCode );
	/// </example>
	/// <remarks>There's an overhead associated with dynamic unwrapping of accessing members into expressions.
	/// Avoid this for high performance sections.</remarks>
	/// <seealso href = "http://maybe.codeplex.com" />
	/// <seealso href = "http://blogs.msdn.com/alexj/archive/2008/03/03/maybe-there-is-more.aspx" />
	/// <seealso href = "http://blogs.developpeur.org/miiitch/archive/2008/02/29/vendredi-c-est-expression-tree.aspx" />
	public static class Walk
	{
		/// <summary>
		/// Check the type to see if it's Nullable.
		/// </summary>
		/// <param name = "theType">Type to check.</param>
		/// <returns><c>true</c> if type is Nullable, <c>false</c> otherwise.</returns>
		/// <see href="http://davidhayden.com/blog/dave/archive/2006/11/26/IsTypeNullableTypeConverter.aspx"/>
		private static bool IsNullable( this Type theType )
		{
			return ( theType.IsGenericType && theType.GetGenericTypeDefinition().Equals( typeof( Nullable< > ) ) );
		}

		private static readonly MethodInfo _maybeMethod;

		/// <summary>
		/// 	For types that return a nullable
		/// </summary>
		private static readonly MethodInfo _maybeNullableMethod;

		///// <summary>
		///// For future version where you don't need 2 expressions on primitives?
		///// For types that return a primitive (not nullable)
		///// to convert them to nullable
		///// </summary>
		//private static readonly MethodInfo maybePrimitiveMethod;
		static Walk()
		{
			Expression< Func< object > > exp = () => MaybeShallow< object, object >( null, null );
			//var name = Member.Name(exp);
			var name = "MaybeShallow";

			_maybeMethod = typeof( Walk ).GetMethod( name, BindingFlags.Public | BindingFlags.Static );

			Expression< Func< int? > > expNullable = () => MaybeShallowNullable< object, int? >( null, null );
			name = "MaybeShallowNullable";

			_maybeNullableMethod = typeof( Walk ).GetMethod( name );

			//For future version where you don't need 2 expressions on primitives?
			//maybePrimitiveMethod = typeof(Walk).GetMethod(Member.Name(() => Walk.MaybeShallowPrimitive<object, int>(null, null)));
		}

		/// <summary>
		/// Wraps properties access that results in a nullable types
		/// </summary>
		/// <typeparam name = "TSource">Source type.</typeparam>
		/// <typeparam name = "TResult">Nullable result type</typeparam>
		/// <param name = "source">Source data.</param>
		/// <param name = "expression">Expression to access properties.</param>
		/// <returns>Nullable result experession.</returns>
		public static TResult MaybeShallowNullable< TSource, TResult >( this TSource source, Expression< Func< TSource, TResult > > expression )
			where TSource : class
			//			where V:struct
		{
			if( typeof( TResult ).IsNullable() == false )
				throw new ArgumentException( "MaybeShallowNullable is for nullable types" );
			if( source == null )
				return default( TResult );
			return expression.Compile()( source );
		}

		/// <summary>
		/// Wrapes properties shallow access for expression that returns nullable type.
		/// </summary>
		/// <typeparam name = "TSource">Source type.</typeparam>
		/// <typeparam name = "TResult">Nullable result type.</typeparam>
		/// <param name = "source">Source data.</param>
		/// <param name = "expression">Shallow {source.Property} access expression.</param>
		/// <returns>Result in nullable type.</returns>
		public static TResult? MaybeShallowPrimitive< TSource, TResult >( this TSource source, Expression< Func< TSource, TResult > > expression )
			where TSource : class
			where TResult : struct
		{
			if( typeof( TResult ).IsNullable() )
				throw new ArgumentException( "MaybeShallowPrimitive is not for nullables" );
			return source != null ? expression.Compile()( source ) : new TResult?();
		}

		/// <summary>
		/// Wraps deep access to <paramref name="source"/> properties.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <typeparam name="TCurry">The type of the curry.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="expression">The expression.</param>
		/// <param name="finalAccessor">The final accessor.</param>
		/// <returns>Results of using <paramref name="finalAccessor"/> acting on result of <paramref name="expression"/>.</returns>
		public static TResult? MaybeDeepToNullable< TSource, TCurry, TResult >( this TSource source, Expression< Func< TSource, TCurry > > expression, Func< TCurry, TResult > finalAccessor )
			where TSource : class
			where TResult : struct
			where TCurry : class
		{
			var curry = source.MaybeDeep( expression );
			if( curry != null )
				return finalAccessor( curry );
			return null;
		}

		/// <summary>
		/// 	Goes as deep as the expression so x.MaybeDeep&lt;Z,X>( x=>x.y.z)
		/// </summary>
		/// <typeparam name = "TSource"></typeparam>
		/// <typeparam name = "TResult"></typeparam>
		/// <param name = "t"></param>
		/// <param name = "ex"></param>
		/// <returns></returns>
		/// <seealso href="http://blogs.developpeur.org/miiitch/archive/2008/02/29/vendredi-c-est-expression-tree.aspx"/>
		public static TResult MaybeDeep< TSource, TResult >( this TSource t, Expression< Func< TSource, TResult > > ex )
			where TSource : class
		{
			if( typeof( TResult ).IsClass == false && typeof( TResult ).IsNullable() == false )
				throw new ArgumentException( "MaybeDeep does not support primitives" );
			// It handles only the case of the demo
			if( ex.Body is MemberExpression )
			{
				var memberEx = ConvertMemberToMethodCall( ex.Body as MemberExpression );

				var lambda = Expression.Lambda( memberEx, new[] { ex.Parameters[ 0 ] } );

				//Changed from as V to support things that are not classes
				return ( TResult )lambda.Compile().DynamicInvoke( new object[] { t } );
			}

			else if( ex.Body is NewExpression )
			{
//				var newExp = ( NewExpression )ex.Body;

				return ex.Compile()( t );
			}
			else
				throw new NotSupportedException( "Expression body type is not supported." );
		}

		/// <summary>
		/// 	Only goes one level in so x.MaybeShawllow(x=>x.y)
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <typeparam name = "TV"></typeparam>
		/// <param name = "t"></param>
		/// <param name = "selector"></param>
		/// <returns></returns>
		public static TV MaybeShallow< T, TV >( this T t, Func< T, TV > selector )
			where T : class
			where TV : class
		{
			return t == null ? null : selector( t );
		}

		/// <summary>
		/// 	This method converts a call from a member in method call. Basically:
		/// 	'. MaProp' becomes '. Maybe (p => p.MaProp)'
		/// </summary>
		/// <param name = "memberExpression"></param>
		/// <returns></returns>
		private static MethodCallExpression ConvertMemberToMethodCall( MemberExpression memberExpression )
		{
			Expression ex;

			// The recursive call is made here
			if( memberExpression.Expression is MemberExpression )
				ex = ConvertMemberToMethodCall( memberExpression.Expression as MemberExpression );
			else
				ex = memberExpression.Expression;

			// A Generic method retrieves the "Maybe"
			MethodInfo methodInfo = _maybeMethod;
			Type type;

			//default case is a property

			if( memberExpression.Member is PropertyInfo )
			{
				var prop = ( PropertyInfo )memberExpression.Member;
				type = prop.PropertyType;
			}
			else if( memberExpression.Member is FieldInfo )
			{
				var field = ( FieldInfo )memberExpression.Member;
				type = field.FieldType;
			}
			else
				throw new NotImplementedException( "Expression member type is not supported." );

			if( type.IsNullable() )
				methodInfo = _maybeNullableMethod.MakeGenericMethod( new[] { memberExpression.Member.DeclaringType, type } );
			else if( type.IsPrimitive )
			{
				//methodInfo = maybePrimitiveMethod.MakeGenericMethod(new Type[] { memberExpression.Member.DeclaringType, type });
				//For future version where you don't need 2 expressions on primitives?
				throw new NotSupportedException( "Primitives are not supported" );
			}
			else //class
			{
				// Obligatory passage: get a version of the standard method "Maybe"
				methodInfo = methodInfo.MakeGenericMethod(
					new[] { memberExpression.Member.DeclaringType, type } );
			}

			// Create a parameter to the lambda passed in parameter Maybe
			var p = Expression.Parameter( memberExpression.Member.DeclaringType, "p" );

			// Create the lambda
			var maybeLamba = Expression.Lambda(
				Expression.MakeMemberAccess( p, memberExpression.Member ),
				new[] { p } );

			// Create the call Maybe
			var result = Expression.Call(
				null, methodInfo,
				new[] { ex, maybeLamba } );
			return result;
		}
	}
}