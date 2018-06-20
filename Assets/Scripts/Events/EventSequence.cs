using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSequence {
	public EventAbstract[] events;
	private int index;

	public EventSequence(EventAbstract[] events) {
		this.events = events;
	}

	public void StartSequence() {
		index = 0;
		events[0].BeginEvent(this);
	}

	public void NextEvent() {
		index++;
		if (index < events.Length) {
			events[index].BeginEvent(this);
		} else {
			GameManager.instance.eventManager.EndEventSequence();
		}
	}
}
