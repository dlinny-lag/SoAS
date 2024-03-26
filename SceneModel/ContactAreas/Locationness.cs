using System;
using Shared.Utils;

namespace SceneModel.ContactAreas
{
    public enum SingleNoSymmetry
    {
        None = 0
    }
    public enum LeftRightSymmetry
    {
        None = 0,
        Left = 1,
        Right = 2
    }

    public enum TopBottomSymmetry
    {
        None = 0,
        Top = 1,
        Bottom = 2
    }

    public enum FrontBackSymmetry
    {
        None = 0,
        Front = 1,
        Back = 2
    }

    public enum Linear3Asymmetry
    {
        None = 0,
        Front = 1,
        Middle = 2,
        Back = 3
    }

    public enum Azimuthal3Asymmetry
    {
        None = 0,
        Back = 1,
        Left = 2,
        Right = 3
    }

    public abstract class Locationness
    {
        public abstract string Prefix { get; }
        public abstract bool IsAny { get; }
    }

    public abstract class SingleSymmetryLocationness : Locationness
    {
        public abstract int Code { get; }
        public sealed override bool IsAny => Code == 0;
    }

    public abstract class Locationness<TSymmetry> : SingleSymmetryLocationness
        where TSymmetry: Enum
    {
        public abstract TSymmetry Type { get; }

        public override string Prefix
        {
            get
            {
                if (Type.IsNone())
                    return "";
                return Type.ToString(); // TODO: string representation of a enum may be a bad idea
            }
        }

        private int code = -1;
        private int GetCode()
        {
            if (code < 0)
                code = Convert.ToInt32(Type);
            return code;
        }
        public sealed override int Code => GetCode();
    }

    public abstract class DoubleSymmetryLocationness : Locationness
    {
        public abstract int Code1 { get; }
        public abstract int Code2 { get; }
    }

    public abstract class Locationness<TSymmetry1, TSymmetry2> : DoubleSymmetryLocationness
        where TSymmetry1: Enum
        where TSymmetry2: Enum
    {
        public abstract TSymmetry1 Type1 { get; }
        public abstract TSymmetry2 Type2 { get; }

        public override string Prefix => $"{(Type1.IsNone() ? string.Empty : Type1.ToString())}{(Type2.IsNone() ? string.Empty : Type2.ToString())}";


        private int code1 = -1;
        private int code2 = -1;
        private int GetCode1()
        {
            if (code1 < 0)
                code1 = Convert.ToInt32(Type1);
            return code1;
        }
        private int GetCode2()
        {
            if (code2 < 0)
                code2 = Convert.ToInt32(Type2);
            return code2;
        }
        public sealed override int Code1 => GetCode1();
        public sealed override int Code2 => GetCode2();
        public sealed override bool IsAny => Code1 + Code2 == 0;
    }

    public sealed class Single : Locationness<SingleNoSymmetry>
    {
        public static readonly Single Any = new Single();
        public override SingleNoSymmetry Type => SingleNoSymmetry.None;
        public override string Prefix => "";
    }

    public sealed class LeftRight : Locationness<LeftRightSymmetry>
    {
        public static readonly LeftRight Any = new LeftRight(LeftRightSymmetry.None);
        public static readonly LeftRight Left = new LeftRight(LeftRightSymmetry.Left);
        public static readonly LeftRight Right = new LeftRight(LeftRightSymmetry.Right);

        private LeftRight(LeftRightSymmetry type)
        {
            Type = type;
        }

        public override LeftRightSymmetry Type { get; }
    }

    public sealed class FrontBack : Locationness<FrontBackSymmetry>
    {
        public static readonly FrontBack Any = new FrontBack(FrontBackSymmetry.None);
        public static readonly FrontBack Front = new FrontBack(FrontBackSymmetry.Front);
        public static readonly FrontBack Back = new FrontBack(FrontBackSymmetry.Back);

        private FrontBack(FrontBackSymmetry type)
        {
            Type = type;
        }

        public override FrontBackSymmetry Type { get; }
    }

    public sealed class TopBottom : Locationness<TopBottomSymmetry>
    {
        public static readonly TopBottom Any = new TopBottom(TopBottomSymmetry.None);
        public static readonly TopBottom Top = new TopBottom(TopBottomSymmetry.Top);
        public static readonly TopBottom Bottom = new TopBottom(TopBottomSymmetry.Bottom);

