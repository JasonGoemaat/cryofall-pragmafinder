# Investigate Pragmium Sensor

ItemPragmiumSensor

* DurbailityMax - 2 * 60 * 60 (7200)
* EnergyConsumptionPerSecond - 2 (uses power banks)
* MaxRange - 100

ItemPragmiumSensorPrivateState : ItemWithDurabilityPrivateState

* ServerTimeToPing

PragmiumSensorSignalKind

* Ping = 0
* Pong = 1

ProtoItemPragmiumSensor : ProtoItemTool, IProtoItemWithHotbarOverlay

* MaximumNumberOfPongsPerScan - 2
* ServerScanInterval - 3
* SignalLevelsNumber - 5
* ServerUpdateIntervalSeconds - 0.1
* ServerUpdateRateIntervalSeconds - 10
* SharedCalculateTimeToPong(byte signalStrength)
	* (1 + SignalLevelsNumber - signalStrength) * 0.5 * ServerScanInterval / SignalLevelsNumber
	* (1 + 5 - signalStrength) * 0.5 * 3 / 5
		* 1, then 2.5 * 3/5
		* 2, then 2 * 3/5
		* 3, then 1.5 * 3/5
		* 4, then 1.0 * 3/5
		* 5, then 0.5 * 3/5
* ClientRemote_OnSignal() - receives ping or pong
* ServerCalculateDistanceSqrToTheClosestPragmiumSpires()
	* var distanceSqr = position.TileSqrDistanceTo(staticWorldObject.TilePosition);
	* 
* ServerCalculateStrengthToTheClosestPragmiumSpires()
	* calls SharedConvertDistanceToSignalStrength()
		* return (byte)Math.Ceiling(SignalLevelsNumber * d / this.MaxRange);
		* So 5 * distance / maxrange - ceiling
		* So distance is 81-100, it is 5
		* So distance is 61-80 it is 4
		* So distance is 41-60 it is 3
		* So distance is 21-40 it is 2
		* So distance is 0-20 it is 1


## Writing

Should really look into ProtoItemPragmiumSensor.ServerSignalReceived += this.SignalReceivedHandler;
Check out 

```cs
private void SignalReceivedHandler(IItem itemSignalSource, PragmiumSensorSignalKind signalKind)
    if (!ReferenceEquals(this.item, itemSignalSource))
    {
        // received a signal for different item
        return;
    }

    var storyboard = signalKind switch
    {
        PragmiumSensorSignalKind.Ping => this.storyboardAnimationSignalPing,
        PragmiumSensorSignalKind.Pong => this.storyboardAnimationSignalPong,
        _ => throw new ArgumentOutOfRangeException(nameof(signalKind), signalKind, null)
    };

    storyboard.Begin(this.grid);
}
```