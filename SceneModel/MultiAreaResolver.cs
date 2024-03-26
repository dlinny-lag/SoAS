using SceneModel.ContactAreas;

namespace SceneModel
{
    public static class MultiAreaResolver
    {
        /// <summary>
        /// resolve stick contact for creature definition
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static ContactArea GetStick(this Sex sex)
        {
            if (sex == Sex.Male || sex == Sex.Both)
                return Penis.Any;
            if (sex == Sex.Female) 
                return Strapon.Any;

            return Stick.Any;
        }


        public static ContactArea GetPenis(this Sex sex)
        {
            if (sex == Sex.Female || sex == Sex.Any)
                return null;
            return Penis.Any; // male + both
        }


        /// <summary>
        /// resolve stick from tag
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static ContactArea ResolveStick(this Sex sex)
        {
            return GetStick(sex); // different purpose, but same logic
        }

        /// <summary>
        /// resolve Either for creature definition
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static ContactArea[] GetEither(this Sex sex)
        {
            if (sex == Sex.Male)
                return new ContactArea[] { Anus.Any };
            if (sex == Sex.Female || sex == Sex.Both)
                return new ContactArea[] { Anus.Any, Vagina.Any };
                
            return new ContactArea[] { Anus.Any, Either.Any };
        }

        public static ContactArea GetVagina(this Sex sex)
        {
            if (sex == Sex.Male || sex == Sex.Any)
                return null;
            return Vagina.Any; // female + both
        }

        /// <summary>
        /// resolve Either from tag
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static ContactArea[] ResolveEither(this Sex sex)
        {
            if (sex == Sex.Male)
                return new ContactArea[] { Anus.Any };
            if (sex == Sex.Female)
                return new ContactArea[] { Anus.Any, Vagina.Any };
                
            return new ContactArea[] {Either.Any};
        }
    }
}