﻿using UnityEngine;
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
			State = BeatState.Empty;
			Instrument = InstrumentType.None;
			DurationMs = 0;
			Velocity = 127;
		}

		// Update is called once per frame
		void Update () {
			//PLAY STATE
			//check to see if the Beat is playing, and should be stopped
			if (_playing){
				// Reduce the amount of time left to play
				_durationRemaining -= Time.deltaTime;
				if (_durationRemaining <= 0) {
					_playing = false;
					_durationRemaining = 0;
					this.State = BeatState.Occupied;
				}
			}
		}

		public void Play(float duration)
		{
			if (HasInstrument())
			{
				DurationMs = (int)(duration * 1000);
				_playing = true;
				_durationRemaining = duration;
				DrumMachine.GetInstance().Play(Instrument, DurationMs, Velocity);
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