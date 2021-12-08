using AtomicTorch.CBND.CoreMod.Helpers.Client;
using AtomicTorch.CBND.CoreMod.Items.Tools.Special;
using AtomicTorch.CBND.GameApi.Data.Items;
using AtomicTorch.CBND.GameApi.Scripting;
using AtomicTorch.CBND.GameApi.Scripting.ClientComponents;
using AtomicTorch.GameEngine.Common.Primitives;
using MyMod.UI.PragmaFinder;
using MyMod.UI.PragmaFinder.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyMod.Scripts.MyMod
{
    public class MyModComponent : ClientComponent
    {
        public static double UpdateInterval => 1.0;

        //public static WriteableBitmap Bitmap;

        public static MyModComponent Instance;

        public MyModComponent() : base(isLateUpdateEnabled: false)
        {
            Api.Logger.Important("MyModComponent()");
            //Bitmap = new WriteableBitmap(200, 200, 96, 96, pixelFormat: PixelFormats.Bgra32, null);
        }

        public static void Init()
        {
            // Api.Logger.Error("MyModComponent.Init()"); // at least errors will show up without opening console
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
        Vector2D pingPosition;

        public override void Update(double deltaTime)
        {
            timeSincePing += deltaTime;

            timeSinceLastUpdate += deltaTime;
            if (timeSinceLastUpdate >= UpdateInterval)
            {
                //Api.Logger.Important("MyModComponent.Update()");
                timeSinceLastUpdate = 0;
                var player = ClientCurrentCharacterHelper.Character;
                ViewModelMainWindow.Instance?.UpdatePosition(player.Position.X, player.Position.Y);
                HUDPragmaFinder.Instance?.UpdatePosition(player.Position.X, player.Position.Y);
            }
        }

        double timeSincePing = 0;
        bool havePing = false;
        bool hadPong = false;

        private void SignalReceivedHandler(IItem itemSignalSource, PragmiumSensorSignalKind signalKind)
        {
            //if (!ReferenceEquals(this.item, itemSignalSource))
            //{
            //    // received a signal for different item
            //    return;
            //}

            if (signalKind == PragmiumSensorSignalKind.Ping)
            {
                if (timeSincePing < 2.8 || timeSincePing > 3.2)
                {
                    // offset ping, possibly just started so ignore
                    havePing = false;
                    hadPong = false;
                    timeSincePing = 0;
                }
                else
                {
                    if (havePing && !hadPong)
                    {
                        ViewModelMainWindow.Instance?.Pong(pingPosition.X, pingPosition.Y, 0);
                        HUDPragmaFinder.Instance?.Pong(pingPosition.X, pingPosition.Y, 0);
                        Api.Logger.Important($"Ping with no pong - PragmiumPing({pingPosition.X},{pingPosition.X},{hadPong})");
                    }

                    var player = ClientCurrentCharacterHelper.Character;
                    pingPosition = player.Position;
                    Api.Logger.Important($"PragmiumPing({pingPosition.X},{pingPosition.X},{hadPong})");

                    havePing = true;
                    hadPong = false;
                    timeSincePing = 0;
                }

                return;
            }

            if (signalKind == PragmiumSensorSignalKind.Pong)
            {
                if (havePing && !hadPong)
                {
                    havePing = false;
                    hadPong = true;
                    Api.Logger.Important($"MyMod: PragmiumFinderComponent - Pong! {timeSincePing} seconds");
                    Api.Logger.Important($"PragmiumPong({pingPosition.X},{pingPosition.X},{timeSincePing})");
                    ViewModelMainWindow.Instance?.Pong(pingPosition.X, pingPosition.Y, timeSincePing);
                    HUDPragmaFinder.Instance?.Pong(pingPosition.X, pingPosition.Y, timeSincePing);
                    return;
                }
                
                Api.Logger.Important($"MyMod: PragmiumFinderComponent - Pong with no ping!");
                return;
            }

            Api.Logger.Important($"MyMod: PragmiumFinderComponent - UNKNOWN SIGNAL!");
        }
    }
}
