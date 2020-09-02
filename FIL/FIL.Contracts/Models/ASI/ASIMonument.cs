using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.ASI
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string AppConfigVersion { get; set; }
        public string Comment { get; set; }
        public string Version { get; set; }
        public string Circle { get; set; }
        public long MonumentId { get; set; }
        public DateTime MaxDate { get; set; }
        public bool IsOptional { get; set; }
        public string Status { get; set; }
    }

    public class Data
    {
        public List<Item> Items { get; set; }
    }

    public class ASIMonument
    {
        public bool IsSuccess { get; set; }
        public Data Data { get; set; }
    }
}