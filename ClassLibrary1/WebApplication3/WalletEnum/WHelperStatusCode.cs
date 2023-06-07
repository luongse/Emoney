using System.ComponentModel;

namespace BusinessObject.WalletBusiness.Enums
{
    public enum WHelperStatusCode
    {
        Failed = 0,
        Success = 1,
    }

    public enum WHelperPhoneCarrier
    {
        Unknown,

        #region Campuchia

        [Description("Smart_PIN")]
        WingSmart,

        [Description("Metfone_PIN")]
        WingMetfone,

        [Description("Mobitel_PIN")]
        WingCellcard,

        [Description("Seatel_PIN")]
        WingSeatel,

        [Description("Cootel_PIN")]
        WingCootel,

        #endregion


        #region Myanmar

        #endregion


        #region Philippines

        #endregion
    }

    public enum WHelperDetailStatusCode
    {
        [Description("Not run transfer")]
        NotRun = -1,

        [Description("Wallet type is not support")]
        NotSupport = -2,

        #region WING

        [Description("Wallet refresh token is error")]
        WalletRefreshTokenError = 13,

        [Description("Wallet sender validation is error")]
        WalletSenderValidateError = 14,

        [Description("Wallet sending confirm is error")]
        WalletSendingConfirmError = 15,

        [Description("Wallet sending confirm is error")]
        WalletSendingParseResultConfirmError = 16,

        [Description("Wallet sending confirm is error")]
        WalletSendingTransactionResultEmpty = 17,

        [Description("Wallet sending confirm is error")]
        WalletSendingConfirmErrorWillRetry = 18,

        [Description("Wallet sending confirm is error")]
        WalletSendingConfirmErrorEmptyResponse = 19,

        [Description("Wallet sending validation is error")]
        WalletSendingValidationTokenError = 20,

        [Description("Wallet sending non-wing")]
        WalletSendingNonWingError = 21,

        [Description("Receiver is lock")]
        WalletSendingReceiverAccountLockError = 22,

        [Description("Receiver not found")]
        WalletSendingUserNotFoundError = 23,

        [Description("Biller suspended")]
        WalletSendingBillerSuspendedError = 24,

        [Description("Receiver not active")]
        WalletSendingReceiverNotActiveError = 25,

        [Description("Receiver is suspended")]
        WalletSendingReceiverSuspendedError = 26,

        [Description("General fail exception or invalid CTXN response")]
        GeneralFailException = 27,

        [Description("Transaction failed to process,Invalid Limit 000049005 for the Liability 000049005")]
        TransactionFailedToProcess = 28,

        [Description("The requested amount has exceeded the maximum tranfer allowed")]
        WingMaximumTranferAllowed = 29,

        [Description("Account number not found")]
        WingAccountNumberNotFound = 42,

        [Description("Wallet is frozen")]
        WalletReceiverAccountFrozen = 43,

        [Description("Invalid account class mapping")]
        WingInvalidAccountClassMapping = 44,

        [Description("Secret code (PIN code) is empty")]
        WingEmptySecretCode = 45,

        [Description("Convert secret code (PIN code) is error")]
        WingConvertSecretCodeError = 46,

        [Description("This service is currently unavailable")]
        WingServiceCurrentlyUnavailable = 47,

        [Description("Balance is not enough")]
        WingBalanceNotEnough = 48,

        [Description("Your account has been blocked")]
        WingMyAccountIsLock = 49,

        #endregion


        [Description("Invalid input")]
        InputDataInvalid = 2,

        #region WAVE

        [Description("Wallet is not register")]
        WalletNotRegister = 3,

        [Description("Wallet is not active")]
        WalletNotActive = 4,

        [Description("Has error in get transfer fee")]
        GetTransferFeeError = 5,

        [Description("Has error in transfer process")]
        WalletTransferError = 6,

        [Description("Has exception error")]
        WalletExceptionError = 7,

        [Description("Has error in transfer process")]
        WalletErrorCheckSecurity = 8,

