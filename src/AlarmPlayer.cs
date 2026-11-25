using System.IO;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace WinClock
{
    /// <summary>
    /// 播放內建鈴聲（<see cref="SG.AlarmSounds"/>）。音量只套用在這次播放，不改變系統音量。
    /// 播放結束或呼叫 <see cref="Stop"/> 時觸發 <see cref="Stopped"/>。
    /// </summary>
    public sealed class AlarmPlayer : IDisposable
    {
        private WaveOutEvent? waveOut;
        private Mp3FileReader? reader;

        public event EventHandler? Stopped;

        public bool IsPlaying => waveOut is not null;

        /// <summary>播放鈴聲；正在播放時先停止。<paramref name="volume"/> 為 0–100。</summary>
        public void Play(string soundName, int volume)
        {
            Stop();
            if (!SG.AlarmSounds.TryGetValue(soundName, out var file)) return;

            reader = new Mp3FileReader(Path.Combine(AppContext.BaseDirectory, file));
            var channel = new SampleChannel(reader) { Volume = Math.Clamp(volume, 0, 100) / 100f };
            waveOut = new WaveOutEvent();
            waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
            waveOut.Init(channel);
            waveOut.Play();
        }

        public void Stop()
        {
            if (waveOut is null) return;
            waveOut.PlaybackStopped -= WaveOut_PlaybackStopped;
            waveOut.Stop();
            waveOut.Dispose();
            waveOut = null;
            reader?.Dispose();
            reader = null;
            Stopped?.Invoke(this, EventArgs.Empty);
        }

        private void WaveOut_PlaybackStopped(object? sender, StoppedEventArgs e) => Stop();

        public void Dispose() => Stop();
    }
}
