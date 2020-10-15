!function(a) { "function" == typeof define && define.amd ? define(["jquery", "moment"], a) : a(jQuery, moment) }(
    function(a, b) {
        var c = { 1: "١", 2: "٢", 3: "٣", 4: "٤", 5: "٥", 6: "٦", 7: "٧", 8: "٨", 9: "٩", 0: "٠" },
            d = { "١": "1", "٢": "2", "٣": "3", "٤": "4", "٥": "5", "٦": "6", "٧": "7", "٨": "8", "٩": "9", "٠": "0" };
        (b.defineLocale || b.lang).call(b,
            "ar-sa",
            {
                months: "يناير_فبراير_مارس_أبريل_مايو_يونيو_يوليو_أغسطس_سبتمبر_أكتوبر_نوفمبر_ديسمبر".split("_"),
                monthsShort: "يناير_فبراير_مارس_أبريل_مايو_يونيو_يوليو_أغسطس_سبتمبر_أكتوبر_نوفمبر_ديسمبر".split("_"),
                weekdays: "الأحد_الإثنين_الثلاثاء_الأربعاء_الخميس_الجمعة_السبت".split("_"),
                weekdaysShort: "أحد_إثنين_ثلاثاء_أربعاء_خميس_جمعة_سبت".split("_"),
                weekdaysMin: "ح_ن_ث_ر_خ_ج_س".split("_"),
                longDateFormat:
                {
                    LT: "HH:mm",
                    LTS: "HH:mm:ss",
                    L: "DD/MM/YYYY",
                    LL: "D MMMM YYYY",
                    LLL: "D MMMM YYYY LT",
                    LLLL: "dddd D MMMM YYYY LT"
                },
                meridiemParse: /ص|م/,
                isPM: function(a) { return"م" === a },
                meridiem: function(a, b, c) { return 12 > a ? "ص" : "م" },
                calendar: {
                    sameDay: "[اليوم على الساعة] LT",
                    nextDay: "[غدا على الساعة] LT",
                    nextWeek: "dddd [على الساعة] LT",
                    lastDay: "[أمس على الساعة] LT",
                    lastWeek: "dddd [على الساعة] LT",
                    sameElse: "L"
                },
                relativeTime: {
                    future: "في %s",
                    past: "منذ %s",
                    s: "ثوان",
                    m: "دقيقة",
                    mm: "%d دقائق",
                    h: "ساعة",
                    hh: "%d ساعات",
                    d: "يوم",
                    dd: "%d أيام",
                    M: "شهر",
                    MM: "%d أشهر",
                    y: "سنة",
                    yy: "%d سنوات"
                },
                preparse: function(a) {
                    return a.replace(/[١٢٣٤٥٦٧٨٩٠]/g, function(a) { return d[a] }).replace(/،/g, ",")
                },
                postformat: function(a) { return a.replace(/\d/g, function(a) { return c[a] }).replace(/,/g, "،") },
                week: { dow: 6, doy: 12 }
            }), a.fullCalendar.datepickerLang("ar-sa",
            "ar",
            {
                closeText: "إغلاق",
                prevText: "&#x3C;السابق",
                nextText: "التالي&#x3E;",
                currentText: "اليوم",
                monthNames:
                [
                    "كانون الثاني", "شباط", "آذار", "نيسان", "مايو", "حزيران", "تموز", "آب", "أيلول", "تشرين الأول",
                    "تشرين الثاني", "كانون الأول"
                ],
                monthNamesShort: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"],
                dayNames: ["الأحد", "الاثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت"],
                dayNamesShort: ["الأحد", "الاثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت"],
                dayNamesMin: ["ح", "ن", "ث", "ر", "خ", "ج", "س"],
                weekHeader: "أسبوع",
                dateFormat: "dd/mm/yy",
                firstDay: 6,
                isRTL: !0,
                showMonthAfterYear: !1,
                yearSuffix: ""
            }), a.fullCalendar.lang("ar-sa",
            {
                buttonText: { month: "شهر", week: "أسبوع", day: "يوم", list: "أجندة" },
                allDayText: "اليوم كله",
                eventLimitText: "أخرى"
            })
    });