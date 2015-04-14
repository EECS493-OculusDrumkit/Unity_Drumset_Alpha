using System;
using DrumKitUtil;

using InstrumentType = BeatsByDre.DrumBeat.InstrumentType;

namespace BeatsByDre
{
	class DrumMachine
	{
		private static DrumMachine _instanceDrumMachine;
		private DrumKit _drumKit;
		
		private DrumMachine()
		{
			_drumKit = new DrumKit("http://drumkit.ngrok.com");
		}
		
		// Singleton getInstance operator
		public static DrumMachine GetInstance()
		{
			return _instanceDrumMachine ?? (_instanceDrumMachine = new DrumMachine());
		}
		
		public void Play(InstrumentType beatType, int durationMs, int velocity = 127)
		{
			switch (beatType)
			{
			case InstrumentType.None:
				break;
			case InstrumentType.BassDrum:
				_drumKit.PlayBassDrum(127, durationMs);
				break;
			case InstrumentType.SnareDrum:
				_drumKit.PlaySnareDrum(127, durationMs);
				break;
			case InstrumentType.TomDrum:
				_drumKit.PlayTom(127, durationMs);
				break;
			case InstrumentType.CrashCymbal:
				_drumKit.PlayCrashCymbol(127, durationMs);
				break;
			case InstrumentType.HiHatCymbal:
				_drumKit.PlayHiHat(127, durationMs);
				break;
			case InstrumentType.Cowbell:
				_drumKit.PlayCowbell(127, durationMs);
				break;
			default:
				throw new InvalidOperationException("Invalid beat type: " + beatType);
			}
		}
	}
}

