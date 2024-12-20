namespace WinClock
{
    /// <summary>
    /// 鬧鐘設定檔
    /// </summary>
    public class AlarmCfg : ObservableObject
    {
        private DateTime alarmDate = DateTime.Today;
        private int hour = DateTime.Now.Hour;
        private int minute = DateTime.Now.Minute;
        private bool isPlaySound = false;
        private string? soundName = null;
        private bool isShowMsg = true;
        private string? msgText = "鬧鈴時間到";
        private bool isEnabled = false;

        public DateTime AlarmDate
        {
            get => alarmDate;
            set
            {
                alarmDate = value;
                OnPropertyChanged();
            }
        }

        public int Hour
        {
            get => hour; set
            {
                hour = value;
                OnPropertyChanged();
            }
        }

        public int Minute
        {
            get => minute; set
            {
                minute = value;
                OnPropertyChanged();
            }
        }

        public bool IsPlaySound
        {
            get => isPlaySound;
            set
            {
                isPlaySound = value; OnPropertyChanged();
            }
        }

        public string? SoundName
        {
            get => soundName; set
            {
                soundName = value;
                OnPropertyChanged();
            }
        }

        public bool IsShowMsg
        {
            get => isShowMsg; set
            {
                isShowMsg = value;
                OnPropertyChanged();
            }
        }

        public string? MsgText
        {
            get => msgText; set
            {
                msgText = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => isEnabled; set
            {
                isEnabled = value;
                OnPropertyChanged();
            }
        }

        //----

        public DateTime GetTargetDateTime()
        {
            return new DateTime(
                AlarmDate.Year, AlarmDate.Month, AlarmDate.Day, Hour, Minute, 0);
        }

        public TimeSpan GetTimeSpan()
        {
            return GetTargetDateTime() - DateTime.Now;
        }

        public int GetMilliseconds()
        {
            return (int)GetTimeSpan().TotalMilliseconds;
        }

        public void AddMinutes(int minutes)
        {
            bool ca = Minute + minutes >= 60;
            Minute = (Minute + minutes) % 60;
            if (ca) Hour++;
            if (Hour >= 24) {
                Hour = 0;
                AlarmDate = AlarmDate.AddDays(1);
            }
        }
    }
}
