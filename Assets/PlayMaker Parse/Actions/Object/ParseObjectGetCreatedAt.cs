// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Get the createdAt property of an Object. Leave ObjectId to none to target the current user.")]
	public class ParseObjectGetCreatedAt : FsmStateAction
	{
		
		[RequiredField]
		public FsmString objectId;
		
		[UIHint(UIHint.Variable)]
		public FsmString createdAt;
		
		public FsmString dateTimeFormat;
		
		[UIHint(UIHint.Variable)]
		public FsmFloat createdAtUnixStamp;
		
		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			objectId = new FsmString {UseVariable=true};
			createdAt = null;
			
			dateTimeFormat = "MMMM dd, yyyy, HH:mm";
			createdAtUnixStamp = null;

			successEvent = null;
			errorEvent = null;
		}
		
		public override void OnEnter()
		{
			ParseObject _object;
			
			if (objectId.IsNone)
			{
				_object = ParseUser.CurrentUser;
			}else{
				_object = PlayMakerParseProxy.GetParseObject(objectId.Value);
			}

			if ( _object==null )
			{
				Fsm.Event(errorEvent);
			}else{
				
				System.DateTime _createdAt =  (System.DateTime)_object.CreatedAt;
				
				if (! createdAt.IsNone)
				{
					createdAt.Value =_createdAt.ToString(dateTimeFormat.Value);
				}
				
				if (!createdAtUnixStamp.IsNone)
				{
					var date = new System.DateTime(1970, 1, 1, 0, 0, 0, _createdAt.Kind);
					var unixTimestamp = System.Convert.ToInt64((_createdAt - date).TotalSeconds);
					
					createdAtUnixStamp.Value = (float)unixTimestamp;
				}
				
				Fsm.Event(successEvent);
			}
			
			Finish();
			
		}
	}
}