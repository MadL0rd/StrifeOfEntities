#pragma warning disable 612,618
#pragma warning disable 0114
#pragma warning disable 0108

using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
		public class LogEventRequest_upgradeGuestAccount : GSTypedRequest<LogEventRequest_upgradeGuestAccount, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_upgradeGuestAccount() : base("LogEventRequest"){
			request.AddString("eventKey", "upgradeGuestAccount");
		}
		
		public LogEventRequest_upgradeGuestAccount Set_username( string value )
		{
			request.AddString("username", value);
			return this;
		}
		
		public LogEventRequest_upgradeGuestAccount Set_displayName( string value )
		{
			request.AddString("displayName", value);
			return this;
		}
		
		public LogEventRequest_upgradeGuestAccount Set_password( string value )
		{
			request.AddString("password", value);
			return this;
		}
		
		public LogEventRequest_upgradeGuestAccount Set_email( string value )
		{
			request.AddString("email", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_upgradeGuestAccount : GSTypedRequest<LogChallengeEventRequest_upgradeGuestAccount, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_upgradeGuestAccount() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "upgradeGuestAccount");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_upgradeGuestAccount SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_upgradeGuestAccount Set_username( string value )
		{
			request.AddString("username", value);
			return this;
		}
		public LogChallengeEventRequest_upgradeGuestAccount Set_displayName( string value )
		{
			request.AddString("displayName", value);
			return this;
		}
		public LogChallengeEventRequest_upgradeGuestAccount Set_password( string value )
		{
			request.AddString("password", value);
			return this;
		}
		public LogChallengeEventRequest_upgradeGuestAccount Set_email( string value )
		{
			request.AddString("email", value);
			return this;
		}
	}
	
}
	

namespace GameSparks.Api.Messages {


}
