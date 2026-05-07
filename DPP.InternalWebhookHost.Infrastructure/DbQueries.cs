namespace DPP.PartnerPaymentIntegration.Infrastructure
{
    public static class DbQueries
    {
        public const string GetFortellisDepartmentCodeList = "usp_Fortellis_Department_Get";
        public const string AddFortellisDepartmentCode = "usp_Fortellis_Department_Insert";
        public const string UpdateFortellisDepartmentCode = "usp_Fortellis_Department_Update";
        public const string DeleteFortellisDepartmentCode = "usp_Fortellis_Department_Delete";

        public const string GetFortellisTransaction = "usp_Fortellis_TransactionDetail_Select_BG";
        public const string InsertFortellisTransaction = "usp_TransactionRecordFortellis_Insert_BG";
        public const string UpdateFortellisTransaction = "usp_TransactionRecordFortellis_Update";
    }
}
