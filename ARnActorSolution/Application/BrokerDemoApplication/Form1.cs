﻿using Actor.Base;
using Actor.Server;
using Actor.Service;
using Actor.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrokerDemoApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private CollectionActor<string> fMemLogger = new CollectionActor<string>();
        private BrokerActor<int> fBroker = new BrokerActor<int>() ;

        private void button1_Click(object sender, EventArgs e)
        {
            // start console
            ActorConsole.Register();
            // logger to broker
            fBroker.Logger = new LoggerActor("Broker.Log");
            // launch worker
            foreach (var item in Enumerable.Range(1, 8))
            {
                var worker = new PiWorker(fMemLogger);
                fBroker.RegisterWorker(worker);
            }
            // launch client
            foreach (var item in Enumerable.Range(1, 10))
            {
                var client = new DemoClient(fBroker);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (fMemLogger == null)
                return;
            listBox1.Items.Clear();
            var task = await Task.Run(()
                =>
            {
                List<string> ls = fMemLogger.ToList();
                return ls;
            });
            StringBuilder sb = new StringBuilder();
            foreach (var s in task.Where(t => t != null))
                listBox1.Items.Add(s);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = timer1.Enabled ? "LaunchTimer" : "StopTimer";
            timer1.Enabled = !timer1.Enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // launch client
            foreach (var item in Enumerable.Range(1, 10))
            {
                var client = new DemoClient(fBroker);
            }
        }
    }
}
