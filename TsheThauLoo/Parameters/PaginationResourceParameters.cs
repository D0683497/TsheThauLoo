namespace TsheThauLoo.Parameters
{
    public class PaginationResourceParameters
    {
        /// <summary>
        /// 最大一頁的項目數
        /// </summary>
        private const int MaxPageSize = 50;
        
        /// <summary>
        /// 預設一頁的項目數
        /// </summary>
        private int _pageSize = 10;

        /// <summary>
        /// 目前頁碼 - 1
        /// </summary>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// 一頁的項目數
        /// </summary>
        /// <returns>如果超過 MaxPageSize 就設為 _pageSize，否則就用原本的值</returns>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}