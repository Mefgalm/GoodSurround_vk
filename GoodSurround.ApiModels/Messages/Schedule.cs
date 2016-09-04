using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodSurround.ApiModels.Messages
{
    public class Schedule
    {

        public string Title { get; set; }
        public ScheduleRow[] ScheduleRows { get; set; }
    }
}
