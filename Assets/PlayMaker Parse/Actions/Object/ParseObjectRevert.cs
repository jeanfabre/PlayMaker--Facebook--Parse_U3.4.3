// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Clears any changes to a Parse Object since the last call to SaveAsynch. Leave ObjectId to none to target the current user. To save changes online, use the action ParseObjectSaveAsync")]
	public class ParseObjectRevert : FsmStateAction
	{
		[RequiredField]
		public FsmString objectId;
		
		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			objectId = new FsmString {UseVariable=true};
			
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

			if (_object ==null )
			{
				Fsm.Event(errorEvent);
			}else{
				_object.Revert();
				Fsm.Event(successEvent);
			}
			
			Finish();
			
		}
	}
}