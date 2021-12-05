using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
using AtomicTorch.CBND.GameApi;
using AtomicTorch.CBND.GameApi.ServicesClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyMod.Scripts.MyMod
{
    [NotPersistent]
    public enum MyModButtons
    {
        [Description("Test")]
        [ButtonInfo(InputKey.F7, Category = "MyMod")]
        Test,

        [Description("Toggle Proximity Finder")]
        [ButtonInfo(InputKey.F8, Category = "MyMod")]
        Toggle
    }
}