        private TopBottom(TopBottomSymmetry type)
        {
            Type = type;
        }

        public override TopBottomSymmetry Type { get; }
    }

    public sealed class Bilateral : Locationness<LeftRightSymmetry, FrontBackSymmetry>
    {
        public static readonly Bilateral Any = new Bilateral(LeftRightSymmetry.None, FrontBackSymmetry.None);

        public static readonly Bilateral Left = new Bilateral(LeftRightSymmetry.Left, FrontBackSymmetry.None);
        public static readonly Bilateral Right = new Bilateral(LeftRightSymmetry.Right, FrontBackSymmetry.None);

        public static readonly Bilateral Front = new Bilateral(LeftRightSymmetry.None, FrontBackSymmetry.Front);
        public static readonly Bilateral Back = new Bilateral(LeftRightSymmetry.None, FrontBackSymmetry.Back);

        public static readonly Bilateral LeftFront = new Bilateral(LeftRightSymmetry.Left, FrontBackSymmetry.Front);
        public static readonly Bilateral LeftBack = new Bilateral(LeftRightSymmetry.Left, FrontBackSymmetry.Back);

        public static readonly Bilateral RightFront = new Bilateral(LeftRightSymmetry.Right, FrontBackSymmetry.Front);
        public static readonly Bilateral RightBack = new Bilateral(LeftRightSymmetry.Right, FrontBackSymmetry.Back);

        private Bilateral(LeftRightSymmetry type1, FrontBackSymmetry type2)
        {
            Type1 = type1;
            Type2 = type2;
        }

        public override LeftRightSymmetry Type1 { get; }
        public override FrontBackSymmetry Type2 { get; }
    }

    public sealed class BilateralVertical : Locationness<LeftRightSymmetry, TopBottomSymmetry>
    {
        public static readonly BilateralVertical Any = new BilateralVertical(LeftRightSymmetry.None, TopBottomSymmetry.None);

        public static readonly BilateralVertical Left = new BilateralVertical(LeftRightSymmetry.Left, TopBottomSymmetry.None);
        public static readonly BilateralVertical Right = new BilateralVertical(LeftRightSymmetry.Right, TopBottomSymmetry.None);

        public static readonly BilateralVertical Top = new BilateralVertical(LeftRightSymmetry.None, TopBottomSymmetry.Top);
        public static readonly BilateralVertical Bottom = new BilateralVertical(LeftRightSymmetry.None, TopBottomSymmetry.Bottom);

        public static readonly BilateralVertical LeftTop = new BilateralVertical(LeftRightSymmetry.Left, TopBottomSymmetry.Top);
        public static readonly BilateralVertical LeftBottom = new BilateralVertical(LeftRightSymmetry.Left, TopBottomSymmetry.Bottom);

        public static readonly BilateralVertical RightTop = new BilateralVertical(LeftRightSymmetry.Right, TopBottomSymmetry.Top);
        public static readonly BilateralVertical RightBottom = new BilateralVertical(LeftRightSymmetry.Right, TopBottomSymmetry.Bottom);

        private BilateralVertical(LeftRightSymmetry type1, TopBottomSymmetry type2)
        {
            Type1 = type1;
            Type2 = type2;
        }

        public override LeftRightSymmetry Type1 { get; }
        public override TopBottomSymmetry Type2 { get; }
    }

    public sealed class Azimuthal3 : Locationness<Azimuthal3Asymmetry>
    {
        public static readonly Azimuthal3 Any = new Azimuthal3(Azimuthal3Asymmetry.None);
        public static readonly Azimuthal3 Back = new Azimuthal3(Azimuthal3Asymmetry.Back);
        public static readonly Azimuthal3 Left = new Azimuthal3(Azimuthal3Asymmetry.Left);
        public static readonly Azimuthal3 Right = new Azimuthal3(Azimuthal3Asymmetry.Right);

        private Azimuthal3(Azimuthal3Asymmetry type)
        {
            Type = type;
        }
        public override Azimuthal3Asymmetry Type { get; }
    }
}