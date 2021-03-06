﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Actor.Server;

namespace TestActor
{
    /// <summary>
    /// Description résumée pour EchoServerActorTest
    /// </summary>
    [TestClass]
    public class EchoServerActorTest
    {

        [TestMethod]
        public void EchoClient100Test()
        {
            TestLauncherActor.Test(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    EchoClientActor aClient = new EchoClientActor();
                    aClient.Connect("EchoServer");
                    aClient.SendMessage("client-" + i.ToString());
                }
            });
        }
    }
}
