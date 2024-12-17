namespace EasyTalkSystem
{
    /// <summary>
    /// 会話時動作を管理される
    /// </summary>
    public interface ITalkingOperationManaged
    {
        /// <summary>
        /// 動作を停止
        /// </summary>
        public void StopOperation();
        /// <summary>
        /// 動作を開始
        /// </summary>
        public void StartOperation();
    }
}
