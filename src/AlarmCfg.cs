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
        private int second = 0;
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

        /// <summary>
        /// 秒。設定視窗不提供輸入（一律 0）；只有貪睡會設成非 0，讓「5 分鐘後」精確到秒。
        /// </summary>
        public int Second
        {
            get => second; set
            {
                second = value;
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
                AlarmDate.Year, AlarmDate.Month, AlarmDate.Day, Hour, Minute, Second);
        }

        /// <summary>
        /// 用一個時間點設定鬧鐘的日期、時、分、秒。
        /// </summary>
        public void SetTarget(DateTime target)
        {
            AlarmDate = target.Date;
            Hour = target.Hour;
            Minute = target.Minute;
            Second = target.Second;
        }

        /// <summary>
        /// 建立一份獨立的副本。設定視窗編輯副本，按「取消」時原本的鬧鐘不受影響。
        /// </summary>
        public AlarmCfg Clone()
        {
            return new AlarmCfg {
                AlarmDate = AlarmDate,
                Hour = Hour,
                Minute = Minute,
                Second = Second,
                IsPlaySound = IsPlaySound,
                SoundName = SoundName,
                IsShowMsg = IsShowMsg,
                MsgText = MsgText,
                IsEnabled = IsEnabled
            };
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
