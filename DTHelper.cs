namespace WinClock
{
    public static class DTHelper
    {
        internal static readonly Dictionary<int, List<SolarTerm>> SolarTerms = new Dictionary<int, List<SolarTerm>>
        {
            {
                2024, new List<SolarTerm>
                {
                    new SolarTerm("小寒", 1, 6, 4, 47),
                    new SolarTerm("大寒", 1, 20, 22, 6),
                    new SolarTerm("立春", 2, 4, 16, 25),
                    new SolarTerm("雨水", 2, 19, 12, 12),
                    new SolarTerm("驚蟄", 3, 5, 10, 19),
                    new SolarTerm("春分", 3, 20, 11, 4),
                    new SolarTerm("清明", 4, 4, 14, 57),
                    new SolarTerm("穀雨", 4, 19, 21, 55),
                    new SolarTerm("立夏", 5, 5, 8, 3),
                    new SolarTerm("小滿", 5, 20, 20, 53),
                    new SolarTerm("芒種", 6, 5, 12, 2),
                    new SolarTerm("夏至", 6, 21, 4, 43),
                    new SolarTerm("小暑", 7, 6, 22, 11),
                    new SolarTerm("大暑", 7, 22, 15, 34),
                    new SolarTerm("立秋", 8, 7, 7, 59),
                    new SolarTerm("處暑", 8, 22, 22, 43),
                    new SolarTerm("白露", 9, 7, 11, 0),
                    new SolarTerm("秋分", 9, 22, 20, 31),
                    new SolarTerm("寒露", 10, 8, 2, 49),
                    new SolarTerm("霜降", 10, 23, 6, 4),
                    new SolarTerm("立冬", 11, 7, 6, 12),
                    new SolarTerm("小雪", 11, 22, 3, 49),
                    new SolarTerm("大雪", 12, 6, 23, 12),
                    new SolarTerm("冬至", 12, 21, 17, 18)
                }
            },
            {
                2025, new List<SolarTerm>
                {
                    new SolarTerm("小寒", 1, 5, 10, 31),
                    new SolarTerm("大寒", 1, 20, 4, 0),
                    new SolarTerm("立春", 2, 3, 22, 10),
                    new SolarTerm("雨水", 2, 18, 18, 5),
                    new SolarTerm("驚蟄", 3, 5, 16, 4),
                    new SolarTerm("春分", 3, 20, 16, 57),
                    new SolarTerm("清明", 4, 4, 20, 42),
                    new SolarTerm("穀雨", 4, 20, 3, 47),
                    new SolarTerm("立夏", 5, 5, 13, 48),
                    new SolarTerm("小滿", 5, 21, 2, 44),
                    new SolarTerm("芒種", 6, 5, 17, 47),
                    new SolarTerm("夏至", 6, 21, 10, 33),
                    new SolarTerm("小暑", 7, 7, 3, 57),
                    new SolarTerm("大暑", 7, 22, 21, 23),
                    new SolarTerm("立秋", 8, 7, 13, 46),
                    new SolarTerm("處暑", 8, 23, 4, 31),
                    new SolarTerm("白露", 9, 7, 16, 49),
                    new SolarTerm("秋分", 9, 23, 2, 19),
                    new SolarTerm("寒露", 10, 8, 8, 39),
                    new SolarTerm("霜降", 10, 23, 11, 51),
                    new SolarTerm("立冬", 11, 7, 12, 3),
                    new SolarTerm("小雪", 11, 22, 9, 36),
                    new SolarTerm("大雪", 12, 7, 5, 5),
                    new SolarTerm("冬至", 12, 21, 23, 4)
                }
            },
            {
                2026, new List<SolarTerm>
                {
                    new SolarTerm("小寒", 1, 5, 16, 24),
                    new SolarTerm("大寒", 1, 20, 9, 46),
                    new SolarTerm("立春", 2, 4, 4, 3),
                    new SolarTerm("雨水", 2, 18, 23, 51),
                    new SolarTerm("驚蟄", 3, 5, 21, 58),
                    new SolarTerm("春分", 3, 20, 22, 41),
                    new SolarTerm("清明", 4, 5, 2, 35),
                    new SolarTerm("穀雨", 4, 20, 9, 31),
                    new SolarTerm("立夏", 5, 5, 19, 41),
                    new SolarTerm("小滿", 5, 21, 8, 28),
                    new SolarTerm("芒種", 6, 5, 23, 40),
                    new SolarTerm("夏至", 6, 21, 16, 16),
                    new SolarTerm("小暑", 7, 7, 9, 50),
                    new SolarTerm("大暑", 7, 23, 3, 7),
                    new SolarTerm("立秋", 8, 7, 19, 38),
                    new SolarTerm("處暑", 8, 23, 10, 16),
                    new SolarTerm("白露", 9, 7, 22, 41),
                    new SolarTerm("秋分", 9, 23, 8, 4),
                    new SolarTerm("寒露", 10, 8, 14, 31),
                    new SolarTerm("霜降", 10, 23, 17, 38),
                    new SolarTerm("立冬", 11, 7, 17, 54),
                    new SolarTerm("小雪", 11, 22, 15, 24),
                    new SolarTerm("大雪", 12, 7, 10, 55),
                    new SolarTerm("冬至", 12, 22, 4, 53)
                }
            },
            {
                2027, new List<SolarTerm>
                {
                    new SolarTerm("小寒", 1, 5, 22, 14),
                    new SolarTerm("大寒", 1, 20, 15, 33),
                    new SolarTerm("立春", 2, 4, 10, 1),
                    new SolarTerm("雨水", 2, 19, 5, 47),
                    new SolarTerm("驚蟄", 3, 6, 3, 52),
                    new SolarTerm("春分", 3, 21, 4, 37),
                    new SolarTerm("清明", 4, 5, 8, 25),
                    new SolarTerm("穀雨", 4, 20, 15, 23),
                    new SolarTerm("立夏", 5, 6, 1, 25),
                    new SolarTerm("小滿", 5, 21, 14, 14),
                    new SolarTerm("芒種", 6, 6, 5, 20),
                    new SolarTerm("夏至", 6, 21, 21, 57),
                    new SolarTerm("小暑", 7, 7, 15, 20),
                    new SolarTerm("大暑", 7, 23, 8, 44),
                    new SolarTerm("立秋", 8, 8, 1, 12),
                    new SolarTerm("處暑", 8, 23, 16, 0),
                    new SolarTerm("白露", 9, 8, 4, 17),
                    new SolarTerm("秋分", 9, 23, 14, 0),
                    new SolarTerm("寒露", 10, 8, 20, 20),
                    new SolarTerm("霜降", 10, 23, 23, 33),
                    new SolarTerm("立冬", 11, 7, 23, 45),
                    new SolarTerm("小雪", 11, 22, 21, 20),
                    new SolarTerm("大雪", 12, 7, 16, 40),
                    new SolarTerm("冬至", 12, 22, 10, 46)
                }
            },
            {
                2028, new List<SolarTerm>
                {
                    new SolarTerm("小寒", 1, 6, 4, 0),
                    new SolarTerm("大寒", 1, 20, 21, 28),
                    new SolarTerm("立春", 2, 4, 15, 39),
                    new SolarTerm("雨水", 2, 19, 11, 33),
                    new SolarTerm("驚蟄", 3, 5, 9, 32),
                    new SolarTerm("春分", 3, 20, 10, 23),
                    new SolarTerm("清明", 4, 4, 14, 9),
                    new SolarTerm("穀雨", 4, 19, 21, 12),
                    new SolarTerm("立夏", 5, 5, 7, 14),
                    new SolarTerm("小滿", 5, 20, 20, 8),
                    new SolarTerm("芒種", 6, 5, 11, 13),
                    new SolarTerm("夏至", 6, 21, 3, 55),
                    new SolarTerm("小暑", 7, 6, 21, 23),
                    new SolarTerm("大暑", 7, 22, 14, 46),
                    new SolarTerm("立秋", 8, 7, 7, 12),
                    new SolarTerm("處暑", 8, 22, 21, 54),
                    new SolarTerm("白露", 9, 7, 10, 15),
                    new SolarTerm("秋分", 9, 22, 19, 43),
                    new SolarTerm("寒露", 10, 8, 2, 7),
                    new SolarTerm("霜降", 10, 23, 5, 17),
                    new SolarTerm("立冬", 11, 7, 5, 32),
                    new SolarTerm("小雪", 11, 22, 3, 3),
                    new SolarTerm("大雪", 12, 6, 22, 34),
                    new SolarTerm("冬至", 12, 21, 16, 31)
                }
            },
            {
                2029, new List<SolarTerm>
                {
                    new SolarTerm("小寒", 1, 5, 9, 54),
                    new SolarTerm("大寒", 1, 20, 3, 13),
                    new SolarTerm("立春", 2, 3, 21, 33),
                    new SolarTerm("雨水", 2, 18, 17, 18),
                    new SolarTerm("驚蟄", 3, 5, 15, 26),
                    new SolarTerm("春分", 3, 20, 16, 8),
                    new SolarTerm("清明", 4, 4, 20, 2),
                    new SolarTerm("穀雨", 4, 20, 2, 57),
                    new SolarTerm("立夏", 5, 5, 13, 6),
                    new SolarTerm("小滿", 5, 21, 1, 52),
                    new SolarTerm("芒種", 6, 5, 17, 4),
                    new SolarTerm("夏至", 6, 21, 9, 41),
                    new SolarTerm("小暑", 7, 7, 3, 13),
                    new SolarTerm("大暑", 7, 22, 20, 32),
                    new SolarTerm("立秋", 8, 7, 13, 2),
                    new SolarTerm("處暑", 8, 23, 3, 41),
                    new SolarTerm("白露", 9, 7, 16, 4),
                    new SolarTerm("秋分", 9, 23, 1, 31),
                    new SolarTerm("寒露", 10, 8, 7, 55),
                    new SolarTerm("霜降", 10, 23, 11, 6),
                    new SolarTerm("立冬", 11, 7, 11, 20),
                    new SolarTerm("小雪", 11, 22, 8, 54),
                    new SolarTerm("大雪", 12, 7, 4, 22),
                    new SolarTerm("冬至", 12, 21, 22, 23)
                }
            },
        };
    }
}
