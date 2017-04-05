﻿using Newtonsoft.Json;
using Pharos.MessageAgent.Data;
using Pharos.MessageTransferAgenClient.DomainEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Pharos.SuperSocketClientProtocol.Extensions;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.MessageTransferAgenClient.WebPackageHandler
{
    public class WebPackageHandler : HttpMessageHandler
    {
        public WebPackageHandler()
        {
            MessageSettings.Current.IsWeb = true;
        }
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                var task = request.Content.ReadAsStringAsync();
                task.Wait();
                var info = JsonConvert.DeserializeObject<PubishInformaction>(task.Result);
                if (info != null)
                {
                    new EventAggregator().RemotePublish(info);
                    return TaskFor(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
                }
            }
            catch { }
            return TaskFor(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));

        }
        private Task<HttpResponseMessage> TaskFor(HttpResponseMessage response)
        {
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}