﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Actor.Base;
using System.Threading.Tasks;

namespace TestActor
{

    class FutureAsyncActorTest : BaseActor
    {
        private string fData;
        public FutureAsyncActorTest()
        {
            Become(new Behavior<string>(t =>
            {
                fData = t;
            }));
            AddBehavior(new Behavior<IActor>(a =>
            {
                a.SendMessage(fData);
            }));
        }
    }

    [TestClass]
    public class FutureTest
    {        

        private async Task<string> GetResult(Future<string> future)
        {
            return await future.ResultAsync();
        }

        [TestMethod]
        public void FutureAsyncTest()
        {
            TestLauncherActor.Test(() =>
            {
                var future = new Future<string>();
                var actor = new FutureAsyncActorTest();
                actor.SendMessage("Test Data");
                var result = GetResult(future);
                actor.SendMessage((IActor)future);
                Assert.AreEqual("Test Data", result.Result);
            });
        }
    }
}
