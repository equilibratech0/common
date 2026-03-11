namespace Shared.Domain.Enums;

public enum MovementEventType
{
    // PayIn — final states where money flows into the account
    TransactionApproved = 1,
    TopupCreated = 2,
    AdjustmentTopupCreated = 3,
    AdjustmentRebateFeeCreated = 4,
    RollingReserveReleased = 5,
    ClaimClose = 6,
    ChargebackClose = 7,
    PayoutError = 8,
    WithdrawalCancelled = 9,
    WithdrawalReturned = 10,
    PartialPayment = 11,

    // PayOut — final states where money flows out of the account
    PayoutFinished = 50,
    ClaimRefund = 51,
    ChargebackRefund = 52,
    SettlementPublished = 53,
    AccountSettlement = 54,
    AdjustmentCreated = 55,
    AdjustmentRollingReserveCreated = 56,
    AdjustmentBalanceFeeCreated = 57,
    WithdrawalPaid = 58,
}
