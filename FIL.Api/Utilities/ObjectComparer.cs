using ObjectsComparer;
using System;

namespace FIL.Api.Utilities
{
    public class ObjectComparersFactory : ComparersFactory
    {
        public override ObjectsComparer.IComparer<T> GetObjectsComparer<T>(ComparisonSettings settings = null, BaseComparer parentComparer = null)
        {
            var comparer = new ObjectsComparer.Comparer<T>(settings, parentComparer, this);

            //Do not compare Fields of Type Guid
            comparer.AddComparerOverride<Guid>(DoNotCompareValueComparer.Instance);

            //Do not compare fields of type dateTime
            comparer.AddComparerOverride<DateTime>(DoNotCompareValueComparer.Instance);

            //Do not compare field Id
            comparer.AddComparerOverride("Id", DoNotCompareValueComparer.Instance);

            return comparer;
        }
    }
}