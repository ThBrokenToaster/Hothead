using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EventAbstract {
	abstract public void BeginEvent(EventSequence callback);
	abstract public void EndEvent();
}
