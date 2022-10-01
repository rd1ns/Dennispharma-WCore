﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SkiTurkish.Model
{
    public class CalendarEvent
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string color { get; set; }
        public bool allDay { get; set; }
    }
}
