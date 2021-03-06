﻿using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.EventCreation;
using System;

namespace FIL.Contracts.Queries.EventCreation
{
    public class SavedEventQuery : IQuery<SavedEventQueryResult>
    {
        public Guid Id { get; set; }
    }
}