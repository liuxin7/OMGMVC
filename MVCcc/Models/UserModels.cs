using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCcc.Models
{
    public class UserModels
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        ///  Required  验证 属性 必填项   不用在前端和后台 都写验证了   
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 邮件
        /// </summary>
        [DisplayName("邮件")]
        public string Email { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        ///   DisplayName   用于在界面上 显示  所对应的汉子
        [DisplayName("消息")]
        public string Message { get; set; }


    }
}