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
//			_drumKit = new DrumKit("http://drumkit.ngrok.com");
			_drumKit = new DrumKit("http://localhost:5000");
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
				_drumKit.PlayBassDrum(velocity, durationMs);
				break;
			case InstrumentType.SnareDrum:
				_drumKit.PlaySnareDrum(velocity, durationMs);
				break;
			case InstrumentType.TomDrum:
				_drumKit.PlayTom(velocity, durationMs);
				break;
			case InstrumentType.CrashCymbal:
				_drumKit.PlayCrashCymbol(velocity, durationMs);
				break;
			case InstrumentType.HiHatCymbal:
				_drumKit.PlayHiHat(velocity, durationMs);
				break;
			case InstrumentType.Cowbell:
				_drumKit.PlayCowbell(velocity, durationMs);
				break;
			default:
				throw new InvalidOperationException("Invalid beat type: " + beatType);
			}
		}
	}
}

