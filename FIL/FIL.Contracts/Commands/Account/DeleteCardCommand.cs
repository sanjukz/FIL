﻿using System;

namespace FIL.Contracts.Commands.Account
{
    public class DeleteCardCommand : BaseCommand
    {
        public Guid AltId { get; set; }
    }
}