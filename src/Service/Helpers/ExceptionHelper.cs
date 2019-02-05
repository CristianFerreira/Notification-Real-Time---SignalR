using System;
using System.Collections.Generic;

namespace Service.Helpers
{
    public static class ExceptionHelper<E> where E : Exception
    {
        public static void Throw(string msgError)
        {
            throw (E)Activator.CreateInstance(typeof(E), msgError);
        }


        public static void Throw(string msgError, string parameter)
        {
            throw (E)Activator.CreateInstance(typeof(E), string.Format(msgError, parameter));
        }

        public static void Throw(string msgError, IList<string> listParameters)
        {
            throw (E)Activator.CreateInstance(typeof(E), string.Format(msgError, ParametersNames(listParameters)));
        }

        private static string ParametersNames(IList<string> listParameters)
        {
            return string.Join(",", listParameters);
        }
    }
}
