﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Actor.Base;
using Actor.Util;

namespace TestActor
{
    [TestClass]
    public class ReceiveActorTest
    {

        class TargetActor : BaseActor
        {
            public TargetActor()
            {
                Become(new Behavior<IActor, int>((a, i) =>
                 {
                     a.SendMessage((IActor)this,i % 2 == 0 ? "even" : "odd");
                 })) ;
            }
        }


        [TestMethod]
        public void ReceiveWait1Test()
        {
            TestLauncherActor.Test(() =>
            {
                var actor = new ReceiveActor<int, string>();
                var target = new TargetActor();
                var resultOdd = actor.Wait((IActor)target, 1);
                Assert.IsTrue(resultOdd.Result.Item2 == "odd");
            }) ;
        }

        [TestMethod]
        public void ReceiveWait2Test()
        {
            TestLauncherActor.Test(() =>
            {
                var actor = new ReceiveActor<int, string>();
                var target = new TargetActor();
                var resultEven = actor.Wait((IActor)target, 2);
                Assert.IsTrue(resultEven.Result.Item2 == "even");
            });
        }

        class TargetActor2 : BaseActor
        {
            public TargetActor2()
            {
                Become(new Behavior<IActor, int>((a, i) =>
                {
                    a.SendMessage((IActor)this, i *2);
                }));
            }
        }

        [TestMethod]
        public void ReceiveWait3Test()
        {
            TestLauncherActor.Test(() =>
            {
                var actor = new ReceiveActor<int>();
                var target = new TargetActor2();
                var resultEven = actor.Wait((IActor)target, 1);
                Assert.IsTrue(resultEven.Result.Item2 == 2);
            });
        }

        [TestMethod]
        public void ReceiveWait4Test()
        {
            TestLauncherActor.Test(() =>
            {
                var actor = new ReceiveActor<int>();
                var target = new TargetActor2();
                var resultEven = actor.Wait((IActor)target, 2);
                Assert.IsTrue(resultEven.Result.Item2 == 4);
            });
        }
    }
}
