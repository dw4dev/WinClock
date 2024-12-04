namespace WinClock
{
    /// <summary>
    /// 節氣資料結構
    /// </summary>
    public class SolarTerm
    {
        /// <summary>
        /// 節氣名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 西元月份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 西元日期
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 開始時
        /// </summary>
        public int Hour { get; set; }
        /// <summary>
        /// 開始分
        /// </summary>
        public int Minute { get; set; }

        public SolarTerm(string name, int month, int day, int hour, int minute)
        {
            Name = name;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
        }
    }
}
