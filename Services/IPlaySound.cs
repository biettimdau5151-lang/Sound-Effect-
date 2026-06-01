using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SoundEffect.Services
{
	internal interface IPlaySound : IDisposable
	{
		float Volume { get; set; }

		Task PlaySound(FileInfo soundFile, CancellationToken cancellationToken);
	}
}
