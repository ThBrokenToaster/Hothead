using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Abstract for an event, the event will trigger callback.NextEvent in EndEvent
 */
abstract public class EventAbstract {
	abstract public void BeginEvent(EventSequence callback);
	abstract public void EndEvent();
}
