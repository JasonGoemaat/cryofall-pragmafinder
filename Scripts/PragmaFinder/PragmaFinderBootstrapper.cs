using AtomicTorch.CBND.CoreMod.Bootstrappers;
using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
using AtomicTorch.CBND.GameApi.Data;
using AtomicTorch.CBND.GameApi.Data.Characters;
using AtomicTorch.CBND.GameApi.Scripting;
using MyMod.Scripts.MyMod;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMod.Scripts.MyMod
{
    [PrepareOrder(afterType: typeof(BootstrapperClientCore))]
    public class MyModBootstrapper : BaseBootstrapper
    {
        static ClientInputContext gameplayInputContext;

        public override void ClientInitialize()
        {
            // base.ClientInitialize();

            Api.Logger.Important("MyMod.Bootstrapper.ClientInitialize() - 2");

            ClientInputManager.RegisterButtonsEnum<MyModButtons>();
            Api.Logger.Important("MyMod.Bootstrapper.ClientInitialize() - Buttons registered");

            BootstrapperClientGame.InitCallback += GameInitHandler;
            Api.Logger.Important("MyMod.Bootstrapper.ClientInitialize() - GameInitHandler set");

            BootstrapperClientGame.ResetCallback += ResetHandler;
            Api.Logger.Important("MyMod.Bootstrapper.ClientInitialize() - ResetHandler set");
        }

        private static void GameInitHandler(ICharacter currentCharacter)
        {
            Api.Logger.Important("MyMod.Bootstrapper.GameInitHandler()");

            MyModComponent.Init();
            Api.Logger.Important("MyMod.Bootstrapper.GameInitHandler() - called MyModComponent.Init()");

            gameplayInputContext = ClientInputContext
                .Start("MyModInputContext")
                .HandleButtonDown(MyModButtons.Test, OnTestButton)
                .HandleButtonDown(MyModButtons.Toggle, OnToggleButton);
        }

        private static void OnTestButton()
        {
            Api.Logger.Important("MyMod.Bootstrapper.OnTestButton()");
        }

        private static void OnToggleButton()
        {
            Api.Logger.Important("MyMod.Bootstrapper.OnTestButton()");
        }

        private static void OnAll()
        {
            Api.Logger.Important("MyMod.Bootstrapper.OnAll()");
        }

        private static void ResetHandler()
        {
            Api.Logger.Important("MyMod.Bootstrapper.ResetHandler()");

            //ClientComponentAutomaton.Instance?.Destroy();
            //ClientComponentAutomaton.Instance = null;

            //gameplayInputContext?.Stop();
            //gameplayInputContext = null;
        }
    }
}