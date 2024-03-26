using System;

namespace AAFModel
{
    public class LoadException : Exception
    {
        public LoadException(string xmlContent, Exception inner) :base(xmlContent, inner)
        {

        }
    }
}