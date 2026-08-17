using ObjCRuntime;

namespace Laerdal.Dfu.Bindings.iOS
{
	[Native]
	public enum DFUError : long
	{
		RemoteLegacyDFUSuccess = 1,
		RemoteLegacyDFUInvalidState = 2,
		RemoteLegacyDFUNotSupported = 3,
		RemoteLegacyDFUDataExceedsLimit = 4,
		RemoteLegacyDFUCrcError = 5,
		RemoteLegacyDFUOperationFailed = 6,
		RemoteSecureDFUSuccess = 11,
		RemoteSecureDFUOpCodeNotSupported = 12,
		RemoteSecureDFUInvalidParameter = 13,
		RemoteSecureDFUInsufficientResources = 14,
		RemoteSecureDFUInvalidObject = 15,
		RemoteSecureDFUSignatureMismatch = 16,
		RemoteSecureDFUUnsupportedType = 17,
		RemoteSecureDFUOperationNotPermitted = 18,
		RemoteSecureDFUOperationFailed = 20,
		RemoteSecureDFUExtendedError = 21,
		RemoteExtendedErrorWrongCommandFormat = 22,
		RemoteExtendedErrorUnknownCommand = 23,
		RemoteExtendedErrorInitCommandInvalid = 24,
		RemoteExtendedErrorFwVersionFailure = 25,
		RemoteExtendedErrorHwVersionFailure = 26,
		RemoteExtendedErrorSdVersionFailure = 27,
		RemoteExtendedErrorSignatureMissing = 28,
		RemoteExtendedErrorWrongHashType = 29,
		RemoteExtendedErrorHashFailed = 30,
		RemoteExtendedErrorWrongSignatureType = 31,
		RemoteExtendedErrorVerificationFailed = 32,
		RemoteExtendedErrorInsufficientSpace = 33,
		RemoteExperimentalButtonlessDFUSuccess = 9001,
		RemoteExperimentalButtonlessDFUOpCodeNotSupported = 9002,
		RemoteExperimentalButtonlessDFUOperationFailed = 9004,
		RemoteButtonlessDFUSuccess = 91,
		RemoteButtonlessDFUOpCodeNotSupported = 92,
		RemoteButtonlessDFUOperationFailed = 94,
		RemoteButtonlessDFUInvalidAdvertisementName = 95,
		RemoteButtonlessDFUBusy = 96,
		RemoteButtonlessDFUNotBonded = 97,
		FileNotSpecified = 101,
		FileInvalid = 102,
		ExtendedInitPacketRequired = 103,
		InitPacketRequired = 104,
		FailedToConnect = 201,
		DeviceDisconnected = 202,
		BluetoothDisabled = 203,
		ServiceDiscoveryFailed = 301,
		DeviceNotSupported = 302,
		ReadingVersionFailed = 303,
		EnablingControlPointFailed = 304,
		WritingCharacteristicFailed = 305,
		ReceivingNotificationFailed = 306,
		UnsupportedResponse = 307,
		BytesLost = 308,
		CrcError = 309,
		InvalidAdvertisementName = 310,
		InvalidInternalState = 500
	}

	public enum DFUFirmwareType : byte
	{
		Softdevice = 1,
		Bootloader = 2,
		Application = 4,
		SoftdeviceBootloader = 3,
		SoftdeviceBootloaderApplication = 7
	}

	[Native]
	public enum DFUState : long
	{
		// note: Nordic's native DFUState enum has no explicit raw values - Swift auto-assigns them
		// sequentially by declaration order. 4.17.0 inserted Connected/Disconnected mid-sequence,
		// which renumbers every case declared after each insertion point vs. 4.16.0. These values
		// MUST be re-verified against the actual Swift source on every future Nordic_Package_Version
		// bump, not just copied forward - see README's "Known issues" section.
		Connecting = 0,
		Connected = 1,
		Starting = 2,
		EnablingDfuMode = 3,
		Uploading = 4,
		Validating = 5,
		Disconnecting = 6,
		Disconnected = 7,
		Completed = 8,
		Aborted = 9
	}

	[Native]
	public enum DFUUuidType : long
	{
		LegacyService = 0,
		LegacyControlPoint = 1,
		LegacyPacket = 2,
		LegacyVersion = 3,
		SecureService = 4,
		SecureControl = 5,
		SecurePacket = 6,
		ButtonlessService = 7,
		ButtonlessCharacteristic = 8,
		ButtonlessWithoutBondSharing = 9,
		ButtonlessWithBondSharing = 10
	}

	[Native]
	public enum LogLevel : long
	{
		Debug = 0,
		Verbose = 1,
		Info = 5,
		Application = 10,
		Warning = 15,
		Error = 20
	}
}
