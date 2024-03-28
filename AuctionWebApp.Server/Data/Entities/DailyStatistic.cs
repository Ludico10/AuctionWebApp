using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class DailyStatistic
{
    public DateOnly DsDate { get; set; }

    public uint DsVisitsCount { get; set; }

    public uint DsNewLotsCount { get; set; }

    public uint DsBidsCount { get; set; }

    public uint DsCompaintsCount { get; set; }

    public uint DsRegistrationsCount { get; set; }

    public uint DsQuestionsCount { get; set; }
}
