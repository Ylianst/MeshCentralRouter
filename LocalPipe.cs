/*
Copyright 2009-2021 Intel Corporation

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MeshCentralRouter
{
    public class LocalPipeServer
    {
        private String name;
        private byte[] buffer = new byte[4096];
        private NamedPipeServerStream pipeServer;

        public delegate void onArgsHandler(string args);
        public event onArgsHandler onArgs;

        public LocalPipeServer(string name)
        {
            this.name = name;
            pipeServer = new NamedPipeServerStream(name, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            pipeServer.BeginWaitForConnection(new AsyncCallback(processConnection), null);
        }

        public void Dispose()
        {
            try { pipeServer.Close(); } catch (Exception) { }
            pipeServer = null;
        }

        private void processConnection(IAsyncResult ar)
        {
            try { pipeServer.EndWaitForConnection(ar); } catch (Exception) { }
            try { pipeServer.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(processRead), null); } catch (Exception) { }
        }


        private void processRead(IAsyncResult ar)
        {
            int len = 0;
            try { len = pipeServer.EndRead(ar); } catch (Exception) { }
            if (len > 0)
            {
                string args = UTF8Encoding.UTF8.GetString(buffer, 0, len);
                pipeServer.Close();
                onArgs(args);
                pipeServer = new NamedPipeServerStream(name, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                pipeServer.BeginWaitForConnection(new AsyncCallback(processConnection), null);
            }
        }

    }


    public class LocalPipeClient
    {
        private byte[] buffer = new byte[4096];
        private NamedPipeClientStream pipeClient;

        public LocalPipeClient(string name)
        {
            pipeClient = new NamedPipeClientStream(".", name, PipeDirection.InOut, PipeOptions.Asynchronous, TokenImpersonationLevel.None);
        }

        public bool TrySendingArguments(string args)
        {
            try
            {
                byte[] buf = UTF8Encoding.UTF8.GetBytes(args);
                pipeClient.Connect(10);
                pipeClient.BeginWrite(buf, 0, buf.Length, new AsyncCallback(processWrite), null);
            }
            catch (Exception) { return false; }
            return true;
        }

        private void processWrite(IAsyncResult ar)
        {
            try { pipeClient.EndWrite(ar); } catch (Exception) { }
            pipeClient.Close();
        }
    }

}
