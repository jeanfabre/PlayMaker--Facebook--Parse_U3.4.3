// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Get the updateAt property of an Object. Leave ObjectId to none to target the current user.")]
	public class ParseObjectGetUpdatedAt : FsmStateAction
	{

		public FsmString objectId;

		[UIHint(UIHint.Variable)]
		public FsmString updatedAt;

		public FsmString dateTimeFormat;

		[UIHint(UIHint.Variable)]
		public FsmFloat updatedAtUnixStamp;

		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			objectId = new FsmString {UseVariable=true};
			updatedAt = null;

			dateTimeFormat = "MMMM dd, yyyy, HH:mm";
			updatedAtUnixStamp = null;

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

				System.DateTime _updatedAt =  (System.DateTime)_object.UpdatedAt;
					
				if (! updatedAt.IsNone)
				{
					updatedAt.Value =_updatedAt.ToString(dateTimeFormat.Value);
				}

				if (!updatedAtUnixStamp.IsNone)
				{
					var date = new System.DateTime(1970, 1, 1, 0, 0, 0, _updatedAt.Kind);
					var unixTimestamp = System.Convert.ToInt64((_updatedAt - date).TotalSeconds);

					updatedAtUnixStamp.Value = (float)unixTimestamp;
				}

				Fsm.Event(successEvent);
			}
			
			Finish();
			
		}
	}
}