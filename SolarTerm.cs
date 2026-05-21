namespace WinClock
{
    /// <summary>
    /// 節氣資料結構
    /// </summary>
    public class SolarTerm(string name, int month, int day, int hour, int minute)
    {
        /// <summary>
        /// 節氣名稱
        /// </summary>
        public string Name { get; set; } = name;
        /// <summary>
        /// 西元月份
        /// </summary>
        public int Month { get; set; } = month;
        /// <summary>
        /// 西元日期
        /// </summary>
        public int Day { get; set; } = day;

        /// <summary>
        /// 開始時
        /// </summary>
        public int Hour { get; set; } = hour;
        /// <summary>
        /// 開始分
        /// </summary>
        public int Minute { get; set; } = minute;
    }
}
