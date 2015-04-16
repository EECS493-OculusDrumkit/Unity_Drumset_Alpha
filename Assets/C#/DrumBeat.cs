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

		private bool _playing;
		private float _durationRemaining;

		// TODO: I'm not sure how we're representing the Mya models yet
		// public Model Image { get; set; }

		// private because it will be changed on state change and state change only
		private BeatState _state;
		public BeatState State { 
			get { return this._state; }

			set 
			{
				this._state = value;
				SetObjectMaterial(GetMaterialFromState(this._state));
			}
		}
		public InstrumentType Instrument { get; set; }

		void Start () {
			Clear ();
		}
			
		// Grab the beat
		void OnTriggerEnter(Collider other){
			Play ();
		}

		// Update is called once per frame
		void Update () {
			//PLAY STATE
			//check to see if the Beat is playing, and if should be stopped
			if (_playing){
				// Reduce the amount of time left to play
				_durationRemaining -= Time.deltaTime;
				if (_durationRemaining <= 0) {
					_playing = false;
					_durationRemaining = 0;
					State = BeatState.Occupied;
				}
			}
		}

		public void Play(float durationSeconds)
		{
			if (HasInstrument())
			{
				DurationMs = (int)(durationSeconds * 1000);
				_playing = true;
				_durationRemaining = durationSeconds;
				DrumMachine.GetInstance().Play(Instrument, DurationMs, Velocity);
				State = BeatState.Active;
			}
		}

		public void Play()
		{
			if (HasInstrument())
			{
				_playing = true;
				_durationRemaining = DurationMs / 1000.0f;
				DrumMachine.GetInstance().Play(Instrument, DurationMs, Velocity);
				State = BeatState.Active;
			}
		}

		private Material GetMaterialFromState(BeatState state) {
			switch (state) {
			case BeatState.Empty:
				return EmptyMaterial;
			case BeatState.Hovered:
				return HoverMaterial;
			case BeatState.Occupied:
				return OccupiedMaterial;
			case BeatState.Active:
				return ActiveMaterial;
			default:
				throw new NotSupportedException("Invalid beat state");
			}
		}

		private void SetObjectMaterial(Material material) {
			this.GetComponent<Renderer> ().material = material;
		}

		public bool HasInstrument()
		{
			return !Instrument.Equals(InstrumentType.None);
		}
		
		public void Clear()
		{
			State = BeatState.Empty;
			Instrument = InstrumentType.None;
//			Instrument = AssignRandomInstrument((new System.Random()).Next(0,6));
			DurationMs = 500;
			Velocity = 127;
		}

		private InstrumentType AssignRandomInstrument(int instrument) {
			switch (instrument) {
			case 0:
				return InstrumentType.None;
			case 1:
				return InstrumentType.BassDrum;
			case 2:
				return InstrumentType.SnareDrum;
			case 3:
				return InstrumentType.TomDrum;
			case 4:
				return InstrumentType.CrashCymbal;
			case 5:
				return InstrumentType.HiHatCymbal;
			case 6:
				return InstrumentType.Cowbell;
			default:
				throw new NotSupportedException("Invalid beat state");
			}
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