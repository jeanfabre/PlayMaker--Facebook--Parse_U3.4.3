// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Get the data available flag of a Parse Object. Leave ObjectId to none to target the current user.")]
	public class ParseObjectGetIsDataAvailable : FsmStateAction
	{
		[RequiredField]
		public FsmString objectId;
		
		public FsmBool isDataAvailable;
		
		public FsmEvent dataIsAvailableEvent;
		public FsmEvent dataIsNotAvailableEvent;
		
		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			objectId = new FsmString {UseVariable=true};
			isDataAvailable = null;
			
			dataIsAvailableEvent = null;
			dataIsNotAvailableEvent = null;
			
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
			
			if (_object==null )
			{
				Fsm.Event(errorEvent);
			}else{
				
				bool _isDataAvailable = _object.IsDataAvailable;
				isDataAvailable.Value = _isDataAvailable;
				
				if (_isDataAvailable)
				{
					Fsm.Event(dataIsAvailableEvent);
				}else{
					Fsm.Event(dataIsNotAvailableEvent);
				}
				Fsm.Event(successEvent);
			}
			
			Finish();
			
		}
	}
}