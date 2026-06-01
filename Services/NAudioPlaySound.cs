using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SoundEffect.Services
{
	internal class NAudioPlaySound : IPlaySound, IDisposable
	{
		private WasapiOut _wavePlayer;

		public float Volume { get; set; } = 100f;

		public void Dispose()
		{
			_wavePlayer?.Dispose();
		}

		public async Task PlaySound(FileInfo soundFile, CancellationToken cancellationToken)
		{
			_wavePlayer?.Stop();
			_wavePlayer?.Dispose();

			_wavePlayer = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 100);
			_wavePlayer.Volume = Volume / 100f;

			using (AudioFileReader reader = new AudioFileReader(soundFile.FullName))
			{
				_wavePlayer.Init(reader);
				_wavePlayer.Play();
				try
				{
					while (_wavePlayer.PlaybackState == PlaybackState.Playing)
					{
						await Task.Delay(100, cancellationToken);
					}
				}
				catch (OperationCanceledException)
				{
					_wavePlayer.Stop();
				}
			}
		}
	}
}
