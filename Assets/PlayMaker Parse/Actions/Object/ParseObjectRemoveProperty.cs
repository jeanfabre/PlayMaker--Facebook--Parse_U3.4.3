// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Removes a property of an Object. Leave ObjectId to none to target the current user. To save changes online, use the action ParseObjectSaveAsync")]
	public class ParseObjectRemoveProperty : FsmStateAction
	{
		[RequiredField]
		public FsmString objectId;
		
		[RequiredField]
		public FsmString propertyKey;

		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			objectId = new FsmString {UseVariable=true};
			propertyKey = null;
			
			successEvent = null;
			errorEvent = null;
		}
		
		public override string ErrorCheck()
		{
			if (! PlayMakerParseProxy.IsPropertyKeyValid(propertyKey.Value))
			{
				return "Parse Property must only contain alphanumeric or underscore characters, and must begin with a letter";
			}
			return "";
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

			if (_object==null  || !_object.ContainsKey(propertyKey.Value))
			{
				Fsm.Event(errorEvent);
			}else{
				_object.Remove(propertyKey.Value);
				Fsm.Event(successEvent);
			}

			Finish();
			
		}
	}
}