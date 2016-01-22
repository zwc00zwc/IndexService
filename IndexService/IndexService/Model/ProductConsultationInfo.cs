using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexService.Model
{
    /// <summary>
    /// 商品咨询信息表
    /// </summary>
    public class ProductConsultationInfo
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long ShopId { get; set; }
        public string ShopName { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ConsultationContent { get; set; }
        public System.DateTime ConsultationDate { get; set; }
        public string ReplyContent { get; set; }
        public Nullable<System.DateTime> ReplyDate { get; set; }
    }
}
