using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers
{
    public static class RequestHelper
    {
        public static string UserId(this HttpRequestMessage httpRequest)
        {
            httpRequest.ValidateHeader();

            httpRequest.Headers.TryGetValues("UserId", out IEnumerable<string> headerList);

            if (headerList == null)
                ExceptionHelper<ArgumentException>.Throw("O Parâmetro UserId não foi informado no header da requsição!");

            return httpRequest.Headers.GetValues("UserId").ToList().FirstOrDefault();
        }

        private static void ValidateHeader(this HttpRequestMessage httpRequest)
        {
            if (httpRequest.Headers == null)
                ExceptionHelper<ArgumentException>.Throw("O Header na requisição é obrigatório!");
        }
    }
}