        [Description("Wallet is stopped")]
        WalletIsStopped = 9,

        [Description("Wallet is not register")]
        WalletNotRegisterNullResponse = 10,

        [Description("Wallet is suspended")]
        WalletIsSuspended = 11,

        [Description("Sending is error")]
        WalletWaveSendingError = 12,

        [Description("No service topup")]
        WalletWaveNoService = 40,

        [Description("Top Up is error")]
        WalletWaveTopUpError = 41,

        [Description("Sending is invalid nonce")]
        WalletWaveSendingInvalidNonceError = 112,

        #endregion


        #region GCASH

        [Description("Wallet get access token is error")]
        WalletGetAccessTokenError = 30,

        [Description("Unable to process your request at the moment")]
        WalletUnableProcessAtTheMoment = 31,

        [Description("No topup subscribe for phone")]
        WalletNoTopUpSubscribe = 35,

        [Description("Create order topup has error")]
        WalletCreateTopUpError = 36,

        #endregion


        #region ABA

        [Description("Wallet transfer money error")]
        AbaWalletSendingError = 50,

        [Description("ABA Mobile is under maintenance now. The sevice will resume shortly. We apologise for the inconvenience")]
        AbaWalletMaintenance = 51,

        [Description("You have insufficient balance in your source account")]
        AbaWalletOutOfBalance = 52,

        [Description("Receiver account number is invalid")]
        AbaToAccountIsNotValid = 53,

        [Description("From account is not exists")]
        AbaFromAccountIsNotExists = 54,

        [Description("You have reached max attempt of trying to make transaction. Please try again after 15min")]
        AbaMaxAttemptsReached = 56,

        #endregion

        #region PIPAY

        [Description("Wallet transfer money error")]
        PipayWalletSendingError = 70,

        [Description("Validate sending error")]
        PipayValidateSendingError = 71,

        [Description("Confirm sending error")]
        PipayConfirmSendingEmptyResponse = 75,

        [Description("Confirm sending is not OK")]
        PipayConfirmSendingHttpStatusNotOk = 76,

        [Description("Confirm sending result is error")]
        PipayConfirmSendingLoadXmlError = 77,

        [Description("Confirm sending response data is null")]
        PipayConfirmSendingConvertDataNull = 78,

        [Description("Confirm sending transaction data is null")]
        PipayConfirmSendingTransactionDataNull = 79,

        #endregion

        #region BINANCE

        [Description("Init payout failed")]
        BinanceInitPayoutFailed = 85,

        [Description("Send request failed")]
        BinanceSendRequestFailed = 86,

        [Description("Send request with response error")]
        BinanceResponseDataNull = 87,

        [Description("Has exception error")]
        BinanceExceptionError = 88,

        [Description("Init refund failed")]
        BinanceInitRefundFailed = 89,

        [Description("Has general exception error")]
        BinanceGeneralExceptionError = 90,

        #endregion

        #region EMONEY
        [Description("Get checkout info empty response  ")]
        EmoneyCheckoutInfoEmptyResponse = 110,

        [Description("Get checkout info Http status is not OK")]
        EmoneyCheckoutInfoHttpStatusNotOK = 111,

        [Description("Get checkout info parse data null")]
        EmoneyCheckoutInfoParseDataNull = 112,

        [Description("Get checkout info convert data null")]
        EmoneyCheckoutInfoConvertDataNull = 113,

        [Description("Checkout info response generate transactionId null")]
        CheckoutInfoGenTransIdEmpty = 114,
        

        [Description("Confirm sending money empty response")]
        EmoneyConfirmSendingEmptyResponse = 115,


        [Description("Confirm sending money Http status is not OK")]
        EmoneyConfirmSendingHttpStatusNotOK = 116,


        [Description("Confirm sending money parse data null")]
        EmoneyConfirmSendingParseDataNull = 117,

        [Description("Confirm sending money convert data null")]
        EmoneyConfirmSendingConvertDataNull = 118,





        #endregion

        [Description("Success")]
        Success = 1,
    }
}