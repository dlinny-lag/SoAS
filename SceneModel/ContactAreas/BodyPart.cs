using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared.Utils;

namespace SceneModel.ContactAreas
{
    public enum VisitResult
    {
        None,
        Removed,
        Replaced
    }

    public sealed class BodyPart : AreaReference
    {
        private static readonly BodyPart Root = new BodyPart(null){isTopRoot = true};
        private static readonly BodyPart None = new BodyPart(null);

        private bool isTopRoot = false;

        private BodyPart(ContactArea area) : base(area)
        {
            Parent = null;
        }

        private readonly List<BodyPart> children = null;

        public static BodyPart TopRoot(ContactArea area, params BodyPart[] children)
        {
            return new BodyPart(area, children) { isTopRoot = true };
        }
        public BodyPart(ContactArea area, params ContactArea[] children) : base(area ?? throw new ArgumentNullException(nameof(area)))
        {
            Parent = Root;
            this.children = new List<BodyPart>(children.Length);
            this.children.AddRange(children.Select(c => new BodyPart(c, None){Parent = this}));
        }
        public BodyPart(ContactArea area, params BodyPart[] children) : base(area ?? throw new ArgumentNullException(nameof(area)))
        {
            Parent = Root;
            this.children = new List<BodyPart>(children.Length);
            foreach (BodyPart child in children)
            {
                if (child == None)
                    break;
                if (child == Root)
                    throw new InvalidOperationException("Unable to add root element as a child");
                if (child.Parent != null && child.Parent != Root)
                    throw new InvalidOperationException("Reuse is not allowed");
                child.Parent = this;
                this.children.Add(child);
            }
        }

        public static BodyPart Leaf(ContactArea area) => new BodyPart(area, None);

        public BodyPart Parent { get; private set; }
        public BodyPart[] Children => children.ToArray();

        public BodyPart Find(ContactArea area, string reversePath = null)
        {
            if (Area == area)
                return this;

            if (string.IsNullOrWhiteSpace(reversePath))
            {
                foreach (var child in children)
                {
                    if (child.Area == area)
                        return child;
                    var retVal = child.Find(area);
                    if (retVal != null)
                        return retVal;
                }
            }
            else
            {
                // TODO: refactor along with Creature.Find
                var parts = reversePath.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries);
                BodyPart current = this;
                for (int i = parts.Length - 1; i >= 0; i--)
                {
                    current = current.Find(parts[i]);
                    if (current == null)
                        return null;
                }

                return current.Find(area, null);
            }
            return null;
        }

        private BodyPart Find(string areaId)
        {
            if (Area.Id == areaId)
                return this;
            foreach (BodyPart child in children)
            {
                var found = child.Find(areaId);
                if (found != null)
                    return found;
            }

            return null;
        }

        public BodyPart[] Parents
        {
            get
            {
                if (Parent == null || Parent.isTopRoot)
                    return Array.Empty<BodyPart>();
                List<BodyPart> retVal = new List<BodyPart>();

                BodyPart p = Parent;
                while (!p.isTopRoot)
                {
                    retVal.Add(p);
                    p = p.Parent;
                } 
                return retVal.ToArray();
            }
        }

        public static readonly char[] Delimiter = "/".ToCharArray();
        public string ReversePath // TODO: cache
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (BodyPart parent in Parents)
                {
                    if (sb.Length > 0)
                        sb.Append(Delimiter);
                    sb.Append(parent.Area.AsString());
                }
                return sb.ToString();
            }
        }

        public override string ToString()
        {
            return ToString(0);
        }


        public string ToString(uint indent)
        {
            StringBuilder sb = new StringBuilder();
            sb.Space(indent).AppendLine(Area.Id);
            foreach (BodyPart child in children)
            {
                sb.Append(child.ToString(indent + 1));
            }

            return sb.ToString();
        }

        public BodyPart Clone()
        {
            BodyPart retVal = new BodyPart(Area, children.Select(c => c.Clone()).ToArray());
            retVal.Parent = Root; // do not clone Parent
            return retVal;
        }

        /// <summary>
        /// Visits all elements from root to nested, from first to last and executes <see cref="process"/> on each element.
        /// if <see cref="process"/> removes processing element it must return <see cref="VisitResult.Removed"/>"/>
        /// if <see cref="process"/> replaces processing element it must return <see cref="VisitResult.Replaced"/>
        /// otherwise it must return <see cref="VisitResult.None"/>
        /// </summary>
        /// <param name="process"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        internal VisitResult VisitAll<T>(Func<BodyPart, T, VisitResult> process, T argument)
        {
            if (Parent == null) // Root or None
                return VisitResult.None;

            BodyPart parent = Parent;
            int indexInParent = Parent?.children?.IndexOf(this) ?? -1;

            VisitResult result = process(this, argument);
            if (result == VisitResult.Removed)
                return VisitResult.Removed; // do not proceed children of removed element
            
            void ProcessChildren(List<BodyPart> childrenList)
            {
                int len = childrenList.Count;
                for (int i = 0; i < len; i++)
                {
                    var child = childrenList[i];
                    VisitResult childResult = child.VisitAll(process, argument);
                    if (childResult == VisitResult.Removed)
                        i--; // element was removed so do not increment index on the next iteration
                
                    // we assume that new elements always have higher index than currently processing element
                    // so newly added elements will be processed by the next iteration

                    len = childrenList.Count; // recalculate len as the child element might be removed or replaced by multiple elements
                }
            }

            if (result == VisitResult.Replaced)
            {
                if (indexInParent < 0)
                    throw new InvalidOperationException("Can't replace static root element");
                // this element is no longer a part of hierarchy
                // do not continue proceeding of this element, but continue to process replacer element
                ProcessChildren(parent.children[indexInParent].children);
            }
            else
                ProcessChildren(children);

            return result;
        }

        /// <summary>
        /// returns the first replacer
        /// </summary>
        /// <param name="replacers"></param>
        /// <returns></returns>
        internal BodyPart ReplaceBy(params BodyPart[] replacers)
        {
            if (this == Root)
                throw new InvalidOperationException("Can't replace static root body part");
            if (this == None)
                throw new InvalidOperationException("Can't replace static none body part");
            if (isTopRoot)
                throw new InvalidOperationException("Can't replace top root body part");

            foreach (BodyPart replacer in replacers)
            {
                if (replacer == Root || replacer == None)
                    throw new InvalidOperationException("Can't replace by static body part");
                if (replacer.Parent != Root)
                    throw new InvalidOperationException("Can't replace by part that is in hierarchy already");
            }

            if (Parent != Root)
            {
                int index = Parent.children.IndexOf(this);
                Parent.children.Remove(this); // exclude from parent's child
                Parent.children.InsertRange(index, replacers);
            }

            Parent = Root; // break reference to parent, i.e. exclude this element from hierarchy
            
            return replacers.Length > 0 ? replacers[0] : null;
        }
    }
}