﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEE
{
    public class Reminder
    {
        public Reminder()
        {
            DeleteStatus = false;
        }
        public int ID { get; set; }
        public string Title { get; set; }
        public string ReminderInfo { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime ReminderDate { get; set; }
        public Nullable<bool> IsDone { get; set; }
        public bool DeleteStatus { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}
