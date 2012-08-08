using System;
using Machine.Specifications;
using Netco.Extensions;
using DateTimeExtensions = Netco.Extensions.DateTimeExtensions;

namespace Netco.Specs.Extensions
{
	[ Subject( typeof( DateTimeExtensions ) ) ]
	public class When_local_time_zone_is_eastern
	{
		private Establish context = () =>
			{
				DateTimeExtensions.SetLocalTimeZone( DateTimeExtensions.CommonTimeZone.EST );
				_testDateTime = DateTime.UtcNow;
			};

		private Because of = () => _result = _testDateTime.ToPresetLocal();

		private It should_convert_to_eastern_time = () => _result.ShouldEqual( _testDateTime - TimeSpan.FromHours( _result.IsDaylightSavingTime() ? 4 : 5 ) );

		private static DateTime _testDateTime;
		private static DateTime _result;
	}
}