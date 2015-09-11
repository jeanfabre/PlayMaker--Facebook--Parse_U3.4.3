// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using Parse;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Parse")]
	[Tooltip("Get the dirty flag of a Parse Object. Leave ObjectId to none to target the current user.")]
	public class ParseObjectGetIsDirty : FsmStateAction
	{
		[RequiredField]
		public FsmString objectId;
		
		public FsmBool isDirty;

		public FsmEvent isDirtyEvent;
		public FsmEvent isNotDirtyEvent;

		public FsmEvent successEvent;
		public FsmEvent errorEvent;
		
		public override void Reset()
		{
			objectId = new FsmString {UseVariable=true};
			isDirty = null;

			isDirtyEvent = null;
			isNotDirtyEvent = null;

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

				bool _isDirty = _object.IsDirty;
				isDirty.Value = _isDirty;

				if (_isDirty)
				{
					Fsm.Event(isDirtyEvent);
				}else{
					Fsm.Event(isNotDirtyEvent);
				}
				Fsm.Event(successEvent);
			}
			
			Finish();
			
		}
	}
}