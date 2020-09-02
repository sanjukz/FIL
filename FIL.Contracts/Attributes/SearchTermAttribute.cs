using System;

namespace FIL.Contracts.Attributes
{
    public class SearchTermAttribute : Attribute
    {
        public string Term { get; set; }

        public SearchTermAttribute(string term)
        {
            Term = term;
        }
    }
}