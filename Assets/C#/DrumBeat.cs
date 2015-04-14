using UnityEngine;
using System.Collections;
using System;
using DrumKitUtil;

namespace BeatsByDre
{
	public class DrumBeat : MonoBehaviour 
	{
		public int Velocity { get; set; }
		public int DurationMs { get; set; }
		public Material EmptyMaterial;
		public Material HoverMaterial;
		public Material OccupiedMaterial;
		public Material ActiveMaterial;

		private bool Playing;
		private float DurationRemaining;

		// TODO: I'm not sure how we're representing the Mya models yet
		// public Model Image { get; set; }
		// private because it will be changed on state change and state change only
		private BeatState _state;
		public BeatState State { 
			get { return this._state; }

			set 
			{
				this._state = value;
//				SetObjectColor(GetColorFromState(this._state));
			}
		}
		public InstrumentType Instrument { get; set; }

//		public DrumBeat(InstrumentType beatType, int durationMs, int velocity = 127)
//		{
//			Instrument = beatType;
//			DurationMs = durationMs;
//			Velocity = velocity;
//		}

		void Start () {
			State = BeatState.Empty;
			Instrument = InstrumentType.None;
			DurationMs = 0;
			Velocity = 127;
		}

		// Update is called once per frame
		void Update () {
			//PLAY STATE
			//check to see if the Beat is playing, and should be stopped
			if (Playing){
			    DurationRemaining -= Time.deltaTime;
			    if (DurationRemaining <= 0) {
					Playing = false;
					DurationRemaining = 0;
					this.State = BeatState.Occupied;
				}
			}

		}


		public void Play(float duration)
		{
			if (HasInstrument())
			{
				Playing = true;
				DurationRemaining = duration;
				DrumMachine.GetInstance().Play(Instrument, DurationMs, Velocity);
			}
		}

//		private Color GetColorFromState(BeatState state) {
//			switch (state) {
//			case BeatState.Empty:
//				break;
//			case BeatState.Hovered:
//				break;
//			case BeatState.Occupied:
//				break;
//			default:
//				throw new NotSupportedException("Invalid beat state");
//			}
//		}

		private void SetObjectColor(Color color) {
			// TODO
		}

		public bool HasInstrument()
		{
			return !Instrument.Equals(InstrumentType.None);
		}
		
		public void Clear()
		{
			Instrument = InstrumentType.None;
		}
		
		public enum InstrumentType
		{
			None, BassDrum, SnareDrum, TomDrum, CrashCymbal, HiHatCymbal, Cowbell
		}

		public enum BeatState 
		{
			Empty, Hovered, Occupied, Active
		}
	}
}