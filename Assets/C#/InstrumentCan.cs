using UnityEngine;
using System;
using System.Collections;

using InstrumentType = BeatsByDre.DrumBeat.InstrumentType;
using BeatState = BeatsByDre.DrumBeat.BeatState;

namespace BeatsByDre
{
	public class InstrumentCan : MonoBehaviour
	{
		public Material HoverMaterial;
		public Material OccupiedMaterial;

		// TODO: Maya Model
		// private Model _model;
		private InstrumentType _instrument;

		public InstrumentType Instrument
		{
			get { return _instrument; }
			set
			{
				_instrument = value;
				SetModelFromInstrument(_instrument);
			}
		}

		private BeatState _state;

		public BeatState State {
			get { return _state; }
			set 
			{
				_state = value;
				SetObjectMaterial (GetMaterialFromState (_state));
			}

		}

		// Use this for initialization
		void Start () {
			State = BeatState.Occupied;
		}

		// Update is called once per frame
		void Update () {

		}

		public void PreviewSound()
		{
			if (HasInstrument())
			{
				DrumMachine.GetInstance().Play(Instrument, 500, 100);
			}
		}

		public bool HasInstrument()
		{
			return !Instrument.Equals(InstrumentType.None);
		}

		private Material GetMaterialFromState(BeatState state) {
			switch (state) {
			case BeatState.Hovered:
				return HoverMaterial;
			case BeatState.Occupied:
				return OccupiedMaterial;
			default:
				throw new NotSupportedException("Invalid beat state");
			}
		}

		private void SetObjectMaterial(Material material) {
			this.GetComponent<Renderer> ().material = material;
		}

		// TODO: Implement
		private void SetModelFromInstrument(InstrumentType instrument)
		{
			// Set model in can according to instrument assigned
			switch (instrument)
			{
			case InstrumentType.BassDrum:
				break;
			case InstrumentType.SnareDrum:
				break;
			case InstrumentType.TomDrum:
				break;
			case InstrumentType.CrashCymbal:
				break;
			case InstrumentType.HiHatCymbal:
				break;
			case InstrumentType.Cowbell:
				break;
			default:
				throw new InvalidOperationException("Invalid beat type: " + instrument);
			}
		}
	}
}