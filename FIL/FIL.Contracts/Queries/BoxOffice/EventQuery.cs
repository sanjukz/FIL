﻿using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;
using System;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class EventQuery : IQuery<EventQueryResult>
    {
        public Guid AltId { get; set; }
    }
}