﻿using AtomicTorch.CBND.CoreMod.Items.Tools.Special;
using AtomicTorch.CBND.GameApi.Data.Items;
using AtomicTorch.CBND.GameApi.Scripting;
using AtomicTorch.CBND.GameApi.Scripting.ClientComponents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyMod.Scripts.MyMod
{
    public class MyModComponent : ClientComponent
    {
        public static double UpdateInterval => 5.0;

        //public static WriteableBitmap Bitmap;

        public static MyModComponent Instance;

        public MyModComponent() : base(isLateUpdateEnabled: false)
        {
            Api.Logger.Important("MyModComponent()");
            //Bitmap = new WriteableBitmap(200, 200, 96, 96, pixelFormat: PixelFormats.Bgra32, null);
        }

        public static void Init()
        {
            Api.Logger.Error("MyModComponent.Init()");
            if (Instance != null)
            {
                Api.Logger.Error("MyModComponent: Instance already exist.");
            }

            Instance = Client.Scene.CreateSceneObject(nameof(MyModComponent))
                .AddComponent<MyModComponent>(true);
        }

        protected override void OnDisable()
        {
            //ReleaseSubscriptions();
            Api.Logger.Important("MyModComponent.OnDisable()");
            ProtoItemPragmiumSensor.ServerSignalReceived -= this.SignalReceivedHandler;
        }

        protected override void OnEnable()
        {
            Api.Logger.Important("MyModComponent.OnEnable()");
            ProtoItemPragmiumSensor.ServerSignalReceived += this.SignalReceivedHandler;
        }

        double timeSinceLastUpdate = 0;

        public override void Update(double deltaTime)
        {
            timeSincePing += deltaTime;

            timeSinceLastUpdate += deltaTime;
            if (timeSinceLastUpdate >= UpdateInterval)
            {
                Api.Logger.Important("MyModComponent.Update()");
                timeSinceLastUpdate = 0;
            }
        }

        double timeSincePing = 0;
        bool havePing = false;

        private void SignalReceivedHandler(IItem itemSignalSource, PragmiumSensorSignalKind signalKind)
        {
            //if (!ReferenceEquals(this.item, itemSignalSource))
            //{
            //    // received a signal for different item
            //    return;
            //}

            if (signalKind == PragmiumSensorSignalKind.Ping)
            {
                havePing = true;
                timeSincePing = 0;
                Api.Logger.Important("MyMod: PragmiumFinderComponent - Ping!");
                return;
            }

            if (signalKind == PragmiumSensorSignalKind.Pong)
            {
                if (havePing)
                {
                    havePing = false;
                    Api.Logger.Important($"MyMod: PragmiumFinderComponent - Pong! {timeSincePing} seconds");
                    return;
                }
                
                Api.Logger.Important($"MyMod: PragmiumFinderComponent - Pong with no ping!");
                return;
            }

            Api.Logger.Important($"MyMod: PragmiumFinderComponent - UNKNOWN SIGNAL!");
        }
    }
}