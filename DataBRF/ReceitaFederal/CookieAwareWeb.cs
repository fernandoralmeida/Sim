﻿using System;
using System.Net;

namespace Sim.Data.OnLine.ReceitaFederal
{
    internal class CookieAwareWeb : WebClient
    {
        private CookieContainer _mContainer;

        public void SetCookieContainer(CookieContainer container)
        {
            _mContainer = container;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            var webRequest = request as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.CookieContainer = _mContainer;
                webRequest.KeepAlive = true;
                webRequest.ProtocolVersion = HttpVersion.Version10;
            }
            return request;
        }
    }
}
