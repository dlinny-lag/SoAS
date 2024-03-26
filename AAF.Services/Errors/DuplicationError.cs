using System;
using AAF.Services.AAFImport;
using AAF.Services.Differences;
using AAF.Services.Reports;
using AAFModel;
using Shared.Utils;

namespace AAF.Services.Errors
{
    public abstract class DuplicationError<T, TDiff> : IntegrityError
        where T:Declared
        where TDiff : Enum
    {
        private readonly DuplicationsHolder<T> duplications;
        protected static readonly bool IsReferenceable = typeof(Referenceable).IsAssignableFrom(typeof(T));
        protected DuplicationError(string id, T reference)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            duplications = new DuplicationsHolder<T>(id, reference);
        }

        public readonly string Id;

        public void UpdateReference(T resolvedReference)
        {
            duplications.Reference = resolvedReference;
        }

        public void AddDuplicates(params T[] duplicates)
        {
            duplications.AddDuplicates(duplicates);
        }

        protected abstract CollectionComparer<T, TDiff> Comparer { get; }

        public override Report Report()
        {
            duplications.Validate();
            Report retVal = new Report();
            if (IsReferenceable)
            {
                Referenceable reference = duplications.Reference as Referenceable;
                retVal.AppendRef(Id, reference.Type);
            }
            else
            {
                if (duplications.Reference is Position)
                    retVal.AppendPos(Id);
                else
                    retVal.Append(Id);
            }
            retVal.Append(" is defined multiple times in the files:").Append(TextLexem.NewLine);
            for(int i = 0; i < duplications.Count; i++)
            {
                if (i > 0)
                    retVal.Append(TextLexem.NewLine);
                retVal.AppendFile(duplications[i].File);
            }

            var diff = Comparer.AreSame(duplications);
            if (!diff.IsNone())
                retVal.Append(TextLexem.NewLine).AppendNotice("And declarations are different in: ").AppendCritical($"{diff}!");

            return retVal;
        }
    }
}